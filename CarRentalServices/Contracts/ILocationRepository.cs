using CarRentalServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Contracts
{
    public interface ILocationRepository
    {
        
            IEnumerable<Location> GetLocations();

            Location? GetLocationById(int id);

            bool InsertLocation(Location customer);

            bool UpdateLocation(Location customer);

            bool DeleteLocation(int id);
        
    }
}
