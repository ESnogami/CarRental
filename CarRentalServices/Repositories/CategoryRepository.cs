using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Enums;
using CarRentalServices.Services;
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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDataService _dataService;

        public CategoryRepository(IDataService dataService)
        {
            _dataService = dataService;
        }

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

        public Category? GetCategoryById(int id)
        {
            var data = _dataService.ExecuteSql($"SELECT * FROM dbo.Categories WHERE IdCategory = {id};");
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                return MapCategories(data.Tables[0]).FirstOrDefault();
            }
            return null;
        }

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

        public bool UpdateCategory(Category category)
        {
            var sql = "UpdateCategory";
            IEnumerable<SqlParameter> parameters = CreateSqlParameters(category, ActionType.Update);
            _dataService.ExecuteStoredProcedure(sql, parameters);
            return true;
        }

        public bool DeleteCategory(int id)
        {
            var sql = "DeleteCategory";
            var category = new Category { IdCategory = id };
            IEnumerable<SqlParameter> parameters = CreateSqlParameters(category, ActionType.Delete);
            var result = _dataService.ExecuteStoredProcedure(sql, parameters);
            return true;
        }

        #region private
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

        private static IEnumerable<SqlParameter> CreateSqlParameters(Category category,
            ActionType actionType)
        {
            var parameters = new List<SqlParameter>();
            switch (actionType)
            {
                case ActionType.Insert:
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Label",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = category.Label ?? (object)DBNull.Value
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Description",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = category.Description ?? (object)DBNull.Value
                    });
                    break;
                case ActionType.Update:
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@p_Label",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = category.Label ?? (object)DBNull.Value
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@p_id",
                        SqlDbType = SqlDbType.Int,
                    
                        Value = category.IdCategory  
                    });
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@categoryId",
                        SqlDbType = SqlDbType.Int,
                        Value = category.IdCategory
                    });
                    break;
                case ActionType.Delete:
                    parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@categoryId",
                        SqlDbType = SqlDbType.Int,
                        Value = category.IdCategory
                    });
                    break;
            }

            return parameters;
        }

        #endregion

    }
}
