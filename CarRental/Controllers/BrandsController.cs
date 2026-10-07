using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrandsController : Controller
    {
        private readonly IBrandRepository _brandRepository;

        public BrandsController([FromKeyedServices("MySql")] IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var lstBrands = _brandRepository.GetBrands();
                return Json(lstBrands);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            try
            {
                var result = _brandRepository.InsertBrand(brand);
                return result > 0 ? CreatedAtAction(nameof(Create), new { id = result }, brand) : Problem("Failed to create brand.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Update(Brand brand)
        {
            try
            {
                var result = _brandRepository.UpdateBrand(brand);
                return result ? Ok() : NoContent();
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
                return BadRequest("Invalid Brand Id!");
            }
            try
            {
                var brand = _brandRepository.GetBrand(id);
                return brand == null ? NotFound() : Ok(brand);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
