using CarRentalServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Contracts
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetCountries();

        Country? GetCountry(int id);

        int InsertCountry(Country country);

        bool UpdateCountry(Country country);

        bool DeleteCountry(int id);
    }
}
