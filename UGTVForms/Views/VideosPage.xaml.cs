using UGTVForms.ViewModels;
using UGTVForms.Services;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace UGTVForms.Views
{
    public partial class VideosPage : ContentPage
    {
        public VideosPage()
        {
            ViewModel = new VideoListViewModel(new NetworkController());

            InitializeComponent();
            listView.BackgroundColor = Color.LightSlateGray;            
        }

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
            Task.Run(async () => await ViewModel.LoadVideos());
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
            var videoVM = button.BindingContext as VideoViewModel;
            if (videoVM != null)
            {
                NavigationPage videoPage = new NavigationPage(new VideoDetailPage(videoVM));                
                await Navigation.PushModalAsync(videoPage);
            }
        }

        public VideoListViewModel ViewModel
        {
            get { return BindingContext as VideoListViewModel; }
            set { BindingContext = value; }
        }
    }
}
