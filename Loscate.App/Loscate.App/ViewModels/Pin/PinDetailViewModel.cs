using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Loscate.App.ViewModels
{
    [QueryProperty(nameof(ShortName), nameof(ShortName))]
    public class PinDetailViewModel : BaseViewModel
    {
        public string ShortName { get; set; }
    }
}
