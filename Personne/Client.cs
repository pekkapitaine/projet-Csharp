using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_C__A3.Personne
{
    public class Client : Personne
    {
        public Client(string nom, string prenom, DateTime dateNaissance, string adressePostale,
                      string adresseMail, string telephone)
            : base(nom, prenom, dateNaissance, adressePostale, adresseMail, telephone) { }
        public override string ToString()
        {
            return $"[Client] {base.ToString()}";
        }
    }


}
