using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyApp.Domain.Interfaces_store
{
    public interface IMultipleResultReader : IDisposable
    {
        Task<IEnumerable<T>> ReadAsync<T>();
        Task<T?> ReadFirstOrDefaultAsync<T>();
    }
}
