using Projet_C__A3.Vehicule;

public abstract class Camion : Vehicule
{
    public double Volume { get; set; }

    protected Camion(string immatriculation, decimal tarifParKm, double volume)
        : base(immatriculation, tarifParKm)
    {
        Volume = volume;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, Volume: {Volume}m³";
    }

    public override VehiculeType GetTypeVehicule()
    {
        return VehiculeType.Camion;
    }
}
