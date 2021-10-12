using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PasswordManager.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    [QueryProperty(nameof(KeyId), nameof(KeyId))]
    public class NewKeyViewModel : BaseViewModel
    {
        private string _key;
        private string _value;
        private int _itemId;
        private string _passwordName;
        private int _keyId;

        public NewKeyViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(_key);
        }

        public int KeyId
        {
            get
            {
                return _keyId;
            }
            set
            {
                _keyId = value;
            }
        }

        public int ItemId
        {
            get
            {
                return _itemId;
            }
            set
            {
                _itemId = value;
            }
        }

        public string PasswordName
        {
            get => _passwordName;
            set => SetProperty(ref _passwordName, value);
        }

        public string Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public void OnAppearing()
        {
            Task.Run(async() =>
            {
                var item = await DataStore.GetItemAsync(_itemId, false);
                PasswordName = item.Name;
                if(_keyId != 0)
                {
                    PasswordKey key = await DataStore.GetKeyAsync(_keyId);
                    Key = key.Key;
                    Value = key.Value;
                }
            });
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            PasswordKey newKey = new PasswordKey()
            {
                Id = KeyId,
                Key = Key,
                Value = Value
            };

            await DataStore.SaveKeyAsync(newKey);
            PasswordItem item = await DataStore.GetItemAsync(_itemId, true);
            item.Keys.Add(newKey);
            await DataStore.SaveItemAsync(item, true);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
