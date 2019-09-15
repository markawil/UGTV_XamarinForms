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
            
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                DisplayAlert("No Connection Available", "", "OK");
                return;
            }
            
            // if no videos were previously loaded for this page, load them, 
            // or if this is the favorites or downloads page (whos collections can be changed
            // frequently) then reload them every time the page appears.
            // this prevents loading the videos again every time the page appears.
            if (ViewModel.VideoPairs.Count == 0 || PageType != VideosPageType.LatestVideos)
            {
                Load();
            }
        }
        
        private void Load()
        {
            switch (PageType)
            {
                case VideosPageType.LatestVideos:                    
                    Task.Run(async () => await ViewModel.LoadVideosAsync());
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
            Load();
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
