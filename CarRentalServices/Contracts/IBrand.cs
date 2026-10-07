using CarRentalServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Contracts
{
    public interface IBrandRepository
    {
        IEnumerable<Brand> GetBrands();

        Brand? GetBrand(int id);

        int InsertBrand(Brand category);

        bool UpdateBrand(Brand category);

        bool DeleteBrand(int id);
    }
}
