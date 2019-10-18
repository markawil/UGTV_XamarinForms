using System;
using System.Collections.Generic;
using System.Linq;
using UGTVForms.Services;
using UGTVForms.ViewModels;
using Xamarin.Essentials;

namespace UGTVForms.Models
{
    /**
     * Favorites and Downloads data stores share a common base that
     * store the VideoModels as JSON in the preferences, with the only
     * difference being a VideoMoel has a file path filled in for
     * the path to the downloaded video, the VideoDetailPageViewModel
     * handles downloading and deleting videos from disk.
     */
    public class DownloadsDataStore : VideoCollectionDataStore
    {
        public DownloadsDataStore() : base(Settings.Downloads_Key)
        {

        }
    }
}
