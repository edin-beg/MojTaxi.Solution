namespace MojTaxi.ApiClient;

public class ApiSettings
{
    public required string BaseUrl { get; init; }
    public string Version { get; init; } = "V5";
    public string BuildNumber { get; init; } = "1";
    public string AppVersion { get; init; } = "1.0.0";
}