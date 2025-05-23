using Projet_C__A3.Vehicules;

public class CamionBenne : Camion
{
    public int NombreBennes { get; set; }
    public bool GrueAuxiliaire { get; set; }

    public CamionBenne(string immatriculation, decimal tarifParKm, double volume, int nombreBennes, bool grueAuxiliaire)
        : base(immatriculation, tarifParKm, volume)
    {
        NombreBennes = nombreBennes;
        GrueAuxiliaire = grueAuxiliaire;
    }

    public override string ToString()
    {
        return $"[CamionBenne] {base.ToString()}, Bennes: {NombreBennes}, Grue: {(GrueAuxiliaire ? "Oui" : "Non")}";
    }

    public override VehiculeType GetTypeVehicule()
    {
        return VehiculeType.CamionBenne;
    }
}
