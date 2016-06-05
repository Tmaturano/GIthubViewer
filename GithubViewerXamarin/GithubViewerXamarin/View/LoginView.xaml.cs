using Github.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GithubViewerXamarin.View
{
    public partial class LoginView : ContentPage
    {
        LoginViewModel _loginViewModel;

        public LoginView()
        {
            InitializeComponent();

            _loginViewModel = new LoginViewModel();
            BindingContext = _loginViewModel;            
        }
    }
}
