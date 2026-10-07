using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Enums;
using CarRentalServices.Services;
using CarRentalServices.Utilities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly IMySqlDataService _mySqlDataService;

        public BrandRepository(IMySqlDataService mySqlDataService)
        {
            _mySqlDataService = mySqlDataService;
        }
        public bool DeleteBrand(int id)
        {
            throw new NotImplementedException();
        }

        public Brand? GetBrand(int id)
        {
            var data = _mySqlDataService.ExecuteSql($"SELECT * FROM brands WHERE IdBrand = {id};");
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                return MapBrands(data.Tables[0]).FirstOrDefault();
            }
            return null;
        }

        public IEnumerable<Brand> GetBrands()
        {
            var data = _mySqlDataService.ExecuteSql("select * from brands;");
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                return MapBrands(data.Tables[0]);
            }
            return [];
        }

        private IEnumerable<Brand> MapBrands(DataTable dataTable)
        {
            var lstBrands = new List<Brand>();
            foreach (DataRow row in dataTable.Rows)
            {
                var brand = new Brand
                {
                    IdBrand = Convert.ToInt32(row["IdBrand"]),
                    Name = (string)row["Name"]
                };
                lstBrands.Add(brand);
            }

            return lstBrands;
        }

        public int InsertBrand(Brand brand)
        {
            var sql = "InsertBrand";
            IEnumerable<MySqlParameter> parameters = CreateParametersMySql(brand, ActionType.Insert);
            var result = _mySqlDataService.ExecSP(sql, parameters);
            if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
               brand.IdBrand = Convert.ToInt32(result.Tables[0].Rows[0]["newBrandId"]);
            }
            return brand.IdBrand;
        }

        public bool UpdateBrand(Brand brand)
        {
            var sql = "update_brand";
            IEnumerable<MySqlParameter> parameters = CreateParametersMySql(brand, ActionType.Update);
            _mySqlDataService.ExecSP(sql, parameters);
            return true;
        }

        private static IEnumerable<MySqlParameter> CreateParametersMySql(Brand brand, ActionType actionType)
        {
            return actionType switch
            {
                ActionType.Insert => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@p_Name", MySqlDbType.VarChar, 50, brand.Name)
                },
                ActionType.Update => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@p_Name", MySqlDbType.VarChar, 50, brand.Name),
                    MySqlParameterBuilder.CreateParameter("@p_IdBrand", MySqlDbType.Int32, (object)brand.IdBrand)
                },
                ActionType.Delete => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@p_id", MySqlDbType.Int32, (object)brand.IdBrand)
                },
                _ => new List<MySqlParameter>()
            };
        }
    }
}
