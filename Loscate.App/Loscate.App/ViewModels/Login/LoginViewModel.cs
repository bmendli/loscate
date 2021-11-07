using Loscate.App.Services;
using Loscate.DTO.Firebase;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Loscate.App.ApiRequests;
using Loscate.App.ApiRequests.User;
using Xamarin.Forms;
using Newtonsoft.Json;
using Loscate.App.Utilities;

namespace Loscate.App.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command GoogleSignInCommand { get; }


        private readonly IFirebaseAuthenticator firebaseAuth;
        public bool IsBusy { get; set; }


        public LoginViewModel()
        {
            GoogleSignInCommand = new Command(GoogleSignIn, () => !IsBusy);

            firebaseAuth = DependencyService.Get<IFirebaseAuthenticator>();
            firebaseAuth.SubscribeToTokenUpdate(TokenUpdate);
        }

        private async void Login()
        {
            if(string.IsNullOrEmpty(firebaseAuth.GetAuthToken()))
            {
                IsBusy = false;
                return;
            }
            try
            {
                var user = await UserRequests.GetUser(firebaseAuth);

                // firebaseAuth.UnSubscribeToTokenUpdate(TokenUpdate);
                Device.BeginInvokeOnMainThread(() => Application.Current.MainPage = new AppShell(user));
                Device.BeginInvokeOnMainThread(() => Shell.Current.GoToAsync("//main"));
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка входа", e.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void TokenUpdate()
        {
            Login();
        }

        private void GoogleSignIn()
        {
            IsBusy = true;
            firebaseAuth.SignIn();
        }
    }
}
