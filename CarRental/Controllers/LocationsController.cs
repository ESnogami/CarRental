using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationsController : Controller
    {
        private readonly ILocationRepository _locationRepository;

        public LocationsController([FromKeyedServices("MsSql")] ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var lstLocations = _locationRepository.GetLocations();
            return Json(lstLocations);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Location location)
        {
            var result = _locationRepository.InsertLocation(location);
            return CreatedAtAction(
                nameof(Index), // Name of the GET action method
                new { id = location.Id }, // Route values for the GET method
                location // The newly created object to return in the body
            );

        }

        [HttpPut]
        public IActionResult Update([FromBody] Location location)
        {
            var result = _locationRepository.UpdateLocation(location);
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}