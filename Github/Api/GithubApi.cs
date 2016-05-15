using Github.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
                }

                return _httpClient;
            }
        }

        //https://api.github.com/users/Tmaturano
        //https://api.github.com/users/Tmaturano/repos

        public static async Task<IList<Repository>> GetUserRepositoryAsync(string user)
        {
            string url = $"https://api.github.com/users/{user}/repos";

            var json = await HttpClient.GetStringAsync(url);
            var repositories = JsonConvert.DeserializeObject<List<Repository>>(json);

            return repositories;
        }

        public static async Task<User> GetUserAsync(string userName)
        {
            string url = $"https://api.github.com/users/{userName}";

            var json = await HttpClient.GetStringAsync(url);
            var user = JsonConvert.DeserializeObject<User>(json);

            return user;
        }
    }
}
