using Projet_C__A3.Vehicules;

public class CamionFrigorifique : Camion
{
    public int GroupesElectrogenes { get; set; }

    public CamionFrigorifique(string immatriculation, decimal tarifParKm, double volume, int groupesElectrogenes)
        : base(immatriculation, tarifParKm, volume)
    {
        GroupesElectrogenes = groupesElectrogenes;
    }

    public override string ToString()
    {
        return $"[CamionFrigorifique] {base.ToString()}, Groupes électrogènes: {GroupesElectrogenes}";
    }

    public override VehiculeType GetTypeVehicule()
    {
        return VehiculeType.CamionFrigorifique;
    }
}
