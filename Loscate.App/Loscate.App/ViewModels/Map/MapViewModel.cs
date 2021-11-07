using Loscate.App.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Loscate.App.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public Command AddPinCommand { get; }
        public Command SearchCommand { get; }

        public MapViewModel()
        {
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
            SearchCommand = new Command(OpenSearchPage);
            AddPinCommand = new Command(OpenAddPinPage);
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
    }
}