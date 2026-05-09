using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CarRentalServices.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly IDataService _dataService;
        private enum ActionType
        {
            Insert,
            Update,
            Delete
        }

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
            throw new NotImplementedException();
        }

        public bool InsertCar(Car car)
        {
            var sql = "InsertCar";
            IEnumerable<SqlParameter> parameters = CreateSqlParameters(car, ActionType.Insert);
            var result = _dataService.ExecuteStoredProcedure(sql, parameters);
            if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                car.IdCar = Convert.ToInt32(result.Tables[0].Rows[0]["newCarId"]);
            }
            return car.IdCar >0;
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
            var parameters = new List<SqlParameter>();
            switch (actionType)
            {
                case ActionType.Insert:
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Description",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = car.Description ?? (object)DBNull.Value
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@VIN",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = car.VinNumber ?? (object)DBNull.Value
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Model",
                        SqlDbType = SqlDbType.Int,
                        Value = car.Model
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Brand",
                        SqlDbType = SqlDbType.Int,
                        Value = car.BrandId
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Color",
                        SqlDbType = SqlDbType.Int,
                        Value = car.ColorId
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@CategoryId",
                        SqlDbType = SqlDbType.Int,
                        Value = car.CategoryId
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Passengers",
                        SqlDbType = SqlDbType.TinyInt,
                        Value = car.Passengers,
                    });
                    break;
                case ActionType.Update:
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@CarId",
                        SqlDbType = SqlDbType.Int,
                        Value = car.IdCar 
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Description",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = car.Description ?? (object)DBNull.Value
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@VIN",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = car.VinNumber ?? (object)DBNull.Value
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Model",
                        SqlDbType = SqlDbType.Int,
                        Value = car.Model == 0 ? DBNull.Value : car.Model
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Brand",
                        SqlDbType = SqlDbType.Int,
                        Value = car.BrandId
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Color",
                        SqlDbType = SqlDbType.Int,
                        Value = car.ColorId
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@CategoryId",
                        SqlDbType = SqlDbType.Int,
                        Value = car.CategoryId
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Passengers",
                        SqlDbType = SqlDbType.TinyInt,
                        Value = car.Passengers,
                    });
                    break;
                case ActionType.Delete:
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@IdCar",
                        SqlDbType = SqlDbType.Int,
                        Value = car.IdCar 
                    });
                    break;
            }

            return parameters;
        }
        #endregion
    }
}
