using MojTaxi.ApiClient;
using MojTaxi.ApiClient.Dtos;
using MojTaxi.ApiClient.Infrastructure;
using MojTaxi.Core.Abstractions;
using Refit;
using System.Text.Json;

namespace MojTaxi.Core.Services;

public sealed class AuthService : IAuthService
{
    private const string ApiVersion = "V5";

    private readonly IClientsApi _api;
    private readonly ISessionStore _sessionStore;
    private readonly ICredentialStore _credentialStore;
    private readonly IClientSession _clientSession;

    public AuthService(
        IClientsApi api,
        ISessionStore sessionStore,
        ICredentialStore credentialStore,
        IClientSession clientSession)
    {
        _api = api;
        _sessionStore = sessionStore;
        _credentialStore = credentialStore;
        _clientSession = clientSession;
    }

    public async Task LoginAsync(string email, string password)
    {
        try
        {
            var res = await _api.LoginClientAsync(ApiVersion, new()
            {
                ["email"] = email,
                ["password"] = password,
                ["device_id"] = "device_id",
                ["build_number"] = "199"
            });

            if (res.Client?.ClientId is null || string.IsNullOrWhiteSpace(res.SessionId))
                throw new InvalidOperationException("Login response missing client/session data.");

            var response = new ClientSessionResponse
            {
                Client = res.Client,
                SessionId = res.SessionId,
                DeviceId = "device_id",
                Expires = DateTime.UtcNow.AddDays(30)
            };

            await _sessionStore.SaveClientDataAsync(
                response.Client.ClientId!,
                response.SessionId!,
                response.DeviceId,
                response.Expires!.Value.ToUniversalTime());

            await _credentialStore.SaveAsync(email, password);

            _clientSession.Set(response);
        }
        catch (ApiException apiEx)
        {
            if (apiEx.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var message = "Invalid login credentials.";

                if (!string.IsNullOrWhiteSpace(apiEx.Content))
                {
                    try
                    {
                        var err = JsonSerializer.Deserialize<LoginErrorResponse>(apiEx.Content);
                        message = err?.Error ?? message;
                    }
                    catch
                    {
                        message = apiEx.Content;
                    }
                }

                throw new InvalidOperationException(message);
            }

            throw;
        }
    }


    public async Task<bool> TryRestoreAsync()
    {
        var (clientId, sessionId, _, _) = await _sessionStore.LoadClientDataAsync();

        if (!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(sessionId))
        {
            try
            {
                var sess = await _api.GetSessionAsync(ApiVersion, clientId!, sessionId!, "300", true);
                _clientSession.Set(sess);

                if (sess.Client?.ClientId is not null && !string.IsNullOrWhiteSpace(sess.SessionId))
                {
                    await _sessionStore.SaveClientDataAsync(clientId!, sess.SessionId!, sess.DeviceId, sess.Expires?.ToUniversalTime());
                    return true;
                }
            }
            catch
            {
                // session invalid/expired -> fallback below
            }
        }

        // fallback: silent auto-login
        var (email, password) = await _credentialStore.LoadAsync();

        if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
        {
            try
            {
                await LoginAsync(email!, password!);
                return true;
            }
            catch
            {
                // credentials invalid
            }
        }

        await LogoutAsync();
        return false;
    }

    public async Task LogoutAsync()
    {
        await _sessionStore.ClearClientDataAsync();
        await _credentialStore.ClearAsync();
        _clientSession.Clear();

        await Shell.Current.GoToAsync("//LoginPage");
    }
}
