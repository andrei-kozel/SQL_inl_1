using System;
using System.Collections.Generic;

namespace Genealogi
{
    class Program
    {
        // Database name
        static string DatabaseName = "Genealogi";
        static GenealogiCRUD crud = new GenealogiCRUD { DatabaseName = DatabaseName };
        static SQLDB db = new SQLDB();

        // Main method
        static void Main(string[] args)
        {
            Console.WindowWidth = 140;

            CreateDataBase();
            GenerateTableData();
            CreateRelationships();
            ChangeNameToOrgana();
            CreateMaraJade();
            CreteBenSkywalker();
            SortListByName();
        }

        /// <summary>
        /// Create new DataBase and Table
        /// </summary>
        private static void CreateDataBase()
        {
            Console.WriteLine($"Lets's create database {DatabaseName} and table People");
            Continue();

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
            Continue();
        }

        /// <summary>
        /// Print Database to the console sorted by Name
        /// </summary>
        private static void SortListByName()
        {
            Console.WriteLine("DataBase sorted by Name:");
            var lst = crud.List(orderBy:"FirstName");
            PrintTable(lst);
            Continue();
        }

        /// <summary>
        /// Create Ben Skywalker
        /// </summary>
        private static void CreteBenSkywalker()
        {
            Console.WriteLine("Let's create Ben Skywalker");
            Continue();

            Person ben = new Person
            {
                Name = "Ben",
                LastName = "Skywalker",
                BirthDate = "26.5 ABY",
                BirthCountry = "Coruscant",
                BirthCity = "Unknown",
                DeathDate = "",
                DeathCountry = "",
                DeathCity = "",
                Mother = crud.Read("Mara").Id,
                Father = crud.Read("Luke").Id
            };
            crud.Create(ben);
            PrintDB();
        }

        /// <summary>
        /// Create Mara Jade
        /// </summary>
        private static void CreateMaraJade()
        {
            Console.WriteLine("Let's create Mara Jade");
            Continue();

            Person mara = new Person
            {
                Name = "Mara",
                LastName = "Jade",
                BirthDate = "17 BBY",
                BirthCountry = "Unknown",
                BirthCity = "Unknown",
                DeathDate = "40 ABY",
                DeathCountry = "Kavan",
                DeathCity = "Unknown",
                Mother = 0,
                Father = 0
            };
            crud.Create(mara);
            PrintDB();
        }

        /// <summary>
        /// Change Leia to Organa
        /// </summary>
        private static void ChangeNameToOrgana()
        {
            Console.WriteLine("Let's change name Leia to Organa");
            Continue();
            var leia = crud.Read("Leia");
            leia.Name = "Organa";
            crud.Update(leia);
            PrintDB();

            Console.WriteLine("Let's change the name back.");
            var organa = crud.Read("Organa");
            organa.Name = "Leia";
            crud.Update(organa);
            PrintDB();
        }

        /// <summary>
        /// Print Database
        /// </summary>
        private static void PrintDB()
        {
            Console.WriteLine("This is DB:");
            var lst = crud.List();
            PrintTable(lst);
            Continue();
        }

        /// <summary>
        /// Create relationships between persons in DB
        /// </summary>
        private static void CreateRelationships()
        {
            Console.WriteLine("Create Relationships");

            // Set Luke mother
            var luke = crud.Read("Luke");
            luke.Mother = crud.Read("Padmé").Id;
            crud.Update(luke);

            // Set Leila's mother and father
            var leia = crud.Read("Leia");
            leia.Mother = crud.Read("Padmé").Id;
            leia.Father = crud.Read("Anakin").Id;
            crud.Update(leia);

            Console.WriteLine();
            var relationshipsList = crud.List();
            PrintTable(relationshipsList);
            Continue();
        }

        /// <summary>
        /// Ask user to press the Enter button and start a new task
        /// </summary>
        private static void Continue()
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue ... ");
            Console.ReadLine();
            Console.Clear();
        }

        /// <summary>
        /// Create initial data
        /// </summary>
        private static void GenerateTableData()
        {
            Person anakin = new Person
            {
                Name = "Anakin",
                LastName = "Skywalker",
                BirthDate = "41 BBY",
                BirthCountry = "Tatooine",
                BirthCity = "Outer Rim",
                DeathDate = "4 ABY",
                DeathCountry = "DS-2",
                DeathCity = "Endor",
                Mother = 0,
                Father = 0
            };
            crud.Create(anakin);

            Person padme = new Person
            {
                Name = "Padmé",
                LastName = "Amidala",
                BirthDate = "46 BBY",
                BirthCountry = "Naboo",
                BirthCity = "Mid Rim",
                DeathDate = "19 BBY",
                DeathCountry = "Subterrel",
                DeathCity = "Polis Massa",
                Mother = 0,
                Father = 0
            };
            crud.Create(padme);

            Person luke = new Person
            {
                Name = "Luke",
                LastName = "Skywalker",
                BirthDate = "19 BBY",
                BirthCountry = "Tatooine",
                BirthCity = "Polis Massa",
                DeathDate = "34 ABY",
                DeathCountry = "Ahch-To",
                DeathCity = "Unknown",
                Mother = 0,
                Father = 0
            };
            crud.Create(luke);

            Person leia = new Person
            {
                Name = "Leia",
                LastName = "Skywalker",
                BirthDate = "19 BBY",
                BirthCountry = "Alderaan",
                BirthCity = "Polis Massa",
                DeathDate = "35 ABY",
                DeathCountry = "Ajan Klosse",
                DeathCity = "Unknown",
                Mother = 0,
                Father = 0
            };
            crud.Create(leia);

            Console.WriteLine("I have created DB with 4 people.");
            Continue();
            PrintDB();
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


