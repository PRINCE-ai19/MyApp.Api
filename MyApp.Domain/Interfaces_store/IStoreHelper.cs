using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyApp.Domain.Interfaces_store
{
    public interface IStoreHelper
    {
        Task<IEnumerable<T>> QueryAsync<T>(string spName, object? parameters = null);
        Task<T?> QueryFirstOrDefaultAsync<T>(string spName, object? parameters = null);
        Task<int> ExecuteAsync(string spName, object? parameters = null);
        Task<IMultipleResultReader> QueryMultipleAsync(string spName, object? parameters = null);
    }
}
