using Microsoft.Extensions.Primitives;
using System.Threading;

namespace TrainzInfo.Services
{
    public class LocomotivesCacheService
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
