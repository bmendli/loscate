using Loscate.App.Services;
using Loscate.DTO.Firebase;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace Loscate.App.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {

        public Command LoginCommand { get; }


        private IFirebaseAuthenticator firebaseAuth;

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);

            firebaseAuth = DependencyService.Get<IFirebaseAuthenticator>();
            firebaseAuth.SignIn();
        }

        private async void OnLoginClicked(object obj)
        {
            //// Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");

            try
            {
                var response = await GetInfo(firebaseAuth.GetAuthToken(), "api/user/getFirebaseUser");
                var user = JsonConvert.DeserializeObject<FirebaseUser>(response);

                await Application.Current.MainPage.DisplayAlert("Info", "Welcome " + user.Name, "OK");
            }
            catch(Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
            }
            
            // App.Current.MainPage = new AppShell();
        }

        public async Task<string> TryGetInfo(string authorizeToken, string url)
        {
            try
            {
                return await GetInfo(authorizeToken, url);
            }
            catch (AccessViolationException)
            {
                firebaseAuth.SignIn();
                return await GetInfo(authorizeToken, url);
            }
        }

        public async Task<string> GetInfo(string authorizeToken, string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizeToken);
                client.BaseAddress = new Uri("https://loscate.site/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = new HttpResponseMessage();

                response = await client.GetAsync(url).ConfigureAwait(false);


                // Verification  
                if (response.IsSuccessStatusCode)
                {
                     var content = await response.Content.ReadAsStringAsync();

                    return content;
                }
                else if(response.StatusCode == HttpStatusCode.Unauthorized)
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
