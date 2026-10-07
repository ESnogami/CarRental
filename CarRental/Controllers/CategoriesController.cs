using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using System.Data;
using System.Runtime.ConstrainedExecution;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController([FromKeyedServices("MySql")] ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet] 
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Category>), 200)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Index()
        {
            try 
            { 
                var lstCategories = _categoryRepository.GetCategories();
                return Json(lstCategories);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Category), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Category Id!");
            }
            try
            {
                var category = _categoryRepository.GetCategoryById(id);
                return category == null ? NotFound() : Ok(category);
            } 
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Category), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Create([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Invalid Category!");
            }
            try
            {
                var result = _categoryRepository.InsertCategory(category);
                return CreatedAtAction(nameof(GetById), new { id = category.IdCategory }, category);
            }
            catch (Exception ex) 
            { 
                return Problem(ex.Message);
            }
        }

        [HttpPatch]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Category), 200)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update([FromBody] Category category)
        {
            if (category == null && category?.IdCategory <= 0)
            {
                return BadRequest("Invalid Category!");
            }
            try
            {
                var result = _categoryRepository.UpdateCategory(category);
                return result ? Ok() : NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), 204)]
        public IActionResult Delete(int id)
        {
            var result = _categoryRepository.DeleteCategory(id);
            return NoContent();
        }

        
    }
}
