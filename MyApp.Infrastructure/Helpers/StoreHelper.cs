using Dapper;
using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Interfaces_store;
using MyApp.Infrastructure.Data.Context;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Helpers
{
    public class StoreHelper : IStoreHelper
    {
        private readonly AppDbContext _context;

        public StoreHelper(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string spName, object? parameters = null)
        {
            var connection = _context.Database.GetDbConnection();
            var dParams = await DapperHelper.MapParametersAsync(connection, spName, parameters);
            return await connection.QueryAsync<T>(
                spName,
                dParams,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string spName, object? parameters = null)
        {
            var connection = _context.Database.GetDbConnection();
            var dParams = await DapperHelper.MapParametersAsync(connection, spName, parameters);
            return await connection.QueryFirstOrDefaultAsync<T>(
                spName,
                dParams,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ExecuteAsync(string spName, object? parameters = null)
        {
            var connection = _context.Database.GetDbConnection();
            var dParams = await DapperHelper.MapParametersAsync(connection, spName, parameters);
            return await connection.ExecuteAsync(
                spName,
                dParams,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
