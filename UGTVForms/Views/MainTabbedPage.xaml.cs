using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGTVForms.Services;
using UGTVForms.ViewModels;
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
            LoadPages();
        }

        LatestVideosViewModel _latestVM = new LatestVideosViewModel(new NetworkController());
        FavoritesViewModel _favVM = new FavoritesViewModel();
        DownloadsViewModel _dlVM = new DownloadsViewModel();
        
        void LoadPages()
        {
            var videosPage = new VideosPage(VideosPageType.LatestVideos, _latestVM);
            videosPage.Icon = "11-clock";
            videosPage.Title = "Latest";

            var favoritesPage = new VideosPage(VideosPageType.Favorites, _favVM);
            favoritesPage.Icon = "star_black";
            favoritesPage.Title = "Favorites";

            var downloadsPage = new VideosPage(VideosPageType.Downloaded, _dlVM);
            downloadsPage.Icon = "download";
            downloadsPage.Title = "Downloads";
            
            Children.Add(videosPage);
            Children.Add(favoritesPage);
            Children.Add(downloadsPage);
            
        }
    }
}
