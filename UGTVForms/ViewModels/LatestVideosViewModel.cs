using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UGTVForms.Services;
using UGTVForms.Models;

namespace UGTVForms.ViewModels
{
    public class LatestVideosViewModel : VideosBaseViewModel
    {
        private readonly INetworkController _networkController;

        public LatestVideosViewModel(INetworkController networkController)
        {
            _networkController = networkController;
            IsRefreshEnabled = true;
            videoPairs = new ObservableCollection<VideoPairModel>();            
        }

        public override async Task LoadVideos()
        {
            IsBusy = true;
            videos = await _networkController.FetchVideosAsync();
            var pairs = videos.CreateVideoPairs();
            VideoPairs = new ObservableCollection<VideoPairModel>(pairs);
            IsBusy = false;
        }       
    }
}