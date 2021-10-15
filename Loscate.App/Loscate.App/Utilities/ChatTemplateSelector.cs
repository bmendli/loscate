﻿using Loscate.App.Models;
using Loscate.App.Views.Cells;
using Xamarin.Forms;

namespace Loscate.App.Utilities
{
    public class ChatTemplateSelector: DataTemplateSelector
    {
        DataTemplate incomingDataTemplate;
        DataTemplate outgoingDataTemplate;

        public ChatTemplateSelector()
        {
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as Message;
            if (messageVm == null)
                return null;

          
            return (messageVm.User == "testUser")? outgoingDataTemplate: incomingDataTemplate;
        }
    }
}