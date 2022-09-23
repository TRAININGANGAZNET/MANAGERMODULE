using Dapper;
using ManagerModule.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerModule.DAL
{
    public class GetDataReadersAsync: IGetDataReadersAsync
    {
        private readonly IConfiguration _configuration;
        public string myConnectionString = string.Empty;
        public GetDataReadersAsync(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public ManagerResponse SaveExecutive<T, U>(string func, U Parameters, string myConnectionString)
        {
            ManagerResponse Reslist = new ManagerResponse();
            try
            {
                Reslist.id = SaveDataScalar<T, U>(Parameters, myConnectionString, func).ToString();
                if (Reslist.id != null)
                {
                    Reslist.Responses = _configuration["Responses:ExecutiveSucess"];
                }
            }
            catch (Exception ex)
            {
                Reslist.Responses = _configuration["Responses:FailMessage"];
            }
            return (Reslist);
        }

        public object SaveDataScalar<T, U>(U Parameters, string connectionstring, string spname)
        {
            object obj = new object();
            using (IDbConnection connection = new SqlConnection(connectionstring))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                using (IDbTransaction _dbTransaction = connection.BeginTransaction())
                {
                    try
                    {
                        obj = connection.ExecuteScalar(spname, Parameters, commandType: CommandType.StoredProcedure);
                        _dbTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        _dbTransaction.Rollback();
                    }
                    return obj;
                }
            }
        }

        public AssignExecResponse AssignExec<T, U>(string sqlGetData, U Parameters, string myConnectionString)
        {
            AssignExecResponse assignExecResponse = new AssignExecResponse();
            try
            {
                object res = SaveDataScalar<T, U>(sqlGetData, Parameters, myConnectionString).ToString();
                int ExcutiveID;
                bool isinserted = Int32.TryParse(res.ToString(), out ExcutiveID);
                if (isinserted)
                {
                    assignExecResponse.ExcutiveID = ExcutiveID;
                    if (ExcutiveID != 0)
                    {
                        assignExecResponse.Responses = _configuration["Responses:UpdateExecutive"];
                    }
                    else
                    {
                        assignExecResponse.Responses = _configuration["Responses:FailReferences"];
                    }
                }

                else
                {
                    assignExecResponse.Responses = _configuration["Responses:FailReferences"];
                }
            } catch (Exception ex)

            {
                assignExecResponse.Responses = _configuration["Responses:FailReferences"];
            }
            return assignExecResponse;
        }
            
            
        

        public async Task<IEnumerable<T>> GetChildDataAsync<T, U>(string sql, string connectionstring)
        {
            List<T> DataList = new List<T>();
            try
            {
                DataList = await LoadData<T, dynamic>(sql, connectionstring);
            }
            catch (Exception ex)
            {
                return null;
            }
            return DataList;
        }

        public async Task<List<T>> LoadData<T, U>(string sql, string connectionstring)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(connectionstring))
                {
                    var rows = await connection.QueryAsync<T>(sql);
                    return rows.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<IEnumerable<T>> GetChildDataAsync<T, U>(string sql, U param, string connectionstring)
        {
            List<T> DataList = new List<T>();
            try
            {
                DataList = await LoadData<T, dynamic>(sql, param, connectionstring);
            }
            catch (Exception ex)
            {
                return null;
            }
            return DataList;
        }

        public async Task<List<T>> LoadData<T, U>(string sql, U Parameters, string connectionstring)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(connectionstring))
                {
                    var rows = await connection.QueryAsync<T>(sql, Parameters);
                    return rows.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public object SaveDataScalar<T, U>(string sql, U Parameters, string connectionstring)
        {
            object obj = new object();
            using (IDbConnection connection = new SqlConnection(connectionstring))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                using (IDbTransaction _dbTransaction = connection.BeginTransaction())
                {
                    try
                    {
                        obj = connection.ExecuteScalar(sql, Parameters);
                        _dbTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        _dbTransaction.Rollback();

                        throw new Exception(ex.Message);
                        //_globalLogger.LogError("Event ID - {0} - Exception - {1}: ", GlobalEngine.HISEventID.GlobalEvent.GetLoggerEventId.Id, ex.Message + ex.StackTrace);
                    }
                    return obj;
                }
            }
        }
    }
}
