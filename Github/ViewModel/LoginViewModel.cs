using Github.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        #endregion

        #region Commands
        public ICommand LoginComamnd{ get; set; }

        #endregion

        #region Constructor
        public LoginViewModel()
        {
            IsSearching = false;

            _messageService = DependencyService.Get<IMessageService>();
            _navigationService = DependencyService.Get<INavigationService>();
            LoginComamnd = new Command(async () => await Login());
        }

        #endregion

        #region Methods/Functions
        public async Task Login()
        {
            IsSearching = true;

            try
            {
                if (LoginName.ToLower() == "adm" && Password == "123")
                {
                    //TODO
                    //consume the gihub api to log in...
                    await _navigationService.NavigateToMainView();
                }
                else
                {
                    await _messageService.ShowAsync("Alert", "Incorrect username or password.");
                }
            }
            catch (Exception ex)
            {
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

        #endregion




    }
}
