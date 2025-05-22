namespace Projet_C__A3.Graphes
{
    public class Lien
    {
        public Noeud Destination { get; }
        public double Distance { get; }

        public Lien(Noeud destination, double distance)
        {
            Destination = destination;
            Distance = distance;
        }
    }
}
