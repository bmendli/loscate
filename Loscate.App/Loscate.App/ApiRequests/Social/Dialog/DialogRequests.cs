using System.Collections.Generic;
using System.Threading.Tasks;
using Loscate.App.Services;
using Loscate.App.Utilities;

namespace Loscate.App.ApiRequests.Social.Dialog
{
    public static class DialogRequests
    {
        public static async Task<List<DTO.Social.Dialog>> GetUserDialogs(IFirebaseAuthenticator firebaseAuthenticator)
        {
            return await ApiRequest.MakeRequest<List<DTO.Social.Dialog>>(firebaseAuthenticator,"api/social/dialog/getUserDialogs");
        }
    }
}