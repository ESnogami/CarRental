using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Enums;
using CarRentalServices.Services;
using CarRentalServices.Utilities;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Repositories
{
    /// <summary>
    /// Repository class for managing Category entities in the database.
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        /// <summary>
        /// The data service used for executing SQL commands and stored procedures.
        /// </summary>
        private readonly IDataService _dataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class with the specified data service.
        /// </summary>
        /// <param name="dataService"></param>
        public CategoryRepository(IDataService dataService)
        {
            _dataService = dataService;
        }

        /// <summary>
        /// Retrieves all categories from the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Category> GetCategories()
        {
            var data = _dataService.ExecuteSql("SELECT * FROM dbo.Categories;");
            List<Category> lstCategories = [];
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                lstCategories = MapCategories(data.Tables[0]);
            }
            return lstCategories;
        }

        /// <summary>
        /// Retrieves a category by its ID from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Category? GetCategoryById(int id)
        {
            var data = _dataService.ExecuteSql($"SELECT * FROM dbo.Categories WHERE IdCategory = {id};");
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                return MapCategories(data.Tables[0]).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// Inserts a new category into the database and updates the category's ID with the newly generated ID.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool InsertCategory(Category category)
        {
            var sql = "InsertCategory";
            IEnumerable<SqlParameter> parameters = CreateSqlParameters(category, ActionType.Insert);
            var result = _dataService.ExecuteStoredProcedure(sql, parameters);
            if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                category.IdCategory = Convert.ToInt32(result.Tables[0].Rows[0]["newCatId"]);
            }
            return true;
        }

        /// <summary>
        /// Updates an existing category in the database.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool UpdateCategory(Category category)
        {
            var sql = "UpdateCategory";
            IEnumerable<SqlParameter> parameters = CreateSqlParameters(category, ActionType.Update);
            _dataService.ExecuteStoredProcedure(sql, parameters);
            return true;
        }

        /// <summary>
        /// Deletes a category from the database by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteCategory(int id)
        {
            var sql = "DeleteCategory";
            var category = new Category { IdCategory = id };
            IEnumerable<SqlParameter> parameters = CreateSqlParameters(category, ActionType.Delete);
            var result = _dataService.ExecuteStoredProcedure(sql, parameters);
            return true;
        }

        #region private
        /// <summary>
        /// Maps a DataTable to a list of Category objects.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private static List<Category> MapCategories(DataTable dataTable)
        {
            var lstCategories = new List<Category>();
            foreach (DataRow row in dataTable.Rows)
            {
                var category = new Category
                {
                    IdCategory = Convert.ToInt32(row["IdCategory"]),
                    Label = (string)row["Label"],
                    Description = (string)row["Description"]
                };
                lstCategories.Add(category);
            }

            return lstCategories;
        }

        /// <summary>
        /// Creates SQL parameters for the specified category and action type.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        private static IEnumerable<SqlParameter> CreateSqlParameters(Category category,
            ActionType actionType)
        {
            return actionType switch
            {
                ActionType.Insert => new List<SqlParameter>
                {
                    SqlParameterBuilder.CreateParameter("@Label", SqlDbType.VarChar, 50, category.Label),
                    SqlParameterBuilder.CreateParameter("@Description", SqlDbType.VarChar, 50, category.Description)
                },
                ActionType.Update => new List<SqlParameter>
                {
                    SqlParameterBuilder.CreateParameter("@p_Label", SqlDbType.VarChar, 50, category.Label),
                    SqlParameterBuilder.CreateParameter("@p_id", SqlDbType.Int, (object)category.IdCategory),
                    SqlParameterBuilder.CreateParameter("@categoryId", SqlDbType.Int, (object)category.IdCategory)
                },
                ActionType.Delete => new List<SqlParameter>
                {
                    SqlParameterBuilder.CreateParameter("@categoryId", SqlDbType.Int, (object)category.IdCategory)
                },
                _ => new List<SqlParameter>()
            };
        }

        #endregion

    }
}
