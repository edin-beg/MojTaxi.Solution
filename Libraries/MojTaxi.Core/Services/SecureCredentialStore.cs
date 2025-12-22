using MojTaxi.Core.Abstractions;

namespace MojTaxi.Core.Services;

public sealed class SecureCredentialStore : ICredentialStore
{
    private const string EmailKey = "cred.email";
    private const string PasswordKey = "cred.password";

    public async Task SaveAsync(string email, string password)
    {
        await SecureStorage.SetAsync(EmailKey, email);
        await SecureStorage.SetAsync(PasswordKey, password);
    }

    public async Task<(string? Email, string? Password)> LoadAsync()
    {
        var email = await SecureStorage.GetAsync(EmailKey);
        var password = await SecureStorage.GetAsync(PasswordKey);
        return (email, password);
    }

    public Task ClearAsync()
    {
        SecureStorage.Remove(EmailKey);
        SecureStorage.Remove(PasswordKey);
        return Task.CompletedTask;
    }
}
