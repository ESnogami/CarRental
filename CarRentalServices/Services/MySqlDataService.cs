using CarRentalServices.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Data;

namespace CarRentalServices.Services
{
    public class MySqlDataService(IConfiguration manager, ILogger<MySqlDataService> logger) : IMySqlDataService
    {
        readonly IConfiguration _configuration = manager;
        readonly ILogger<MySqlDataService> _logger = logger;
        private const string _connStringName = "MysqlConnection";
        public bool BulkInsert(DataTable dtData, string tableName)
        {
            throw new NotImplementedException();
        }

        public DataSet ExecSP(string nameSp)
        {
            return ExecSP(nameSp, []);
        }

        public DataSet ExecSP(string nameSp, IEnumerable<MySqlParameter> parameters)
        {
            var dsData = new DataSet();
            try
            {
                using var conn = new MySqlConnection(_configuration.GetConnectionString(_connStringName));
                var sqlCom = new MySqlCommand(nameSp, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                if (parameters.Any())
                {
                    sqlCom.Parameters.AddRange(parameters.ToArray());
                }
                var daDatos = new MySqlDataAdapter(sqlCom);
                daDatos.Fill(dsData);
            }
            catch (MySqlException sqle)
            {
                _logger.LogError("Error ExecSP: {Message}", sqle.Message);
            }
            return dsData;
        }

        public DataSet ExecuteSql(string sqlText)
        {
            var dsData = new DataSet();
            try
            {
                using var conn = new MySqlConnection(_configuration.GetConnectionString(_connStringName));
                var sqlCom = new MySqlCommand(sqlText, conn)
                {
                    CommandType = CommandType.Text
                };
                conn.Open();
                var daDatos = new MySqlDataAdapter(sqlCom);
                daDatos.Fill(dsData);
            }
            catch (MySqlException sqle)
            {
                _logger.LogError("Error: {message}", sqle.Message);
            }

            return dsData;
        }
    }
}
