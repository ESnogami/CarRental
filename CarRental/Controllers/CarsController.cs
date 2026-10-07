using CarRental.Models;
using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarsController : Controller
    {
        private readonly ICarsRepository _carRepository;

        public CarsController([FromKeyedServices("MySql")] ICarsRepository carRepository)
        {
            _carRepository = carRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var lstCars = MapViewModel(_carRepository.GetCars());
            return Json(lstCars);
        }

        [HttpGet("{id}")]
        public IActionResult GetCarById(int id)
        {
            var car = _carRepository.GetCarById(id);
            return Json(car);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Car car)
        {
            var result = _carRepository.InsertCar(car);
            return CreatedAtAction(
                nameof(GetCarById), // Name of the GET action method
                new { id = car.IdCar }, // Route values for the GET method
                car // The newly created object to return in the body
            );
        }

        [HttpPut]
        public IActionResult Update([FromBody] Car car)
        {
            var result = _carRepository.UpdateCar(car);
            return result ? Ok() : NoContent();
        }

        private List<CarViewModel> MapViewModel(IEnumerable<Car> cars)
        {
            var lstCars = new List<CarViewModel>();
            foreach (var car in cars)
            {
                var carViewModel = new CarViewModel 
                {
                    IdCar = car.IdCar,
                    Description = car.Description,
                    VinNumber = car.VinNumber,
                    Model = car.Model,
                    Brand = car.Brand,
                    Color = car.Color,
                    Category = car.Category,
                };
                lstCars.Add(carViewModel);
            }

            return lstCars;
        }
    }
}
