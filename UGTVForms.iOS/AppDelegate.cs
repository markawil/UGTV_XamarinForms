using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using StoreKit;
using Xamarin.Essentials;

namespace UGTVForms.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            Xamarin.Forms.FormsMaterial.Init();
            Rox.VideoIos.Init();
            LoadApplication(new App());
            IncrementAppCount();
            return base.FinishedLaunching(app, options);
        }

        private void IncrementAppCount()
        {
            string key = "ios.app.count";

            var count = Preferences.Get(key, 0);
            count += 1;
            Preferences.Set(key, count);
            switch (count)
            {
                case 1:
                case 3:
                case 10:
                    SKStoreReviewController.RequestReview();
                    break;
                default:
                    break;
            }
        }
    }
}
