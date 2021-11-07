using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Loscate.App.ApiRequests.Social.Message;
using Loscate.App.Models;
using Loscate.App.Repository;
using Loscate.App.Services;
using Nancy.TinyIoc;
using Xamarin.Forms;

namespace Loscate.App.ViewModels
{
    [QueryProperty(nameof(CompanionUserUID), nameof(CompanionUserUID))]
    [QueryProperty(nameof(CompanionName), nameof(CompanionName))]
    public class ChatPageViewModel : INotifyPropertyChanged
    {
        private readonly IFirebaseAuthenticator firebaseAuthenticator;
        private readonly UserRepository userRepository;
        public string CompanionUserUID { get; set; }
        public string CompanionName { get; set; } = "No Name";
        public bool ShowScrollTap { get; set; } = false;
        public bool LastMessageVisible { get; set; } = true;
        public int PendingMessageCount { get; set; } = 0;
        
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
            userRepository = TinyIoCContainer.Current.Resolve<UserRepository>();

            MessageAppearingCommand = new Command<Message>(OnMessageAppearing);
            MessageDisappearingCommand = new Command<Message>(OnMessageDisappearing);
            LoadMessageCommand = new Command(async () => await ExecuteLoadMessageCommand());
            OnSendCommand = new Command(async () => await SendMessage());

            Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                Task.Run(async () => await CheckNewMessage());
                return true;
            });
        }

        private async Task CheckNewMessage()
        {
            var messages = MessageRequests.GetUserMessage(firebaseAuthenticator, CompanionUserUID).Result;
            if (messages.Count() != Messages.Count)
            {
                await ExecuteLoadMessageCommand();
            }
            
        }

        private async Task SendMessage()
        {
            if (!string.IsNullOrEmpty(TextToSend))
            {
                var sendText = TextToSend;
                TextToSend = string.Empty;
                Messages.Insert(0, new Message() { Text = sendText, User = userRepository.user.Name });
                await MessageRequests.SendMessage(firebaseAuthenticator, CompanionUserUID, sendText);
            }
        }

        private async Task ExecuteLoadMessageCommand()
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