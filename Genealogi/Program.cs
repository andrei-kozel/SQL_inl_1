using System;
using System.Collections.Generic;

namespace Genealogi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 140;
            string DatabaseName = "Genealogi";
            SQLDB db = new SQLDB();
            db.CreateDatabase(DatabaseName);
            db.OpenDatabase(DatabaseName);
            db.CreateTable("People", @"
                            ID int NOT NULL Identity(1,1),
                            LastName varchar(255),
                            FirstName varchar(255),
                            BirthDate varchar(255),
                            BirthCountry varchar(255),
                            BirthCity varchar(255),
                            DeathDate varchar(255),
                            DeathCountry varchar(255),
                            DeathCity varchar(255),
                            Mother int,
                            Father int,
                            ");
            //Person testPerson = new Person
            //{
            //    Name = "Luke",
            //    LastName = "Skywalker",
            //    BirthDate = "1994",
            //    BirthCountry = "Tatooine",
            //    BirthCity = "Tatooine",
            //    DeathDate = "",
            //    DeathCountry = "",
            //    DeathCity = "",
            //    Mother = 1,
            //    Father = 2
            //};
            GenealogiCRUD crud = new GenealogiCRUD { DatabaseName = DatabaseName };
            //crud.Create(testPerson); 

            Person res = crud.Read("Luke");
            if (res != null)
            {
                Console.WriteLine(res.Id + " " + res.Name + " " + res.LastName);
            }

            var lst = crud.List();
            PrintTable(lst);
        }

        /// <summary>
        /// Write table into console
        /// </summary>
        /// <param name="lst">List</param>
        private static void PrintTable(List<Person> lst)
        {
            Console.WriteLine(String.Format("|{10, 3}|{0,10}|{1,10}|{2,12}|{3,15}|{4,12}|{5,12}|{6,15}|{7,12}|{8,12}|{9,12}|",
                                             "Name", "Last Name", "Birth Date", "Birth Country", "Birth City", "Death Date",
                                             "Death Country", "Death City", "Mother", "Father", "ID"));
            foreach (var person in lst)
            {
                Console.WriteLine(String.Format("|{10, 3}|{0,10}|{1,10}|{2,12}|{3,15}|{4,12}|{5,12}|{6,15}|{7,12}|{8,12}|{9,12}|",
                                                person.Name, person.LastName, person.BirthDate, person.BirthCountry, person.BirthCity, person.DeathDate,
                                                person.DeathCountry, person.DeathCity, person.Mother, person.Father, person.Id));
            }
        }
    }
}


