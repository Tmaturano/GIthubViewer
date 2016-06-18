using Github.Model;
using Github.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Github.Api
{
    public static class GithubApi
    {
        private static HttpClient _httpClient;

        public static HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient();
                    _httpClient.DefaultRequestHeaders.Add("User-Agent", "Other");                                        
                    _httpClient.BaseAddress = new Uri("https://api.github.com");
                }

                return _httpClient;
            }
        }

        //https://api.github.com/users/Tmaturano
        //https://api.github.com/users/Tmaturano/repos

        public static async Task<IList<Repository>> GetUserRepositoryAsync(string user, int page = 1)
        {
            //string url = $"https://api.github.com/users/{user}/repos?page={page}";

            string url = $"users/{user}/repos?page={page}";

            var json = await HttpClient.GetStringAsync(url);
            var repositories = JsonConvert.DeserializeObject<List<Repository>>(json);

            return repositories;
        }

        public static async Task<User> GetUserAsync(string userName)
        {
            //string url = $"https://api.github.com/users/{userName}";
            string url = $"users/{userName}";

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            var json = await HttpClient.GetStringAsync(url);
            var user = JsonConvert.DeserializeObject<User>(json, settings);

            user.Name = user.Name ?? "No name found";
            user.Location = user.Location ?? "No location found";
            user.Company = user.Company ?? "No company found";

            return user;
        }
        
        //Basic Authentication
        public static async Task<HttpStatusCode> Login(LoginViewModel login)
        {
            HttpStatusCode statusCode = HttpStatusCode.Forbidden;
            HttpContent content = null;

            try
            {
                var byteArray = Encoding.UTF8.GetBytes($"{login.LoginName}:{login.Password}");
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                HttpResponseMessage response = await HttpClient.GetAsync("user");
                content = response.Content;

                statusCode = response.StatusCode;

                if (statusCode == HttpStatusCode.OK)
                {
                    var result = await content?.ReadAsStringAsync();
                    var userData = new { Name = string.Empty, Login = string.Empty };
                    var userName = JsonConvert.DeserializeAnonymousType(result, userData);

                    Application.Current.Properties["LoggedUser"] = string.IsNullOrWhiteSpace(userName.Name) ?
                        userName.Login : userName.Name;

                    await Application.Current.SavePropertiesAsync();
                }
            }
            catch(Exception ex)
            {
                //Do some logging here
            }

            return statusCode;            
        }
    }
}
