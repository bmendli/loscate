using Xamarin.Forms.Maps;

namespace Loscate.App.Map
{
    public class CustomPin : Pin
    {
        public string UserUID { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Photo { get; set; }
    }
}