using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_C__A3.Personne
{
    public abstract class Personne(string nom, string prenom, DateTime dateNaissance, string adressePostale, string adresseMail, string telephone)
    {
        public string Nom { get; set; } = nom;
        public string Prenom { get; set; } = prenom;
        public DateTime? DateNaissance { get; set; } = dateNaissance;
        public string? AdressePostale { get; set; } = adressePostale;
        public string AdresseMail { get; set; } = adresseMail;
        public string? Telephone { get; set; } = telephone;

        public override string ToString()
        {
            return $"{Prenom} {Nom}, Né(e) le {DateNaissance?.ToShortDateString()}, Adresse : {AdressePostale}, Email : {AdresseMail}, Téléphone : {Telephone}";
        }

        public override bool Equals(object? obj) 
        {
            return obj is Personne other && AdresseMail == other.AdresseMail;
        }
    }
}
