    using Dapper;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    namespace MyApp.Infrastructure.Helpers
    {
        public static class DapperHelper
        {

            public static async Task<DynamicParameters> MapParametersAsync<T>(IDbConnection connection, string spName, T obj)
            {
                var spParams = await GetSpParametersAsync(connection, spName);
                var dynamicParams = new DynamicParameters();

     
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var paramName in spParams)
                {
                    var cleanParamName = paramName.StartsWith("@") ? paramName.Substring(1) : paramName;

                    // Tìm thuộc tính có tên khớp (không phân biệt hoa thường)
                    var prop = properties.FirstOrDefault(p => p.Name.Equals(cleanParamName, System.StringComparison.OrdinalIgnoreCase));

                    if (prop != null)
                    {
                        dynamicParams.Add(paramName, prop.GetValue(obj));
                    }
                }

                return dynamicParams;
            }
            
            private static async Task<List<string>> GetSpParametersAsync(IDbConnection connection, string spName)
            {
            
                // Truy vấn hệ thống để lấy danh sách tham số của Store
                const string sql = @"
                    SELECT name 
                    FROM sys.parameters 
                    WHERE object_id = OBJECT_ID(@spName)";

                var parameters = await connection.QueryAsync<string>(sql, new { spName });
                var paramList = parameters.ToList();

                return paramList;
            }
        }
    }
