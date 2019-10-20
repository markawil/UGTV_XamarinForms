using System.Collections.ObjectModel;
using System.Linq;
using UGTVForms.Models;
using UGTVForms.Services;

namespace UGTVForms.ViewModels
{
    public class FavoritesViewModel : VideosBaseViewModel
    {
        private readonly IVideoDataStore favoritesDataStore;
        private readonly IVideoDataStore downloadsDataStore;

        public FavoritesViewModel(IVideoDataStore favoritesDataStore,
                                  IVideoDataStore downloadsDataStore)
        {
            IsRefreshEnabled = false;
            videoPairs = new ObservableCollection<VideoPairModel>();
            this.favoritesDataStore = favoritesDataStore;
            this.downloadsDataStore = downloadsDataStore;
        }

        public override void LoadVideos()
        {
            videos = favoritesDataStore.All().ToList();
            var downloads = downloadsDataStore.All().ToList();
            foreach (var video in videos)
            {
                video.Favorited = true;
                video.Downloaded = downloads.Contains(video);
                if (video.Downloaded)
                {
                    var downloadedVideo = downloads.First(d => d.Id == video.Id);
                    video.DownloadedFilePath = downloadedVideo.DownloadedFilePath;
                }
            }
            var pairs = videos.CreateVideoPairs();
            VideoPairs = new ObservableCollection<VideoPairModel>(pairs);
        }
    }
}
