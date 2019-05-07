using System;
using System.Json;
using UGTVForms.Services;

namespace UGTVForms.Models
{
    public class VideoModel
    {
        public VideoModel(JsonValue jsonValue)
        {
            _jsonValue = jsonValue;
        }

        private JsonValue _jsonValue;
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
    }
}
