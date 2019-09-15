using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Json;
using UGTVForms.ViewModels;
using System.Linq;
using UGTVForms.Models;
using Xamarin.Forms;
using System.Net;

namespace UGTVForms.Services
{
    public class NetworkController : INetworkController
    {
        private readonly string ugtvURLPath = "http://services.usergroup.tv/api/post";                

        public async Task<List<VideoModel>> FetchVideosAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(ugtvURLPath);
                    var response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var jsonResult = JsonValue.Parse(content);
                        return jsonResult.VideosFromJson().ToList();
                    }

                    throw new Exception();
                }
            }
            catch
            {
                MessagingCenter.Send(this, Settings.NetworkCallFailedKey);
            }

            return null;
        }        
    }

    public interface INetworkController
    {
        Task<List<VideoModel>> FetchVideosAsync();        
    }
}
