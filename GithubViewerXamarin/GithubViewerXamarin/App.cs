using Github.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace GithubViewerXamarin
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application

            DependencyService.Register<IMessageService, MessageService>();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            //Can Load Data
            //Can Cache data
            //when the app is launched  
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            //when the user switches to another app.
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
