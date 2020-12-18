using System;
using System.Threading.Tasks;

namespace indexer.service.Support
{
    public interface IAsyncObserver<T>
    {
        Task OnCompletedAsync();
        Task OnErrorAsync(Exception error);
        Task OnNextAsync(T value);
    }
}