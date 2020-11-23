using System;
using System.ComponentModel.DataAnnotations;
using Core.Enumerations;

namespace Core.Entities.Admin
{
   public class CustomerOfficial : BaseEntity
    {
        public CustomerOfficial()
        {
        }

        public CustomerOfficial(int customerId, string name, string designation, string gender, string phone, string mobile, string mobile2, string email, string personalEmail, string isValid, DateTime addedOn)
        {
            this.CustomerId = customerId;
            this.Name = name;
            this.Designation = designation;
            this.Gender = gender;
            this.Phone = phone;
            this.Mobile = mobile;
            this.Mobile2 = mobile2;
            this.Email = email;
            this.PersonalEmail = personalEmail;
            this.IsValid = isValid;
            this.AddedOn = addedOn;

        }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Gender { get; set; } = "M";
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Mobile2 { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [EmailAddress]
        public string PersonalEmail { get; set; }
        public string PersonalMobile {get; set;}
        public string IsValid { get; set; } = "t";
        public string Scope {get; set; } = "HR";
        public DateTime AddedOn { get; set; } = DateTime.Now;
        public virtual Customer Customer {get; set;}

    }
}