using Microsoft.AspNetCore.Mvc;
using OpsHub.ViewModels;

namespace OpsHub.Controllers;

public class NetworkController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction(nameof(IpAddresses));
    }
    public IActionResult IpAddresses()
    {
        var ipAddresses = new List<IpAddressViewModel>
        {
            new()
            {
                Address = "192.168.1.2",

                MachineName = "PLC KL Packing",
                Hostname = "plc-kl-packing",

                Type = "PLC",
                Subnet = "192.168.1.0/24",

                Hardware = "",
                Software = "",

                Area = "Packing",
                Line = "",
                Floor = "",
                Machine = "",
                Project = "",

                Status = "OK",
                ScreeningStatus = "Alive"
            },

            new()
            {
                Address = "192.168.1.3",

                MachineName = "Local Server Database",
                Hostname = "local-server-db",

                Type = "PC",
                Subnet = "192.168.1.0/24",

                Hardware = "",
                Software = "MSSQL",

                Area = "",
                Line = "",
                Floor = "",
                Machine = "",
                Project = "",

                Status = "OK",
                ScreeningStatus = "Alive"
            },

            new()
            {
                Address = "192.168.1.4",

                MachineName = "PLC Gateway Packing (Master)",
                Hostname = "plc-gateway-packing",

                Type = "PLC",
                Subnet = "192.168.1.0/24",

                Hardware = "M340 BMX-NOC0401",
                Software = "Unity Pro XL V8.0",

                Area = "Packing",
                Line = "Packing",
                Floor = "",
                Machine = "OEE",
                Project = "",

                Status = "OK",
                ScreeningStatus = "Alive"
            },

            new()
            {
                Address = "192.168.1.5",

                MachineName = "Router Digitalization (Sanghaling Center)",
                Hostname = "router-digitalization",

                Type = "Router",
                Subnet = "192.168.1.0/24",

                Hardware = "",
                Software = "",

                Area = "",
                Line = "",
                Floor = "",
                Machine = "",
                Project = "",

                Status = "OK",
                ScreeningStatus = "Alive"
            },

            new()
            {
                Address = "192.168.1.6",

                MachineName = "PLC M221 Line A",
                Hostname = "plc-m221-line-a",

                Type = "PLC",
                Subnet = "192.168.1.0/24",

                Hardware = "M221 - TM221CE40R",
                Software = "EcoStruxure Basic",

                Area = "A",
                Line = "A",
                Floor = "1",
                Machine = "FILLING",
                Project = "PRODUKSI",

                Status = "OK",
                ScreeningStatus = "Alive"
            },

            new()
            {
                Address = "192.168.1.7",

                MachineName = "PLC Discharge IBC G2",
                Hostname = "plc-discharge-ibc-g2",

                Type = "PLC",
                Subnet = "192.168.1.0/24",

                Hardware = "M221 - TM221CE40R",
                Software = "EcoStruxure Machine Expert - Basic",

                Area = "G2",
                Line = "G2",
                Floor = "2",
                Machine = "DISCHARGE",
                Project = "IBC",

                Status = "OK",
                ScreeningStatus = "Alive"
            },

            new()
            {
                Address = "192.168.1.8",

                MachineName = "Reserved IP",
                Hostname = "",

                Type = "",
                Subnet = "192.168.1.0/24",

                Hardware = "",
                Software = "",

                Area = "",
                Line = "",
                Floor = "",
                Machine = "",
                Project = "",

                Status = "Reserved",
                ScreeningStatus = "Unknown"
            },

            new()
            {
                Address = "192.168.1.9",

                MachineName = "",
                Hostname = "",

                Type = "",
                Subnet = "192.168.1.0/24",

                Hardware = "",
                Software = "",

                Area = "",
                Line = "",
                Floor = "",
                Machine = "",
                Project = "",

                Status = "Available",
                ScreeningStatus = "Unknown"
            }
        };

        return View(ipAddresses);
    }
}