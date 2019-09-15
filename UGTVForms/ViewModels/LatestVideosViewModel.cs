using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UGTVForms.Services;
using UGTVForms.Models;
using System.Linq;

namespace UGTVForms.ViewModels
{
    public class LatestVideosViewModel : VideosBaseViewModel
    {
        private readonly INetworkController _networkController;
        private readonly IVideosDataStore _latestVideosDataStore;
        private readonly IVideoDataStore _favoritesDataStore;

        public LatestVideosViewModel(INetworkController networkController, 
                                     IVideosDataStore latestVideosDataStore,
                                     IVideoDataStore favoritesDataStore)
        {
            _networkController = networkController;
            _latestVideosDataStore = latestVideosDataStore;
            _favoritesDataStore = favoritesDataStore;
            IsRefreshEnabled = true;
            videoPairs = new ObservableCollection<VideoPairModel>();            
        }

        public override async Task LoadVideosAsync()
        {
            IsBusy = true;
            videos = await _networkController.FetchVideosAsync();
            var favorites = _favoritesDataStore.All().ToList();
            foreach (var video in videos)
            {
                video.Favorited = favorites.Contains(video);                    
            }
            _latestVideosDataStore.AddItems(videos);
            var pairs = videos.CreateVideoPairs();
            VideoPairs = new ObservableCollection<VideoPairModel>(pairs);
            IsBusy = false;
        }       
    }
}