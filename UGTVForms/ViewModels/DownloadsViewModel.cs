using System;
using System.Collections.ObjectModel;
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
            // Load videos that are saved to disk (if any)
        }
    }
}
