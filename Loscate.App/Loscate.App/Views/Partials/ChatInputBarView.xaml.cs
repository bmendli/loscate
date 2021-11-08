using System;
using Loscate.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Loscate.App.Views.Partials
{
    public partial class ChatInputBarView : ContentView
    {
        public ChatInputBarView()
        {
            InitializeComponent();
        }
        
        public void Handle_Completed(object sender, EventArgs e)
        {
            (this.Parent.Parent.BindingContext as ChatPageViewModel).OnSendCommand.Execute(null);
            chatTextInput.Focus();
        }

        public void UnFocusEntry(){
            chatTextInput?.Unfocus();
        }
    }
}