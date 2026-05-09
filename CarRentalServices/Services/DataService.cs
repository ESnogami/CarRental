using CarRentalServices.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace CarRentalServices.Services
{
    public class DataService(IConfiguration manager, ILogger<DataService> logger) : IDataService
    {

        readonly IConfiguration _configuration = manager;
        readonly ILogger<DataService> _logger = logger;
        private const string _connStringName = "CarRental";

        public DataSet ExecuteStoredProcedure(string nameSp, IEnumerable<IDbDataParameter> parameters)
        {
            var dsData = new DataSet();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString(_connStringName));                
                var sqlCom = new SqlCommand(nameSp, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                if (parameters.Any())
                {
                    sqlCom.Parameters.AddRange(parameters.ToArray());
                }
                var daDatos = new SqlDataAdapter(sqlCom);
                daDatos.Fill(dsData);
            }
            catch (SqlException sqle)
            {
                _logger.LogError(sqle, "Error executing stored procedure {StoredProcedure}: {ErrorMessage}", nameSp, sqle.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error executing stored procedure {StoredProcedure}: {ErrorMessage}", nameSp, ex.Message);
            }
            return dsData;
        }

        public DataSet ExecSP(string nameSp)
        {            
            return ExecuteStoredProcedure(nameSp, []);
        }

        public bool BulkInsert(DataTable dtData, string tableName)
        {
            using var bulkCopy = new SqlBulkCopy(_configuration.GetConnectionString(_connStringName));
            bulkCopy.DestinationTableName = "dbo." + tableName;
            try
            {
                // Write from the source to the destination.                   
                bulkCopy.WriteToServer(dtData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bulk insert error to {Table}: {ErrorMessage}", tableName, ex.Message);
                return false;
            }
            return true;
        }

        public DataSet ExecuteSql(string sqlText)
        {
            var dsData = new DataSet();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString(_connStringName));
                var sqlCom = new SqlCommand(sqlText, conn)
                {
                    CommandType = CommandType.Text
                };
                conn.Open();
                var daDatos = new SqlDataAdapter(sqlCom);
                daDatos.Fill(dsData);
            }
            catch (SqlException sqle)
            {
                _logger.LogError(sqle, "SQL error executing query");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error executing query");
            }
            
            return dsData;
        }


    }
}
