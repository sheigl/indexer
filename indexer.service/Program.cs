using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using indexer.common.Models;
using indexer.service.Clients;
using indexer.service.Support;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace indexer.service
{
    class Program
    {
        static async Task Main(string[] args) => await BuildApp().ExecuteAsync(args);

        private static CommandLineApplication BuildApp()
        {
            CommandLineApplication app = new CommandLineApplication<RootCommand>();
            app.Conventions.UseDefaultConventions()
                .UseConstructorInjection(ConfigureServices());

            return app;
        }

        private static IServiceProvider ConfigureServices()
        {
            ServiceCollection services = new ServiceCollection(); 

            services
                .AddSingleton<IConfiguration>(ConfigureConfiguration())
                .AddSingleton<IConsole>(PhysicalConsole.Singleton)
                .AddScoped<FileSystemObserver>()
                .AddHttpClient<ApiClient>((provider, client) => client.BaseAddress = new Uri(provider.GetRequiredService<IConfiguration>()["apibase"]));

            return services.BuildServiceProvider();
        }

        private static IConfiguration ConfigureConfiguration()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables("ASPNETCORE_");

            return builder.Build();
        }

        /*
        private static HttpClient _client = new HttpClient();
        static async Task Main(string[] args)
        {
            List<string> pathsToScan = new List<string>(args);

            Console.WriteLine($"Indexing {(String.Join(", ", pathsToScan))}");

            List<FileEntry> files  = new List<FileEntry>();

            if (File.Exists("cache.json"))
            {
                files = JsonConvert.DeserializeObject<List<FileEntry>>(await File.ReadAllTextAsync("cache.json"));
            }
            else
            {
                foreach (var item in pathsToScan)
                {
                    GetFilesRecursive(new DirectoryInfo(item), files);
                }
            }

            

            if (files.Any())
            {
                await File.WriteAllTextAsync("cache.json", JsonConvert.SerializeObject(files));

                foreach (var item in files)
                {
                    string json = JsonConvert.SerializeObject(item);

                    HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/file");
                    req.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var res = await _client.SendAsync(req);
                    res.EnsureSuccessStatusCode();
                }                

                File.Delete("cache.json");
            }
        }

        private static void GetFilesRecursive(DirectoryInfo directory, List<FileEntry> files = null)
        {
            if (files == null)
                files = new List<FileEntry>();

            if (!directory.Exists)
                return;

            foreach (var item in directory.EnumerateFiles())
            {
                Console.WriteLine($"Saving {item.FullName}");

                string path = item.FullName.Replace(item.Name, "").Trim().TrimEnd('/');

                files.Add(new FileEntry
                {
                    Name = item.Name,
                    FullName = item.FullName,
                    Extension = item.Extension,
                    Path = path,
                    Size = item.Length,
                    CreationTime = item.CreationTime,
                    LastAccessTime = item.LastAccessTime,
                    LastWriteTime = item.LastWriteTime,
                    IndexedDate = DateTime.Now
                });
            }

            foreach (var item in directory.EnumerateDirectories())
            {
                GetFilesRecursive(item, files);
            }
        }*/
    }
}
