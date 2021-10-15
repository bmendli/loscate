using Loscate.DTO.Firebase;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Security.Claims;

namespace Loscate.Site.Utilities
{
    public static class UserUtilities
    {
        public static FirebaseUser ToFirebaseUser(this ClaimsPrincipal user)
        {
            return GetFirebaseUser(user);
        }
        private static FirebaseUser GetFirebaseUser(ClaimsPrincipal user)
        {
            if (user != null)
            {
                if (user.Identity != null)
                {
                    //https://firebase.google.com/docs/rules/rules-and-auth
                    var name = user.Claims.Where(x => x.Type == "name").SingleOrDefault()?.Value;
                    var uid = user.Claims.SingleOrDefault(x => x.Type == "user_id")?.Value;
                    var pictureUrl = user.Claims.SingleOrDefault(x => x.Type == "picture")?.Value;

                    var firebaseUser = JObject.Parse(user.Claims.SingleOrDefault(x => x.Type == "firebase")?.Value);
                    var email = firebaseUser["identities"]["email"][0].ToString();

                    return new FirebaseUser(uid, name, email, pictureUrl, false);
                }
                throw new NullReferenceException("null user identity");
            }
            throw new NullReferenceException("null user");
        }
    }
}
