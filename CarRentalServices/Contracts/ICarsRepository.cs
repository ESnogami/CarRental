using CarRentalServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Contracts
{
    public interface ICarsRepository
    {
        IEnumerable<Car> GetCars();

        Car? GetCarById(int id);

        int InsertCar(Car car);

        bool UpdateCar(Car car);

        bool DeleteCar(int id);
    }
}
