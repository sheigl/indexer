using System;
using System.Threading.Tasks;

namespace indexer.service.Support
{
    public interface IAsyncObserver<T>
    {
        void OnCompleted();
        void OnError(Exception error);
        Task OnNextAsync(T value);
    }
}