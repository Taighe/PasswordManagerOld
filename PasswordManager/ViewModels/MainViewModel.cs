using PasswordManager.Models;
using PasswordManager.Utilities;
using PasswordManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PasswordManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<PasswordItem> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command FindDBCommand { get; }
        public Command DeleteAllCommand { get; }
        public Command ImportCommand { get; }
        public Command ExportCommand { get; }
        public Command<PasswordItem> ItemTapped { get; }
        private PasswordItem _selectedItem;

        public MainViewModel()
        {
            Title = "Passwords";
            Items = new ObservableCollection<PasswordItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<PasswordItem>(OnItemSelected);
            AddItemCommand = new Command(OnAddItem);
            FindDBCommand = new Command(OnFindDB);
            DeleteAllCommand = new Command(OnDeleteAll);
            ImportCommand = new Command(OnImport);
            ExportCommand = new Command(OnExport);
        }

        private async void OnExport(object obj)
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Export Passwords", "Are you sure you want to export your passwords to file? This can be shared between other devices with PasswordManager installed.", "Yes", "No");
            if (answer)
            {
                var items = await DataStore.GetItemsAsync(false, true);
                var path = await ImportExportUtil.ExportAsync(items);
                if(path != null)
                {
                    answer = await App.Current.MainPage.DisplayAlert("Passwords Exported", $"Passwords saved to {path}", "Copy", "Cancel");
                    if (answer)
                        await Clipboard.SetTextAsync(path);
                }
            }
        }

        private async void OnImport(object obj)
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Import Passwords", "Are you sure you want to import passwords from file? This will overwrite your current passwords. Also the imported file will be deleted.", "Yes", "No");
            if (answer)
            {
                FileResult result = await FilePicker.PickAsync(PickOptions.Default);
                if(result != null)
                {
                    try
                    {
                        var items = await ImportExportUtil.ImportAsync(result.FullPath);

                        await DataStore.ClearItems();

                        foreach (var item in items)
                        {
                            PasswordItem pi = await DataStore.SaveItemAsync(item, true, true);
                            foreach(var key in pi.Keys)
                            {
                                await DataStore.SaveKeyAsync(key, true);
                            }
                        }

                        if(File.Exists(result.FullPath))
                        {
                            File.Delete(result.FullPath);
                        }

                        await ExecuteLoadItemsCommand();
                        await App.Current.MainPage.DisplayAlert("Passwords Imported", "Passwords imported successfuly.", "OK");
                    }
                    catch(Exception e)
                    {
                        await App.Current.MainPage.DisplayAlert("Could not import Passwords", "There was a problem importing your passwords.", "OK");
                        return;
                    }
                }
            }
        }

        private async void OnDeleteAll(object obj)
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Delete All Passwords", "Are you sure you want to delete all your passwords? (This cannot be undone.)", "Yes", "No");
            if (answer)
            {
                await DataStore.ClearItems();
                await ExecuteLoadItemsCommand();
            }
        }

        private async void OnFindDB(object obj)
        {
            var path = Constants.DatabasePath;
            bool answer = await App.Current.MainPage.DisplayAlert("Your Database", Constants.DatabaseFullPath, "Copy", "Cancel");

            if (answer)
                await Clipboard.SetTextAsync(path);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                var items = await DataStore.GetItemsAsync(true);
                Items.Clear();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
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

        public PasswordItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(PasswordItem item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(PasswordItemDetailPage)}?{nameof(PasswordItemDetailViewModel.ItemId)}={item.Id}");
        }
    }
}
