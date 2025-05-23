using Projet_C__A3.Vehicule;

public class CamionCiterne : Camion
{
    public string TypeProduit { get; set; }

    public CamionCiterne(string immatriculation, decimal tarifParKm, double volume, string typeProduit)
        : base(immatriculation, tarifParKm, volume)
    {
        TypeProduit = typeProduit;
    }

    public override string ToString()
    {
        return $"[CamionCiterne] {base.ToString()}, Produit: {TypeProduit}";
    }

    public override VehiculeType GetTypeVehicule()
    {
        return VehiculeType.CamionCiterne;
    }
}
