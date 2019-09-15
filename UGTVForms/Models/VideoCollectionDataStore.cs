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
            video.Favorited = true;
            var favoritedVideos = All();

            if (favoritedVideos.Contains(video))
            {
                return false;
            }

            if (!favoritedVideos.Any()) // no videos have been saved yet
            {
                var jsonValues = new JsonValue[] { video.JsonValue };
                var jsonArray = new JsonArray(jsonValues);
                Preferences.Set(PreferencesKey, jsonArray.ToString());
                return true;
            }

            var result = Preferences.Get(PreferencesKey, string.Empty);
            var jsonValue = JsonValue.Parse(result);
            var videosArray = jsonValue as JsonArray;
            videosArray.Add(video.JsonValue);
            Preferences.Set(PreferencesKey, videosArray.ToString());

            return true;
        }

        public bool DeleteItem(string id)
        {
            var result = Preferences.Get(PreferencesKey, string.Empty);

            if (string.IsNullOrEmpty(result))
            {
                // there's no items in favorites to delete
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
                {
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
                if (video != null)
                    video.Favorited = true;

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
            videos.ForEach(v => v.Favorited = true);
            return videos;
        }
    }
}
