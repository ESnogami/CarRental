using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Repositories
{
    public class LocationRepository: ILocationRepository
    {
        private readonly IDataService _dataService;
        private enum ActionType
        {
            Insert,
            Update,
            Delete
        }

        public LocationRepository(IDataService dataService)
        {
            _dataService = dataService;
        }

        public IEnumerable<Location> GetLocations()
        {
            var sql = "dbo.GetLocations";
            var data = _dataService.ExecSP(sql);
            var lstLocations = new List<Location>();
            if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                lstLocations = MapLocations(data.Tables[0]);
            }
            return lstLocations;
        }

        public Location? GetLocationById(int id)
        {
            throw new NotImplementedException();
        }

        public bool InsertLocation(Location location)
        {
            var sql = "InsertLocation";
            IEnumerable<SqlParameter> parameters = CreateSqlParameters(location, ActionType.Insert);
            var result = _dataService.ExecuteStoredProcedure(sql, parameters);
            if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                location.Id = Convert.ToInt32(result.Tables[0].Rows[0]["newLocationId"]);
            }
            return location.Id > 0;
        }


        public bool UpdateLocation(Location customer)
        {
            var sql = "UpdateLocation";
            IEnumerable<SqlParameter> parameters = CreateSqlParameters(customer, ActionType.Update);
            var result = _dataService.ExecuteStoredProcedure(sql, parameters);
            return true;
        }

        public bool DeleteLocation(int id)
        {
            throw new NotImplementedException();
        }

        #region private
        private List<Location> MapLocations(DataTable dataTable)
        {
            var lstLocations = new List<Location>();
            foreach (DataRow row in dataTable.Rows)
            {
                var customer = new Location
                {
                    Id = Convert.ToInt32(row["IdLocation"]),
                    StreetName = (string)row["StreetName"],
                    StreetNumber = (string)row["StreetNumber"],
                    State = (string)row["State"],
                    City = (string)row["City"],
                    CountryId = Convert.ToInt32(row["CountryId"]),
                    CountryName = (string)row["CountryName"]
                };
                lstLocations.Add(customer);
            }
            return lstLocations;
        }


        private IEnumerable<SqlParameter> CreateSqlParameters(Location location, ActionType actionType)
        {
            var parameters = new List<SqlParameter>();
            switch (actionType)
            {
                case ActionType.Insert:
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@StreetName",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = location.StreetName ?? (object)DBNull.Value
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@StreetNumber",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = location.StreetNumber ?? (object)DBNull.Value
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@City",
                        SqlDbType = SqlDbType.VarChar,
                        Value = location.City ?? (object)DBNull.Value
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@State",
                        SqlDbType = SqlDbType.VarChar,
                        Value = location.State
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@CountryId",
                        SqlDbType = SqlDbType.Int,
                        Value = location.CountryId
                    });                    
                    break;
                case ActionType.Update:
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@StreetName",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = location.StreetName ?? (object)DBNull.Value
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@StreetNumber",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = location.StreetNumber ?? (object)DBNull.Value
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@City",
                        SqlDbType = SqlDbType.VarChar,
                        Value = location.City ?? (object)DBNull.Value
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@State",
                        SqlDbType = SqlDbType.VarChar,
                        Value = location.State
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@CountryId",
                        SqlDbType = SqlDbType.Int,
                        Value = location.CountryId
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@locationId",
                        SqlDbType = SqlDbType.Int,
                        Value = location.Id
                    });
                    break;

                case ActionType.Delete:
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@LocationId",
                        SqlDbType = SqlDbType.Int,
                        Value = location.Id
                    });
                    break;
            }

            return parameters;
        }

        #endregion
    }
}
