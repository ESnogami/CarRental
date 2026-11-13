using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Enums;
using CarRentalServices.Services;
using CarRentalServices.Utilities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IMySqlDataService _mySqlDataService;

        public CountryRepository(IMySqlDataService mySqlDataService)
        {
            _mySqlDataService = mySqlDataService;
        }

        public bool DeleteCountry(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Country> GetCountries()
        {
            var sql = "GetAllCountries";
            var data = _mySqlDataService.ExecSP(sql);
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                return MapCountries(data.Tables[0]);
            }
            return [];
        }

        public Country? GetCountry(int id)
        {
            var sql = "GetCountryById";
            IEnumerable<MySqlParameter> parameters = CreateParametersMySql(new Country { IdCountry = id }, ActionType.SelectById);
            var data = _mySqlDataService.ExecSP(sql, parameters);
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                return MapCountries(data.Tables[0]).FirstOrDefault();
            }
            return null;
        }

        public int InsertCountry(Country country)
        {
            var sql = "InsertCountry";
            IEnumerable<MySqlParameter> parameters = CreateParametersMySql(country, ActionType.Insert);
            var result = _mySqlDataService.ExecSP(sql, parameters);
            if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                country.IdCountry = Convert.ToInt32(result.Tables[0].Rows[0]["newCountryId"]);
            }
            return country.IdCountry;
        }

        public bool UpdateCountry(Country country)
        {
            var sql = "update_country";
            IEnumerable<MySqlParameter> parameters = CreateParametersMySql(country, ActionType.Update);
            _mySqlDataService.ExecSP(sql, parameters);
            return true;
        }

        #region "Private Methods"
        private static List<Country> MapCountries(DataTable dataTable)
        {
            var lstCountries = new List<Country>();
            foreach (DataRow row in dataTable.Rows)
            {
                var country = new Country
                {
                    IdCountry = Convert.ToInt32(row["IdCountry"]),
                    Name = (string)row["Name"],
                    IsoCode = (string)row["IsoCode"]
                };
                lstCountries.Add(country);
            }

            return lstCountries;
        }

        private static IEnumerable<MySqlParameter> CreateParametersMySql(
            Country country,
            ActionType actionType)
        {
            return actionType switch
            {
                ActionType.Insert => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@p_Name", MySqlDbType.VarChar, 50, country.Name),
                    MySqlParameterBuilder.CreateParameter("@p_IsoCode", MySqlDbType.VarChar, 10, country.IsoCode)
                },
                ActionType.Update => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@p_Name", MySqlDbType.VarChar, 50, country.Name),
                    MySqlParameterBuilder.CreateParameter("@p_IsoCode", MySqlDbType.VarChar, 10, country.IsoCode),
                    MySqlParameterBuilder.CreateParameter("@p_id", MySqlDbType.Int32, (object)country.IdCountry)
                },
                ActionType.Delete => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@p_id", MySqlDbType.Int32, (object)country.IdCountry)
                },
                ActionType.SelectById => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@p_id", MySqlDbType.Int32, (object)country.IdCountry)
                },
                _ => new List<MySqlParameter>()
            };
        }
        #endregion
    }
}
