using UGTVForms.Services;
using UGTVForms.ViewModels;

namespace UGTVForms.Models
{
    public interface IVideoDataStore : IDataStore<VideoModel>
    { }

    /**
     * Favorites and Downloads data stores share a common base that
     * store the VideoModels as JSON in the preferences, with the only
     * difference being DownloadsDatastore has a file path filled in for
     * the path to the downloaded video
     */
    public class FavoritesDataStore : VideoCollectionDataStore
    {
        public FavoritesDataStore() : base(Settings.Favorites_Key)
        {

        }
    }
}
