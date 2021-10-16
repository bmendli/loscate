﻿using Loscate.App.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Loscate.App.Utilities
{
    public class ApiRequest
    {
        private readonly IFirebaseAuthenticator firebaseAuth;
        private readonly string authorizeToken;
        private readonly string url;
        public ApiRequest(IFirebaseAuthenticator firebaseAuthenticator, string url)
        {
            this.firebaseAuth = firebaseAuthenticator;
            this.authorizeToken = firebaseAuthenticator.GetAuthToken();
            this.url = url;
        }
        
        public static async Task<T> MakeRequest<T>(IFirebaseAuthenticator firebaseAuthenticator, string url)
        {
            var request = new ApiRequest(firebaseAuthenticator, url);
            var responseJson = await request.Run();
            return JsonConvert.DeserializeObject<T>(responseJson);
        }

        public async Task<string> Run()
        {
            try
            {
                return await MakeRequest(authorizeToken, url);
            }
            catch (AccessViolationException)
            {
                firebaseAuth.SignIn();
                return await MakeRequest(authorizeToken, url);
            }
        }

        private async Task<string> MakeRequest(string authorizeToken, string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizeToken);
                client.BaseAddress = new Uri("https://loscate.site/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = new HttpResponseMessage();

                response = await client.GetAsync(url).ConfigureAwait(false);


                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    return content;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new AccessViolationException();
                }
                else
                {
                    throw new WebException("wrong status responce status code");
                }
            }
        }
    }
}
