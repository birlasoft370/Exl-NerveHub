
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MicSer.Security.UserMgt.Repository;

//public class DapperUtility
//{
//    static Lazy<DapperUtility>? objDapper = null;
//    private static string connStr = string.Empty;

//    public static DapperUtility CreateInstance(string connString)
//    {
//        connStr = connString;

//        objDapper ??= new Lazy<DapperUtility>();

//        return objDapper.Value;
//    }

//    internal static IDbConnection GetOpenConnection()
//    {
//        var connection = new SqlConnection(connStr);
//        connection.Open();
//        return connection;
//    }

//    public async Task<IEnumerable<T>> GetAllAsync<T>(string storedProcName, DynamicParameters DynamicParameters) where T : class
//    {
//        using var connection = GetOpenConnection();
//        var result = (await connection.QueryAsync<T>(storedProcName, DynamicParameters, commandType: CommandType.StoredProcedure));
//        return result.ToList();
//    }


//    public async Task<T> GetAsync<T>(string storedProcName, DynamicParameters dynamicParameters) where T : class
//    {
//        using var connection = GetOpenConnection();
//        return (await connection.QueryAsync<T>(storedProcName, dynamicParameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
//    }

//    public async Task<T> Get<T>(string queryText, DynamicParameters dynamicParameters) where T : class
//    {
//        using var connection = GetOpenConnection();
//        return (await connection.QueryAsync<T>(queryText.ToString(), param: dynamicParameters)).FirstOrDefault();
//    }

//}
