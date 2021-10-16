using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Loscate.App.ApiRequests.Social.Dialog;
using Loscate.App.ApiRequests.Social.Message;
using Loscate.App.Models;
using Loscate.App.Services;
using Loscate.DTO.Firebase;
using Xamarin.Forms;

namespace Loscate.App.ViewModels
{
    [QueryProperty(nameof(CompanionUserUID), nameof(CompanionUserUID))]
    [QueryProperty(nameof(CompanionName), nameof(CompanionName))]
    public class ChatPageViewModel: INotifyPropertyChanged
    {
        private readonly IFirebaseAuthenticator firebaseAuthenticator;
        public string CompanionUserUID { get; set; }
        public string CompanionName { get; set; } = "No Name";
        public bool ShowScrollTap { get; set; } = false;
        public bool LastMessageVisible { get; set; } = true;
        public int PendingMessageCount { get; set; } = 0;
        public bool PendingMessageCountVisible { get { return PendingMessageCount > 0; } }

        public Queue<Message> DelayedMessages { get; set; } = new Queue<Message>();
        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();
        public string TextToSend { get; set; }
        public ICommand OnSendCommand { get; set; }
        public ICommand MessageAppearingCommand { get; set; }
        public ICommand MessageDisappearingCommand { get; set; }
        
        public Command LoadMessageCommand { get; }

        public ChatPageViewModel()
        {
            firebaseAuthenticator = DependencyService.Get<IFirebaseAuthenticator>();

            MessageAppearingCommand = new Command<Message>(OnMessageAppearing);
            MessageDisappearingCommand = new Command<Message>(OnMessageDisappearing);
            LoadMessageCommand = new Command(async () => await ExecuteLoadMessageCommand());

            OnSendCommand = new Command(() =>
            {
                if(!string.IsNullOrEmpty(TextToSend)){
                    Messages.Insert(0, new Message() { Text = TextToSend, User = "testUser" });
                    TextToSend = string.Empty;
                }
            });

            // Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            // {
            //     if (LastMessageVisible)
            //     {
            //         Messages.Insert(0, new Message(){ Text = "New message test" , User="Mario"});
            //     }
            //     else
            //     {
            //         DelayedMessages.Enqueue(new Message() { Text = "New message test" , User = "Mario"});
            //         PendingMessageCount++;
            //     }
            //     return true;
            // });
        }
        
        async Task ExecuteLoadMessageCommand()
        {
            try
            {
                Messages.Clear();

                var messages = await MessageRequests.GetUserMessage(firebaseAuthenticator, CompanionUserUID);
                foreach (var message in messages)
                {
                    Messages.Insert(0, new Message() { Text = message.Text, User = message.SendUser.Name });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void OnAppearing()
        {   
            LoadMessageCommand.Execute(null);
        }

        void OnMessageAppearing(Message message)
        {
            var idx = Messages.IndexOf(message);
            if (idx <= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    while (DelayedMessages.Count > 0)
                    {
                        Messages.Insert(0, DelayedMessages.Dequeue());
                    }
                    ShowScrollTap = false;
                    LastMessageVisible = true;
                    PendingMessageCount = 0;
                });
            }
        }

        void OnMessageDisappearing(Message message)
        {
            var idx = Messages.IndexOf(message);
            if (idx >= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ShowScrollTap = true;
                    LastMessageVisible = false;
                });

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}