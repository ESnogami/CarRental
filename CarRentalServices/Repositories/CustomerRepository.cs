using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Enums;
using CarRentalServices.Services;
using CarRentalServices.Utilities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDataService _dataService;

        public CustomerRepository( IDataService dataService)
        {
            _dataService = dataService;
        }
        public bool DeleteCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public Customer? GetCustomerById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetCustomers()
        {
            var sql = "dbo.GetCustomers";
            var data = _dataService.ExecSP(sql);
            var lstCustomers = new List<Customer>();
            if(data != null && data.Tables.Count>0 && data.Tables[0].Rows.Count > 0)
            {
                lstCustomers= MapCustomers(data.Tables[0]);
            }
            return lstCustomers;
        }

        public bool InsertCustomer(Customer customer)
        {
            var sql = "InsertCustomer";
            IEnumerable<SqlParameter> parameters = CreateSqlParameters(customer, ActionType.Insert);
            var result = _dataService.ExecuteStoredProcedure(sql, parameters);
            if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                customer.IdCustomer = Convert.ToInt32(result.Tables[0].Rows[0]["newCustomerId"]);
            }
            return customer.IdCustomer > 0;
        }

        public bool UpdateCustomer(Customer customer)
        {
            var sql = "UpdateCustomer";
            IEnumerable<SqlParameter> parameters = CreateSqlParameters(customer, ActionType.Update);
            _dataService.ExecuteStoredProcedure(sql, parameters);
            return true;
        }

        #region private
        private List<Customer> MapCustomers(DataTable dataTable)
        {
            var lstCustomers = new List<Customer>();
            foreach (DataRow row in dataTable.Rows)
            {
                var customer = new Customer
                {
                    IdCustomer = Convert.ToInt32(row["IdCustomer"]),
                    FirstName = (string)row["FirstName"],
                    LastName = (string)row["LastName"],
                    SocialSecurityNumber = (string)row["SSN"],
                    Email = (string)row["Email"],
                    CountryName = (string)row["Country"],
                    MobilePhone = (string)row["Mobile"],
                    StateName = (string)row["StateName"]
                };
                lstCustomers.Add(customer);
            }
            return lstCustomers;
        }

        private static IEnumerable<SqlParameter> CreateSqlParameters(Customer customer, ActionType actionType)
        {
            return actionType switch
            {
                ActionType.Insert => new List<SqlParameter>
                {
                    SqlParameterBuilder.CreateParameter("@FirstName", SqlDbType.VarChar, 50, customer.FirstName),
                    SqlParameterBuilder.CreateParameter("@LastName", SqlDbType.VarChar, 50, customer.LastName),
                    SqlParameterBuilder.CreateParameter("@SSN", SqlDbType.VarChar, customer.SocialSecurityNumber),
                    SqlParameterBuilder.CreateParameter("@Email", SqlDbType.VarChar, customer.Email),
                    SqlParameterBuilder.CreateParameter("@CountryId", SqlDbType.Int, customer.CountryId),
                    SqlParameterBuilder.CreateParameter("@Mobile", SqlDbType.VarChar, customer.MobilePhone),
                    SqlParameterBuilder.CreateParameter("@StateName", SqlDbType.VarChar, customer.StateName)
                },
                ActionType.Update => new List<SqlParameter>
                {
                    SqlParameterBuilder.CreateParameter("@FirstName", SqlDbType.VarChar, 50, customer.FirstName),
                    SqlParameterBuilder.CreateParameter("@LastName", SqlDbType.VarChar, 50, customer.LastName),
                    SqlParameterBuilder.CreateParameter("@SSN", SqlDbType.VarChar, customer.SocialSecurityNumber),
                    SqlParameterBuilder.CreateParameter("@Email", SqlDbType.VarChar, customer.Email),
                    SqlParameterBuilder.CreateParameter("@CountryId", SqlDbType.Int, customer.CountryId),
                    SqlParameterBuilder.CreateParameter("@Mobile", SqlDbType.VarChar, customer.MobilePhone),
                    SqlParameterBuilder.CreateParameter("@StateName", SqlDbType.VarChar, customer.StateName),
                    SqlParameterBuilder.CreateParameter("@customerId", SqlDbType.Int, customer.IdCustomer)
                },
                _ => new List<SqlParameter>()
            };      
                        
        }
        #endregion
    }
}
