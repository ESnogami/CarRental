using CarRentalServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Contracts
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetCustomers();

        Customer? GetCustomerById(int id);

        bool InsertCustomer(Customer customer);

        bool UpdateCustomer(Customer customer);

        bool DeleteCustomer(int id);
    }
}
