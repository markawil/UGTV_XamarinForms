using System;
using System.Collections.Generic;
using System.Json;
using UGTVForms.Services;

namespace UGTVForms.ViewModels
{
    public class VideoModel : BaseViewModel
    {
        public override bool Equals(object obj)
        {
            return Id == ((VideoModel)obj).Id;
        }

        public string Id
        {
            get { return JsonValue["Id"].ToString(); }
        }

        public VideoModel(JsonValue jsonValue)
        {
            JsonValue = jsonValue;
            Downloaded = !string.IsNullOrEmpty(DownloadedFilePath);
        }

        private bool favorited;
        public bool Favorited
        {
            get => favorited;
            set
            {
                favorited = value;
                OnPropertyChanged(nameof(Favorited));
            }
        }

        private bool _downloaded;
        public bool Downloaded
        {
            get
            {
                return _downloaded;
            }
            set
            {
                _downloaded = value;
                OnPropertyChanged(nameof(Downloaded));
            }
        }

        static readonly string DownloadFilePath_Key = "DownloadFilePath";
        private string _downloadFilePath;
        public string DownloadedFilePath
        {
            get
            {
                if (_downloadFilePath != null)
                {
                    return _downloadFilePath;
                }
                else if (JsonValue.ContainsKey(DownloadFilePath_Key))
                {
                    _downloadFilePath = JsonValue[DownloadFilePath_Key];
                }
                return _downloadFilePath;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Downloaded = false;
                }
                else
                {
                    Downloaded = true;
                }
                JsonValue[DownloadFilePath_Key] = value;
                _downloadFilePath = value;
            }
        }

        public JsonValue JsonValue { get; }

        public string VideoTitle
        {
            get { return JsonValue["Title"]; }
        }

        public string Author
        {
            get
            {
                var speakers = JsonValue["Speakers"] as JsonArray;
                if (speakers != null && speakers.Count > 0)
                {
                    var name = speakers[0]["Name"];
                    return name;
                }

                return string.Empty;
            }
        }

        public DateTime VideoDateTime
        {
            get
            {
                try
                {
                    var dateTimeString = JsonValue["PostDate"].ToString().Trim('"');
                    DateTime dateTimeValue =
                    DateTime.ParseExact
                            (dateTimeString, "yyyy-MM-ddTHH:mm:ss",
                             null);
                    return dateTimeValue;
                }
                catch
                {
                    return DateTime.Now;
                }
            }
        }

        public string VideoDateTimeFormatted
        {
            get
            {
                return VideoDateTime.UGTVDateTimeFormatted();
            }
        }

        public string VideoUrlPathHi
        {
            get { return JsonValue["Mp4Video"]; }
        }

        public string VideoUrlPathLow
        {
            get { return JsonValue["Mp4VideoLow"]; }
        }

        public string Summary
        {
            get { return JsonValue["Exceprt"]; }
        }

        public string ImageURLPath
        {
            get { return JsonValue["Thumbnail"]; }
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<string>.Default.GetHashCode(Id);
        }
    }
}
