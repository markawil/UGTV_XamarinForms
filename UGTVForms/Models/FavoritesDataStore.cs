using UGTVForms.Services;
using UGTVForms.ViewModels;

namespace UGTVForms.Models
{
    public interface IVideoDataStore : IDataStore<VideoModel>
    { }
    
    public class FavoritesDataStore : VideoCollectionDataStore
    {
        public FavoritesDataStore() : base(Settings.Favorites_Key)
        {

        }
    }
}
