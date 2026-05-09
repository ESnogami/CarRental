using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarsController : Controller
    {
        private readonly ICarRepository _carRepository;

        public CarsController([FromKeyedServices("MsSql")] ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var lstCars = _carRepository.GetCars();
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
    }
}
