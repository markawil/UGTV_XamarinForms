using System;
using System.Net;
using System.Threading.Tasks;

namespace UGTVForms.Services
{
    public class DownloadFileController : IDownloadFileController
    {
        public DownloadFileController()
        {
            webClient = new WebClient();
        }
        
        public event EventHandler<int> DownloadProgressChanged;
        public event EventHandler<string> DownloadFileCompletedWithPath;
        public event EventHandler<string> DownloadFailureMessage;

        private string lastDownloadedFilePath = string.Empty;
        private WebClient webClient;
        
        public void DownloadVideo(string videoUrl)
        {
            try
            {
                var uri = new Uri(videoUrl);
                using (webClient)
                {
                    webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                    webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
                    string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    lastDownloadedFilePath = filePath + getFilename(videoUrl);
                    webClient.DownloadFileAsync(uri, lastDownloadedFilePath);
                }
            }
            catch (Exception ex)
            {
                DownloadFailureMessage?.Invoke(this, ex.Message);
            }
        }

        void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                DownloadFailureMessage?.Invoke(this, "Download was cancelled");
                return;
            }

            if (e.Error != null) // We have an error! Retry a few times, then abort.
            {
                DownloadFailureMessage?.Invoke(this, "An error ocurred while trying to download file");
                return;
            }

            DownloadFileCompletedWithPath?.Invoke(this, this.lastDownloadedFilePath);
        }

        void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgressChanged?.Invoke(this, e.ProgressPercentage);
        }
        
        public void CancelCurrentDownload()
        {
            webClient.CancelAsync();
        }

        private string getFilename(string hreflink)
        {
            Uri uri = new Uri(hreflink);
            string filename = System.IO.Path.GetFileName(uri.LocalPath);
            return filename;
        }
    }
    
    public interface IDownloadFileController
    {
        void DownloadVideo(string videoUrl);
        void CancelCurrentDownload();
        event EventHandler<int> DownloadProgressChanged;
        event EventHandler<string> DownloadFileCompletedWithPath;
        event EventHandler<string> DownloadFailureMessage;
    }
}
