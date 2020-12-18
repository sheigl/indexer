using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace indexer.service
{
    [Command("indexd", Description = "File watcher and indexer")]
    [Subcommand(typeof(IndexCommand))]
    [HelpOption]
    internal class RootCommand
    {
        public Task<int> OnExecuteAsync()
        {
            return Task.FromResult(1);
        }
    }
}