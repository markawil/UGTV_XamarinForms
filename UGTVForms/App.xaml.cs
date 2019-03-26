using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using UGTVForms.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace UGTVForms
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            var navPage = new VideosPage();
            MainPage = navPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
