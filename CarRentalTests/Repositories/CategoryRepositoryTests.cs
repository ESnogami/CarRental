using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Repositories;
using Moq;
using System.Data;

namespace CarRentalTests.Repositories
{
    public class CategoryRepositoryTests
    {
        private readonly Mock<IDataService> _mockDataService;
        private readonly CategoryRepository _categoryRepository;

        public CategoryRepositoryTests()
        {
            _mockDataService = new Mock<IDataService>();
            _categoryRepository = new CategoryRepository(_mockDataService.Object);
        }

        #region Helper Methods

        private static DataSet CreateDataSetWithCategories(dynamic[] categories)
        {
            var dataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("IdCategory", typeof(int));
            table.Columns.Add("Label", typeof(string));
            table.Columns.Add("Description", typeof(string));

            foreach (var category in categories)
            {
                table.Rows.Add(category.IdCategory, category.Label, category.Description);
            }

            dataSet.Tables.Add(table);
            return dataSet;
        }

        private static DataSet CreateDataSetWithInsertResult(int newCatId)
        {
            var dataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("newCatId", typeof(int));
            table.Rows.Add(newCatId);
            dataSet.Tables.Add(table);
            return dataSet;
        }

        #endregion

        #region GetCategories Tests

        [Fact]
        public void GetCategories_WhenDataServiceReturnsData_ShouldReturnCategories()
        {
            // Arrange
            var dataSet = CreateDataSetWithCategories(
            [
                new { IdCategory = 1, Label = "Sedan", Description = "4-door sedan" },
                new { IdCategory = 2, Label = "SUV", Description = "Sport utility vehicle" }
            ]);

            _mockDataService
                .Setup(ds => ds.ExecuteSql("SELECT * FROM dbo.Categories;"))
                .Returns(dataSet);

            // Act
            var result = _categoryRepository.GetCategories();

            // Assert
            Assert.NotNull(result);
            var categories = result.ToList();
            Assert.Equal(2, categories.Count);
            Assert.Equal("Sedan", categories[0].Label);
            Assert.Equal("SUV", categories[1].Label);
            _mockDataService.Verify(ds => ds.ExecuteSql("SELECT * FROM dbo.Categories;"), Times.Once);
        }

        [Fact]
        public void GetCategories_WhenDataServiceReturnsEmptyDataSet_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyDataSet = new DataSet();
            _mockDataService
                .Setup(ds => ds.ExecuteSql("SELECT * FROM dbo.Categories;"))
                .Returns(emptyDataSet);

            // Act
            var result = _categoryRepository.GetCategories();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockDataService.Verify(ds => ds.ExecuteSql("SELECT * FROM dbo.Categories;"), Times.Once);
        }

        [Fact]
        public void GetCategories_WhenDataServiceReturnsEmptyTable_ShouldReturnEmptyList()
        {
            // Arrange
            var dataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("IdCategory", typeof(int));
            table.Columns.Add("Label", typeof(string));
            table.Columns.Add("Description", typeof(string));
            dataSet.Tables.Add(table);

            _mockDataService
                .Setup(ds => ds.ExecuteSql("SELECT * FROM dbo.Categories;"))
                .Returns(dataSet);

            // Act
            var result = _categoryRepository.GetCategories();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetCategoryById Tests

        [Fact]
        public void GetCategoryById_WhenCategoryExists_ShouldReturnCategory()
        {
            // Arrange
            var categoryId = 1;
            var dataSet = CreateDataSetWithCategories(new[]
            {
                new { IdCategory = 1, Label = "Sedan", Description = "4-door sedan" }
            });

            _mockDataService
                .Setup(ds => ds.ExecuteSql($"SELECT * FROM dbo.Categories WHERE IdCategory = {categoryId};"))
                .Returns(dataSet);

            // Act
            var result = _categoryRepository.GetCategoryById(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.IdCategory);
            Assert.Equal("Sedan", result.Label);
            Assert.Equal("4-door sedan", result.Description);
            _mockDataService.Verify(
                ds => ds.ExecuteSql($"SELECT * FROM dbo.Categories WHERE IdCategory = {categoryId};"),
                Times.Once);
        }

        [Fact]
        public void GetCategoryById_WhenCategoryDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var categoryId = 999;
            var emptyDataSet = new DataSet();

            _mockDataService
                .Setup(ds => ds.ExecuteSql($"SELECT * FROM dbo.Categories WHERE IdCategory = {categoryId};"))
                .Returns(emptyDataSet);

            // Act
            var result = _categoryRepository.GetCategoryById(categoryId);

            // Assert
            Assert.Null(result);
            _mockDataService.Verify(
                ds => ds.ExecuteSql($"SELECT * FROM dbo.Categories WHERE IdCategory = {categoryId};"),
                Times.Once);
        }

