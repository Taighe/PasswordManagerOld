using PasswordManager.Services;
using PasswordManager.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PasswordManager
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            DependencyService.Register<PasswordStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
