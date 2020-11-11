using UGTVForms.ViewModels;
using UGTVForms.Services;
using Xamarin.Forms;
using System.Threading.Tasks;
using UGTVForms.Models;
using Xamarin.Essentials;

namespace UGTVForms.Views
{
    public enum VideosPageType
    {
        LatestVideos,
        Favorites,
        Downloaded
    }
    
    public partial class VideosPage : ContentPage
    {
        private readonly IVideoDataStore favoritesDataStore;
        private readonly IVideoDataStore downloadsDataStore;

        public VideosPage(VideosPageType pageType,
                          VideosBaseViewModel vm,
                          IVideoDataStore favoritesDataStore,
                          IVideoDataStore downloadsDataStore)
        {
            PageType = pageType;
            ViewModel = vm;
            this.favoritesDataStore = favoritesDataStore;
            this.downloadsDataStore = downloadsDataStore;
            InitializeComponent();
        }

        public VideosPageType PageType { get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if (PageType == VideosPageType.LatestVideos && Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                DisplayAlert("No Connection Available", "", "OK");
                return;
            }

            MessagingCenter.Subscribe<NetworkController>(this, Settings.NetworkCallFailedKey, (sender) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert(Settings.MESSAGE_CALL_FAILURE, string.Empty, "OK");
                    var vm_ = ViewModel as LatestVideosViewModel;
                    if (vm_ != null) { vm_.IsBusy = false; }
                });
            });

            Load();
        }

        private void Load()
        {
            switch (PageType)
            {
                case VideosPageType.LatestVideos:
                    // if no videos have been downloaded yet
                    if (ViewModel.VideoPairs.Count == 0)
                    {
                        Task.Run(async () => await ViewModel.LoadVideosAsync());
                    }
                    else
                    {
                        ViewModel.LoadVideos();
                    }
                    break;
                default:
                    ViewModel.LoadVideos();
                    break;
            }            
        }

        void Handle_SearchClicked(object sender, System.EventArgs e)
        {
            ViewModel.ShowSearchBar = !ViewModel.ShowSearchBar;
            
            if (!ViewModel.ShowSearchBar)
            {
                ViewModel.SearchCanceled();
            }   
        }

        void Handle_SearchButtonPressed(object sender, System.EventArgs e)
        {
            ViewModel.SearchVideos(searchbar.Text);
        }

        void Handle_Refreshing(object sender, System.EventArgs e)
        {
            if (PageType == VideosPageType.LatestVideos)
            {
                Task.Run(async () => await ViewModel.LoadVideosAsync());
            }
        }        

        async void Handle_VideoButtonClicked(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var videoVM = button.BindingContext as VideoModel;
            if (videoVM != null)
            {
                NavigationPage videoPage = new NavigationPage(new VideoDetailPage(videoVM, favoritesDataStore, downloadsDataStore));                
                await Navigation.PushModalAsync(videoPage);
            }
        }

        public VideosBaseViewModel ViewModel
        {
            get { return BindingContext as VideosBaseViewModel; }
            set { BindingContext = value; }
        }
    }
}
