using System;

namespace Genealogi
{
    class Program
    {
        static void Main(string[] args)
        {
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
            //    Name = "Andrei",
            //    LastName = "Kozel",
            //    BirthDate = "1994",
            //    BirthCountry = "Belarus",
            //    BirthCity = "Molodechno",
            //    DeathDate = "",
            //    DeathCountry = "",
            //    DeathCity = "",
            //    Mother = 1,
            //    Father = 2
            //};
            GenealogiCRUD crud = new GenealogiCRUD { DatabaseName = DatabaseName };
            //crud.Create(testPerson); 
            Person res = crud.Read("Andrei");
            Console.WriteLine(res.Id + " " + res.Name + " " + res.LastName );
            Person res2 = crud.Read("Test2");
            Console.WriteLine(res2.Id + " " + res2.Name + " " + res2.LastName);
            crud.Delete(res2);

        }
    }
}

