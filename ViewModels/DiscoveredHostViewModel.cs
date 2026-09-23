namespace OpsHub.ViewModels;

public class DiscoveredHostViewModel
{
    public string Address { get; set; } = "";
    public string Hostname { get; set; } = "";
    public string InterfaceName { get; set; } = "";
    public string Network { get; set; } = "";
    public long ResponseTime { get; set; }
    public string Status { get; set; } = "";
}