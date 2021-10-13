using PasswordManager.Models;
using PasswordManager.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PasswordManager.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class PasswordItemDetailViewModel : BaseViewModel
    {
        private int itemId;
        private string text;
        private List<PasswordKey> keys;
        private PasswordKey _selectedItem;
        public Command DeleteItemCommand { get; }
        public Command AddKeyCommand { get; }
        public Command LoadKeysCommand { get; }
        public Command<PasswordKey> EditCommand { get; }
        public Command<PasswordKey> DeleteCommand { get; }
        public Command<PasswordKey> ItemTapped { get; }
        public PasswordKey SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        public PasswordItemDetailViewModel()
        {
            Title = "Passwords";
            DeleteItemCommand = new Command(OnDeleteItem);
            AddKeyCommand = new Command(OnAddKey);
            ItemTapped = new Command<PasswordKey>(OnItemSelected);
            LoadKeysCommand = new Command(async () => await ExecuteLoadKeys());
            EditCommand = new Command<PasswordKey>(OnEdit);
            DeleteCommand = new Command<PasswordKey>(OnDeleteCommand);
        }

        private async void OnEdit(PasswordKey key)
        {
            string dest = $"{nameof(NewKeyPage)}?{nameof(NewKeyViewModel.ItemId)}={itemId}&{nameof(NewKeyViewModel.KeyId)}={key.Id}";
            await Shell.Current.GoToAsync(dest);
        }

        private async void OnDeleteCommand(PasswordKey key)
        {
            await DataStore.DeleteItemAsync<PasswordKey>(key.Id);
            await ExecuteLoadKeys();
        }

        private async Task ExecuteLoadKeys()
        {
            IsBusy = true;

            try
            {
                var item = await DataStore.GetItemAsync(itemId, true);
                Id = item.Id;
                Name = item.Name;
                Keys = item.Keys;                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        async void OnItemSelected(PasswordKey key)
        {
            if (key == null)
                return;

            await Clipboard.SetTextAsync(key.Value);
            //await App.Current.MainPage.DisplayAlert("Success", string.Format("Your key has been copied to the clipboard."), "OK");
        }

        private async void OnAddKey(object obj)
        {
            string dest = $"{nameof(NewKeyPage)}?{nameof(NewKeyViewModel.ItemId)}={itemId}&{nameof(NewKeyViewModel.KeyId)}={0}";
            await Shell.Current.GoToAsync(dest);
        }

        private async void OnDeleteItem(object obj)
        {
            PasswordItem item = await DataStore.GetItemAsync(itemId, true);
            await DataStore.DeleteItemAsync(item);
            await Shell.Current.GoToAsync("..");
        }

        public int Id { get; set; }

        public string Name
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public List<PasswordKey> Keys
        {
            get => keys;
            set => SetProperty(ref keys, value);
        }

        public int ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
            }
        }

        public async void LoadItemId(int itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId, true);
                Id = item.Id;
                Name = item.Name;
                Keys = item.Keys;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
