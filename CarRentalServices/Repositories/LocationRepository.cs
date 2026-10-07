using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Enums;
using CarRentalServices.Utilities;
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
            return actionType switch
            {
                ActionType.Insert => new List<SqlParameter>
                {
                    SqlParameterBuilder.CreateParameter("@StreetName", SqlDbType.VarChar, 50, location.StreetName),
                    SqlParameterBuilder.CreateParameter("@StreetNumber", SqlDbType.VarChar, 50, location.StreetNumber),
                    SqlParameterBuilder.CreateParameter("@City", SqlDbType.VarChar, location.City),
                    SqlParameterBuilder.CreateParameter("@State", SqlDbType.VarChar, location.State),
                    SqlParameterBuilder.CreateParameter("@CountryId", SqlDbType.Int, (object)location.CountryId)
                },
                ActionType.Update => new List<SqlParameter>
                {
                    SqlParameterBuilder.CreateParameter("@StreetName", SqlDbType.VarChar, 50, location.StreetName),
                    SqlParameterBuilder.CreateParameter("@StreetNumber", SqlDbType.VarChar, 50, location.StreetNumber),
                    SqlParameterBuilder.CreateParameter("@City", SqlDbType.VarChar, location.City),
                    SqlParameterBuilder.CreateParameter("@State", SqlDbType.VarChar, location.State),
                    SqlParameterBuilder.CreateParameter("@CountryId", SqlDbType.Int, (object)location.CountryId),
                    SqlParameterBuilder.CreateParameter("@locationId", SqlDbType.Int, (object)location.Id)
                },
                ActionType.Delete => new List<SqlParameter>
                {
                    SqlParameterBuilder.CreateParameter("@LocationId", SqlDbType.Int, (object)location.Id)
                },
                _ => new List<SqlParameter>()
            };
        }

        #endregion
    }
}
