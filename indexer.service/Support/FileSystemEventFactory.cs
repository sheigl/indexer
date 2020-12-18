using System.IO;
using System.Threading.Tasks;

namespace indexer.service.Support
{
    public class FileSystemEventFactory
    {
        private readonly FileSystemEventArgs _args;

        public FileSystemEventFactory(FileSystemEventArgs args)
        {
            _args = args;
        }

        public async Task HandleAsync()
        {

        }
    }
}