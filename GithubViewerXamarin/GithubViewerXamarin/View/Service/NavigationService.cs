using Github.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubViewerXamarin.View.Service
{
    public class NavigationService : INavigationService
    {
        public async Task NavigateToLoginView()
        {
            await App.Current.MainPage.Navigation.PushAsync(new LoginView());
        }

        public async Task NavigateToMainView()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
        }
    }
}
