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
}
