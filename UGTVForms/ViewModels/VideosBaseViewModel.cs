using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using UGTVForms.Models;
using UGTVForms.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace UGTVForms.ViewModels
{
    public class VideosBaseViewModel : INotifyPropertyChanged
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
        
        // Should be overridden.
        public virtual async Task LoadVideos()
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
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadVideos()
        {
            IsBusy = true;
            videos = await _networkController.FetchVideosAsync();
            var pairs = videos.CreateVideoPairs();
            VideoPairs = new ObservableCollection<VideoPairModel>(pairs);
            IsBusy = false;
        }       
        #endregion
    }
}
