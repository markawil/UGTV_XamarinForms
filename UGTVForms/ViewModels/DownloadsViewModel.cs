using System;
using System.Collections.ObjectModel;
using System.Linq;
using UGTVForms.Models;
using UGTVForms.Services;
using UGTVForms.ViewModels;

namespace UGTVForms.ViewModels
{
    public class DownloadsViewModel : VideosBaseViewModel
    {
        private readonly IVideoDataStore downloadsDataStore;
        private readonly IVideoDataStore favoritesDataStore;

        public DownloadsViewModel(IVideoDataStore downloadsDataStore,
                                  IVideoDataStore favoritesDataStore)
        {
            IsRefreshEnabled = false;
            videoPairs = new ObservableCollection<VideoPairModel>();
            this.downloadsDataStore = downloadsDataStore;
            this.favoritesDataStore = favoritesDataStore;
        }

        public override void LoadVideos()
        {
            videos = downloadsDataStore.All().ToList();
            var favorites = favoritesDataStore.All().ToList();
            foreach (var video in videos)
            {
                if (favorites.Contains(video))
                {
                    video.Favorited = true;
                }
            }
            var pairs = videos.CreateVideoPairs();
            VideoPairs = new ObservableCollection<VideoPairModel>(pairs);
        }
    }
}
