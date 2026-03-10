using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrainzInfoServices
{
    public class MainImageCacheService
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
