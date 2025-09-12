using MojTaxi.ApiClient.Dtos;
using Refit;

namespace MojTaxi.ApiClient.Infrastructure;

public interface IDriversApi
{
    // POST /{ver}/drivers/login
    [Post("/{ver}/drivers/login")]
    Task<DriverLoginResponse> LoginDriverAsync([AliasAs("ver")] string version,
        [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> form);

    // GET /{ver}/drivers/organisations
    [Get("/{ver}/drivers/organisations")]
    Task<List<OrganisationDto>> GetOrganisationsAsync([AliasAs("ver")] string version);

    // GET /{ver}/drivers/stands?organisation_id=...&session_id=...
    [Get("/{ver}/drivers/stands")]
    Task<List<StandDto>> GetStandsAsync([AliasAs("ver")] string version,
        [Query] string organisation_id, [Query] string session_id);

    // GET /{ver}/drivers/{driver_id}?session_id=...
    [Get("/{ver}/drivers/{driver_id}")]
    Task<DriverDto> GetDriverAsync([AliasAs("ver")] string version, string driver_id, [Query] string session_id);

    // GET /{ver}/drivers/{driver_id}/session/{session_id}
    [Get("/{ver}/drivers/{driver_id}/session/{session_id}")]
    Task<DriverSessionResponse> GetDriverSessionAsync([AliasAs("ver")] string version, string driver_id, string session_id);

    // POST /{ver}/drivers/{driver_id}/status
    [Post("/{ver}/drivers/{driver_id}/status")]
    Task<DriverDto> ChangeStatusAsync([AliasAs("ver")] string version, string driver_id,
        [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> form);

    // POST /{ver}/drivers/{driver_id}/alert
    [Post("/{ver}/drivers/{driver_id}/alert")]
    Task<AlertResponseDto> SendAlertAsync([AliasAs("ver")] string version, string driver_id,
        [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> form);

    // POST /{ver}/drivers/{driver_id}/vehicle/{vehicle_id}/location
    [Post("/{ver}/drivers/{driver_id}/vehicle/{vehicle_id}/location")]
    Task<ApiMessageDto> UpdateVehicleLocationAsync([AliasAs("ver")] string version, string driver_id, string vehicle_id,
        [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> form);

    // POST /{ver}/transaction (request card payment)
    [Post("/{ver}/transaction")]
    Task<TransactionRequestResponse> RequestCardPaymentAsync([AliasAs("ver")] string version,
        [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> form);

    // GET /{ver}/drivers/transaction?session_id=...&transaction_id=...
    [Get("/{ver}/drivers/transaction")]
    Task<TransactionStatusResponse> GetTransactionStatusAsync([AliasAs("ver")] string version,
        [Query] string session_id, [Query] string transaction_id);
}