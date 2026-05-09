using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Entities
{
    public class Category
    {
        [Key]
        public int IdCategory { get; set; }

        public string? Label { get; set; }

        public string? Description { get; set; }
    }
}
