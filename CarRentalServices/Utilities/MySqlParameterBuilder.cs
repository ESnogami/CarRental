using MySqlConnector;
using System;

namespace CarRentalServices.Utilities
{
    /// <summary>
    /// Generic helper class for building MySQL parameters consistently across all repositories
    /// </summary>
    public static class MySqlParameterBuilder
    {
        public static MySqlParameter CreateParameter(
            string name,
            MySqlDbType type,
            int? size,
            object? value)
        {
            var parameter = new MySqlParameter
            {
                ParameterName = name,
                MySqlDbType = type,
                Value = value ?? (object)DBNull.Value
            };

            if (size.HasValue)
                parameter.Size = size.Value;

            return parameter;
        }

        public static MySqlParameter CreateParameter(string name, MySqlDbType type, object? value)
            => CreateParameter(name, type, null, value);
    }
}
