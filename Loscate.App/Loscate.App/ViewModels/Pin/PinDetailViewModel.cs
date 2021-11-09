using Loscate.App.Repository;
using Loscate.App.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Loscate.App.ApiRequests.Social.Message;
using Loscate.App.Views;
using Nancy.TinyIoc;
using Xamarin.Forms;

namespace Loscate.App.ViewModels
{
    [QueryProperty(nameof(ShortName), nameof(ShortName))]
    [QueryProperty(nameof(FullName), nameof(FullName))]
    [QueryProperty(nameof(UserUID), nameof(UserUID))]
    public class PinDetailViewModel : BaseViewModel
    {
        private readonly UserRepository userRepository;
        public Command WriteMessageCommand { get; }
        public ImageSource Img { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Photo { get; set; }

        private string userUID;

        public string UserUID
        {
            get => userUID;
            set
            {
                userUID = value;
                VisibleWriteButton = !(userRepository.user.UID == UserUID);
            }
        }

        public bool VisibleWriteButton { get; set; }

        private readonly IFirebaseAuthenticator firebaseAuthenticator;

        public PinDetailViewModel()
        {
            firebaseAuthenticator = DependencyService.Get<IFirebaseAuthenticator>();
            userRepository = TinyIoCContainer.Current.Resolve<UserRepository>();
           
            WriteMessageCommand = new Command(async () => await WriteMessage());
            Photo = PhotoBase64DTO.PhotoBase64;

            Img = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(PhotoBase64DTO.PhotoBase64)));
        }

        private async Task WriteMessage()
        {
            var sendText = $"Привет!\nМеня заинтерисовала твоя метка \"{ShortName}\".";
            await MessageRequests.SendMessage(firebaseAuthenticator, UserUID, sendText);

            await Application.Current.MainPage.DisplayAlert("Уведомление", "Сообщение успешно отправлено!\nВы можете написать владельцу в сообщениях или подождать пока, он вам ответит.", "ОК");
            
            await Shell.Current.GoToAsync("..");
        }
    }
}
