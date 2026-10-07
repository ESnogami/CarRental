using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Repositories;
using Moq;
using System.Data;
using Xunit;

namespace CarRental.Tests.Categories
{
    public class CategoryRepositoryTests
    {
        private readonly Mock<IDataService> _mockDataService;
        private readonly CategoryRepository _repository;

        public CategoryRepositoryTests()
        {
            _mockDataService = new Mock<IDataService>();
            _repository = new CategoryRepository(_mockDataService.Object);
        }

        #region GetCategories Tests

        [Fact]
        public void GetCategories_WithValidData_ReturnsListOfCategories()
        {
            // Arrange
            var dataSet = CreateCategoriesDataSet(new[]
            {
                (1, "SUV", "Sport Utility Vehicle"),
                (2, "Sedan", "Four-door passenger car")
            });

            _mockDataService.Setup(dataService => dataService.ExecuteSql(It.IsAny<string>()))
                .Returns(dataSet);

            // Act
            var result = _repository.GetCategories();

            // Assert
            Assert.NotNull(result);
            var categories = result.ToList();
            Assert.Equal(2, categories.Count);
            Assert.Equal("SUV", categories[0].Label);
            Assert.Equal("Sedan", categories[1].Label);
            Assert.Equal("Sport Utility Vehicle", categories[0].Description);
        }

        [Fact]
        public void GetCategories_WithNoData_ReturnsEmptyList()
        {
            // Arrange
            var emptyDataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("IdCategory", typeof(int));
            table.Columns.Add("Label", typeof(string));
            table.Columns.Add("Description", typeof(string));
            emptyDataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecuteSql(It.IsAny<string>()))
                .Returns(emptyDataSet);

            // Act
            var result = _repository.GetCategories();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetCategories_WithNullDataSet_ReturnsEmptyList()
        {
            // Arrange
            var emptyDataSet = new DataSet();

            _mockDataService.Setup(x => x.ExecuteSql(It.IsAny<string>()))
                .Returns(emptyDataSet);

            // Act
            var result = _repository.GetCategories();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetCategories_CallsExecuteSqlWithCorrectQuery()
        {
            // Arrange
            var emptyDataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("IdCategory", typeof(int));
            table.Columns.Add("Label", typeof(string));
            table.Columns.Add("Description", typeof(string));
            emptyDataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecuteSql(It.IsAny<string>()))
                .Returns(emptyDataSet);

            // Act
            _repository.GetCategories();

            // Assert
            _mockDataService.Verify(x => x.ExecuteSql("SELECT * FROM dbo.Categories;"), Times.Once);
        }

        #endregion

        #region GetCategoryById Tests

        [Fact]
        public void GetCategoryById_WithValidId_ReturnsCategory()
        {
            // Arrange
            int categoryId = 1;
            var dataSet = CreateCategoriesDataSet(new[]
            {
                (1, "SUV", "Sport Utility Vehicle")
            });

            _mockDataService.Setup(x => x.ExecuteSql(It.IsAny<string>()))
                .Returns(dataSet);

            // Act
            var result = _repository.GetCategoryById(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.IdCategory);
            Assert.Equal("SUV", result.Label);
            Assert.Equal("Sport Utility Vehicle", result.Description);
        }

        [Fact]
        public void GetCategoryById_WithNonExistentId_ReturnsNull()
        {
            // Arrange
            int categoryId = 999;
            var emptyDataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("IdCategory", typeof(int));
            table.Columns.Add("Label", typeof(string));
            table.Columns.Add("Description", typeof(string));
            emptyDataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecuteSql(It.IsAny<string>()))
                .Returns(emptyDataSet);

            // Act
            var result = _repository.GetCategoryById(categoryId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetCategoryById_CallsExecuteSqlWithCorrectQuery()
        {
            // Arrange
            int categoryId = 5;
            var emptyDataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("IdCategory", typeof(int));
            table.Columns.Add("Label", typeof(string));
            table.Columns.Add("Description", typeof(string));
            emptyDataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecuteSql(It.IsAny<string>()))
                .Returns(emptyDataSet);

            // Act
            _repository.GetCategoryById(categoryId);

            // Assert
            _mockDataService.Verify(x => x.ExecuteSql($"SELECT * FROM dbo.Categories WHERE IdCategory = {categoryId};"), Times.Once);
        }

