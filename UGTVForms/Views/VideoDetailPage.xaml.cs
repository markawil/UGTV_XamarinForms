using System;
using System.Collections.Generic;
using UGTVForms.ViewModels;
using Xamarin.Forms;

namespace UGTVForms.Views
{
    public partial class VideoDetailPage : ContentPage
    {
        private readonly VideoViewModel viewModel;

        public VideoDetailPage(VideoViewModel viewModel)
        {
            InitializeComponent();
            this.BackgroundColor = Color.LightSlateGray;
            this.viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            webview.Source = viewModel.VideoUrlPathLow;
        }

        async void Close_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
