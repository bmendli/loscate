using Loscate.App.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Loscate.App.ApiRequests.Map;
using Loscate.App.Map;
using Loscate.App.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;
using Loscate.App.Repository;

namespace Loscate.App.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public Command AddPinCommand { get; }
        public Command SearchCommand { get; }
        
        public Command RefreshCommand { get; }

        private readonly CustomMap map;
        private readonly IFirebaseAuthenticator firebaseAuthenticator;
        private IMapService mapService;
        private Position startPosition = new Position(60.01001948251575, 30.374142388879566); //spbstu

        public MapViewModel(CustomMap map)
        {
            this.map = map;
            mapService = DependencyService.Get<IMapService>();
            firebaseAuthenticator = DependencyService.Get<IFirebaseAuthenticator>();
            mapService.OnPinClickSubscribe(OnPinClick);
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
            SearchCommand = new Command(OpenSearchPage);
            AddPinCommand = new Command(OpenAddPinPage);
            RefreshCommand = new Command(LoadPins);

            map.MoveToRegion(MapSpan.FromCenterAndRadius(startPosition, Distance.FromMiles(1.0)));

            LoadPins();
        }

        private void LoadPins()
        {
            var task = Task.Run(async () => await MapRequests.GetPins(firebaseAuthenticator));
            var res = task.Result;
            var customPins = res.Select(ConvertPin).ToList();

            map.Pins.Clear();
            
            map.CustomPins = new List<CustomPin>(customPins);

            customPins.ForEach(p => map.Pins.Add(p));
        }

        private CustomPin ConvertPin(DTO.Map.Pin pin)
        {
            return new CustomPin()
            {
                Position = new Position(pin.Latitude, pin.Longitude),
                ShortName = pin.ShortName,
                FullName = pin.FullName,
                Photo = pin.PhotoBase64,
                Label = pin.ShortName,
                UserUID = pin.UserUID
            };
        }

        private async void OpenAddPinPage()
        {
            await Shell.Current.GoToAsync($"{nameof(AddPinPage)}");
        }

        private async void OpenSearchPage()
        {
            await Shell.Current.GoToAsync($"{nameof(SearchPinPage)}");
        }

        public ICommand OpenWebCommand { get; }

        private void OnPinClick(CustomPin pin)
        {
            PhotoBase64DTO.PhotoBase64 = pin.Photo;

            Shell.Current.GoToAsync($"{nameof(PinDetailPage)}?" +
                $"{nameof(PinDetailViewModel.ShortName)}={pin.ShortName}&" +
                $"{nameof(PinDetailViewModel.FullName)}={pin.FullName}&" +
                $"{nameof(PinDetailViewModel.UserUID)}={pin.UserUID}");
        }
    }
}