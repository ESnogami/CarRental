using MySqlConnector;
using System.Data;

namespace CarRentalServices.Contracts
{
    public interface IMySqlDataService
    {
        bool BulkInsert(DataTable dtData, string tableName);
        DataSet ExecSP(string nameSp);
        DataSet ExecSP(string nameSp, IEnumerable<MySqlParameter> parameters);
        DataSet ExecuteSql(string sqlText);
    }
}   