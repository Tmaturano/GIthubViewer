﻿using Github.Api;
using Github.Interfaces;
using Github.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Github.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        #region Private variables

        private int _totalRepositories = 0;
        private readonly IMessageService _messageService;

        #endregion

        #region Properties

        public string LoggedUserName
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("LoggedUser"))
                {
                    return $"Welcome {Application.Current.Properties["LoggedUser"] as string}";
                }

                return string.Empty;
            }
        }

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

        public int NextPage { get; private set; } = 1;

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
            }
        }

        private User _user;

        public User User
        {
            get { return _user; }
            private set
            {
                _user = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(User)));
            }
        }

        private ObservableCollection<Repository> _repositories;

        public ObservableCollection<Repository> Repositories
        {
            get
            {
                return _repositories;
            }
            private set
            {
                _repositories = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Repositories)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RepositoriesLoaded)));
            }
        }

        private bool _isSearching;

        public bool IsSearching
        {
            get { return _isSearching; }
            set
            {
                _isSearching = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSearching)));
            }
        }

        public bool RepositoriesLoaded
        {
            get
            {
                return Repositories?.Count > 0;
            }
        }

        public string TotalRepositories
        {
            get
            {
                return $"{_totalRepositories} repository(ies)";
            }
        }

        #endregion

        #region Constructor

        public UserViewModel()
        {
            _messageService = DependencyService.Get<IMessageService>();

            IsSearching = false;
            GetUserCommand = new Command(async () => await GetUserRepository());
            OpenGithubCommand = new Command(OpenGithubSite);
        }

        #endregion

        #region Commands
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GetUserCommand { get; set; }
        public ICommand OpenGithubCommand { get; set; }

        #endregion

        #region Methods
        public ObservableCollection<Repository> SearchInRepositoryList(string repositoryName)
        {
            ObservableCollection<Repository> repositories;

            if (!string.IsNullOrWhiteSpace(repositoryName))
            {
                repositories = new ObservableCollection<Repository>(Repositories.Where(r => r.Name.ToLower()
                .Contains(repositoryName.ToLower())));
            }
            else
            {
                repositories = Repositories;
            }

            _totalRepositories = repositories.Count;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalRepositories)));

            return repositories;
        }

        private async Task GetUserRepository()
        {
            IsSearching = true;
            try
            {
                NextPage = 1;

                var user = await GithubApi.GetUserAsync(UserName);
                User = user;

                var repositories = await GithubApi.GetUserRepositoryAsync(UserName);
                Repositories = new ObservableCollection<Repository>(repositories);

                if (Repositories.Count.Equals(30))
                    NextPage++;
                else
                    NextPage = 0;

                _totalRepositories = Repositories.Count;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalRepositories)));
            }
            catch (Exception ex)
            {
                //Could be logged
                //The common error that can occurs here is the Http 404 not found or Response == null(No internet connection).
                string errorMessage = string.Empty;

                if (ex.Message.Contains("NameResolutionFailure"))
                {
                    errorMessage = "No internet connection.";
                }

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

        public async Task GetMoreRepositories()
        {
            IsSearching = true;

            if (NextPage > 0)
            {
                var repositories = await GithubApi.GetUserRepositoryAsync(UserName, NextPage);

                NextPage++;

                if (repositories.Count < 30)
                {
                    NextPage = 0;
                }

                foreach (var item in repositories)
                {
                    Repositories.Add(item);
                }

                _totalRepositories += repositories.Count;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalRepositories)));
            }

            IsSearching = false;
        }

        private void OpenGithubSite()
        {
            Device.OpenUri(new Uri(User.GitUrl));
        }

        #endregion                 

    }
}
