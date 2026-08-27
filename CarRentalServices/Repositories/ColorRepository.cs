using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using CarRentalServices.Enums;
using CarRentalServices.Services;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Repositories
{
    public class ColorRepository : IColorRepository
    {
        private readonly IMySqlDataService _mySqlDataService;

        public ColorRepository(IMySqlDataService mySqlDataService) { 
            _mySqlDataService = mySqlDataService;   
        }

        public bool DeleteColor(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Color> GetColors()
        {
            var data = _mySqlDataService.ExecuteSql("select * from colors;");
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                return MapColors(data.Tables[0]);
            }
            return [];
        }

        public Color? GetColor(int id)
        {
            var data = _mySqlDataService.ExecuteSql($"SELECT * FROM colors WHERE IdColor = {id};");
            if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                return MapColors(data.Tables[0]).FirstOrDefault();
            }
            return null;
        }

        public int InsertColor(Color Color)
        {
            var sql = "insertColor";
            IEnumerable<MySqlParameter> parameters = CreateParametersMySql(Color, ActionType.Insert);
            var result = _mySqlDataService.ExecSP(sql, parameters);
            if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                Color.IdColor = Convert.ToInt32(result.Tables[0].Rows[0]["newColorId"]);
            }
            return Color.IdColor;         
        }

        public bool UpdateColor(Color color)
        {
            var sql = "update_color";
            IEnumerable<MySqlParameter> parameters = CreateParametersMySql(color, ActionType.Update);
            _mySqlDataService.ExecSP(sql, parameters);
            return true;
        }


        #region "Private Methods"
        private static List<Color> MapColors(DataTable dataTable)
        {
            var lstColors = new List<Color>();
            foreach (DataRow row in dataTable.Rows)
            {
                var color = new Color
                {
                    IdColor = Convert.ToInt32(row["IdColor"]),
                    Name = (string)row["Name"]
                };
                lstColors.Add(color);
            }

            return lstColors;
        }
        private static IEnumerable<MySqlParameter> CreateParametersMySql(
            Color color,
            ActionType actionType)
        {
            var parameters = new List<MySqlParameter>();
            switch (actionType)
            {
                case ActionType.Insert:
                    parameters.Add(new MySqlParameter()
                    {
                        ParameterName = "@p_Name",
                        MySqlDbType = MySqlDbType.VarChar,
                        Size = 50,
                        Value = color.Name?? (object)DBNull.Value
                    });                    
                    break;
                case ActionType.Update:
                    parameters.Add(new MySqlParameter()
                    {
                        ParameterName = "@p_Name",
                        MySqlDbType = MySqlDbType.VarChar,
                        Size = 50,
                        Value = color.Name ?? (object)DBNull.Value
                    });
                    parameters.Add(new MySqlParameter()
                    {
                        ParameterName = "@p_id",
                        MySqlDbType = MySqlDbType.VarChar,
                        Size = 50,
                        Value = color.Name ?? (object)DBNull.Value
                    });
                    break;
                case ActionType.Delete:
                    parameters.Add(new MySqlParameter()
                    {
                        ParameterName = "@p_id",
                        MySqlDbType = MySqlDbType.Int32,
                        Value = color.IdColor
                    });
                    break;
            }

            return parameters;
        }
        #endregion
    }
}
