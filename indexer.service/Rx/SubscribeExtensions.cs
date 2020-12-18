using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using indexer.service.Support;

namespace indexer.service.Rx
{
    public static class SubscribeExtensions
    {
        public static IDisposable SubscribeAsync<T>(this IObservable<T> source, IAsyncObserver<T> observer)
        {
            return source.Select(e => Observable.Defer(() => observer.OnNextAsync(e).ToObservable()))
                .Concat()
                .Subscribe(
                e => { }, // empty
                observer.OnError,
                observer.OnCompleted);
        }
    }
}