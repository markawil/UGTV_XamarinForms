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
            Preferences.Remove(Settings.Favorites_Key);
            BuildDependencies();
            MessagingCenter.Subscribe<NetworkController>(this, Settings.NetworkCallFailedKey, (sender) =>
            {
                DisplayAlert(Settings.MESSAGE_CALL_FAILURE, string.Empty, "OK");
            });
            
            LoadPages();
        }

        IVideosDataStore _videosDataStore;
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
            _videosDataStore = new VideosDataStore();            
            _favoritesDataStore = new FavoritesDataStore();            
            _favVM = new FavoritesViewModel(_favoritesDataStore);
            _downloadsDataStore = new DownloadsDataStore();
            _dlVM = new DownloadsViewModel(_downloadsDataStore);
            _latestVM = new LatestVideosViewModel(_networkController, _videosDataStore, _favoritesDataStore);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<NetworkController>(this, Settings.NetworkCallFailedKey);
        }
    }
}
