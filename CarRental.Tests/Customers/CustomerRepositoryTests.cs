using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Repositories;
using Moq;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CarRental.Tests.Customers
{
    public class CustomerRepositoryTests
    {
        private readonly Mock<IMySqlDataService> _mockDataService;
        private readonly CustomerRepository _repository;

        public CustomerRepositoryTests()
        {
            _mockDataService = new Mock<IMySqlDataService>();
            _repository = new CustomerRepository(_mockDataService.Object);
        }

        #region getCustomers Tests
        [Fact]
        public void GetCustomers_WithValidData_ReturnsListOfCustomers()
        {
            // Arrange
            var dataSet = CreateCustomersDataSet(new[]
            {
                (1, "Eduardo", "Suarez", "SSN1001", "eduardo@email.com", "Mexico", 1, "9981234567", "Quintana Roo"),
                (2, "Juan", "Perez", "SSN1002", "juan@email.com", "Mexico", 1, "9991112222", "Yucatan")
            });

            _mockDataService.Setup(x => x.ExecSP(It.IsAny<string>()))
                .Returns(dataSet);

            // Act
            var result = _repository.GetCustomers();

            // Assert
            Assert.NotNull(result);
            var customers = result.ToList();
            Assert.Equal(2, customers.Count);
            Assert.Equal("Eduardo", customers[0].FirstName);
            Assert.Equal("Juan", customers[1].FirstName);
        }
        #endregion

        #region InsertCustomer Tests
        [Fact]
        public void InsertCustomer_WithValidCustomer_ReturnsTrue()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "Eduardo",
                LastName = "Suarez",
                SocialSecurityNumber = "SSN1001",
                Email = "eduardo@email.com",
                CountryName = "Mexico",
                CountryId = 1,
                MobilePhone = "9981234567",
                StateName = "Quintana Roo"
            };

            var dataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("newCustomerId", typeof(int));
            table.Rows.Add(5);
            dataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecSP(It.IsAny<string>(), It.IsAny<IEnumerable<MySqlParameter>>()))
                .Returns(dataSet);

            // Act
            var result = _repository.InsertCustomer(customer);

            // Assert
            Assert.True(result);
            Assert.Equal(5, customer.IdCustomer);
        }
        #endregion

        #region UpdateCustomer Tests
        [Fact]
        public void UpdateCustomer_WithValidCustomer_ReturnsTrue()
        {
            // Arrange
            var customer = new Customer
            {
                IdCustomer = 1,
                FirstName = "Eduardo",
                LastName = "Suarez",
                SocialSecurityNumber = "SSN1001",
                Email = "eduardo@email.com",
                CountryName = "Mexico",
                CountryId = 1,
                MobilePhone = "9981234567",
                StateName = "Quintana Roo"
            };

            var dataSet = new DataSet();
            var table = new DataTable();
            dataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecSP(It.IsAny<string>(), It.IsAny<IEnumerable<MySqlParameter>>()))
                .Returns(dataSet);

            // Act
            var result = _repository.UpdateCustomer(customer);

            // Assert
            Assert.True(result);
        }
        #endregion

        #region GetCustomer Tests
        [Fact]
        public void GetCustomerById_WithValidId_ReturnsCustomer()
        {
            // Arrange
            int customerId = 1;
            var dataSet = CreateCustomersDataSet(new[]
            {
                (1, "Eduardo", "Suarez", "SSN1001", "eduardo@email.com", "Mexico", 1, "9981234567", "Quintana Roo")
            });

            _mockDataService.Setup(x => x.ExecSP(It.IsAny<string>(), It.IsAny<IEnumerable<MySqlParameter>>()))
                .Returns(dataSet);

            // Act
            var result = _repository.GetCustomerById(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.IdCustomer);
            Assert.Equal("Eduardo", result.FirstName);
        }
        #endregion

        #region Helper Methods
        private static DataSet CreateCustomersDataSet(
            params (int Id, string FirstName, string LastName, string SSN, string Email, string Country, int CountryId, string Mobile, string StateName)[] customers)
        {
            var dataSet = new DataSet();
            var table = new DataTable();

            table.Columns.Add("IdCustomer", typeof(int));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("LastName", typeof(string));
            table.Columns.Add("SSN", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Country", typeof(string));
            table.Columns.Add("CountryId", typeof(int));
            table.Columns.Add("Mobile", typeof(string));
            table.Columns.Add("StateName", typeof(string));

            foreach (var customer in customers)
            {
                table.Rows.Add(
                    customer.Id,
                    customer.FirstName,
                    customer.LastName,
                    customer.SSN,
                    customer.Email,
                    customer.Country,
                    customer.CountryId,
                    customer.Mobile,
                    customer.StateName);
            }

            dataSet.Tables.Add(table);
            return dataSet;
        }
        #endregion
    }
}
