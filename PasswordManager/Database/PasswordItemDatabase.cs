using NuGet.Common;
using PasswordManager.Models;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Database
{
    public class PasswordItemDatabase
    {
        static SQLiteAsyncConnection Database;
        public static readonly AsyncLazy<PasswordItemDatabase> Instance = new AsyncLazy<PasswordItemDatabase>(async () =>
        {
            var instance = new PasswordItemDatabase();
            CreateTableResult result = await Database.CreateTableAsync<PasswordItem>();
            result = await Database.CreateTableAsync<PasswordKey>();
            return instance;
        });

        public PasswordItemDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabaseFullPath, Constants.Flags);
        }

        // ...
        public Task<List<PasswordItem>> GetItemsAsync()
        {
            return Database.Table<PasswordItem>().ToListAsync();
        }
        public async Task<List<PasswordItem>> GetItemsWithChildrenAsync()
        {
            return await Database.GetAllWithChildrenAsync<PasswordItem>();
        }

        public async Task<PasswordItem> GetItemWithChildrenAsync(int id)
        {
            return await Database.GetWithChildrenAsync<PasswordItem>(id);
        }

        public Task<List<PasswordItem>> GetItemsNotDoneAsync()
        {
            // SQL queries are also possible
            return Database.QueryAsync<PasswordItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<PasswordItem> GetItemAsync(int id)
        {
            return Database.Table<PasswordItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(PasswordItem item, bool forceInsert = false)
        {
            if (item.Id == 0 || forceInsert)
            {
                return Database.InsertAsync(item);
            }
            else
            {
                return Database.UpdateAsync(item);
            }
        }

        public async Task<int> SaveItemWithChildrenAsync(PasswordItem item, bool forceInsert = false)
        {
            if (item.Id == 0 || forceInsert)
            {
                await Database.InsertWithChildrenAsync(item);
            }
            else
            {
                await Database.UpdateWithChildrenAsync(item);
            }

            return await Task.FromResult(0);
        }

        public Task<int> SaveKeyAsync(PasswordKey item, bool forceInsert)
        {
            if (item.Id == 0 || forceInsert)
            {
                return Database.InsertAsync(item);

            }
            else
            {
                return Database.UpdateAsync(item);
            }
        }

        public Task<PasswordKey> GetKeyAsync(int id)
        {
            return Database.Table<PasswordKey>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> DeleteItemAsync<T>(int id)
        {
            return Database.DeleteAsync<T>(id);
        }

        public Task<int> DeleteItemWithChildrenAsync(PasswordItem item)
        {
            foreach (var key in item.Keys)
            {
                Database.DeleteAsync<PasswordKey>(key.Id);
            }

            return Database.DeleteAsync<PasswordItem>(item.Id);
        }

        public async Task<int> ClearItemsAsync()
        {
            await  Database.DeleteAllAsync<PasswordItem>();
            await Database.DeleteAllAsync<PasswordKey>();
            return await Task.FromResult(0);
        }
    }
}
