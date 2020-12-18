using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using indexer.service.Clients;
using McMaster.Extensions.CommandLineUtils;

namespace indexer.service
{
    [Command("index", Description = "Fully indexes a location")]
    [HelpOption]

    internal class IndexCommand
    {
        private readonly IConsole _console;
        private readonly ApiClient _client;
        private readonly List<FileSystemWatcher> _watchers 
            = new List<FileSystemWatcher>();

        [Argument(0, "paths")]
        [DirectoryExists]
        [Required]
        public string[] Paths { get; set; }
        
        [Option("--exclude-type", CommandOptionType.MultipleValue)]
        public string[] ExcludedFileTypes { get; set; }

        [Option("--exclude-location", CommandOptionType.MultipleValue)]
        [DirectoryExists]
        public string[] ExcludedLocations { get; set; }

        public IndexCommand(
            IConsole console,
            ApiClient client
            )
        {
            _console = console;
            _client = client;
        }

        public Task<int> OnExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}