        #endregion

        #region InsertCategory Tests

        [Fact]
        public void InsertCategory_WhenSuccessful_ShouldReturnTrue()
        {
            // Arrange
            var category = new Category { Label = "Hatchback", Description = "Compact car" };
            var dataSet = CreateDataSetWithInsertResult(newCatId: 5);

            _mockDataService
                .Setup(ds => ds.ExecuteStoredProcedure(
                    "InsertCategory",
                    It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()))
                .Returns(dataSet);

            // Act
            var result = _categoryRepository.InsertCategory(category);

            // Assert
            Assert.True(result);
            Assert.Equal(5, category.IdCategory);
            _mockDataService.Verify(
                ds => ds.ExecuteStoredProcedure(
                    "InsertCategory",
                    It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()),
                Times.Once);
        }

        [Fact]
        public void InsertCategory_WhenNoResultReturned_ShouldReturnTrue()
        {
            // Arrange
            var category = new Category { Label = "Truck", Description = "Heavy truck" };
            var emptyDataSet = new DataSet();

            _mockDataService
                .Setup(ds => ds.ExecuteStoredProcedure(
                    "InsertCategory",
                    It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()))
                .Returns(emptyDataSet);

            // Act
            var result = _categoryRepository.InsertCategory(category);

            // Assert
            Assert.True(result);
            _mockDataService.Verify(
                ds => ds.ExecuteStoredProcedure(
                    "InsertCategory",
                    It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()),
                Times.Once);
        }

        #endregion

        #region UpdateCategory Tests

        [Fact]
        public void UpdateCategory_WhenSuccessful_ShouldReturnTrue()
        {
            // Arrange
            var category = new Category
            {
                IdCategory = 1,
                Label = "Sedan Updated",
                Description = "Updated 4-door sedan"
            };

            var emptyDataSet = new DataSet();

            _mockDataService
                .Setup(ds => ds.ExecuteStoredProcedure(
                    "UpdateCategory",
                    It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()))
                .Returns(emptyDataSet);

            // Act
            var result = _categoryRepository.UpdateCategory(category);

            // Assert
            Assert.True(result);
            _mockDataService.Verify(
                ds => ds.ExecuteStoredProcedure(
                    "UpdateCategory",
                    It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()),
                Times.Once);
        }

        #endregion

        #region DeleteCategory Tests

        [Fact]
        public void DeleteCategory_WhenSuccessful_ShouldReturnTrue()
        {
            // Arrange
            var categoryId = 1;
            var emptyDataSet = new DataSet();

            _mockDataService
                .Setup(ds => ds.ExecuteStoredProcedure(
                    "DeleteCategory",
                    It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()))
                .Returns(emptyDataSet);

            // Act
            var result = _categoryRepository.DeleteCategory(categoryId);

            // Assert
            Assert.True(result);
            _mockDataService.Verify(
                ds => ds.ExecuteStoredProcedure(
                    "DeleteCategory",
                    It.IsAny<IEnumerable<Microsoft.Data.SqlClient.SqlParameter>>()),
                Times.Once);
        }

        #endregion

        
    }
}
