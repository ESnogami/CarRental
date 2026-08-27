using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Enums;
using CarRentalServices.Services;
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
            var parameters = new List<SqlParameter>();
            switch (actionType)
            {
                case ActionType.Insert:
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@FirstName",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = customer.FirstName ?? (object)DBNull.Value
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@LastName",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = customer.LastName ?? (object)DBNull.Value
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@SSN",
                        SqlDbType = SqlDbType.VarChar,
                        Value = customer.SocialSecurityNumber
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Email",
                        SqlDbType = SqlDbType.VarChar,
                        Value = customer.Email
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@CountryId",
                        SqlDbType = SqlDbType.Int,
                        Value = customer.CountryId
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Mobile",
                        SqlDbType = SqlDbType.VarChar,
                        Value = customer.MobilePhone
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@StateName",
                        SqlDbType = SqlDbType.VarChar,
                        Value = customer.StateName,
                    });
                    break;
                case ActionType.Update:
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@FirstName",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = customer.FirstName ?? (object)DBNull.Value
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@LastName",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = customer.LastName ?? (object)DBNull.Value
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@SSN",
                        SqlDbType = SqlDbType.VarChar,
                        Value = customer.SocialSecurityNumber
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Email",
                        SqlDbType = SqlDbType.VarChar,
                        Value = customer.Email
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@CountryId",
                        SqlDbType = SqlDbType.Int,
                        Value = customer.CountryId
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Mobile",
                        SqlDbType = SqlDbType.VarChar,
                        Value = customer.MobilePhone
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@StateName",
                        SqlDbType = SqlDbType.VarChar,
                        Value = customer.StateName,
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@customerId",
                        SqlDbType = SqlDbType.Int,
                        Value = customer.IdCustomer
                    });
                    break;

                case ActionType.Delete:
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@customerId",
                        SqlDbType = SqlDbType.Int,
                        Value = customer.IdCustomer
                    });
                    break;
            }

            return parameters;
        }
        #endregion
    }
}
