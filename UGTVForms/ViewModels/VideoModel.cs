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
            get { return _jsonValue["Id"].ToString(); }
        }
        
        
        public VideoModel(JsonValue jsonValue)
        {
            _jsonValue = jsonValue;
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

        private bool downloaded;
        public bool Downloaded
        {
            get => downloaded;
            set
            {
                downloaded = value;
                OnPropertyChanged(nameof(Downloaded));
            }
        }  
        
        public string DownloadedFilePath { get; set; }   

        private readonly JsonValue _jsonValue;
        public JsonValue JsonValue
        {
            get { return _jsonValue; }
        }

        public string VideoTitle
        {
            get { return _jsonValue["Title"]; }
        }

        public string Author
        {
            get
            {
                var speakers = _jsonValue["Speakers"] as JsonArray;
                if (speakers != null && speakers.Count > 0)
                {
                    var name = speakers[0]["Name"];
                    return name;
                }

                return String.Empty;
            }
        }

        public DateTime VideoDateTime
        {
            get
            {
                try
                {
                    var dateTimeString = _jsonValue["PostDate"].ToString().Trim('"');
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
            get { return _jsonValue["Mp4Video"]; }
        }

        public string VideoUrlPathLow
        {
            get { return _jsonValue["Mp4VideoLow"]; }
        }

        public string Summary
        {
            get { return _jsonValue["Exceprt"]; }
        }

        public string ImageURLPath
        {
            get { return _jsonValue["Thumbnail"]; }
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<string>.Default.GetHashCode(Id);
        }
    }
}
