using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Enums;
using CarRentalServices.Utilities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CarRentalServices.Repositories
{
    public class CarRepository : ICarsRepository
    {
        private readonly IDataService _dataService;

        public CarRepository(IDataService dataService)
        {
            _dataService = dataService;
        }
        public bool DeleteCar(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Car> GetCars()
        {
            var sql = "GetCars";
            var data = _dataService.ExecSP(sql);
            List<Car> lstCars = [];
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                lstCars = MapCars(data.Tables[0]);
            }
            return lstCars;
        }

        public Car? GetCarById(int id)
        {
           
            var data = _dataService.ExecuteSql($"SELECT * FROM Cars WHERE IdCar = {id};");
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                return MapCars(data.Tables[0]).FirstOrDefault();
            }
            return null;
        }


        public int InsertCar(Car car)
        {
            var sql = "InsertCar";
            IEnumerable<SqlParameter> parameters = CreateSqlParameters(car, ActionType.Insert);
            var result = _dataService.ExecuteStoredProcedure(sql, parameters);
            if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                car.IdCar = Convert.ToInt32(result.Tables[0].Rows[0]["newCarId"]);
            }
            return car.IdCar;
        }


        public bool UpdateCar(Car car)
        {
            var sql = "UpdateCar";
            IEnumerable<SqlParameter> parameters = CreateSqlParameters(car, ActionType.Update);
            _dataService.ExecuteStoredProcedure(sql, parameters);
            return true;
        }

        #region private
        private static List<Car> MapCars(DataTable dataTable)
        {
            var lstCars = new List<Car>();
            foreach (DataRow row in dataTable.Rows)
            {
                var car = new Car
                {
                    IdCar = Convert.ToInt32(row["IdCar"]),
                    VinNumber = (string)row["VIN"],
                    Description = (string)row["Description"],
                    Model = Convert.ToInt32(row["Model"]),
                    Brand = (string)row["BrandName"],
                    Color = (string)row["ColorName"],
                    Category = (string)row["catDescription"],
                    Passengers = Convert.ToByte(row["Passengers"]),
                };
                lstCars.Add(car);
            }

            return lstCars;
        }

        private static IEnumerable<SqlParameter> CreateSqlParameters(Car car, ActionType actionType)
        {
            return actionType switch
            {
                ActionType.Insert => new List<SqlParameter>
                {
                    SqlParameterBuilder.CreateParameter("@Description", SqlDbType.VarChar, 50, car.Description),
                    SqlParameterBuilder.CreateParameter("@VIN", SqlDbType.VarChar, 50, car.VinNumber),
                    SqlParameterBuilder.CreateParameter("@Model", SqlDbType.Int, (object)car.Model),
                    SqlParameterBuilder.CreateParameter("@Brand", SqlDbType.Int, (object)car.BrandId),
                    SqlParameterBuilder.CreateParameter("@Color", SqlDbType.Int, (object)car.ColorId),
                    SqlParameterBuilder.CreateParameter("@CategoryId", SqlDbType.Int, (object)car.CategoryId),
                    SqlParameterBuilder.CreateParameter("@Passengers", SqlDbType.TinyInt, (object)car.Passengers)
                },
                ActionType.Update => new List<SqlParameter>
                {
                    SqlParameterBuilder.CreateParameter("@CarId", SqlDbType.Int, (object)car.IdCar),
                    SqlParameterBuilder.CreateParameter("@Description", SqlDbType.VarChar, 50, car.Description),
                    SqlParameterBuilder.CreateParameter("@VIN", SqlDbType.VarChar, 50, car.VinNumber),
                    SqlParameterBuilder.CreateParameter("@Model", SqlDbType.Int, car.Model == 0 ? (object)DBNull.Value : car.Model),
                    SqlParameterBuilder.CreateParameter("@Brand", SqlDbType.Int, (object)car.BrandId),
                    SqlParameterBuilder.CreateParameter("@Color", SqlDbType.Int, (object)car.ColorId),
                    SqlParameterBuilder.CreateParameter("@CategoryId", SqlDbType.Int, (object)car.CategoryId),
                    SqlParameterBuilder.CreateParameter("@Passengers", SqlDbType.TinyInt, (object)car.Passengers)
                },
                ActionType.Delete => new List<SqlParameter>
                {
                    SqlParameterBuilder.CreateParameter("@IdCar", SqlDbType.Int, (object)car.IdCar)
                },
                _ => new List<SqlParameter>()
            };
        }

        #endregion
    }
}
