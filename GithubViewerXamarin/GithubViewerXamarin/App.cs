using Github.Interfaces;
using GithubViewerXamarin.View;
using GithubViewerXamarin.View.Service;
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
            DependencyService.Register<IMessageService, MessageService>();
            DependencyService.Register<INavigationService, NavigationService>();

            //MainPage = new MainPage();
            MainPage = new NavigationPage(new LoginView());
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
