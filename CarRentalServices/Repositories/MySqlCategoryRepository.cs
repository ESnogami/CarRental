using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Enums;
using CarRentalServices.Utilities;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using System.Data;

namespace CarRentalServices.Repositories
{
    public class MySqlCategoryRepository : ICategoryRepository
    {

        private readonly IMySqlDataService _mySqlDataService;

        public MySqlCategoryRepository(IMySqlDataService mySqlDataService)
        {
            _mySqlDataService = mySqlDataService;
        }

        #region public
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Category> GetCategories()
        {
            var data = _mySqlDataService.ExecuteSql("SELECT * FROM carrental.categories;");
            List<Category> lstCategories = [];
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                lstCategories = MapCategories(data.Tables[0]);
            }
            return lstCategories;
        }

        public Category? GetCategoryById(int id)
        {
            var data = _mySqlDataService.ExecuteSql($"SELECT * FROM carrental.categories WHERE IdCategory = {id};");
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                return MapCategories(data.Tables[0]).FirstOrDefault();
            }
            return null;
        }

        public bool InsertCategory(Category category)
        {
            var sql = "insertCategory";
            IEnumerable<MySqlParameter> parameters = CreateParametersMySql(category, ActionType.Insert);
            var result = _mySqlDataService.ExecSP(sql, parameters);
            if(result.Tables.Count>0 && result.Tables[0].Rows.Count > 0)
            {
                category.IdCategory= Convert.ToInt32(result.Tables[0].Rows[0]["newCatId"]);
            }
            return true;
        }

        public bool UpdateCategory(Category category)
        {
            var sql = "UpdateCategory";
            IEnumerable<MySqlParameter> parameters = CreateParametersMySql(category, ActionType.Update);
            _mySqlDataService.ExecSP(sql, parameters);
            return true;
        }

        public bool DeleteCategory(int id)
        {
            var sql = "deleteCategory";
            var category = new Category { IdCategory = id };
            IEnumerable<MySqlParameter> parameters = CreateParametersMySql(category, ActionType.Delete);
            var result = _mySqlDataService.ExecSP(sql, parameters);
            return true;
        }
        #endregion

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

        private static IEnumerable<MySqlParameter> CreateParametersMySql(Category category,
            ActionType actionType)
        {
            return actionType switch
            {
                ActionType.Insert => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@Label", MySqlDbType.VarChar, 50, category.Label),
                    MySqlParameterBuilder.CreateParameter("@Description", MySqlDbType.VarChar, 50, category.Description)
                },
                ActionType.Update => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@Label", MySqlDbType.VarChar, 50, category.Label),
                    MySqlParameterBuilder.CreateParameter("@Description", MySqlDbType.VarChar, 50, category.Description),
                    MySqlParameterBuilder.CreateParameter("@categoryId", MySqlDbType.Int32, category.IdCategory)
                },
                ActionType.Delete => new List<MySqlParameter>
                {
                    MySqlParameterBuilder.CreateParameter("@categoryId", MySqlDbType.Int32, category.IdCategory)
                },
                _ => new List<MySqlParameter>()
            };
        }
        #endregion
    }
}
