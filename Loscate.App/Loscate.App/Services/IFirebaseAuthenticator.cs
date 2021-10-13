namespace Loscate.App.Services
{
    public interface IFirebaseAuthenticator
    {
        string GetAuthToken();
        void SignIn();
        void SignOut();
    }
}
