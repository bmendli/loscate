using Loscate.App.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Loscate.App.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}