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

        public DownloadsViewModel(IVideoDataStore downloadsDataStore)
        {
            IsRefreshEnabled = false;
            videoPairs = new ObservableCollection<VideoPairModel>();
            this.downloadsDataStore = downloadsDataStore;
        }

        public override void LoadVideos()
        {
            videos = downloadsDataStore.All().ToList();
            var pairs = videos.CreateVideoPairs();
            VideoPairs = new ObservableCollection<VideoPairModel>(pairs);
        }
    }
}
