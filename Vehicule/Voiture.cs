using Projet_C__A3.Vehicules;

public class Voiture : Vehicule
{
    public int NombrePassagers { get; set; }

    public Voiture(string immatriculation, decimal tarifParKm, int nombrePassagers)
        : base(immatriculation, tarifParKm)
    {
        NombrePassagers = nombrePassagers;
    }

    public override string ToString()
    {
        return $"[Voiture] {base.ToString()}, Passagers: {NombrePassagers}";
    }

    public override VehiculeType GetTypeVehicule()
    {
        return VehiculeType.Voiture;
    }
}
