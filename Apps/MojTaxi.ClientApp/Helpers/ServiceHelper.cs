using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.ClientApp.Helpers
{
    public static class ServiceHelper
    {
        public static IServiceProvider Services { get; private set; } = null!;

        internal static void Initialize(IServiceProvider provider)
            => Services = provider;

        public static T? Get<T>() => Services.GetService<T>();
    }
}
