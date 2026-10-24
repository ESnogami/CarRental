using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Repositories;
using Moq;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CarRental.Tests.Brands
{
    public class BrandRepositoryTests
    {

        private readonly Mock<IMySqlDataService> _mockDataService;
        private readonly BrandRepository _repository;

        public BrandRepositoryTests()
        {
            _mockDataService = new Mock<IMySqlDataService>();
            _repository = new BrandRepository(_mockDataService.Object);
        }

        #region getBrands Tests
        [Fact]
        public void GetBrands_WithValidData_ReturnsListOfBrands()
        {
            // Arrange
            var dataSet = CreateBrandsDataSet(new[]
            {
                (1, "Toyota"),
                (2, "Honda")
            });

            _mockDataService.Setup(x => x.ExecuteSql(It.IsAny<string>()))
              .Returns(dataSet);

            // Act
            var result = _repository.GetBrands();

            // Assert
            Assert.NotNull(result);
            var brands = result.ToList();
            Assert.Equal(2, brands.Count);
            Assert.Equal("Toyota", brands[0].Name);
            Assert.Equal("Honda", brands[1].Name);
        }
        #endregion

        #region InsertBrand Tests
        [Fact]
        public void InsertBrand_WithValidColor_ReturnsTrue()
        {
            // Arrange
            var color = new Brand
            {
                Name = "Toyota"
            };

            var dataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("newBrandId", typeof(int));
            table.Rows.Add(5); // Simulated generated ID
            dataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecSP(It.IsAny<string>(), It.IsAny<IEnumerable<MySqlParameter>>()))
                .Returns(dataSet);

            // Act
            var result = _repository.InsertBrand(color);

            // Assert

            Assert.Equal(5, color.IdBrand);
        }
        #endregion
        #region Helper Methods

        /// <summary>
        /// Creates a DataSet with categories for testing.
        /// </summary>
        private static DataSet CreateBrandsDataSet(params (int Id, string Name)[] brands
            )
        {
            var dataSet = new DataSet();
            var table = new DataTable();

            table.Columns.Add("IdBrand", typeof(int));
            table.Columns.Add("Name", typeof(string));


            foreach (var brand in brands)
            {
                table.Rows.Add(brand.Id, brand.Name);
            }

            dataSet.Tables.Add(table);
            return dataSet;
        }

        #endregion

       
    }
}
