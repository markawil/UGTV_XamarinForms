using UGTVForms.Models;
using UGTVForms.Services;
using UGTVForms.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UGTVForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();
            BuildDependencies();
            MessagingCenter.Subscribe<NetworkController>(this, Settings.NetworkCallFailedKey, (sender) =>
            {
                DisplayAlert(Settings.MESSAGE_CALL_FAILURE, string.Empty, "OK");
            });
            
            LoadPages();
        }

        IVideoDataStore _favoritesDataStore;
        IVideoDataStore _downloadsDataStore;
        
        LatestVideosViewModel _latestVM;
        FavoritesViewModel _favVM;
        DownloadsViewModel _dlVM;

        INetworkController _networkController;

        void LoadPages()
        {
            var videosPage = new VideosPage(VideosPageType.LatestVideos,
                                            _latestVM,
                                            _favoritesDataStore,
                                            _downloadsDataStore)
            {
                IconImageSource = "11-clock",
                Title = "Latest"
            };

            var favoritesPage = new VideosPage(VideosPageType.Favorites,
                                               _favVM,
                                               _favoritesDataStore,
                                               _downloadsDataStore)
            {
                IconImageSource = "star_black",
                Title = "Favorites"
            };

            var downloadsPage = new VideosPage(VideosPageType.Downloaded,
                                               _dlVM,
                                               _favoritesDataStore,
                                               _downloadsDataStore)
            {
                IconImageSource = "download",
                Title = "Downloads"
            };

            Children.Add(videosPage);
            Children.Add(favoritesPage);
            Children.Add(downloadsPage);            
        }
        
        void BuildDependencies()
        {
            _networkController = new NetworkController();
            _favoritesDataStore = new FavoritesDataStore();
            _downloadsDataStore = new DownloadsDataStore();
            _favVM = new FavoritesViewModel(_favoritesDataStore, _downloadsDataStore);            
            _dlVM = new DownloadsViewModel(_downloadsDataStore, _favoritesDataStore);
            _latestVM = new LatestVideosViewModel(_networkController, _favoritesDataStore, _downloadsDataStore);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<NetworkController>(this, Settings.NetworkCallFailedKey);
        }
    }
}
