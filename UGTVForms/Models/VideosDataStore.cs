using System.Collections.Generic;
using System.Json;
using System.Linq;
using UGTVForms.ViewModels;

namespace UGTVForms.Models
{
    public interface IVideosDataStore : IDataStore<VideoModel>
    {
        void AddItems(IEnumerable<VideoModel> videos);        
    }
    
    public class VideosDataStore : IVideosDataStore
    {        
        List<VideoModel> _videos = new List<VideoModel>();
                        
        public bool AddItem(VideoModel video)
        {
            _videos.Add(video);
            return true;
        }

        public void AddItems(IEnumerable<VideoModel> videos)
        {
            _videos.AddRange(videos);
        }

        public bool DeleteItem(string id)
        {
            return _videos.Remove(_videos.FirstOrDefault(v => v.Id == id));
        }

        public VideoModel GetItem(string id)
        {
            return _videos.FirstOrDefault(v => v.Id == id);
        }

        public IEnumerable<VideoModel> All()
        {
            return _videos;
        }

        public bool UpdateItem(VideoModel video)
        {
            if (_videos.Contains(video))
            {
                int index = _videos.IndexOf(video);
                _videos[index] = video;
                return true;
            }

            return false;
        }
    }
}
