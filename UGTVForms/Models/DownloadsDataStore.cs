using System;
using System.Collections.Generic;
using System.Linq;
using UGTVForms.Services;
using UGTVForms.ViewModels;
using Xamarin.Essentials;

namespace UGTVForms.Models
{ 
    public class DownloadsDataStore : VideoCollectionDataStore
    {
        public DownloadsDataStore() : base(Settings.Downloads_Key)
        {

        }
    }
}
