using System.Threading.Tasks;
using Loscate.App.Services;
using Loscate.App.Utilities;
using Loscate.DTO.Firebase;

namespace Loscate.App.ApiRequests.User
{
    public static class UserRequests
    {
        public static async Task<FirebaseUser> GetUser(IFirebaseAuthenticator firebaseAuthenticator)
        {
            return await ApiRequest.MakeRequest<FirebaseUser>(firebaseAuthenticator,"api/user/getFirebaseUser");
        }
    }
}