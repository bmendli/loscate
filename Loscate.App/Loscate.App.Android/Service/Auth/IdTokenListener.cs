using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            auth.CurrentUser.GetIdToken(false).AsAsync<GetTokenResult>().ContinueWith((task) =>
            {
                IdTokenChanged?.Invoke(this, new TokenChangedEventArgs { Token = task.Result.Token });
            });

        }

    }
}