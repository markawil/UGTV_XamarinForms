﻿using System;
using System.IO;
using System.Windows.Input;
using UGTVForms.Models;
using UGTVForms.Services;
using Xamarin.Essentials;
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

        public string VideoImg { get => video.ImageURLPath; }
        public string VideoURLPath
        {
            get
            {
                if (string.IsNullOrEmpty(video.DownloadedFilePath))
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

        public VideoDetailPageViewModel(VideoModel video,
                                        IVideoDataStore favoritesDataStore,
                                        IVideoDataStore downloadsDataStore,
                                        IDownloadFileController downloadFileController)
        {
            FavoriteCommand = new Command(FavoriteVideo);
            DownloadCommand = new Command(DownloadOrDeleteVideo);

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
            //System.Diagnostics.Debug.WriteLine(e);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Code to run on the main thread
                double p = e / 100.0;
                Progress = p;
            });
        }

        void DownloadFileController_DownloadFileCompletedWithPath(object sender, string e)
        {
            DownloadButtonText = "Remove Download";
            video.DownloadedFilePath = e;
            downloadsDataStore.AddItem(video);
            IsDownloading = false;
        }

        void DownloadFileController_DownloadFailureMessage(object sender, string e)
        {
            DownloadFailureMessage?.Invoke(this, e);
            IsDownloading = false;
        }

        public string VideoTitle { get => video.VideoTitle; }

        private void DownloadOrDeleteVideo(object obj)
        {
            if (video.Downloaded)
            {
                File.Delete(video.DownloadedFilePath);
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

        public void CancelDownload()
        {
            downloadFileController.CancelCurrentDownload();
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