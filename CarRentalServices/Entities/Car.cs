using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Entities
{
    public class Car
    {
        [Key]
        public int IdCar { get; set; }

        public required string Description { get; set; }

        public required string VinNumber { get; set; }

        public int Model { get; set; }

        public int BrandId { get; set; }
        public string? Brand { get; set; }

        public int ColorId { get; set; }
        public string? Color { get; set; }

        public int CategoryId { get; set; }
        public string? Category { get; set; }

        public byte  Passengers { get; set; }
    }
}
