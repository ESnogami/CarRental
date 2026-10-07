using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace CarRentalServices.Utilities
{
    /// <summary>
    /// Generic helper class for building SQL Server parameters consistently across all repositories
    /// </summary>
    public static class SqlParameterBuilder
    {
        public static SqlParameter CreateParameter(
            string name,
            SqlDbType type,
            int? size,
            object? value)
        {
            var parameter = new SqlParameter
            {
                ParameterName = name,
                SqlDbType = type,
                Value = value ?? (object)DBNull.Value
            };

            if (size.HasValue)
                parameter.Size = size.Value;

            return parameter;
        }

        public static SqlParameter CreateParameter(string name, SqlDbType type, object? value)
            => CreateParameter(name, type, null, value);
    }
}
