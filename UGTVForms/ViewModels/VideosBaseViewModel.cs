using System.Collections.Generic;
using UGTVForms.Models;
using UGTVForms.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace UGTVForms.ViewModels
{
    public abstract class VideosBaseViewModel : BaseViewModel
    {
        public void SearchVideos(string text)
        {
            var results = from video in videos
                          where video.VideoTitle
                                     .ToLower()
                                     .Contains(text.ToLower())
                          select video;

            var pairs = results.ToList().CreateVideoPairs();
            VideoPairs = new ObservableCollection<VideoPairModel>(pairs);
        }        
        
        public void SearchCanceled()
        {
            if (videos != null && videos.Count > 0)
            {
                videos.Reverse();
                var pairs = videos.CreateVideoPairs();
                VideoPairs = new ObservableCollection<VideoPairModel>(pairs);
            }
        }
        
        // should be overridden.
        public virtual void LoadVideos()
        {
            VideoPairs = new ObservableCollection<VideoPairModel>();
        }
        
        // Should be overridden.
        public virtual async Task LoadVideosAsync()
        {
            await Task.Run(() => VideoPairs = new ObservableCollection<VideoPairModel>());
        }
                
        protected List<VideoModel> videos;

        protected ObservableCollection<VideoPairModel> videoPairs;
        public ObservableCollection<VideoPairModel> VideoPairs
        {
            get
            {
                return videoPairs;
            }

            set
            {
                videoPairs = value;
                OnPropertyChanged("VideoPairs");
            }
        }

        private bool _showSearchBar;
        public bool ShowSearchBar
        {
            get
            {
                return _showSearchBar;
            }
            set
            {
                _showSearchBar = value;
                OnPropertyChanged("ShowSearchBar");
            }             
        }

        public bool IsRefreshEnabled { get; set; }
    }
    
}
