using Loscate.App.Map;
using Loscate.DTO.Map;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Loscate.App.Views
{
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();

            CustomMap customMap = new CustomMap
            {
                MapType = MapType.Street,
                IsShowingUser = true
            };

            Content = customMap;

            CustomPin pin = new CustomPin
            {
                Type = PinType.Place,
                Position = new Position(60.01001948251575, 30.374142388879566),
                Label = "SPBSTU",
                Address = "Санкт-Петербург Политехническая ул., 29",
                Name = "Санкт-Петербургский Политехнический Университет Петра Великого",
                Url = "https://www.spbstu.ru/",
                IcoUrl = "https://www.spbstu.ru/upload/branding/logo_vert.png"
            };
            customMap.CustomPins = new List<CustomPin> { pin };
            customMap.Pins.Add(pin);
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMiles(1.0)));
        }
    }
}