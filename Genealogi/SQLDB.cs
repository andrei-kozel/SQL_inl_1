using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Genealogi
{
    class SQLDB
    {
        public string DatabaseName { get; set; } = "Master";
        public string ConnectionString { get; set; } = @"Data Source=.\SQLExpress;Integrated Security=true;database={0}";

        /// <summary>
        /// Executes SQL code
        /// </summary>
        /// <param name="sqlString">The SQL String</param>
        /// <param name="parameters">Tuples with parameters</param>
        /// <returns>Number of affected rows</returns>
        internal long ExecuteSQL(string sqlString, params (string, string)[] parameters)
        {
            long rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(string.Format(ConnectionString, DatabaseName)))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sqlString, connection))
                    {
                        SetParameters(parameters, command);
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return rowsAffected;
        }

        public DataTable GetDataTable(string sqlString, params (string, string)[] parameters)
        {
            var dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(string.Format(ConnectionString, DatabaseName)))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlString, connection))
                {
                    SetParameters(parameters, command);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public void CreateDatabase(string name)
        {
            if (DoesDatabaseExist(name))
            {
                Console.WriteLine("Sorry, db already exist");
            }
            else
            {
                ExecuteSQL("CREATE DATABASE " + name);
                OpenDatabase(name);
            }
        }

        public void OpenDatabase(string name)
        {
            DatabaseName = name;
        }

        public void CreateTable(string tableName, string fields)
        {
            if (DoesTableExist(tableName))
            {
                Console.WriteLine("Sorry, table already exist");
            }
            {
                ExecuteSQL($"CREATE TABLE {tableName} ({fields})");
            }
        }

        public bool DoesDatabaseExist(string databaseName)
        {
            var db = GetDataTable("SELECT name FROM sys.databases WHERE name = @name", ("@name", databaseName));
            return db.Rows.Count > 0 ? true : false;
        }

        public bool DoesTableExist(string tableName)
        {
            var db = GetDataTable("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tableName", ("@tableName", tableName));
            return db.Rows.Count > 0 ? true : false;
        }

        private void SetParameters((string, string)[] parameters, SqlCommand command)
        {
            foreach (var item in parameters)
            {
                command.Parameters.AddWithValue(item.Item1, item.Item2);
            }
        }
    }
}
