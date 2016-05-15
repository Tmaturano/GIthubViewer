using Github;
using Github.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GithubViewerXamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new UserViewModel();
            //Set the action for the TextChanged event
            searchRepository.TextChanged += SearchRepository_TextChanged;
        }

        public void SearchRepository_TextChanged(object sender, TextChangedEventArgs e)
        {
            repositoriesList.ItemsSource = (BindingContext as UserViewModel).SearchInRepositoryList(searchRepository.Text);
        }
    }
}
