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
        // The name of the database
        public string DatabaseName { get; set; } = "Master";
        // Connection string
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

        /// <summary>
        /// Gets a datatable from the DB
        /// </summary>
        /// <param name="sqlString">used to execute</param>
        /// <param name="parameters">used to send parameters</param>
        /// <returns>DataTable</returns>
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

        /// <summary>
        /// Create Database
        /// </summary>
        /// <param name="name">Table Name</param>
        public void CreateDatabase(string name)
        {
            if (DoesDatabaseExist(name))
            {
                Console.WriteLine("Sorry, DB already exists. I will use this DB instead.");
            }
            else
            {
                ExecuteSQL("CREATE DATABASE " + name);
                OpenDatabase(name);
            }
        }
        /// <summary>
        /// Open Database with a specific name
        /// </summary>
        /// <param name="name">used to specify the database name</param>
        public void OpenDatabase(string name)
        {
            DatabaseName = name;
        }

        /// <summary>
        /// Create table
        /// </summary>
        /// <param name="tableName">used to set table name</param>
        /// <param name="fields">used to specify fields in the table</param>
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

        /// <summary>
        /// Check if database exist
        /// </summary>
        /// <param name="databaseName">Database name</param>
        /// <returns>True if DB exist</returns>
        public bool DoesDatabaseExist(string databaseName)
        {
            var db = GetDataTable("SELECT name FROM sys.databases WHERE name = @name", ("@name", databaseName));
            return db.Rows.Count > 0 ? true : false;
        }

        /// <summary>
        /// Check if table exist in the Database
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <returns>True if table exist</returns>
        public bool DoesTableExist(string tableName)
        {
            var db = GetDataTable("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tableName", ("@tableName", tableName));
            return db.Rows.Count > 0 ? true : false;
        }

        /// <summary>
        /// Add params to a SQLCommand
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="command"></param>
        private void SetParameters((string, string)[] parameters, SqlCommand command)
        {
            foreach (var item in parameters)
            {
                command.Parameters.AddWithValue(item.Item1, item.Item2);
            }
        }
    }
}
