namespace OpsHub.ViewModels;
public class DashboardViewModel
{
    public int TotalAssets { get; set; }
    public int AvailableAssets { get; set; }
    public int LoanedAssets { get; set; }
    public int LowStockItems { get; set; }

    public List<AttentionItemViewModel> AttentionItems { get; set; } = [];
    public List<NetworkUsageViewModel> Networks { get; set; } = [];
    public List<ActivityItemViewModel> RecentActivities { get; set; } = [];
    public List<MaintenanceItemViewModel> MaintenanceItems { get; set; } = [];
}

public class AttentionItemViewModel
{
    public string Type { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Severity { get; set; } = "";
}

public class NetworkUsageViewModel
{
    public string Name { get; set; } = "";
    public string Prefix { get; set; } = "";
    public int Used { get; set; }
    public int Total { get; set; }

    public int Percentage =>
        Total == 0 ? 0 : (int)Math.Round((double)Used / Total * 100);
}

public class ActivityItemViewModel
{
    public string Action { get; set; } = "";
    public string Description { get; set; } = "";
    public string User { get; set; } = "";
    public DateTime Timestamp { get; set; }
}

public class MaintenanceItemViewModel
{
    public string AssetName { get; set; } = "";
    public DateTime DueDate { get; set; }

    public bool IsOverdue => DueDate.Date < DateTime.Today;
}