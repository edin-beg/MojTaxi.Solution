using Refit;
using MojTaxi.ApiClient.Dtos;
using System.Net.Http;

namespace MojTaxi.ApiClient.Infrastructure;


public interface IClientsApi
{
    // POST /{ver}/clients (register)
    [Post("/{ver}/clients")]
    Task<ApiMessageDto> CreateClientAsync([AliasAs("ver")] string version,
        [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> form);

    // POST /{ver}/clients/activate
    [Post("/{ver}/clients/activate")]
    Task<ApiMessageDto> ActivateClientAsync([AliasAs("ver")] string version,
        [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> form);

    // POST /{ver}/clients/login
    /// <summary>
    /// Method for login a client
    /// </summary>
    /// <param name="version"></param>
    /// <param name="form">email, password, device_id, build_number</param>
    /// <returns></returns>
    [Post("/{ver}/clients/login")]
    Task<ClientLoginResponse> LoginClientAsync([AliasAs("ver")] string version,
        [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> form);

    // GET /{ver}/clients?session_id=...
    [Get("/{ver}/clients")]
    Task<ClientDto> GetCurrentClientAsync([AliasAs("ver")] string version, [Query] string session_id);

    // POST /{ver}/clients (update)
    [Post("/{ver}/clients")]
    Task<ApiMessageDto> UpdateClientAsync([AliasAs("ver")] string version,
        [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> form);

    // Recovery
    [Post("/{ver}/clients/recover/initiate")]
    Task<ApiMessageDto> StartRecoveryAsync([AliasAs("ver")] string version,
        [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> form);

    [Post("/{ver}/clients/recover/{token}")]
    Task<ApiMessageDto> CompleteRecoveryAsync([AliasAs("ver")] string version, string token,
        [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> form);

    // GET /{ver}/clients/{client_id}/session/{session_id}
    [Get("/{ver}/clients/{client_id}/session/{session_id}")]
    Task<ClientSessionResponse> GetSessionAsync([AliasAs("ver")] string version, string client_id, string session_id,
        [Query] string? build_number = null, [Query] bool? exclude_vehicles = null);
}