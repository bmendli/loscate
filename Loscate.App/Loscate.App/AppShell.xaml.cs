using Loscate.App.Repository;
using Loscate.App.Services;
using Loscate.App.ViewModels;
using Loscate.App.Views;
using Loscate.DTO.Firebase;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Loscate.App
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell(FirebaseUser user)
        {
            var userRepository = new UserRepository();
            userRepository.user = user;
            var container = TinyIoCContainer.Current.Register(userRepository);


            
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(MapPage), typeof(MapPage));
            Routing.RegisterRoute(nameof(MyProfilePage), typeof(MyProfilePage));
            Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
        }

    }
}
