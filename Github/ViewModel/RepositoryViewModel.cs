using Github.Api;
using Github.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;


namespace Github.ViewModel
{
    public class RepositoryViewModel : INotifyPropertyChanged
    {

        #region Commands and Events
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GetRepositoryCommand { get; set; }

        #endregion


        #region Properties

        private string _user;

        public string User
        {
            get { return _user; }
            set
            {
                _user = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(User)));
            }
        }

        private ObservableCollection<Repository> _repositories;

        public ObservableCollection<Repository> Repositories
        {
            get { return _repositories; }
            set
            {
                _repositories = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Repositories)));
            }
        }

        private bool isSearching;

        public bool IsSearching
        {
            get { return isSearching; }
            set
            {
                isSearching = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSearching)));
            }
        }

        private string _search;

        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Search)));
            }
        }

        #endregion

        #region Methods

        public void SearchInRepository()
        {
            //Search through the repository list the Search item typed
   

        }

        //validate if the Input user is not empty in GetRepositoryCommand

        public async void GetRepositoriesAsync(string userName)
        {
            var repositories = await GithubApi.GetUserRepositoryAsync(userName);
            Repositories = new ObservableCollection<Repository>(repositories);
        }

        #endregion

        #region Constructor

        public RepositoryViewModel()
        {

            IsSearching = false;

            GetRepositoryCommand = new Xamarin.Forms.Command(async () =>
            {
                IsSearching = true;
                var repositories = await GithubApi.GetUserRepositoryAsync(User);
                Repositories = new ObservableCollection<Repository>(repositories);
                IsSearching = false;
            });
        }

        #endregion
    }
}
