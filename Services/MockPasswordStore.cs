using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public class MockPasswordStore : PasswordStore
    {
        public MockPasswordStore()
        {
            ClearItems();
            MockPasswordItem("Google", "Password", "1234567890");
            MockPasswordItem("Netflix", "Password", "Passw0rd");
        }
        
        private async Task<bool> MockPasswordItem(string name, string key, string value)
        {
            var item = new PasswordItem { Name = name };
            item.Keys = new List<PasswordKey>();
            var savedItem = await SaveItemAsync(item, false);
            var k = new PasswordKey { Key = key, Value = value };
            await SaveKeyAsync(k);
            item.Keys.Add(k);
            await SaveItemAsync(savedItem, true);

            return await Task.FromResult(true);
        }
    }
}