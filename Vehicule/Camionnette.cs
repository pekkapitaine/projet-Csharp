using Projet_C__A3.Vehicule;

public class Camionnette : Vehicule
{
    public string Usage { get; set; }

    public Camionnette(string immatriculation, decimal tarifParKm, string usage)
        : base(immatriculation, tarifParKm)
    {
        Usage = usage;
    }

    public override string ToString()
    {
        return $"[Camionnette] {base.ToString()}, Usage: {Usage}";
    }

    public override VehiculeType GetTypeVehicule()
    {
        return VehiculeType.Camionnette;
    }
}
