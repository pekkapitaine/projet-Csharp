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
}
