﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Genealogi
{
    class GenealogiCRUD
    {
        public string DatabaseName { get; set; }
        public int MaxRows { get; set; } = 10; // Max rows to return when searching
        public string OrderBy { get; set; } = "lastName";

        /// <summary>
        /// Create a person in Database
        /// </summary>
        /// <param name="person">Person object</param>
        public void Create(Person person)
        {
            var db = new SQLDB();
            db.OpenDatabase(DatabaseName);
            db.ExecuteSQL(@"INSERT INTO People (
                                FirstName, 
                                LastName, 
                                BirthDate, 
                                BirthCountry, 
                                BirthCity,  
                                DeathDate, 
                                DeathCountry, 
                                DeathCity, 
                                Mother, 
                                Father) 
                            VALUES (
                                @FirstName, 
                                @LastName, 
                                @BirthDate, 
                                @BirthCountry,
                                @BirthCity, 
                                @DeathDate, 
                                @DeathCountry, 
                                @DeathCity, 
                                @Mother, 
                                @Father)",
                ("@FirstName", person.Name),
                ("@LastName", person.LastName),
                ("@BirthDate", person.BirthDate),
                ("@BirthCountry", person.BirthCountry),
                ("@BirthCity", person.BirthCity),
                ("@DeathDate", person.DeathDate),
                ("@DeathCountry", person.DeathCountry),
                ("@DeathCity", person.DeathCity),
                ("@Mother", person.Mother.ToString()),
                ("@Father", person.Father.ToString())
                );
        }

        /// <summary>
        /// Returns a list of children
        /// </summary>
        /// <param name="person">Person object</param>
        /// <returns>List of chlildren</returns>
        internal List<Person> GetChildren(Person person)
        {
            return List(filter: $"Mother LIKE {person.Id} OR Father Like {person.Id}" );
        }

        /// <summary>
        /// Get a person from Database by name
        /// </summary>
        /// <param name="name">String Name</param>
        /// <returns>Person Object</returns>
        public Person Read(string name)
        {
            var db = new SQLDB();
            db.OpenDatabase(DatabaseName);
            var dt = db.GetDataTable(@"SELECT TOP 1 * FROM People WHERE FIrstName LIKE @name", ("@name", name));
            if(dt.Rows.Count <= 0)
            {
                return null;
            }

            return GetPersonObject(dt.Rows[0]);
        }

        /// <summary>
        /// Get a person from Database by Id
        /// </summary>
        /// <param name="id">Int Id</param>
        /// <returns>Person Object</returns>
        public Person Read(int id)
        {
            var db = new SQLDB();
            db.OpenDatabase(DatabaseName);
            var dt = db.GetDataTable(@"SELECT TOP 1 * FROM People WHERE Id LIKE @Id", ("@Id", id.ToString()));
            if (dt.Rows.Count <= 0)
            {
                return null;
            }

            return GetPersonObject(dt.Rows[0]);
        }

        /// <summary>
        /// Generate Person object based on DataRow returned from Database 
        /// </summary>
        /// <param name="dataRow">DataRow from Database</param>
        /// <returns>Person object</returns>
        private Person GetPersonObject(DataRow dataRow)
        {
            return new Person
            {
                Id = Convert.ToInt32(dataRow["id"]),
                Name = dataRow["FirstName"].ToString(),
                LastName = dataRow["LastName"].ToString(),
                BirthDate = dataRow["BirthDate"].ToString(),
                BirthCountry = dataRow["BirthCountry"].ToString(),
                BirthCity = dataRow["BirthCity"].ToString(),
                DeathDate = dataRow["DeathDate"].ToString(),
                DeathCountry = dataRow["DeathCountry"].ToString(),
                DeathCity = dataRow["DeathCity"].ToString(),
                Mother = Convert.IsDBNull(dataRow["Mother"]) ? 0 : (int)dataRow["Mother"],
                Father = Convert.IsDBNull(dataRow["Father"]) ? 0 : (int)dataRow["Father"]
            };
        }


        /// <summary>
        /// Delete the person from Database based on Person.Id
        /// </summary>
        /// <param name="person">Person Object</param>
        public void Delete(Person person)
        {
            var db = new SQLDB();
            db.OpenDatabase(DatabaseName);
            db.ExecuteSQL(@"DELETE FROM People WHERE ID = @Id", ("@Id", person.Id.ToString()));
        }

        /// <summary>
        /// Update the person in Database based on Person.Id
        /// </summary>
        /// <param name="person">Person Object</param>
        public void Update(Person person)
        {
            var db = new SQLDB();
            db.OpenDatabase(DatabaseName);
            db.ExecuteSQL(@"UPDATE People SET  
                                FirstName= @FirstName, 
                                LastName = @LastName, 
                                BirthDate = @BirthDate, 
                                BirthCountry = @BirthCountry, 
                                BirthCity = @BirthCity,  
                                DeathDate = @DeathDate, 
                                DeathCountry = @DeathCountry, 
                                DeathCity = @DeathCity, 
                                Mother = @Mother, 
                                Father = @Father
                            WHERE Id = @Id",
                                ("@FirstName", person.Name),
                                ("@LastName", person.LastName),
                                ("@BirthDate", person.BirthDate),
                                ("@BirthCountry", person.BirthCountry),
                                ("@BirthCity", person.BirthCity),
                                ("@DeathDate", person.DeathDate),
                                ("@DeathCountry", person.DeathCountry),
                                ("@DeathCity", person.DeathCity),
                                ("@Mother", person.Mother.ToString()),
                                ("@Father", person.Father.ToString()),
                                ("@Id", person.Id.ToString()));
        }

        /// <summary>
        /// Check if the person exist. By Name
        /// </summary>
        /// <param name="name">String Name</param>
        /// <returns>True or False</returns>
        public bool DoesPersonExist(string name) {
            if(Read(name) != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if the person exist. By ID
        /// </summary>
        /// <param name="id">Int Id</param>
        /// <returns>True or False</returns>
        public bool DoesPersonExist(int id) {
            if (Read(id) != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get father
        /// </summary>
        /// <param name="person">Person object</param>
        public void GetFather(Person person) 
        {
            var father = Read(person.Father);
            Console.WriteLine($"Father: {father.Name} {father.LastName}");
        }
        
        /// <summary>
        /// Get mother
        /// </summary>
        /// <param name="person">Person object</param>
        public void GetMother(Person person) {
            var mother = Read(person.Mother); 
            Console.WriteLine($"Mother: {mother.Name} {mother.LastName}");
        }



        /// <summary>
        /// Generate and returns list with Person objects
        /// </summary>
        /// <param name="filter">Used to filter data</param>
        /// <param name="orderBy">Used to sort data</param>
        /// <returns>List with Person objects</returns>
        public List<Person> List(string filter = "", string orderBy = "") 
        {
            var lst = new List<Person>();
            var sql = $"SELECT * FROM People";
            var db = new SQLDB();
            db.OpenDatabase(DatabaseName);

            if (filter != "") sql += " WHERE " + filter;
            if (orderBy != "") sql += " ORDER BY " + orderBy;

            var data = db.GetDataTable(sql);

            foreach(DataRow row in data.Rows)
            {
                lst.Add(GetPersonObject(row));
            }

            return lst;
        }

    }
}
