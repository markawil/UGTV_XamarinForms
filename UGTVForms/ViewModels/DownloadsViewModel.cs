using System;
using System.Collections.ObjectModel;
using UGTVForms.Models;
using UGTVForms.ViewModels;

namespace UGTVForms.ViewModels
{
    public class DownloadsViewModel : VideosBaseViewModel
    {
        public DownloadsViewModel()
        {
            IsRefreshEnabled = false;
            videoPairs = new ObservableCollection<VideoPairModel>();
        }
    }
}
