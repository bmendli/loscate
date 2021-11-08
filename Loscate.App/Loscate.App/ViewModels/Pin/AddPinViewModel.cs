using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System;
using System.IO;
using Loscate.App.ApiRequests.Map;
using Loscate.App.Services;

namespace Loscate.App.ViewModels
{
    public class AddPinViewModel : BaseViewModel
    {
        public Command SelectPhotoCommand { get; }
        public Command AddGeoTagCommand { get; }
        public Command AddPinCommand { get; }
        public ImageSource SelectImg { get; set; }

        public string ShortText { get; set; }
        public string FullText { get; set; }
        public string AddresName { get; set; }

        public bool MapVisible { get; set; }

        public bool ElementsVisible { get; set; }

        private readonly Geocoder geocoder = new Geocoder();
        private readonly IFirebaseAuthenticator firebaseAuthenticator;
        private Position position;
        private string base64photo;

        public AddPinViewModel()
        {
            firebaseAuthenticator = DependencyService.Get<IFirebaseAuthenticator>();

            SelectPhotoCommand = new Command(async() => await SelectPhoto());
            AddPinCommand = new Command(async () => await AddPin());
            AddGeoTagCommand = new Command(AddGeoTag);

            SelectImg = ImageSource.FromFile("noPhoto.jpg");
            MapVisible = false;
            ElementsVisible = true;
        }

        private async Task AddPin()
        {
            var validMessage = ValidateData();
            if (!string.IsNullOrEmpty(validMessage))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", validMessage, "ОК");
            }
            else
            {
                DTO.Map.Pin pin = new DTO.Map.Pin()
                {
                    Latitude = position.Latitude,
                    Longitude = position.Longitude,
                    ShortName = ShortText,
                    FullName = FullText,
                    PhotoBase64 = base64photo
                };

                var res = await MapRequests.AddPin(firebaseAuthenticator, pin);

                if (res != "OK")
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Ошибка добавления", "ОК");
                }
            }
        }

        private string ValidateData()
        {
            if (string.IsNullOrEmpty(base64photo))
            {
                return "Выберите фото!";
            }

            if (string.IsNullOrEmpty(ShortText))
            {
                return "Краткое описание не может быть пустым!";
            }

            if (ShortText.Length < 10)
            {
                return "Краткое описание не может быть меньше 10 символов!";
            }


            if (string.IsNullOrEmpty(FullText))
            {
                return "Подробное описание не может быть пустым!";
            }

            if (FullText.Length < 20)
            {
                return "Подробное описание не может быть меньше 20 символов!";
            }

            if (position.Latitude == 0)
            {
                return "Выберите геометку!";
            }

            return null;
        }

        private void AddGeoTag()
        {
            MapVisible = true;
            ElementsVisible = false;
        }

        public async void MapClick(Position position)
        {
            var addreses = await geocoder.GetAddressesForPositionAsync(position);
            var addresName = addreses.FirstOrDefault();

            if(addresName != null)
            {
                AddresName = addresName;
            }

            this.position = position;

            MapVisible = false;
            ElementsVisible = true;
        }

        private async Task SelectPhoto()
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Выбор фото метки"
            });

            if (result == null) return;

            var stream = await result.OpenReadAsync();

            base64photo = ConvertToBase64(await result.OpenReadAsync());

            var resultPhoto = ImageSource.FromStream(() => stream);
            SelectImg = resultPhoto;
        }

        public string ConvertToBase64(Stream stream)
        {
            var bytes = new Byte[(int)stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);

            return Convert.ToBase64String(bytes);
        }
    }
}
