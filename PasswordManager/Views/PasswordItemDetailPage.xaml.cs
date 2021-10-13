using PasswordManager.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace PasswordManager.Views
{
    public partial class PasswordItemDetailPage : ContentPage
    {
        PasswordItemDetailViewModel _viewModel;

        public PasswordItemDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new PasswordItemDetailViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {

        }
    }
}