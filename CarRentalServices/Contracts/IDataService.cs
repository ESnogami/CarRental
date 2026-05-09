using Microsoft.Data.SqlClient;
using System.Data;
//using MongoDB.Bson;
//using MongoDB.Driver;

namespace CarRentalServices.Contracts
{
    public interface IDataService
    {
        bool BulkInsert(DataTable dtData, string tableName);
        DataSet ExecSP(string nameSp);
        DataSet ExecuteStoredProcedure(string nameSp, IEnumerable<IDbDataParameter> parameters);
        DataSet ExecuteSql(string sqlText);

    }
}