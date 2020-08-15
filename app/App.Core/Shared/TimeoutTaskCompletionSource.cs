using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core.Shared
{
    public class TimeoutTaskCompletionSource<T> : TaskCompletionSource<T>
    {
        public TimeoutTaskCompletionSource()
        {
            var cancellationSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            CancellationToken = cancellationSource.Token;
            CancellationToken.Register(() => TrySetCanceled());
        }

        public CancellationToken CancellationToken { get; }
    }
}