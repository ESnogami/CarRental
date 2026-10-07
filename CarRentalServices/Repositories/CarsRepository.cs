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
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Repositories
{
    public class CarsRepository : ICarsRepository
    {
        private readonly IMySqlDataService _mySqlDataService;
        public CarsRepository(IMySqlDataService mySqlDataService)
        {
            _mySqlDataService = mySqlDataService;
        }
        public bool DeleteCar(int id)
        {
            throw new NotImplementedException();
        }

        public Car? GetCarById (int id)
        {
            var theCar = new Car { IdCar = id, Description = string.Empty, VinNumber = string.Empty };
            var data = _mySqlDataService.ExecSP("getCarbyId", CreateParametersMySql(theCar, ActionType.SelectById));
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                return MapCars(data.Tables[0]).FirstOrDefault();
            }
            return null;
        }

        public IEnumerable<Car> GetCars()
        {

            var data = _mySqlDataService.ExecSP("get_Cars");
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                return MapCars(data.Tables[0]);
            }
            return [];
        }

        

        public int InsertCar(Car car)
        {
            var sql = "insert_cars";
            IEnumerable<MySqlParameter> parameters = CreateParametersMySql(car, ActionType.Insert);
            var result = _mySqlDataService.ExecSP(sql, parameters);
            if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                car.IdCar = Convert.ToInt32(result.Tables[0].Rows[0]["newCarId"]);
            }
            return car.IdCar;
        }

        public bool UpdateCar(Car car)
        {
            var sql = "UpdateCar";
            IEnumerable<MySqlParameter> parameters = CreateParametersMySql(car, ActionType.Update);
            _mySqlDataService.ExecSP(sql, parameters);
            return true;
        }

        #region Private Methods
        private IEnumerable<Car> MapCars(DataTable dataTable)
        {
            var lstCars = new List<Car>();
            foreach (DataRow row in dataTable.Rows)
            {
                var car = new Car
                {
                    IdCar = Convert.ToInt32(row["IdCar"]),
                    Description = (string)row["Description"],
                    VinNumber = (string)row["VIN"],
                    Model = Convert.ToInt32(row["Model"]),
                    BrandId = Convert.ToInt32(row["IdBrand"]),
                    Brand = row["BrandName"] != DBNull.Value ? (string)row["BrandName"] : null,
                    ColorId = Convert.ToInt32(row["IdColor"]),
                    Color = row["ColorName"] != DBNull.Value ? (string)row["ColorName"] : null,
                    CategoryId = Convert.ToInt32(row["IdCategory"]),
                    Category = row["CatDescription"] != DBNull.Value ? (string)row["CatDescription"] : null

                };
                lstCars.Add(car);
            }

            return lstCars;
        }

        private static IEnumerable<MySqlParameter> CreateParametersMySql(Car car, ActionType actionType)
        {
            return actionType switch
            {
                ActionType.Insert => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@p_Description", MySqlDbType.VarChar, 50, car.Description),
                    MySqlParameterBuilder.CreateParameter("@p_VIN", MySqlDbType.VarChar, 50, car.VinNumber),
                    MySqlParameterBuilder.CreateParameter("@p_Model", MySqlDbType.Int32, (object)car.Model),
                    MySqlParameterBuilder.CreateParameter("@p_BrandId", MySqlDbType.Int32, (object)car.BrandId),
                    MySqlParameterBuilder.CreateParameter("@p_ColorId", MySqlDbType.Int32, (object)car.ColorId),
                    MySqlParameterBuilder.CreateParameter("@p_CategoryId", MySqlDbType.Int32, (object)car.CategoryId),
                    MySqlParameterBuilder.CreateParameter("@p_Passengers", MySqlDbType.Int16, (object)car.Passengers)
                },
                ActionType.Update => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@p_Description", MySqlDbType.VarChar, 50, car.Description),
                    MySqlParameterBuilder.CreateParameter("@p_VIN", MySqlDbType.VarChar, 50, car.VinNumber),
                    MySqlParameterBuilder.CreateParameter("@p_Model", MySqlDbType.Int32, (object)car.Model),
                    MySqlParameterBuilder.CreateParameter("@p_BrandId", MySqlDbType.Int32, (object)car.BrandId),
                    MySqlParameterBuilder.CreateParameter("@p_ColorId", MySqlDbType.Int32, (object)car.ColorId),
                    MySqlParameterBuilder.CreateParameter("@p_CategoryId", MySqlDbType.Int32, (object)car.CategoryId),
                    MySqlParameterBuilder.CreateParameter("@p_Passengers", MySqlDbType.Int16, (object)car.Passengers),
                    MySqlParameterBuilder.CreateParameter("@p_IdCar", MySqlDbType.Int32, (object)car.IdCar)
                },
                ActionType.Delete => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@p_id", MySqlDbType.Int32, (object)car.IdCar)
                },
                ActionType.SelectById => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@p_IdCar", MySqlDbType.Int32, (object)car.IdCar)
                },
                _ => new List<MySqlParameter>()
            };
        }
        #endregion
    }
}
