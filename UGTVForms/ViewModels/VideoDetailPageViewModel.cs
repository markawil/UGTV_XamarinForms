using System;
using System.Json;
using System.Windows.Input;
using UGTVForms.Models;
using UGTVForms.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UGTVForms.ViewModels
{
    public class VideoDetailPageViewModel : BaseViewModel
    {
        private readonly VideoModel video;

        public ICommand FavoriteCommand { get; set; }
        public ICommand DownloadCommand { get; set; }

        public string VideoImg { get => video.ImageURLPath; }        
        public string VideoURLPath { get => video.VideoUrlPathLow; }
        
        public VideoDetailPageViewModel(VideoModel video)
        {
            FavoriteCommand = new Command(FavoriteVideo);
            DownloadCommand = new Command(DownloadVideo);
            
            this.video = video;
        }
        
        public string VideoTitle { get => video.VideoTitle; }

        private void DownloadVideo()
        {
            
        }     

        public void FavoriteVideo()
        {
            var result = Preferences.Get(Settings.Favorites_Key, string.Empty);
            if (!string.IsNullOrEmpty(result))
            {
                var jsonValue = JsonValue.Parse(result);
                var videosArray = jsonValue as JsonArray;
                videosArray.Add(video.JsonValue);
                Preferences.Set(Settings.Favorites_Key, videosArray.ToString());                
            }
            else
            {
                var jsonValues = new JsonValue[] { video.JsonValue };
                var jsonArray = new JsonArray(jsonValues);
                Preferences.Set(Settings.Favorites_Key, jsonArray.ToString());
            }
        }
    }
}
