using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using MySqlConnector;
using System.Text;
using System.Data;

namespace CarRental.Tests.Colors
{
    public class ColorRepositoryTests
    {
        private readonly Mock<IMySqlDataService> _mockDataService;
        private readonly ColorRepository _repository;

        public ColorRepositoryTests()
        {
            _mockDataService = new Mock<IMySqlDataService>();
            _repository = new ColorRepository(_mockDataService.Object);

            }

        #region getColors Tests
        [Fact]
        public void GetColors_WithValidData_ReturnsListOfColors()
        {
            // Arrange
            var dataSet = CreateColorsDataSet(new[]
            {
                (1, "Amarillo"),
                (2, "Azul")
            });

            _mockDataService.Setup(x => x.ExecSP(It.IsAny<string>()))
              .Returns(dataSet);

            // Act
            var result = _repository.GetColors();

            // Assert
            Assert.NotNull(result);
            var colors = result.ToList();
            Assert.Equal(2, colors.Count);
            Assert.Equal("Amarillo", colors[0].Name);
            Assert.Equal("Azul", colors[1].Name);
        }
        #endregion

        #region InsertColor Tests

        [Fact]
        public void InsertColor_WithValidColor_ReturnsTrue()
        {
            // Arrange
            var color = new Color
            {
                Name = "Amarillo"
            };

            var dataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("newColorId", typeof(int));
            table.Rows.Add(5); // Simulated generated ID
            dataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecSP(It.IsAny<string>(), It.IsAny<IEnumerable<MySqlParameter>>()))
                .Returns(dataSet);

            // Act
            var result = _repository.InsertColor(color);

            // Assert
         
            Assert.Equal(5, color.IdColor);
        }


        #endregion

        #region UpdateColor Tests   
        [Fact]
        public void UpdateColor_WithValidColor_ReturnsTrue()
        {
            // Arrange
            var color = new Color
            {
                IdColor = 1,
                Name = "Updated Color"
            };

            var dataSet = new DataSet();
            var table = new DataTable();
            dataSet.Tables.Add(table);

            _mockDataService.Setup(x => x.ExecSP(It.IsAny<string>(), It.IsAny<IEnumerable<MySqlParameter>>()))
                .Returns(dataSet);

            // Act
            var result = _repository.UpdateColor(color);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region GetColor Tests

        [Fact]
        public void GetColorById_WithValidId_ReturnsColor()
        {
            // Arrange
            int colorId = 1;
            var dataSet = CreateColorsDataSet(new[]
            {
                (1, "Amarillo")
            });

            _mockDataService.Setup(x => x.ExecSP(It.IsAny<string>(), It.IsAny<IEnumerable<MySqlParameter>>()))
                .Returns(dataSet);

            // Act
            var result = _repository.GetColor(colorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.IdColor);
            Assert.Equal("Amarillo", result.Name);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a DataSet with categories for testing.
        /// </summary>
        private static DataSet CreateColorsDataSet(params (int Id, string Name)[] colors
            )
        {
            var dataSet = new DataSet();
            var table = new DataTable();

            table.Columns.Add("IdColor", typeof(int));
            table.Columns.Add("Name", typeof(string));
           

            foreach (var color in colors)
            {
                table.Rows.Add(color.Id, color.Name);
            }

            dataSet.Tables.Add(table);
            return dataSet;
        }

            #endregion

       }
    }
