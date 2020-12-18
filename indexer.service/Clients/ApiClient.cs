using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using indexer.common.Models;
using Newtonsoft.Json;

namespace indexer.service.Clients
{
    public class ApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(HttpClient client) =>
            _client = client;

        public async Task UpsertFileAsync(FileEntry file)
        {
            string json = JsonConvert.SerializeObject(file);

            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "file");
            req.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await _client.SendAsync(req);
            res.EnsureSuccessStatusCode();
        }
    }
}