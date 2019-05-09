using System;
using System.Collections.Generic;
using UGTVForms.Models;
using UGTVForms.ViewModels;
using Xamarin.Forms;

namespace UGTVForms.Views
{
    public partial class VideoDetailPage : ContentPage
    {
        public VideoDetailPage(VideoModel video)
        {
            InitializeComponent();
            ViewModel = new VideoDetailPageViewModel(video);            
        }

        async void Close_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        
        public VideoDetailPageViewModel ViewModel
        {
            get { return BindingContext as VideoDetailPageViewModel; }
            set { BindingContext = value; }
        }
    }
}
