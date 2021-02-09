using System;
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
        /// Get a person from Database
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
        /// Generate Person object based on DataRow returned from Database 
        /// </summary>
        /// <param name="dataRow">DataRow from Database</param>
        /// <returns>Person object</returns>
        private Person GetPersonObject(DataRow dataRow)
        {
            return new Person
            {
                Id = (int)dataRow["id"],
                Name = dataRow["FirstName"].ToString(),
                LastName = dataRow["LastName"].ToString(),
                BirthDate = dataRow["BirthDate"].ToString(),
                BirthCountry = dataRow["BirthCountry"].ToString(),
                BirthCity = dataRow["BirthCity"].ToString(),
                DeathDate = dataRow["DeathDate"].ToString(),
                DeathCountry = dataRow["DeathCountry"].ToString(),
                DeathCity = dataRow["DeathCity"].ToString(),
                Mother = (int)dataRow["Mother"],
                Father = (int)dataRow["Father"]
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

        //public bool DoesPersonExist(string name) {/* Massor med kod */}
        //public bool DoesPersonExist(int Id) {/* Massor med kod */}
        //public void GetFather(Person person) {/* Massor med kod */}
        //public void GetMother(Person person) {/* Massor med kod */}
        //public List<Person> List(string paramValue, string filter = "firstName LIKE @i nput") {/* Massor med kod */}

    }
}
