using CarRentalServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalServices.Contracts
{
    public interface IColorRepository
    {
        IEnumerable<Color> GetColors();

        Color? GetColor(int id);

        int InsertColor(Color category);

        bool UpdateColor(Color category);

        bool DeleteColor(int id);
    }
}
