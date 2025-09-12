using Microsoft.Extensions.DependencyInjection;
using MojTaxi.Core.Abstractions;
using MojTaxi.Core.Implementations;

namespace MojTaxi.Core;


public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registruje Core servise (auth) – očekuješ da je ApiClient već registrovan (Refit).
    /// ISessionStore mora doći iz MAUI appa (npr. SecureStorageStore).
    /// </summary>
    public static IServiceCollection AddMojTaxiCore(this IServiceCollection services)
        => services
            .AddTransient<IClientAuthService, ClientAuthService>()
            .AddTransient<IDriverAuthService, DriverAuthService>();
}