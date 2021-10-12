using PasswordManager.Models;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PasswordManager.Views
{
    public partial class NewKeyPage : ContentPage
    {
        private NewKeyViewModel _viewModel;

        public NewKeyPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new NewKeyViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}