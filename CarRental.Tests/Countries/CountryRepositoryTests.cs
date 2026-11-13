using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Repositories;
using Moq;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CarRental.Tests.Countries
{
    public class CountryRepositoryTests
    {
        private readonly Mock<IMySqlDataService> _mockDataService;
        private readonly CountryRepository _repository;

        public CountryRepositoryTests()
        {
            _mockDataService = new Mock<IMySqlDataService>();
            _repository = new CountryRepository(_mockDataService.Object);
        }

        #region getCountries Tests
        [Fact]
        public void GetCountries_WithValidData_ReturnsListOfCountries()
        {
            // Arrange
            var dataSet = CreateCountriesDataSet(new[]
            {
                (1, "Mexico", "MX"),
                (2, "China", "CN")
            });

            _mockDataService.Setup(x => x.ExecSP(It.IsAny<string>()))
                .Returns(dataSet);

            // Act
            var result = _repository.GetCountries();

            // Assert
            Assert.NotNull(result);
            var countries = result.ToList();
            Assert.Equal(2, countries.Count);
            Assert.Equal("Mexico", countries[0].Name);
            Assert.Equal("China", countries[1].Name);
        }
        #endregion

        #region InsertCountry Tests
        [Fact]
        public void InsertCountry_WithValidCountry_ReturnsTrue()
        {
            // Arrange
            var country = new Country
            {
                Name = "Mexico",
                IsoCode = "MX"
            };

            var dataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("newCountryId", typeof(int));
            table.Rows.Add(5);
            dataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecSP(It.IsAny<string>(), It.IsAny<IEnumerable<MySqlParameter>>()))
                .Returns(dataSet);

            // Act
            var result = _repository.InsertCountry(country);

            // Assert
            Assert.Equal(5, country.IdCountry);
        }
        #endregion

        #region UpdateCountry Tests
        [Fact]
        public void UpdateCountry_WithValidCountry_ReturnsTrue()
        {
            // Arrange
            var country = new Country
            {
                IdCountry = 1,
                Name = "Mexico Actualizado",
                IsoCode = "MX"
            };

            var dataSet = new DataSet();
            var table = new DataTable();
            dataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecSP(It.IsAny<string>(), It.IsAny<IEnumerable<MySqlParameter>>()))
                .Returns(dataSet);

            // Act
            var result = _repository.UpdateCountry(country);

            // Assert
            Assert.True(result);
        }
        #endregion

        #region GetCountry Tests
        [Fact]
        public void GetCountryById_WithValidId_ReturnsCountry()
        {
            // Arrange
            int countryId = 1;
            var dataSet = CreateCountriesDataSet(new[]
            {
                (1, "Mexico", "MX")
            });

            _mockDataService.Setup(x => x.ExecSP(It.IsAny<string>(), It.IsAny<IEnumerable<MySqlParameter>>()))
                .Returns(dataSet);

            // Act
            var result = _repository.GetCountry(countryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.IdCountry);
            Assert.Equal("Mexico", result.Name);
        }
        #endregion

        #region Helper Methods
        private static DataSet CreateCountriesDataSet(params (int Id, string Name, string IsoCode)[] countries)
        {
            var dataSet = new DataSet();
            var table = new DataTable();

            table.Columns.Add("IdCountry", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("IsoCode", typeof(string));

            foreach (var country in countries)
            {
                table.Rows.Add(country.Id, country.Name, country.IsoCode);
            }

            dataSet.Tables.Add(table);
            return dataSet;
        }
        #endregion
    }
}
