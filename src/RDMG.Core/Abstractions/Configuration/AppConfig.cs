namespace RDMG.Core.Abstractions.Configuration;

public class AppConfig
{
    public const string AppConfigName = "Config";
    public string DefaultAdminPassword { get; set; } = string.Empty;
    public string DefaultUserPassword { get; set; } = string.Empty;
}