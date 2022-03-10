using System;
using System.Threading.Tasks;
using DigitalSignEditor.Data.Models;

namespace DigitalSignEditor.Data {
    public interface ISignRepository {
        int Create<T>(T item) where T : BaseObject;
        Task<int> CreateAsync<T>(T item) where T : BaseObject;
        int Delete<T>(T item);
        Task<int> DeleteAsync<T>(T item);
        int MakeActive<T>(T item, bool active) where T : BaseObject;
        Task<int> MakeActiveAsync<T>(T item, bool active) where T : BaseObject;
        T Read<T>(Func<SignContext, T> work);
        Task<T> ReadAsync<T>(Func<SignContext, T> work);
        int Update<T>(T item) where T : BaseObject;
        Task<int> UpdateAsync<T>(T item) where T : BaseObject;
    }
}