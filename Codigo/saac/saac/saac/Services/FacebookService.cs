using Newtonsoft.Json;
using saac.Models;
using saac.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services
{
    public class FacebookService : IFacebookService
    {
        public async Task<Facebook> GetFacebookProfileAsync(string accessToken)
        {
            var requestUrl =
                "https://graph.facebook.com/v2.7/me/?fields=name,email,birthday,picture&access_token="
                + accessToken;

            var httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);

            var facebookProfile = JsonConvert.DeserializeObject<Facebook>(userJson);

            return facebookProfile;

        }
    }
}
