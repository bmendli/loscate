using Loscate.App.Models;
using Loscate.App.Repository;
using Loscate.App.Views.Cells;
using Nancy.TinyIoc;
using Xamarin.Forms;

namespace Loscate.App.Utilities
{
    public class ChatTemplateSelector : DataTemplateSelector
    {
        private readonly UserRepository userRepository;
        private DataTemplate incomingDataTemplate;
        private DataTemplate outgoingDataTemplate;

        public ChatTemplateSelector()
        {
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
            
            userRepository = TinyIoCContainer.Current.Resolve<UserRepository>();
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as Message;
            if (messageVm == null)
                return null;


            //return (messageVm.User == userRepository.user.Name) ? outgoingDataTemplate : incomingDataTemplate;
            return (messageVm.User == userRepository.user.Name) ? incomingDataTemplate : outgoingDataTemplate;
        }
    }
}