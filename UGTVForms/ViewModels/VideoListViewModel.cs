using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UGTVForms.Services;
using System.Linq;
using System.Collections.Generic;

namespace UGTVForms.ViewModels
{
    public class VideoListViewModel : BaseViewModel
    {
        private readonly INetworkController _networkController;

        public VideoListViewModel(INetworkController networkController)
        {
            _networkController = networkController;
            videoPairs = new ObservableCollection<VideoPairViewModel>();            
        }

        public async Task LoadVideos()
        {
            IsBusy = true;
            videos = await _networkController.FetchVideosAsync();
            CreateVideoPairs(videos);
            IsBusy = false;
        }
        
        private void CreateVideoPairs(List<VideoViewModel> videoViewModels)
        {
            videoViewModels.Reverse();
            originalVideoPairs = new List<VideoPairViewModel>();
            int videoIndxCount = 1;
            foreach (var video in videoViewModels)
            {
                VideoPairViewModel videoPair = null;
                if (videoIndxCount == 1) 
                {
                    videoPair = new VideoPairViewModel();
                    videoPair.Video1 = video;
                    videoIndxCount++;
                    originalVideoPairs.Add(videoPair);
                }
                else
                {
                    videoPair = originalVideoPairs.Last();
                    videoPair.Video2 = video;
                    videoIndxCount = 1;
                }                    
            }                
            
            VideoPairs = new ObservableCollection<VideoPairViewModel>(originalVideoPairs);
        }

        public void SearchVideos(string text)
        {
            var results = from video in videos
                          where video.VideoTitle.Contains(text)
                          select video;

            CreateVideoPairs(results.ToList());
        }
        
        public void SearchCanceled()
        {
            videos.Reverse();
            CreateVideoPairs(videos);
        }

        string titleText = "UserGroup.TV";
        public string TitleText
        {
            get
            {
                return titleText;
            }

            set
            {
                titleText = value;
            }
        }

        private List<VideoPairViewModel> originalVideoPairs;
        private List<VideoViewModel> videos;

        ObservableCollection<VideoPairViewModel> videoPairs;
        public ObservableCollection<VideoPairViewModel> VideoPairs
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
    }
}