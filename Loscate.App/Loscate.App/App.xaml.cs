using Loscate.App.Services;
using Loscate.App.Utilities;
using Loscate.App.Views;
using Loscate.DTO.Firebase;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;

namespace Loscate.App
{
    public partial class App : Application
    {
        private readonly IFirebaseAuthenticator firebaseAuth;
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            firebaseAuth = DependencyService.Get<IFirebaseAuthenticator>();
            firebaseAuth.SubscribeToTokenUpdate(TokenUpdate);
            if (firebaseAuth.IsHaveUser()) Login();
        }

        private async void Login()
        {
            try
            {
                var userRequest = new ApiRequest(firebaseAuth, "api/user/getFirebaseUser");
                var responseJson = await userRequest.Run();
                var user = JsonConvert.DeserializeObject<FirebaseUser>(responseJson);
                firebaseAuth.UnSubscribeToTokenUpdate(TokenUpdate);
                Device.BeginInvokeOnMainThread(() => Application.Current.MainPage = new AppShell(user));
            }
            catch
            {
                firebaseAuth.UnSubscribeToTokenUpdate(TokenUpdate);
                Device.BeginInvokeOnMainThread(() => MainPage = new LoginPage());
            }
        }

        private void TokenUpdate()
        {
            if (string.IsNullOrWhiteSpace(firebaseAuth.GetAuthToken()))
            {
                firebaseAuth.UnSubscribeToTokenUpdate(TokenUpdate);
                MainPage = new LoginPage();
            }
            else
            {
               Login();
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {
        }
    }
}
