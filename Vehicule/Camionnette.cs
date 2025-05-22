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
}
