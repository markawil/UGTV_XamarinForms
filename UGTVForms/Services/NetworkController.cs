using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Json;
using UGTVForms.ViewModels;
using System.Linq;

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

        public async Task<List<VideoViewModel>> FetchVideosAsync()
        {
            var uri = new Uri(ugtvURLPath);
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jsonResult = JsonValue.Parse(content);
                var videosArray = jsonResult as JsonArray;                
                var videos = new List<VideoViewModel>();
                
                foreach (var videoInfo in videosArray)
                {
                    var video = new VideoViewModel(videoInfo);
                    videos.Add(video);
                }

                return videos;
            }

            return null;
        }
    }

    public interface INetworkController
    { 
        Task<List<VideoViewModel>> FetchVideosAsync();
    }
}
