using Dapper;
using MyApp.Domain.Interfaces_store;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Helpers
{
    public class DapperMultipleResultReader : IMultipleResultReader
    {
        private readonly SqlMapper.GridReader _gridReader;

        public DapperMultipleResultReader(SqlMapper.GridReader gridReader)
        {
            _gridReader = gridReader;
        }

        public async Task<IEnumerable<T>> ReadAsync<T>()
        {
            return await _gridReader.ReadAsync<T>();
        }

        public async Task<T?> ReadFirstOrDefaultAsync<T>()
        {
            return await _gridReader.ReadFirstOrDefaultAsync<T>();
        }

        public void Dispose()
        {
            _gridReader.Dispose();
        }
    }
}
