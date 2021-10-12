using PasswordManager.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PasswordManager.Views
{
    public partial class MainPage : ContentPage
    {
        MainViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new MainViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}