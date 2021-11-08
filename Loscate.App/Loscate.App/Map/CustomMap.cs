using Loscate.DTO.Map;
using System.Collections.Generic;

namespace Loscate.App.Map
{
    public class CustomMap : Xamarin.Forms.Maps.Map
    {
        public List<CustomPin> CustomPins { get; set; }
    }
}
