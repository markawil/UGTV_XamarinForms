using UGTVForms.ViewModels;
using UGTVForms.Services;
using Xamarin.Forms;
using System.Threading.Tasks;
using UGTVForms.Models;

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
        public VideosPage(VideosPageType pageType, VideosBaseViewModel vm)
        {
            PageType = pageType;
            ViewModel = vm;        

            InitializeComponent();
        }

        public VideosPageType PageType { get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel.VideoPairs.Count == 0)
            {
                Load();
            }
        }
        
        private void Load()
        {
            switch (PageType)
            {
                case VideosPageType.LatestVideos:
                    ViewModel = new LatestVideosViewModel(new NetworkController());
                    var vm = ViewModel as LatestVideosViewModel;
                    Task.Run(async () => await vm.LoadVideos());
                    break;
                case VideosPageType.Favorites:
                    ViewModel = new FavoritesViewModel();
                    
                    break;
                case VideosPageType.Downloaded:
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
                NavigationPage videoPage = new NavigationPage(new VideoDetailPage(videoVM));                
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
