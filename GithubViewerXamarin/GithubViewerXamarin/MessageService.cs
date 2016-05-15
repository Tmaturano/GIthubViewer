using Github.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubViewerXamarin
{
    public class MessageService : IMessageService
    {
        public async Task ShowAsync(string title, string message)
        {
            await App.Current.MainPage.DisplayAlert(title, message, "Ok");
        }

        public MessageService()
        {

        }
    }
}
