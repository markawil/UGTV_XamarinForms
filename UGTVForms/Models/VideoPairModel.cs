using System;
using UGTVForms.ViewModels;

namespace UGTVForms.Models
{
    public class VideoPairModel
    {
        public VideoModel Video1 { get; set; }
        public VideoModel Video2 { get; set; }
        
        public bool HasSecondVideo { get => Video2 != null; }
    }
}