using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Admin
{
   public class CustomerOfficial : BaseEntity
    {
        public CustomerOfficial()
        {
        }

        public CustomerOfficial(int customerId, string name, string designation, string gender, string phone, string mobile, string mobile2, string email, string personalEmail, bool isValid, DateTime addedOn)
        {
            this.CustomerId = customerId;
            this.Name = name;
            this.Designation = designation;
            this.Gender = gender;
            this.Phone = phone;
            this.Mobile = mobile;
            this.Mobile2 = mobile2;
            this.email = email;
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
        public string email { get; set; }
        public string PersonalEmail { get; set; }
        public bool IsValid { get; set; } = true;
        public DateTime AddedOn { get; set; } = DateTime.Now;

    }
}