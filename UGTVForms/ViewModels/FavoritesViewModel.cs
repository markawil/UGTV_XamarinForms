using System;
using System.Collections.ObjectModel;
using System.Json;
using System.Threading.Tasks;
using UGTVForms.Models;
using UGTVForms.Services;
using Xamarin.Essentials;

namespace UGTVForms.ViewModels
{
    public class FavoritesViewModel : VideosBaseViewModel 
    {
        public FavoritesViewModel()
        {
            IsRefreshEnabled = false;
        }
        
        public override async Task LoadVideos()
        {
            var result = Preferences.Get("favoritesJSON", string.Empty);
            var videosJSON = JsonValue.Parse(result);
            videos = videosJSON.VideosFromJSONValue();
            var pairs = videos.CreateVideoPairs();   
            VideoPairs = new ObservableCollection<VideoPairModel>(pairs);
        }        
    }
}
