using System;

namespace Loscate.App.Services
{
    public interface IFirebaseAuthenticator
    {
        string GetAuthToken();
        void SignIn();
        void SignOut();
        bool IsHaveUser();
        void SubscribeToTokenUpdate(Action action);
        void UnSubscribeToTokenUpdate(Action action);
    }
}
