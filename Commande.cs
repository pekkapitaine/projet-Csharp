using Projet_C__A3.Personne;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Projet_C__A3
{
    public class Commande
    {
        public Client Client { get; }
        public string VilleDepart { get; }
        public string VilleArrivee { get; }
        public double Kilometrage { get; }
        public Vehicule Vehicule { get; }
        public Salarie Chauffeur { get; }
        public DateTime DateCommande { get; }
        public decimal Prix => (decimal)Kilometrage * ( Vehicule.TarifParKm + Chauffeur.TarifHoraire );
        public bool PaiementEffectue { get; set; }

        public Commande(Client client, string adresseDepart, string adresseArrivee, double kilometrage,
                         Vehicule vehicule, Salarie chauffeur, DateTime dateCommande, bool paiement)
        {
            Client = client;
            VilleDepart = adresseDepart;
            VilleArrivee = adresseArrivee;
            Kilometrage = kilometrage;
            Vehicule = vehicule;
            Chauffeur = chauffeur;
            DateCommande = dateCommande;
            PaiementEffectue = paiement;
        }

        public override string ToString()
        {
            return $"{Client.Prenom} {Client.Nom} | {VilleDepart} -> {VilleArrivee} | {Kilometrage} km | Véhicule: {Vehicule.Immatriculation} | Chauffeur: {Chauffeur.AdresseMail} | Date: {DateCommande:yyyy-MM-dd} | Prix: {Prix}e | Payé: {(PaiementEffectue ? "Oui" : "Non")}";
        }


    }
}
