using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.SQLite;

namespace eAgenda.Controladores.Shared
{
    public delegate T ConverterDelegate<T>(IDataReader reader);
   

    public static class Db
    {
        private static readonly string connectionString = "";
        private static readonly string bancoEscolhido = "";

        static Db()
        {
            bancoEscolhido = ConfigurationManager.AppSettings["bancodedados"].ToLower().Trim();
            connectionString = ConfigurationManager.ConnectionStrings[bancoEscolhido].ConnectionString;

        }

        public static int Insert(string sql, Dictionary<string, object> parameters)
        {

            if(bancoEscolhido == "dbsqlserver")
            {
                SqlConnection connection = new SqlConnection(connectionString);

                SqlCommand command = new SqlCommand(sql.AppendSelectIdentityOrRowid(), connection);

                command.SetParameters(parameters);

                connection.Open();

                int id = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                return id;
            }
            else
            {

                SQLiteConnection connection = new SQLiteConnection(connectionString);

                SQLiteCommand command = new SQLiteCommand(sql.AppendSelectIdentityOrRowid(), connection);

                command.SetParametersSqlLite(parameters);

                connection.Open();

                int id = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                return id;
            }
        }

        public static void Update(string sql, Dictionary<string, object> parameters = null)
        {
            if (bancoEscolhido == "dbsqlserver")
            {

                SqlConnection connection = new SqlConnection(connectionString);

                SqlCommand command = new SqlCommand(sql, connection);

                command.SetParameters(parameters);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();

            }
            else
            {
                SQLiteConnection connection = new SQLiteConnection(connectionString);

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                command.SetParametersSqlLite(parameters);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static void Delete(string sql, Dictionary<string, object> parameters)
        {
            Update(sql, parameters);
        }

        public static List<T> GetAll<T>(string sql, ConverterDelegate<T> convert, Dictionary<string, object> parameters = null)
        {
            if (bancoEscolhido == "dbsqlserver")
            {
                SqlConnection connection = new SqlConnection(connectionString);

                SqlCommand command = new SqlCommand(sql, connection);

                command.SetParameters(parameters);

                connection.Open();

                var list = new List<T>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = convert(reader);
                        list.Add(obj);
                    }

                }

                connection.Close();
                return list;
            }
            else
            {
                SQLiteConnection connection = new SQLiteConnection(connectionString);

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                command.SetParametersSqlLite(parameters);

                connection.Open();

                var list = new List<T>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = convert(reader);
                        list.Add(obj);
                    }

                }

                connection.Close();
                return list;
            }
        }

        public static T Get<T>(string sql, ConverterDelegate<T> convert, Dictionary<string, object> parameters)
        {
            if (bancoEscolhido == "dbsqlserver")
            {

                SqlConnection connection = new SqlConnection(connectionString);

                SqlCommand command = new SqlCommand(sql, connection);

                command.SetParameters(parameters);

                connection.Open();

                T t = default;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        t = convert(reader);
                }

                connection.Close();
                return t;
            }
            else
            {
                SQLiteConnection connection = new SQLiteConnection(connectionString);

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                command.SetParametersSqlLite(parameters);

                connection.Open();

                T t = default;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        t = convert(reader);
                }

                connection.Close();
                return t;

            }
        }

        public static bool Exists(string sql, Dictionary<string, object> parameters)
        {
            if (bancoEscolhido == "dbsqlsever")
            {

                SqlConnection connection = new SqlConnection(connectionString);

                SqlCommand command = new SqlCommand(sql, connection);

                command.SetParameters(parameters);

                connection.Open();

                int numberRows = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                return numberRows > 0;
            }
            else {

                SQLiteConnection connection = new SQLiteConnection(connectionString);

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                command.SetParametersSqlLite(parameters);

                connection.Open();

                int numberRows = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                return numberRows > 0;

            }
        }

        private static void SetParameters(this SqlCommand command, Dictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return;

            foreach (var parameter in parameters)
            {
                string name = parameter.Key;

                object value = parameter.Value.IsNullOrEmpty() ? DBNull.Value : parameter.Value;

                SqlParameter dbParameter = new SqlParameter(name, value);

                command.Parameters.Add(dbParameter);
            }
        }

        private static void SetParametersSqlLite(this SQLiteCommand command, Dictionary<string,object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return;

            foreach (var parameter in parameters)
            {
                string name = parameter.Key;

                object value = parameter.Value.IsNullOrEmpty() ? DBNull.Value : parameter.Value;

                SQLiteParameter dbParameter = new SQLiteParameter(name, value);

                command.Parameters.Add(dbParameter);
            }
        }

        private static string AppendSelectIdentityOrRowid(this string sql)
        {
            if(bancoEscolhido == "dbsqlsever")
            {
                return sql + ";SELECT SCOPE_IDENTITY()";
            }

            return sql + ";SELECT  LAST_INSERT_ROWID()";
        }

        public static bool IsNullOrEmpty(this object value)
        {
            return (value is string && string.IsNullOrEmpty((string)value)) ||
                    value == null;
        }

    }
}
