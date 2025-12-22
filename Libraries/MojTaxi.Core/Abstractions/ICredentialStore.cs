namespace MojTaxi.Core.Abstractions;

public interface ICredentialStore
{
    Task SaveAsync(string email, string password);
    Task<(string? Email, string? Password)> LoadAsync();
    Task ClearAsync();
}
