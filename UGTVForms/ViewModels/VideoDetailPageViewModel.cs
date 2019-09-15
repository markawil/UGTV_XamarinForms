using System;
using System.Windows.Input;
using UGTVForms.Models;
using UGTVForms.Services;
using Xamarin.Forms;

namespace UGTVForms.ViewModels
{
    public class VideoDetailPageViewModel : BaseViewModel
    {
        private readonly VideoModel video;
        private readonly IVideoDataStore favoritesDataStore;
        private readonly IVideoDataStore downloadsDataStore;
        private readonly IDownloadFileController downloadFileController;

        public ICommand FavoriteCommand { get; set; }
        public ICommand DownloadCommand { get; set; }

        // props to the user on SO for showing how to disable toolbar items in Xam.forms:
        // https://stackoverflow.com/questions/27803038/disable-toolbaritem-xamarin-forms

        public string VideoImg { get => video.ImageURLPath; }
        public string VideoURLPath
        {
            get
            {
                if (video.Downloaded == false)
                {
                    return video.VideoUrlPathLow;
                }
                else
                {
                    return video.DownloadedFilePath;
                }
            }
        }

        public event EventHandler<string> DownloadFailureMessage;

        public
            VideoDetailPageViewModel(VideoModel video,
                                        IVideoDataStore favoritesDataStore,
                                        IVideoDataStore downloadsDataStore,
                                        IDownloadFileController downloadFileController)
        {
            FavoriteCommand = new Command(FavoriteVideo);
            DownloadCommand = new Command(DownloadVideo);

            this.video = video;
            this.favoritesDataStore = favoritesDataStore;
            this.downloadsDataStore = downloadsDataStore;
            this.downloadFileController = downloadFileController;
            this.downloadFileController.DownloadFailureMessage += DownloadFileController_DownloadFailureMessage;
            this.downloadFileController.DownloadFileCompletedWithPath += DownloadFileController_DownloadFileCompletedWithPath;
            this.downloadFileController.DownloadProgressChanged += DownloadFileController_DownloadProgressChanged;
            
            if (video.Favorited)
            {
                FavoriteButtonText = "Unfavorite";                
            }

            if (video.Downloaded)
            {
                DownloadButtonText = "Remove Download";
            }
        }

        void DownloadFileController_DownloadProgressChanged(object sender, int e)
        {
            Progress = e / 10;
        }

        void DownloadFileController_DownloadFileCompletedWithPath(object sender, string e)
        {
            DownloadButtonText = "Remove Download";
            video.DownloadedFilePath = e;
            video.Downloaded = true;
            downloadsDataStore.AddItem(video);
            IsDownloading = false;
        }

        void DownloadFileController_DownloadFailureMessage(object sender, string e)
        {
            DownloadFailureMessage?.Invoke(this, e);
            IsDownloading = false;
        }

        public string VideoTitle { get => video.VideoTitle; }

        private void DownloadVideo(object obj)
        {
            if (video.Downloaded)
            {
                video.Downloaded = false;
                downloadsDataStore.DeleteItem(video.Id);
                DownloadButtonText = "Download"; 
            }
            else
            {
                Progress = 0;
                IsDownloading = true;
                downloadFileController.DownloadVideo(VideoURLPath);
                DownloadButtonText = "Downloading...";
            }
        }

        public void FavoriteVideo(object obj)
        {
            if (video.Favorited)
            {
                video.Favorited = false;
                favoritesDataStore.DeleteItem(video.Id);
                FavoriteButtonText = "Favorite";
            }
            else
            {
                video.Favorited = true;
                favoritesDataStore.AddItem(video);
                FavoriteButtonText = "Unfavorite";
            }
        }

        private string favoriteButtonText = "Favorite";
        public string FavoriteButtonText
        {
            get => favoriteButtonText;
            set
            {
                favoriteButtonText = value;
                OnPropertyChanged(nameof(FavoriteButtonText));
            }
        }

        private string downloadButtonText = "Download";
        public string DownloadButtonText
        {
            get => downloadButtonText;
            set
            {
                downloadButtonText = value;
                OnPropertyChanged(nameof(DownloadButtonText));
            }
        }

        private bool isDownloading;
        public bool IsDownloading
        {
            get => isDownloading;
            set
            {
                isDownloading = value;
                OnPropertyChanged(nameof(IsDownloading));
            }
        }

        private double progress;
        public double Progress
        {
            get => progress;
            set
            {
                progress = value;
                OnPropertyChanged(nameof(Progress)); 
            }
        }        
    }
}