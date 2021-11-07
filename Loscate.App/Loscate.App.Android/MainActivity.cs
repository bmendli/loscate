using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api;
using Android.Content;
using Firebase;
using Firebase.Auth;
using Loscate.App.Droid.Service;
using Android.Gms.Extensions;
using System.Threading.Tasks;
using Loscate.App.Services;
using Xamarin.Forms;

namespace Loscate.App.Droid
{
    [Activity(Label = "Loscate.App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private const int RequestLocationId = 0;
        private GoogleSignInOptions gso;
        private GoogleApiClient googleApiClient;
        private FirebaseAuth firebaseAuth;
        private FirebaseAuthentication firebaseAuthManager;
        private IdTokenListener idTokenListener = new IdTokenListener();
        private readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);

            gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken("563040333393-nh05lvif64909jvvfhu0r1hq7f2t15nt.apps.googleusercontent.com")
                .RequestEmail()
                .Build();

            googleApiClient = new GoogleApiClient.Builder(this)
                .AddApi(Auth.GOOGLE_SIGN_IN_API, gso).Build();
            googleApiClient.Connect();


            firebaseAuth = GetFirebaseAuth();

            FirebaseAuth.Instance.AddIdTokenListener(idTokenListener);
           
            idTokenListener.IdTokenChanged += OnIdTokenChanged;

            firebaseAuthManager = (FirebaseAuthentication)DependencyService.Get<IFirebaseAuthenticator>();

            firebaseAuthManager.SignInAction = SignIn;
            firebaseAuthManager.SignOutAction = SignOut;
            firebaseAuthManager.firebaseAuth = firebaseAuth;


            LoadApplication(new App());
        }

        private void OnIdTokenChanged(object sender, IdTokenListener.TokenChangedEventArgs e)
        {
            Console.WriteLine("TokenChanged: " + e.Token);
            firebaseAuthManager.token = e.Token;
            firebaseAuthManager.OnTokenChange?.Invoke();
        }


        private void SignIn()
        {
            if (firebaseAuth.CurrentUser == null)
            {
                var intent = Auth.GoogleSignInApi.GetSignInIntent(googleApiClient);
                StartActivityForResult(intent, 1);
            }
        }

        private void SignOut()
        {
            firebaseAuth.SignOut();
            Auth.GoogleSignInApi.SignOut(googleApiClient);
        }

        private FirebaseAuth GetFirebaseAuth()
        {
            var app = FirebaseApp.InitializeApp(this);
            FirebaseAuth mAuth;

            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("loscate")
                    .SetApplicationId("loscate")
                    .SetApiKey("AIzaSyCpm_bIDe1uRtMikNuvHnvy8uCi8boczck")
                    .SetDatabaseUrl("https://loginwith-79490.firebaseio.com")
                    .SetStorageBucket("loscate.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(this, options);
                mAuth = FirebaseAuth.Instance;
            }
            else
            {
                mAuth = FirebaseAuth.Instance;
            }
            return mAuth;
        }

        protected override void OnStart()
        {
            base.OnStart();

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                }
                else
                {
                    // Permissions already granted - display a message.
                }
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 1)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                if (result.IsSuccess)
                {
                    GoogleSignInAccount account = result.SignInAccount;
                    LoginWithFirebase(account);
                }
            }
        }

        private void LoginWithFirebase(GoogleSignInAccount account)
        {
            var credentials = GoogleAuthProvider.GetCredential(account.IdToken, null);
            firebaseAuth.SignInWithCredential(credentials);
        }

        protected override void OnResume()
        {
            base.OnResume();

            Xamarin.Essentials.Platform.OnResume();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode == RequestLocationId)
            {
                //if ((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted))
                // Permissions granted - display a message.
                //else
                // Permissions denied - display a message.
            }
            else
            {
                Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}