namespace MojTaxi.Client.Models;

public class CountryInfo
{
    public string? Name { get; set; }
    public string? Code { get; set; } // +387
    public string? Flag { get; set; } // 🇧🇦
    public string DisplayName => $"{Flag} {Name}";
}
