using System;
using System.Collections.Generic;
using System.Text;

namespace Genealogi
{
    class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string BirthCountry { get; set; }
        public string BirthCity { get; set; }
        public string DeathDate { get; set; }
        public string DeathCountry { get; set; }
        public string DeathCity { get; set; }
        public int Mother { get; set; }
        public int Father { get; set; }
    }
}
