using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Loscate.App.ViewModels
{
    [QueryProperty(nameof(Name), nameof(Name))]
    [QueryProperty(nameof(Url), nameof(Url))]
    [QueryProperty(nameof(IcoUrl), nameof(IcoUrl))]
    public class PinDetailViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string IcoUrl { get; set; }

    }
}
