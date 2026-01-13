using CommunityToolkit.Mvvm.ComponentModel;

namespace MojTaxi.ClientApp.Services
{
    public partial class BusyService : ObservableObject
    {
        [ObservableProperty] 
        private bool isBusy;
        
        [ObservableProperty] 
        private string text = "Učitavam...";

        // Mali helper da ne zaboraviš finally
        public IDisposable Show(string? text = null)
        {
            if (!string.IsNullOrWhiteSpace(text))
                Text = text;

            IsBusy = true;
            return new BusyScope(this);
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
                _busy.IsBusy = false;
            }
        }
    }
}
