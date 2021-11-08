using Firebase.Auth;
using Loscate.App.Services;
using System;
using Xamarin.Forms;


[assembly: Dependency(typeof(Loscate.App.Droid.Service.FirebaseAuthentication))]
namespace Loscate.App.Droid.Service
{

    public class FirebaseAuthentication : IFirebaseAuthenticator
    {
        public string token;
        public FirebaseAuth firebaseAuth;
        public Action SignInAction;
        public Action SignOutAction;
        public Action OnTokenChange;

        public void SignIn()
        {
            SignInAction();
        }

        public void SignOut()
        {
            SignOutAction();
        }

        public string GetAuthToken()
        {
            return token;
        }

        public bool IsHaveUser()
        {
            return firebaseAuth.CurrentUser != null;
        }

        public void SubscribeToTokenUpdate(Action action)
        {
            OnTokenChange += action;
        }

        public void UnSubscribeToTokenUpdate(Action action)
        {
            OnTokenChange -= action;
        }
    }
}