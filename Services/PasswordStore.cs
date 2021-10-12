using PasswordManager.Database;
using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public class PasswordStore : IDataStore<PasswordItem, PasswordKey>
    {
        public async Task<PasswordItem> SaveItemAsync(PasswordItem item, bool children, bool forceInsert = false)
        {
            PasswordItemDatabase db = await PasswordItemDatabase.Instance;
            if(!children)
            {
                await db.SaveItemAsync(item, forceInsert);
            }
            else
            {
                await db.SaveItemWithChildrenAsync(item, forceInsert);
            }

            return await Task.FromResult(item);
        }

        public async Task<PasswordKey> SaveKeyAsync(PasswordKey key, bool forceInsert = false)
        {
            PasswordItemDatabase db = await PasswordItemDatabase.Instance;
            await db.SaveKeyAsync(key, forceInsert);
            return await Task.FromResult(key);
        }

        public async Task<bool> DeleteItemAsync<T>(int id)
        {
            PasswordItemDatabase db = await PasswordItemDatabase.Instance;
            await db.DeleteItemAsync<T>(id);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(PasswordItem item)
        {
            PasswordItemDatabase db = await PasswordItemDatabase.Instance;
            PasswordItem i = item as PasswordItem;
            await db.DeleteItemWithChildrenAsync(i);
            return await Task.FromResult(true);
        }

        public async Task<PasswordItem> GetItemAsync(int id, bool children)
        {
            PasswordItemDatabase db = await PasswordItemDatabase.Instance;
            if(!children)
                return await Task.FromResult(await db.GetItemAsync(id));

            return await Task.FromResult(await db.GetItemWithChildrenAsync(id));
        }

        public async Task<IEnumerable<PasswordItem>> GetItemsAsync(bool forceRefresh = false, bool children = false)
        {
            PasswordItemDatabase db = await PasswordItemDatabase.Instance;
            if(!children)
                return await Task.FromResult(await db.GetItemsAsync());

            return await Task.FromResult(await db.GetItemsWithChildrenAsync());
        }

        public async Task<bool> ClearItems()
        {
            PasswordItemDatabase db = await PasswordItemDatabase.Instance;
            await db.ClearItemsAsync();
            return await Task.FromResult(true);
        }

        public async Task<PasswordKey> GetKeyAsync(int id)
        {
            PasswordItemDatabase db = await PasswordItemDatabase.Instance;
            return await Task.FromResult(await db.GetKeyAsync(id));
        }
    }
}