using Projet_C__A3.Vehicules;

public abstract class Vehicule : IVehicule
{
    public string Immatriculation { get; set; }
    public decimal TarifParKm { get; set; }

    protected Vehicule(string immatriculation, decimal tarifParKm)
    {
        Immatriculation = immatriculation;
        TarifParKm = tarifParKm;
    }

    public override string ToString()
    {
        return $"Immatriculation: {Immatriculation}, Tarif/km: {TarifParKm}e";
    }

    public override bool Equals(object obj)
    {
        if (obj is Vehicule other)
        {
            return Immatriculation == other.Immatriculation;
        }
        return false;
    }

    public abstract VehiculeType GetTypeVehicule();
}
