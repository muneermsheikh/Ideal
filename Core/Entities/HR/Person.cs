using System;

namespace Core.Entities.HR
{
    public class PersonEnt
    {
        public PersonEnt()
        {
        }

        public PersonEnt(string firstName, string secondName, string familyName, 
            string knownAs, string gender, string pPNo, string aadharNo, DateTime dOB)
        {
            FirstName = firstName;
            SecondName = secondName;
            FamilyName = familyName;
            KnownAs = knownAs;
            Gender = gender;
            PPNo = pPNo;
            AadharNo = aadharNo;
            DOB = dOB;
        }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FamilyName { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string PPNo { get; set; }
        public string AadharNo { get; set; }
        public DateTime? DOB { get; set; }

        public string FullName {get {return FirstName + ", " + FamilyName;} }
    }
}