        #endregion

        #region InsertCategory Tests

        [Fact]
        public void InsertCategory_WithValidCategory_ReturnsTrue()
        {
            // Arrange
            var category = new Category
            {
                Label = "Truck",
                Description = "Light truck"
            };

            var dataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("newCatId", typeof(int));
            table.Rows.Add(5); // Simulated generated ID
            dataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecuteStoredProcedure(It.IsAny<string>(), It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()))
                .Returns(dataSet);

            // Act
            var result = _repository.InsertCategory(category);

            // Assert
            Assert.True(result);
            Assert.Equal(5, category.IdCategory);
        }

        [Fact]
        public void InsertCategory_CallsExecuteStoredProcedureWithCorrectName()
        {
            // Arrange
            var category = new Category
            {
                Label = "Truck",
                Description = "Light truck"
            };

            var emptyDataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("newCatId", typeof(int));
            emptyDataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecuteStoredProcedure(It.IsAny<string>(), It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()))
                .Returns(emptyDataSet);

            // Act
            _repository.InsertCategory(category);

            // Assert
            _mockDataService.Verify(
                x => x.ExecuteStoredProcedure("InsertCategory", It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()),
                Times.Once);
        }

        [Fact]
        public void InsertCategory_WithNullResult_ReturnsTrue()
        {
            // Arrange
            var category = new Category
            {
                Label = "Truck",
                Description = "Light truck"
            };

            var emptyDataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("newCatId", typeof(int));
            emptyDataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecuteStoredProcedure(It.IsAny<string>(), It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()))
                .Returns(emptyDataSet);

            // Act
            var result = _repository.InsertCategory(category);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region UpdateCategory Tests

        [Fact]
        public void UpdateCategory_WithValidCategory_ReturnsTrue()
        {
            // Arrange
            var category = new Category
            {
                IdCategory = 1,
                Label = "Updated SUV",
                Description = "Updated description"
            };

            var dataSet = new DataSet();
            var table = new DataTable();
            dataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecuteStoredProcedure(It.IsAny<string>(), It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()))
                .Returns(dataSet);

            // Act
            var result = _repository.UpdateCategory(category);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UpdateCategory_CallsExecuteStoredProcedureWithCorrectName()
        {
            // Arrange
            var category = new Category
            {
                IdCategory = 1,
                Label = "Updated SUV",
                Description = "Updated description"
            };

            var dataSet = new DataSet();
            var table = new DataTable();
            dataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecuteStoredProcedure(It.IsAny<string>(), It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()))
                .Returns(dataSet);

            // Act
            _repository.UpdateCategory(category);

            // Assert
            _mockDataService.Verify(
                x => x.ExecuteStoredProcedure("UpdateCategory", It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()),
                Times.Once);
        }

        #endregion

        #region DeleteCategory Tests

        [Fact]
        public void DeleteCategory_WithValidId_ReturnsTrue()
        {
            // Arrange
            int categoryId = 1;
            var dataSet = new DataSet();
            var table = new DataTable();
            dataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecuteStoredProcedure(It.IsAny<string>(), It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()))
                .Returns(dataSet);

            // Act
            var result = _repository.DeleteCategory(categoryId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteCategory_CallsExecuteStoredProcedureWithCorrectName()
        {
            // Arrange
            int categoryId = 1;
            var dataSet = new DataSet();
            var table = new DataTable();
            dataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecuteStoredProcedure(It.IsAny<string>(), It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()))
                .Returns(dataSet);

            // Act
            _repository.DeleteCategory(categoryId);

            // Assert
            _mockDataService.Verify(
                x => x.ExecuteStoredProcedure("DeleteCategory", It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()),
                Times.Once);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a DataSet with categories for testing.
        /// </summary>
        private static DataSet CreateCategoriesDataSet(params (int Id, string Label, string Description)[] categories)
        {
            var dataSet = new DataSet();
            var table = new DataTable();

            table.Columns.Add("IdCategory", typeof(int));
            table.Columns.Add("Label", typeof(string));
            table.Columns.Add("Description", typeof(string));

            foreach (var category in categories)
            {
                table.Rows.Add(category.Id, category.Label, category.Description);
            }

            dataSet.Tables.Add(table);
            return dataSet;
        }

        #endregion
    }
}
