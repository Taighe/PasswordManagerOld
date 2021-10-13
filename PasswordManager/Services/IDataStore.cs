using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public interface IDataStore<T, D>
    {
        Task<T> SaveItemAsync(T item, bool children, bool forceInsert = false);
        Task<D> SaveKeyAsync(D key, bool forceInsert = false);
        Task<bool> DeleteItemAsync<E>(int id);
        Task<bool> DeleteItemAsync(T item);

        Task<bool> ClearItems();
        Task<T> GetItemAsync(int id, bool children);
        Task<D> GetKeyAsync(int id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false, bool children = false);
    }
}
