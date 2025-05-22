namespace Projet_C__A3.Graphes
{
    public class Noeud
    {
        public string Nom { get; }

        public Noeud(string nom)
        {
            Nom = nom;
        }

        public override bool Equals(object obj) => obj is Noeud other && Nom == other.Nom;
        public override int GetHashCode() => Nom.GetHashCode();
        public override string ToString() => Nom;
    }
}
