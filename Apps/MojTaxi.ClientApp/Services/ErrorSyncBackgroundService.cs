using MojTaxi.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.ClientApp.Services
{
    public class ErrorSyncBackgroundService
    {
        private readonly ILocalErrorStore _store;
        private readonly IRemoteErrorSender _remote;

        public ErrorSyncBackgroundService(ILocalErrorStore store,
                                          IRemoteErrorSender remote)
        {
            _store = store;
            _remote = remote;
            Start();
        }

        private void Start()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Sync();
                    await Task.Delay(30000);
                }
            });
        }

        private async Task Sync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                return;

            var list = await _store.GetAllAsync();

            foreach (var item in list)
            {
                if (await _remote.SendAsync(item))
                    await _store.DeleteAsync(item.Id);
            }
        }
    }

}
