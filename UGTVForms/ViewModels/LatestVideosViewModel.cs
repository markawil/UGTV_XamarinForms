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
        private readonly IVideoDataStore _favoritesDataStore;
        private readonly IVideoDataStore _downloadsDataStore;

        public LatestVideosViewModel(INetworkController networkController,
                                     IVideoDataStore favoritesDataStore,
                                     IVideoDataStore downloadsDataStore)
        {
            _networkController = networkController;
            _favoritesDataStore = favoritesDataStore;
            _downloadsDataStore = downloadsDataStore;
            IsRefreshEnabled = true;
            videoPairs = new ObservableCollection<VideoPairModel>();            
        }

        public override void LoadVideos()
        {
            UpdateVideosIfFavoritedOrDownloaded();
        }

        public override async Task LoadVideosAsync()
        {
            IsBusy = true;
            videos = await _networkController.FetchVideosAsync();
            UpdateVideosIfFavoritedOrDownloaded();
            var pairs = videos.CreateVideoPairs();
            VideoPairs = new ObservableCollection<VideoPairModel>(pairs);
            IsBusy = false;
        }

        public void UpdateVideosIfFavoritedOrDownloaded()
        {
            var favorites = _favoritesDataStore.All().ToList();
            var downloads = _downloadsDataStore.All().ToList();
            // turn on the favorited and downloaded icons if the
            // video was favorited or downloaded
            foreach (var video in videos)
            {
                video.Favorited = favorites.Contains(video);
                video.Downloaded = downloads.Contains(video);
                if (video.Downloaded)
                {
                    var downloadedVideo = downloads.First(d => d.Id == video.Id);
                    video.DownloadedFilePath = downloadedVideo.DownloadedFilePath;
                }
            }            
        }
    }
}