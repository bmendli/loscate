using Loscate.App.Services;
using System;
using Xamarin.Forms;


[assembly: Dependency(typeof(Loscate.App.Droid.Service.FirebaseAuthentication))]
namespace Loscate.App.Droid.Service
{

    public class FirebaseAuthentication : IFirebaseAuthenticator
    {
        public string token;
        public Action SignInAction;
        public Action SignOutAction;

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
    }
}