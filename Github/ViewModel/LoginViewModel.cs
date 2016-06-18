using Github.Api;
using Github.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Github.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        #region Private Variables
        private readonly IMessageService _messageService;
        private readonly INavigationService _navigationService;

        #endregion

        #region Properties
        public ImageSource GitLogo
        {
            get
            {
                return Device.OnPlatform(
                                iOS: ImageSource.FromFile("Resources/drawable/gitviewer_logo.jpg"),
                                Android: ImageSource.FromFile("Resources/gitviewer_logo.jpg"),
                                WinPhone: ImageSource.FromFile("Assets/gitviewer_logo.jpg"));
            }
        }

        private string _loginName;

        public string LoginName
        {
            get { return _loginName; }
            set
            {
                _loginName = value;
                Notify(nameof(LoginName));
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                Notify(nameof(Password));
            }
        }

        private bool _isSearching;

        public bool IsSearching
        {
            get { return _isSearching; }
            set
            {
                _isSearching = value;
                Notify(nameof(IsSearching));
            }
        }

        private bool _loginFailureVisibility;

        public bool LoginFailureVisibility
        {
            get { return _loginFailureVisibility; }
            set
            {
                _loginFailureVisibility = value;
                Notify(nameof(LoginFailureVisibility));
            }
        }

        #endregion

        #region Commands
        public ICommand LoginComamnd { get; set; }

        #endregion

        #region Constructor
        public LoginViewModel()
        {
            IsSearching = false;
            LoginFailureVisibility = false;

            _messageService = DependencyService.Get<IMessageService>();
            _navigationService = DependencyService.Get<INavigationService>();
            LoginComamnd = new Command(async () => await Login());

            CheckApplicationKeys();
        }

        #endregion

        #region Methods/Functions
        public async Task Login()
        {
            IsSearching = true;

            //Review this try/catch block, the exception need to be adjusted for github code errors
            try
            {
                //Improve this logging validation for MVVM purposes
                if (string.IsNullOrWhiteSpace(LoginName) || string.IsNullOrWhiteSpace(Password))
                {
                    await _messageService.ShowAsync("Alert", "Login name and password are required.");
                }
                else
                {
                    //https://forums.xamarin.com/discussion/50036/how-to-store-user-session-to-stay-logged-in
                    //https://forums.xamarin.com/discussion/8199/how-to-save-user-settings
                    //https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Settings
                                      
                    var statusCode = await GithubApi.Login(this);

                    if (statusCode == HttpStatusCode.OK)
                    {                             
                        await _navigationService.NavigateToMainView();
                    }
                    else
                    {
                        LoginFailureVisibility = true;
                    }                        
                }
            }
            catch (Exception ex)
            {
                //www.adventuresinxamarinforms.com/tag/connectivity
                //www.codeproject.com/Tips/870548/Xamarin-forms-Check-network-connectivity-in-IOS-an
                //create a static method to check if there's internet connection
                string errorMessage = string.Empty;
                if (ex.Message.Contains("404"))
                {
                    errorMessage = "User not found!";
                }
                else
                    errorMessage = ex.Message;

                await _messageService.ShowAsync("Alert", errorMessage);
            }

            IsSearching = false;
        }

        private void CheckApplicationKeys()
        {
            if (!Application.Current.Properties.ContainsKey("LoggedUser"))
            {
                Application.Current.Properties.Add("LoggedUser", string.Empty);
            }
        }

        #endregion
    }
}
