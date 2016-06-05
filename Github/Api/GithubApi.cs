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

        public static async Task<IList<Repository>> GetUserRepositoryAsync(string user, int page = 1)
        {
            string url = $"https://api.github.com/users/{user}/repos?page={page}";

            var json = await HttpClient.GetStringAsync(url);
            var repositories = JsonConvert.DeserializeObject<List<Repository>>(json);

            return repositories;
        }

        public static async Task<User> GetUserAsync(string userName)
        {
            string url = $"https://api.github.com/users/{userName}";

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            var json = await HttpClient.GetStringAsync(url);
            var user = JsonConvert.DeserializeObject<User>(json, settings);

            user.Name = user.Name ?? "No name found";
            user.Location = user.Location ?? "No location found";
            user.Company = user.Company ?? "No company found";

            return user;
        }
    }
}
