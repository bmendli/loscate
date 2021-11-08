using Loscate.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Loscate.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPinPage : ContentPage
    {
        private readonly AddPinViewModel addPinViewModel;
        public AddPinPage()
        {
            InitializeComponent();
            this.BindingContext = addPinViewModel = new AddPinViewModel();
        }

        private void Map_MapClicked(object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
        {
            addPinViewModel.MapClick(e.Position);
        }
    }
}