using Azure.Core.GeoJson;
using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private ICustomerRepository _customerRepository;

        public CustomersController([FromKeyedServices("MsSql")] ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var lstCustomers = _customerRepository.GetCustomers();
            return Json(lstCustomers);
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int id)
        {
            var customer = _customerRepository.GetCustomerById(id);
            return Json(customer);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Customer customer)
        {
            var result = _customerRepository.InsertCustomer(customer);
            return CreatedAtAction(
                nameof(GetCustomerById),
                new { id = customer.IdCustomer },
                customer
            );
        }

        [HttpPut]
        public IActionResult Update([FromBody] Customer customer)
        {
            var result = _customerRepository.UpdateCustomer(customer);
            return result ? Ok() : NotFound();
        }
    }
}