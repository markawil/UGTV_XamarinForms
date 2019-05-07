using System;
using System.Collections.Generic;
using UGTVForms.Models;
using UGTVForms.ViewModels;
using Xamarin.Forms;

namespace UGTVForms.Views
{
    public partial class VideoDetailPage : ContentPage
    {
        private readonly VideoModel videoModel;

        public VideoDetailPage(VideoModel video)
        {
            InitializeComponent();
            this.BackgroundColor = Color.LightSlateGray;
            this.videoModel = video;
            this.Title = video.VideoTitle;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            webview.Source = videoModel.VideoUrlPathLow;
        }

        async void Close_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
