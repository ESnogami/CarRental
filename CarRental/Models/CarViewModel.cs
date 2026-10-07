using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class CarViewModel
    {
        [Key]
        public int IdCar { get; set; }

        public required string Description { get; set; }

        public required string VinNumber { get; set; }

        public int Model { get; set; }
     
        public string? Brand { get; set; }
        
        public string? Color { get; set; }
      
        public string? Category { get; set; }

        public byte Passengers { get; set; }
    }
}
