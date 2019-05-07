using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using UGTVForms.Models;

namespace UGTVForms.Services
{
    public static class Extensions
    {
        public static string UGTVDateTimeFormatted(this DateTime dt)
        {
            return dt.ToString("MM/dd/yy");
        }
        
        public static List<VideoPairModel> CreateVideoPairs(this List<VideoModel> videoModels)
        {
            videoModels.Reverse();
            var originalVideoPairs = new List<VideoPairModel>();
            int videoIndxCount = 1;
            foreach (var video in videoModels)
            {
                VideoPairModel videoPair = null;
                if (videoIndxCount == 1) 
                {
                    videoPair = new VideoPairModel();
                    videoPair.Video1 = video;
                    videoIndxCount++;
                    originalVideoPairs.Add(videoPair);
                }
                else
                {
                    videoPair = originalVideoPairs.Last();
                    videoPair.Video2 = video;
                    videoIndxCount = 1;
                }                    
            }                
            
            return originalVideoPairs;
        }
        
        public static List<VideoModel> VideosFromJSONValue(this JsonValue jsonValue)
        {
            var videosArray = jsonValue as JsonArray;                
            var videos = new List<VideoModel>();
            
            foreach (var videoInfo in videosArray)
            {
                var video = new VideoModel(videoInfo);
                videos.Add(video);
            }

            return videos;
        }
    }
}
