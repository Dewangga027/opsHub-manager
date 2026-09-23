using Microsoft.AspNetCore.Mvc;
using OpsHub.ViewModels;

namespace OpsHub.Controllers;

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        var model = new DashboardViewModel
        {
            TotalAssets = 128,
            AvailableAssets = 91,
            LoanedAssets = 12,
            LowStockItems = 5,

            AttentionItems =
            [
                new()
                {
                    Type = "Inventory",
                    Title = "SFP 10G hampir habis",
                    Description = "2 unit tersedia dari minimum 5 unit.",
                    Severity = "danger"
                },
                new()
                {
                    Type = "Loan",
                    Title = "LAN Tester terlambat dikembalikan",
                    Description = "Dipinjam oleh Budi · overdue 3 hari.",
                    Severity = "warning"
                },
                new()
                {
                    Type = "Network",
                    Title = "Subnet hampir penuh",
                    Description = "10.20.2.0/24 telah menggunakan 91% kapasitas.",
                    Severity = "warning"
                }
            ],

            Networks =
            [
                new()
                {
                    Name = "Management",
                    Prefix = "10.20.1.0/24",
                    Used = 184,
                    Total = 254
                },
                new()
                {
                    Name = "Production",
                    Prefix = "10.20.2.0/24",
                    Used = 231,
                    Total = 254
                },
                new()
                {
                    Name = "Office",
                    Prefix = "10.20.3.0/24",
                    Used = 92,
                    Total = 254
                }
            ],

            MaintenanceItems =
            [
                new()
                {
                    AssetName = "UPS-02",
                    DueDate = DateTime.Today
                },
                new()
                {
                    AssetName = "Router-04",
                    DueDate = DateTime.Today.AddDays(5)
                }
            ],

            RecentActivities =
            [
                new()
                {
                    Action = "IP Assigned",
                    Description = "10.20.1.34 assigned to SW-ACCESS-03",
                    User = "Admin",
                    Timestamp = DateTime.Now.AddMinutes(-12)
                },
                new()
                {
                    Action = "Asset Loan",
                    Description = "LAN Tester LT-003 checked out",
                    User = "Budi",
                    Timestamp = DateTime.Now.AddMinutes(-38)
                },
                new()
                {
                    Action = "Stock Out",
                    Description = "SFP 10G quantity decreased by 2",
                    User = "Admin",
                    Timestamp = DateTime.Now.AddHours(-1)
                }
            ]
        };

        return View(model);
    }
}