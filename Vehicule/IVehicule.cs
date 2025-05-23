using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_C__A3.Vehicules
{
    interface IVehicule
    {
        VehiculeType GetTypeVehicule();

        string Immatriculation { get; set; }

        decimal TarifParKm { get; set; }
    }
}
