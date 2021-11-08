using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loscate.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Loscate.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogsPage : ContentPage
    {
        private DialogsViewModel dialogsViewModel;

        public DialogsPage()
        {
            InitializeComponent();
            this.BindingContext = dialogsViewModel = new DialogsViewModel();
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            dialogsViewModel.OnAppearing();
        }
    }
}