using System.Collections.Generic;
using System.Threading.Tasks;
using Loscate.App.Services;
using Loscate.App.Utilities;

namespace Loscate.App.ApiRequests.Social.Message
{
    public static class MessageRequests
    {
        public static async Task<List<DTO.Social.Message>> GetUserMessage(IFirebaseAuthenticator firebaseAuthenticator,
            string pairUid)
        {
            return await ApiRequest.MakeRequest<List<DTO.Social.Message>>(firebaseAuthenticator,
                $"api/social/message/getDialogMessages?pairUid={pairUid}");
        }

        public static async Task<string> SendMessage(IFirebaseAuthenticator firebaseAuthenticator, string pairUid,
            string msg)
        {
            return await ApiRequest.MakeRequest<string>(firebaseAuthenticator,
                $"api/social/message/sendMessage?pairUid={pairUid}&msg={msg}");
        }
    }
}