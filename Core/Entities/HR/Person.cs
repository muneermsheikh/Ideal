using System;

namespace Core.Entities.HR
{
    public class Person
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FamilyName { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string PPNo { get; set; }
        public string AadharNo { get; set; }
        public DateTime DOB { get; set; }

        public string FullName {get {return FirstName + ", " + FamilyName;} }
    }
}