using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UGTVForms.Models;
using UGTVForms.Services;
using UGTVForms.ViewModels;
using Xamarin.Forms;

namespace UGTVForms.Views
{
    public partial class VideoDetailPage : ContentPage
    {
        string warningDownloadingMessage = "You are currently downloading a video, if you close this window it will be cancelled, continue with closing?";

        public VideoDetailPage(VideoModel video,
                               IVideoDataStore favoritesDataStore,
                               IVideoDataStore downloadsDataStore)
        {
            InitializeComponent();
            var downloadFileController = new DownloadFileController();
            ViewModel = new VideoDetailPageViewModel(video,
                                                     favoritesDataStore,
                                                     downloadsDataStore,
                                                     downloadFileController);
            ViewModel.DownloadFailureMessage += ViewModel_DownloadFailureMessage;
            if (ViewModel.Speaker != null)
            {
                Title = ViewModel.Speaker;
            }
        }

        void ViewModel_DownloadFailureMessage(object sender, string e)
        {
            DisplayAlert("Error", "Download failed: " + e, "OK");
        }

        async void Close_Clicked(object sender, System.EventArgs e)
        {
            if (ViewModel.IsDownloading)
            {
                bool stop = await DisplayAlert("Warning", warningDownloadingMessage, "Stop Download", "Cancel");
                if (stop)
                {
                    ViewModel.CancelDownload();
                    await Close();
                }
            }
            else
            {
                await Close();
            }
        }

        private async Task Close()
        {
            await videoPlayer.Stop();
            await Navigation.PopModalAsync();
        }

        public VideoDetailPageViewModel ViewModel
        {
            get { return BindingContext as VideoDetailPageViewModel; }
            set { BindingContext = value; }
        }
    }
}
