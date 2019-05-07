using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Json;
using UGTVForms.ViewModels;
using System.Linq;
using UGTVForms.Models;

namespace UGTVForms.Services
{
    public class NetworkController : INetworkController
    {
        private readonly HttpClient client;
        private readonly string ugtvURLPath = "http://services.usergroup.tv/api/post";

        public NetworkController()
        {
            client = new HttpClient();
        }

        public async Task<List<VideoModel>> FetchVideosAsync()
        {
            var uri = new Uri(ugtvURLPath);
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jsonResult = JsonValue.Parse(content);
                return jsonResult.VideosFromJSONValue();
            }

            return null;
        }
    }

    public interface INetworkController
    { 
        Task<List<VideoModel>> FetchVideosAsync();
    }
}
