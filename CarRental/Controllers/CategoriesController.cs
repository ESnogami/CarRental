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
        //private readonly IDataService _dataService;
        //private readonly IMySqlDataService _mySqlDataService;
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController([FromKeyedServices("MsSql")] ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            
            var lstCategories = _categoryRepository.GetCategories();
            return Json(lstCategories);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        { 
            var category = _categoryRepository.GetCategoryById(id);
            return Json(category);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Category category)
        {
            var result = _categoryRepository.InsertCategory(category);
            return CreatedAtAction( nameof(GetById),  new { id = category.IdCategory },  category );
        }

        [HttpPatch]
        public IActionResult Update([FromBody] Category category)
        {
            var result = _categoryRepository.UpdateCategory(category);
            return result ? Ok() : NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var sql = "DeleteCategory";
            var result = _categoryRepository.DeleteCategory(id);
            return NoContent();
        }

        
    }
}
