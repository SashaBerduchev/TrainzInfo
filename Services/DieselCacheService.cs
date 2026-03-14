using Microsoft.Extensions.Primitives;

namespace Services
{
    public class DieselCacheService
    {
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        public IChangeToken GetToken() => new CancellationChangeToken(_tokenSource.Token);
        public void Clear()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
            _tokenSource = new CancellationTokenSource();
        }
    }
}
