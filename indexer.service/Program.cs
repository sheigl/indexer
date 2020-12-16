using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using indexer.common.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace indexer.service
{
    class Program
    {
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
                if (item.Name.StartsWith("."))
                    continue;

                Console.WriteLine($"Saving {item.FullName}");

                string path = item.FullName.Replace(item.Name, "");
                path = path.Substring(0, path.Length - 1);

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
                if (item.Name.StartsWith("."))
                    continue;

                GetFilesRecursive(item, files);
            }
        }
    }
}
