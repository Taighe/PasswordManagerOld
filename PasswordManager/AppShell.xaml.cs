using PasswordManager.ViewModels;
using PasswordManager.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PasswordManager
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PasswordItemDetailPage), typeof(PasswordItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(NewKeyPage), typeof(NewKeyPage));

        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
