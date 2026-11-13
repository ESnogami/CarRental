using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        private readonly ICountryRepository _countryRepository;

        public CountriesController([FromKeyedServices("MySql")] ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var lstCountries = _countryRepository.GetCountries();
                return Json(lstCountries);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Country Id!");
            }
            try
            {
                var country = _countryRepository.GetCountry(id);
                return country == null ? NotFound() : Ok(country);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Create(Country country)
        {
            try
            {
                var result = _countryRepository.InsertCountry(country);
                return result > 0 ? CreatedAtAction(nameof(GetById), new { id = result }, country) : Problem("Failed to create country.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Update(Country country)
        {
            try
            {
                var result = _countryRepository.UpdateCountry(country);
                return result ? Ok() : NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
