using PasswordManager.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PasswordManager.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Shell.Current.GoToAsync($"//{nameof(MainPage)}"));
        }

        public ICommand OpenWebCommand { get; }
    }
}