using Android.Gms.Extensions;
using Firebase.Auth;
using System;

namespace Loscate.App.Droid
{
    public class IdTokenListener : Java.Lang.Object, FirebaseAuth.IIdTokenListener
    {
        public EventHandler<TokenChangedEventArgs> IdTokenChanged;

        public class TokenChangedEventArgs : EventArgs
        {
            public string Token { get; set; }
        }
        public void OnIdTokenChanged(FirebaseAuth auth)
        {
            if(auth.CurrentUser == null)
            {
                IdTokenChanged?.Invoke(this, new TokenChangedEventArgs { Token = string.Empty });
            }
            else
            {
                auth.CurrentUser.GetIdToken(false).AsAsync<GetTokenResult>().ContinueWith((task) =>
                {
                    IdTokenChanged?.Invoke(this, new TokenChangedEventArgs { Token = task.Result.Token });
                });
            }
        }

    }
}