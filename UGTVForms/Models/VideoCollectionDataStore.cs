using System.Collections.Generic;
using System.Json;
using System.Linq;
using UGTVForms.Services;
using UGTVForms.ViewModels;
using Xamarin.Essentials;

namespace UGTVForms.Models
{
    public class VideoCollectionDataStore : IVideoDataStore
    {
        private readonly string PreferencesKey;

        public VideoCollectionDataStore(string prefsKey)
        {
            PreferencesKey = prefsKey;
        }

        public bool AddItem(VideoModel video)
        {
            var allVideos = All();

            if (allVideos.Contains(video))
            {   // video already saved
                return false;
            }

            if (!allVideos.Any()) // no videos have been saved yet
            {
                var jsonValues = new JsonValue[] { video.JsonValue };
                var jsonArray = new JsonArray(jsonValues);
                Preferences.Set(PreferencesKey, jsonArray.ToString());
                return true;
            }

            // otherwise a favorites array of videos already exist in preferences,
            // so just add to it.
            var result = Preferences.Get(PreferencesKey, string.Empty);
            var jsonValue = JsonValue.Parse(result);
            var videosArray = jsonValue as JsonArray;
            videosArray.Add(video.JsonValue);
            Preferences.Set(PreferencesKey, videosArray.ToString());

            return true;
        }

        public virtual bool DeleteItem(string id)
        {
            var result = Preferences.Get(PreferencesKey, string.Empty);

            if (string.IsNullOrEmpty(result))
            {
                // there's no items in favorites or downloads to delete
                return false;
            }
            else
            {
                var jsonValue = JsonValue.Parse(result);
                var videos = jsonValue.VideosFromJson().ToList();
                var success = videos.Remove(videos.FirstOrDefault((v) => v.Id == id));
                if (success)
                {
                    var videosJsonArray = videos.Select(v => v.JsonValue).ToList();
                    var array = new JsonArray(videosJsonArray.ToArray());
                    Preferences.Set(PreferencesKey, array.ToString());
                    return true;
                }
                else
                {   // if the last video was removed, just delete the empty array
                    if (videos.Count == 0)
                    {
                        Preferences.Remove(PreferencesKey);
                        return true;
                    }
                    return false;
                }
            }
        }

        public VideoModel GetItem(string id)
        {
            var result = Preferences.Get(PreferencesKey, string.Empty);

            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            else
            {
                var jsonValue = JsonValue.Parse(result);
                var videos = jsonValue.VideosFromJson().ToList();
                var video = videos.FirstOrDefault(v => v.Id == id);
                
                return video;
            }
        }

        public IEnumerable<VideoModel> All()
        {
            var result = Preferences.Get(PreferencesKey, string.Empty);
            
            if (string.IsNullOrEmpty(result))
            {
                return new List<VideoModel>();
            }

            var jsonValue = JsonValue.Parse(result);
            var videos = jsonValue.VideosFromJson().ToList();
            return videos;
        }
    }
}
