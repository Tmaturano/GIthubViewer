using Github.Interfaces;
using GithubViewerXamarin.View;
using GithubViewerXamarin.View.Service;

using Xamarin.Forms;

namespace GithubViewerXamarin
{
    public class App : Application
    {
        public static bool IsUserLoggedIn
        {
            get
            {
                return Current.Properties.ContainsKey("LoggedUser") &&
                    string.IsNullOrWhiteSpace(Current.Properties["LoggedUser"] as string);
            }
        }

        public App()
        {            
            DependencyService.Register<IMessageService, MessageService>();
            DependencyService.Register<INavigationService, NavigationService>();

            if (IsUserLoggedIn)
            {
                MainPage = new NavigationPage(new MainPage());
            }
            else
            {                
                MainPage = new NavigationPage(new LoginView());
            }
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
