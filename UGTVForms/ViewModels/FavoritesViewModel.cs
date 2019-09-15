using System;
using System.Collections.ObjectModel;
using System.Json;
using System.Linq;
using System.Threading.Tasks;
using UGTVForms.Models;
using UGTVForms.Services;
using Xamarin.Essentials;

namespace UGTVForms.ViewModels
{
    public class FavoritesViewModel : VideosBaseViewModel
    {
        private readonly IVideoDataStore favoritesDataStore;

        public FavoritesViewModel(IVideoDataStore favoritesDataStore)
        {
            IsRefreshEnabled = false;
            videoPairs = new ObservableCollection<VideoPairModel>();
            this.favoritesDataStore = favoritesDataStore;
        }

        public override void LoadVideos()
        {
            videos = favoritesDataStore.All().ToList();
            var pairs = videos.CreateVideoPairs();
            VideoPairs = new ObservableCollection<VideoPairModel>(pairs);
        }
    }
}
