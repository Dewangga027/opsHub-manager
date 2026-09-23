using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using OpsHub.Services;
using OpsHub.ViewModels;

namespace OpsHub.Controllers;

public class NetworkController : Controller
{
    private readonly NetworkDiscoveryService
        _networkDiscoveryService;
    private readonly IWebHostEnvironment _environment;

    public NetworkController(
        NetworkDiscoveryService networkDiscoveryService,
        IWebHostEnvironment environment)
    {
        _networkDiscoveryService = networkDiscoveryService;
        _environment = environment;
    }


    public async Task<IActionResult> IpAddresses()
    {
        var discoveredHosts =
            await _networkDiscoveryService
                .DiscoverAsync();

        var inventoryPath = Path.Combine(
            _environment.ContentRootPath, "Data", "Ip-addresses.json");
        var savedHosts = System.IO.File.Exists(inventoryPath)
            ? JsonSerializer.Deserialize<List<IpAddressViewModel>>(
                await System.IO.File.ReadAllTextAsync(inventoryPath),
                new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? []
            : [];
        var savedHostsByAddress = savedHosts
            .Where(host => !string.IsNullOrWhiteSpace(host.Address))
            .GroupBy(host => host.Address)
            .ToDictionary(group => group.Key, group => group.First());

        var ipAddresses =
            discoveredHosts
                .Select(host =>
                {
                    savedHostsByAddress.TryGetValue(host.Address, out var saved);

                    return new IpAddressViewModel
                    {
                        Address = host.Address,

                        MachineName = saved?.MachineName ?? "",

                        Hostname = !string.IsNullOrWhiteSpace(host.Hostname)
                            ? host.Hostname
                            : saved?.Hostname ?? "",

                        Type = saved?.Type ?? "Discovered",

                        Subnet = host.Network,

                        Area = saved?.Area ?? host.InterfaceName,

                        Status = "Discovered",

                        ScreeningStatus = host.Status
                    };
                })
                .ToList();

        return View(ipAddresses);
}
}
