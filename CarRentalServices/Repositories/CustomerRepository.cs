using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Enums;
using CarRentalServices.Services;
using CarRentalServices.Utilities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IMySqlDataService _mySqlDataService;

        public CustomerRepository(IMySqlDataService mySqlDataService)
        {
            _mySqlDataService = mySqlDataService;
        }

        public bool DeleteCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public Customer? GetCustomerById(int id)
        {
            var sql = "GetCustomerById";
            IEnumerable<MySqlParameter> parameters = CreateParametersMySql(new Customer { IdCustomer = id }, ActionType.SelectById);
            var data = _mySqlDataService.ExecSP(sql, parameters);
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                return MapCustomers(data.Tables[0]).FirstOrDefault();
            }
            return null;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            var sql = "GetCustomers";
            var data = _mySqlDataService.ExecSP(sql);
            var lstCustomers = new List<Customer>();
            if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                lstCustomers = MapCustomers(data.Tables[0]);
            }
            return lstCustomers;
        }

        public bool InsertCustomer(Customer customer)
        {
            var sql = "InsertCustomer";
            IEnumerable<MySqlParameter> parameters = CreateParametersMySql(customer, ActionType.Insert);
            var result = _mySqlDataService.ExecSP(sql, parameters);
            if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                customer.IdCustomer = Convert.ToInt32(result.Tables[0].Rows[0]["newCustomerId"]);
            }
            return customer.IdCustomer > 0;
        }

        public bool UpdateCustomer(Customer customer)
        {
            var sql = "UpdateCustomer";
            IEnumerable<MySqlParameter> parameters = CreateParametersMySql(customer, ActionType.Update);
            _mySqlDataService.ExecSP(sql, parameters);
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
                    CountryId = Convert.ToInt32(row["CountryId"]),
                    MobilePhone = (string)row["Mobile"],
                    StateName = (string)row["StateName"]
                };
                lstCustomers.Add(customer);
            }
            return lstCustomers;
        }

        private static IEnumerable<MySqlParameter> CreateParametersMySql(Customer customer, ActionType actionType)
        {
            return actionType switch
            {
                ActionType.Insert => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@p_FirstName", MySqlDbType.VarChar, 50, customer.FirstName),
                    MySqlParameterBuilder.CreateParameter("@p_LastName", MySqlDbType.VarChar, 50, customer.LastName),
                    MySqlParameterBuilder.CreateParameter("@p_SSN", MySqlDbType.VarChar, 20, customer.SocialSecurityNumber),
                    MySqlParameterBuilder.CreateParameter("@p_Email", MySqlDbType.VarChar, 100, customer.Email),
                    MySqlParameterBuilder.CreateParameter("@p_CountryId", MySqlDbType.Int32, (object)customer.CountryId),
                    MySqlParameterBuilder.CreateParameter("@p_Mobile", MySqlDbType.VarChar, 20, customer.MobilePhone),
                    MySqlParameterBuilder.CreateParameter("@p_StateName", MySqlDbType.VarChar, 50, customer.StateName)
                },
                ActionType.Update => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@p_FirstName", MySqlDbType.VarChar, 50, customer.FirstName),
                    MySqlParameterBuilder.CreateParameter("@p_LastName", MySqlDbType.VarChar, 50, customer.LastName),
                    MySqlParameterBuilder.CreateParameter("@p_SSN", MySqlDbType.VarChar, 20, customer.SocialSecurityNumber),
                    MySqlParameterBuilder.CreateParameter("@p_Email", MySqlDbType.VarChar, 100, customer.Email),
                    MySqlParameterBuilder.CreateParameter("@p_CountryId", MySqlDbType.Int32, (object)customer.CountryId),
                    MySqlParameterBuilder.CreateParameter("@p_Mobile", MySqlDbType.VarChar, 20, customer.MobilePhone),
                    MySqlParameterBuilder.CreateParameter("@p_StateName", MySqlDbType.VarChar, 50, customer.StateName),
                    MySqlParameterBuilder.CreateParameter("@p_customerId", MySqlDbType.Int32, (object)customer.IdCustomer)
                },
                ActionType.SelectById => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@p_id", MySqlDbType.Int32, (object)customer.IdCustomer)
                },
                _ => new List<MySqlParameter>()
            };
        }
        #endregion
    }
}
