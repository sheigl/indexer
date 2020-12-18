using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using indexer.service.Clients;
using indexer.service.Rx;
using indexer.service.Support;
using McMaster.Extensions.CommandLineUtils;

namespace indexer.service
{
    [Command("watch", Description = "Watches a location for changes")]
    [HelpOption]

    internal class WatchCommand
    {
        private readonly IConsole _console;
        private readonly FileSystemObserver _observer;

        [Argument(0, "paths")]
        [DirectoryExists]
        [Required]
        public string[] Paths { get; set; }
        
        [Option("--exclude-type", CommandOptionType.MultipleValue)]
        public string[] ExcludedFileTypes { get; set; }

        [Option("--exclude-location", CommandOptionType.MultipleValue)]
        [DirectoryExists]
        public string[] ExcludedLocations { get; set; }

        public WatchCommand(
            IConsole console,
            FileSystemObserver observer
            )
        {
            _console = console;
            _observer = observer;
        }

        public Task<int> OnExecuteAsync(CancellationToken cancellationToken)
        {
            foreach (var path in Paths)
            {
                FileSystemWatcher watcher = new FileSystemWatcher(path);

                var renamed = Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
                    handler => watcher.Renamed += handler,
                    handler => watcher.Renamed -= handler
                );

                var error = Observable.FromEventPattern<ErrorEventHandler, ErrorEventArgs>(
                    handler => watcher.Error += handler,
                    handler => watcher.Error -= handler
                );

                var changed = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                    handler => watcher.Changed += handler,
                    handler => watcher.Changed -= handler
                )
                .Select(args => args.EventArgs)
                .Subscribe(_observer);

                var created = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                    handler => watcher.Created += handler,
                    handler => watcher.Created -= handler
                )
                .Select(args => args.EventArgs)
                .Subscribe(_observer);

                var deleted = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                    handler => watcher.Deleted += handler,
                    handler => watcher.Deleted -= handler
                )
                .Throttle(TimeSpan.FromMilliseconds(100))
                .TakeUntil(other => cancellationToken.IsCancellationRequested)
                .Select(args => args.EventArgs)
                .SubscribeAsync(_observer);

                watcher.EnableRaisingEvents = true;
            }

            while (!cancellationToken.IsCancellationRequested) { }

            return Task.FromResult(0);
        }
    }
}