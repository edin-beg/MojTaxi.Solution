namespace MojTaxi.Core.Abstractions;

/// <summary>
/// Apstrakcija nad SecureStorage/Preferences; implementacija u MAUI appu.
/// Ključevima razlikujemo klijenta i vozača.
/// </summary>
public interface ISessionStore
{
    Task SaveAsync(string key, string value, CancellationToken ct = default);
    Task<string?> GetAsync(string key, CancellationToken ct = default);
    Task RemoveAsync(string key, CancellationToken ct = default);
}