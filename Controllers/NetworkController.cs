using Microsoft.AspNetCore.Mvc;
using OpsHub.Services;

namespace OpsHub.Controllers;

public class NetworkController : Controller
{
    private readonly NetworkScannerService _networkScanner;
    public NetworkController(NetworkScannerService networkScanner)
    {
        _networkScanner = networkScanner;
    }

    public async Task<IActionResult> IpAddresses()
    {
        var ipAddresses = await _networkScanner.GetIpAddressesAsync();

        return View(ipAddresses);
    }
}