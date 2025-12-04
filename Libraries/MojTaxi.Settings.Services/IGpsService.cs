
namespace MojTaxi.Settings.Services
{
    public interface IGpsService
    {
        Task<bool> IsGpsEnabledAsync();
    }
}