using Loscate.App.Repository;
using Loscate.App.Services;
using Loscate.App.Views;
using Loscate.DTO.Firebase;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Text;
using Loscate.App.Models;
using Xamarin.Forms;

namespace Loscate.App.ViewModels
{
    public class MyProfileViewModel : BaseViewModel
    {
        public UserRepository UserRepository { get; set; }

        private IFirebaseAuthenticator firebaseAuth;
        public Command SignOutCommand { get; }
        public Command EditAccountCommand { get; }

        public Command OpenMyPinsCommand { get; }

        public MyProfileViewModel()
        {
            UserRepository = TinyIoCContainer.Current.Resolve<UserRepository>();
            SignOutCommand = new Command(SignOut);
            EditAccountCommand = new Command(EditAccount);
            OpenMyPinsCommand = new Command(OpenMyPins);
        }

        private async void EditAccount()
        {
            await Shell.Current.GoToAsync($"{nameof(EditProfilePage)}");
        }

        private async void OpenMyPins()
        {
            await Shell.Current.GoToAsync($"{nameof(MyPinsPage)}");
        }

        private void SignOut()
        {
            firebaseAuth = DependencyService.Get<IFirebaseAuthenticator>();

            firebaseAuth.SubscribeToTokenUpdate(TokenUpdate);
            firebaseAuth.SignOut();
        }

        private void TokenUpdate()
        {
            if (string.IsNullOrEmpty(firebaseAuth.GetAuthToken()))
            {
                firebaseAuth.UnSubscribeToTokenUpdate(TokenUpdate);
                Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", "Ошибка выхода", "OK");
            }
        }
    }
}