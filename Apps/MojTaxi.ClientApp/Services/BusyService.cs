using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using MojTaxi.ClientApp.Popups;

namespace MojTaxi.ClientApp.Services;

public sealed class BusyService
{
    private readonly SemaphoreSlim _gate = new(1, 1);
    private LoadingPopup? _popup;
    private int _counter;

    public async Task<IDisposable> ShowAsync(string text = "Učitavam...")
    {
        await _gate.WaitAsync();
        try
        {
            _counter++;

            // Ako je već prikazan, samo update teksta i vrati scope
            if (_popup != null)
            {
                _popup.BindingContext = new { Text = text };
                return new BusyScope(this);
            }

            _popup = new LoadingPopup
            {
                BindingContext = new { Text = text }
            };

            // Mora na UI thread
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var page = Application.Current?.MainPage;
                if (page is null)
                    throw new InvalidOperationException("MainPage is null.");

                await page.ShowPopupAsync(_popup);
            });

            return new BusyScope(this);
        }
        finally
        {
            _gate.Release();
        }
    }

    private async Task HideInternalAsync()
    {
        await _gate.WaitAsync();
        try
        {
            _counter--;
            if (_counter > 0) return;

            _counter = 0;

            var popup = _popup;
            _popup = null;

            if (popup == null) return;

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                popup.CloseAsync();
            });
        }
        finally
        {
            _gate.Release();
        }
    }

    private sealed class BusyScope : IDisposable
    {
        private readonly BusyService _busy;
        private bool _disposed;

        public BusyScope(BusyService busy) => _busy = busy;

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            // fire-and-forget bez bacanja exceptiona
            _ = _busy.HideInternalAsync();
        }
    }
}
