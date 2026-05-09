using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Entities
{
    public class Customer
    {
        [Key]
        public int IdCustomer { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SocialSecurityNumber { get; set; }

        public string Email { get; set; }

        public string CountryName { get; set; }
        public int CountryId { get; set; }

        public string MobilePhone { get; set; }

        public string StateName { get; set; }
    }
}
