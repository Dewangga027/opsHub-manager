using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using OpsHub.ViewModels;

namespace OpsHub.Services;

public class NetworkDiscoveryService
{
    private const int PingTimeout = 500;

    public async Task<List<DiscoveredHostViewModel>> DiscoverAsync()
    {
        var discoveredHosts = new List<DiscoveredHostViewModel>();

        var interfaces = NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(IsUsableEthernetInterface);

        foreach (var networkInterface in interfaces)
        {
            var ipProperties =
                networkInterface.GetIPProperties();

            var ipv4Addresses =
                ipProperties.UnicastAddresses
                    .Where(x =>
                        x.Address.AddressFamily ==
                        AddressFamily.InterNetwork);

            foreach (var addressInfo in ipv4Addresses)
            {
                if (addressInfo.IPv4Mask == null)
                {
                    continue;
                }

                var hosts = await ScanSubnetAsync(
                    networkInterface.Name,
                    addressInfo.Address,
                    addressInfo.IPv4Mask
                );

                discoveredHosts.AddRange(hosts);
            }
        }

        return discoveredHosts
            .OrderBy(x => ParseLastOctet(x.Address))
            .ToList();
    }


    private static bool IsUsableEthernetInterface(
        NetworkInterface networkInterface)
    {
        return
            networkInterface.NetworkInterfaceType ==
                NetworkInterfaceType.Ethernet
            &&
            networkInterface.OperationalStatus ==
                OperationalStatus.Up;
    }


    private async Task<List<DiscoveredHostViewModel>>
        ScanSubnetAsync(
            string interfaceName,
            IPAddress localAddress,
            IPAddress subnetMask)
    {
        var hosts =
            new List<DiscoveredHostViewModel>();

        var networkAddress =
            CalculateNetworkAddress(
                localAddress,
                subnetMask);

        var prefixLength =
            GetPrefixLength(subnetMask);

        // Versi awal kita hanya scan /24 atau lebih kecil.
        if (prefixLength < 24)
        {
            return hosts;
        }

        var networkBytes =
            networkAddress.GetAddressBytes();

        var tasks = new List<Task<DiscoveredHostViewModel?>>();

        for (var host = 1; host <= 254; host++)
        {
            var targetBytes =
                (byte[])networkBytes.Clone();

            targetBytes[3] = (byte)host;

            var targetAddress =
                new IPAddress(targetBytes);

            tasks.Add(
                ProbeHostAsync(
                    interfaceName,
                    targetAddress,
                    $"{networkAddress}/{prefixLength}"
                )
            );
        }

        var results =
            await Task.WhenAll(tasks);

        hosts.AddRange(
            results.Where(x => x != null)!
        );

        return hosts;
    }


    private static async Task<DiscoveredHostViewModel?>
        ProbeHostAsync(
            string interfaceName,
            IPAddress address,
            string network)
    {
        try
        {
            using var ping = new Ping();

            var reply =
                await ping.SendPingAsync(
                    address,
                    PingTimeout);

            if (reply.Status != IPStatus.Success)
            {
                return null;
            }

            var hostname =
                await ResolveHostnameAsync(address);

            return new DiscoveredHostViewModel
            {
                Address = address.ToString(),
                Hostname = hostname,
                InterfaceName = interfaceName,
                Network = network,
                ResponseTime = reply.RoundtripTime,
                Status = "Alive"
            };
        }
        catch
        {
            return null;
        }
    }


    private static async Task<string>
        ResolveHostnameAsync(IPAddress address)
    {
        try
        {
            var entry =await Dns.GetHostEntryAsync(address);

            if (!string.IsNullOrWhiteSpace(entry.HostName))
            {
                return entry.HostName;
            }
        }
        catch
        {
            // ignore
        }

        return "";
    }


    private static IPAddress CalculateNetworkAddress(
        IPAddress address,
        IPAddress subnetMask)
    {
        var addressBytes =
            address.GetAddressBytes();

        var maskBytes =
            subnetMask.GetAddressBytes();

        var networkBytes =
            new byte[addressBytes.Length];

        for (var i = 0; i < addressBytes.Length; i++)
        {
            networkBytes[i] =
                (byte)(
                    addressBytes[i]
                    &
                    maskBytes[i]
                );
        }

        return new IPAddress(networkBytes);
    }


    private static int GetPrefixLength(
        IPAddress subnetMask)
    {
        return subnetMask
            .GetAddressBytes()
            .Sum(x =>
                Convert
                    .ToString(x, 2)
                    .Count(bit => bit == '1'));
    }


    private static int ParseLastOctet(
        string address)
    {
        var parts =
            address.Split('.');

        return int.TryParse(
            parts.LastOrDefault(),
            out var value)
                ? value
                : 0;
    }
}