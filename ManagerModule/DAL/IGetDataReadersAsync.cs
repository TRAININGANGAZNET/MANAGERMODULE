using ManagerModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerModule.DAL
{
    public interface IGetDataReadersAsync
    {
        ManagerResponse SaveExecutive<T, U>(string sqlGetData, U Parameters, string myConnectionString);
        Task<IEnumerable<T>> GetChildDataAsync<T, U>(string sql, string connectionstring);
        Task<IEnumerable<T>> GetChildDataAsync<T, U>(string sql, U Parameters, string connectionstring);
        AssignExecResponse AssignExec<T, U>(string sqlGetData, U Parameters, string myConnectionString);
    }
}
