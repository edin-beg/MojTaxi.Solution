
namespace MojTaxi.Core.Abstractions;

public interface IGpsService
{
    Task<bool> IsGpsEnabledAsync();
}
