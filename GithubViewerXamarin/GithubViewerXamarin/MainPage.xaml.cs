using Github;
using Github.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GithubViewerXamarin
{
    public partial class MainPage : ContentPage
    {
        private UserViewModel _userViewModel;

        public MainPage()
        {
            InitializeComponent();
            _userViewModel = new UserViewModel();

            BindingContext = _userViewModel;
            //Set the action for the TextChanged event
            searchRepository.TextChanged += SearchRepository_TextChanged;
        }

        public void SearchRepository_TextChanged(object sender, TextChangedEventArgs e)
        {
            repositoriesList.ItemsSource = _userViewModel.SearchInRepositoryList(searchRepository.Text);
        }

        //Change to MVVM - Paging just for testing
        public async void ItemAppearing(object sender, ItemVisibilityEventArgs args)
        {
            var items = repositoriesList.ItemsSource as IList;

            //If the last renderized item on the screen is equals the last item of my list
            if (items.Count > 0 && args.Item == items[items.Count - 1])
            {
                await _userViewModel.GetMoreRepositories();
            }
        }
    }
}
