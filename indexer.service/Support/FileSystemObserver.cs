using System;
using System.IO;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using indexer.service.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace indexer.service.Support
{
    public class FileSystemObserver : IAsyncObserver<FileSystemEventArgs>
    {
        private readonly IServiceProvider _provider;

        public FileSystemObserver(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task OnCompletedAsync()
        {
            throw new NotImplementedException();
        }

        public Task OnErrorAsync(Exception error)
        {
            throw new NotImplementedException();
        }

        public Task OnNextAsync(FileSystemEventArgs value)
        {
            using (var scope = _provider.CreateScope())
            {
                var client = scope.ServiceProvider.GetRequiredService<ApiClient>();

                client.UpsertFileAsync(null)
                    .ToObservable();
            }
        }
    }
}