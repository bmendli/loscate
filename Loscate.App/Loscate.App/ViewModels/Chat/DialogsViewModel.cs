using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Loscate.App.ApiRequests.Social.Dialog;
using Loscate.App.ApiRequests.Social.Message;
using Loscate.App.Models;
using Loscate.App.Services;
using Loscate.App.Views;
using Loscate.DTO.Firebase;
using Loscate.DTO.Social;
using Xamarin.Forms;

namespace Loscate.App.ViewModels
{
    public class DialogsViewModel : BaseViewModel
    {
        private readonly IFirebaseAuthenticator firebaseAuthenticator;
        private Dialog _selectedDialog;
        public ObservableCollection<Dialog> Dialogs { get; }
        public Command LoadDialogsCommand { get; }
        public Command AddDialogCommand { get; }
        public Command<Dialog> ItemTapped { get; }
        
        public Dialog SelectedDialog
        {
            get => _selectedDialog;
            set
            {
                SetProperty(ref _selectedDialog, value);
                OnDialogSelected(value);
            }
        }
        
        public DialogsViewModel()
        {
            Dialogs = new ObservableCollection<Dialog>();
            firebaseAuthenticator = DependencyService.Get<IFirebaseAuthenticator>();
            LoadDialogsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Dialog>(OnDialogSelected);

            AddDialogCommand = new Command(OnAddDialog);
        }
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Dialogs.Clear();

                var dialogs = await DialogRequests.GetUserDialogs(firebaseAuthenticator);
                foreach (var dialog in dialogs)
                {
                    Dialogs.Add(dialog);
                }

                // var items = await DataStore.GetItemsAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void OnAppearing()
        {
            IsBusy = true;
            SelectedDialog = null;
        }
        
        private async void OnAddDialog(object obj)
        {
            //await Shell.Current.GoToAsync(nameof(NewItemPage));
        }
        
        async void OnDialogSelected(Dialog dialog)
        {
            if (dialog == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
           // await Shell.Current.GoToAsync($"{nameof(ChatPage)}?{nameof(ChatPageViewModel.CompanionUser)}={dialog.Companion}");
           await Shell.Current.GoToAsync($"{nameof(ChatPage)}?{nameof(ChatPageViewModel.CompanionUserUID)}={dialog.Companion.UID}&{nameof(ChatPageViewModel.CompanionName)}={dialog.Companion.Name}");
           //TODO
        }
    }
}