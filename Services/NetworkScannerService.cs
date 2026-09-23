using System.Net.NetworkInformation;
using System.Text.Json;
using OpsHub.ViewModels;

namespace OpsHub.Services;

public class NetworkScannerService
{
    private readonly IWebHostEnvironment _environment;

    public NetworkScannerService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<List<IpAddressViewModel>> GetIpAddressesAsync()
    {
        var filePath = Path.Combine(
            _environment.ContentRootPath,
            "Data",
            "ip-addresses.json"
        );

        if (!File.Exists(filePath))
        {
            return [];
        }

        var json = await File.ReadAllTextAsync(filePath);

        var ipAddresses =
            JsonSerializer.Deserialize<List<IpAddressViewModel>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? [];

        foreach (var ipAddress in ipAddresses)
        {
            ipAddress.ScreeningStatus =
                await GetScreeningStatusAsync(ipAddress.Address);
        }

        return ipAddresses;
    }

    private static async Task<string> GetScreeningStatusAsync(
        string ipAddress)
    {
        try
        {
            using var ping = new Ping();

            var reply =
                await ping.SendPingAsync(ipAddress, 1000);

            return reply.Status == IPStatus.Success
                ? "Alive"
                : "Dead";
        }
        catch
        {
            return "Unknown";
        }
    }
}