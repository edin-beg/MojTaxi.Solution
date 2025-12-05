using MojTaxi.Core.Abstractions;
using MojTaxi.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.Core.Implementations
{
    public class ErrorLogger : IErrorLogger
    {
        private readonly ILocalErrorStore _store;

        public ErrorLogger(ILocalErrorStore store)
        {
            _store = store;
        }

        public async Task LogAsync(Exception ex)
        {
            var entry = new ErrorEntry
            {
                Message = ex.Message,
                StackTrace = ex.ToString(),
                Platform = DeviceInfo.Platform.ToString(),
                Device = $"{DeviceInfo.Manufacturer} {DeviceInfo.Model}",
                AppVersion = AppInfo.VersionString
            };

            await _store.SaveAsync(entry);
        }
    }
}
