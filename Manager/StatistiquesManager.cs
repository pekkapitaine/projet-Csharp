using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projet_C__A3.Personne;

namespace Projet_C__A3.Manager
{
    public static class StatistiquesManager
    {
        
        public static void AfficherNombreLivraisonsParChauffeur()
        {
            var commandes = CommandeManager.ChargerCommandes();

            var livraisonsParChauffeur = commandes
                .GroupBy(c => c.Chauffeur.AdresseMail)
                .Select(g => new
                {
                    Chauffeur = g.Key,
                    NbLivraisons = g.Count()
                });

            Console.WriteLine("===== LIVRAISONS PAR CHAUFFEUR =====");
            foreach (var item in livraisonsParChauffeur)
            {
                Console.WriteLine($"Chauffeur : {item.Chauffeur} - Livraisons : {item.NbLivraisons}");
            }
        }

        public static void AfficherCommandesParPeriode()
        {
            DateTime dateDebut = Utils.LireDateObligatoire("Date de début : ");
            DateTime? dateFin = Utils.LireDate("Date de fin (vide si pas de fin) : ");

            if (dateFin.HasValue && dateFin < dateDebut)
            {
                Console.WriteLine("La date de fin ne peut pas être antérieure à la date de début.");
                return;
            }

            var commandes = CommandeManager.ChargerCommandes();
            var commandesFiltrees = commandes.Where(c =>
                c.DateCommande >= dateDebut &&
                (!dateFin.HasValue || c.DateCommande <= dateFin.Value)).ToList();

            Console.WriteLine($"===== COMMANDES DU {dateDebut:yyyy-MM-dd} {(dateFin.HasValue ? $"AU {dateFin:yyyy-MM-dd}" : "JUSQU'À MAINTENANT")} =====");

            if (commandesFiltrees.Count == 0)
            {
                Console.WriteLine("Aucune commande trouvée pour cette période.");
                return;
            }

            foreach (var commande in commandesFiltrees)
            {
                Console.WriteLine($"{commande.Client.Nom} {commande.Client.Prenom} - {commande.DateCommande:yyyy-MM-dd} - Prix : {commande.Prix:F2}e");
            }
        }


        public static void AfficherMoyennePrixCommandes()
        {
            var commandes = CommandeManager.ChargerCommandes();
            if (!commandes.Any())
            {
                Console.WriteLine("Aucune commande trouvée.");
                return;
            }

            var moyenne = commandes.Average(c => c.Prix);
            Console.WriteLine($"Moyenne des prix des commandes : {moyenne:F2}e");
        }

        public static void AfficherMoyenneCommandesParClient()
        {
            var commandes = CommandeManager.ChargerCommandes();

            var groupes = commandes
                .GroupBy(c => c.Client.AdresseMail)
                .Select(g => g.Count());

            if (!groupes.Any())
            {
                Console.WriteLine("Aucune commande trouvée.");
                return;
            }

            double moyenne = groupes.Average();
            Console.WriteLine($"Moyenne des commandes par client : {moyenne:F2}");
        }
        
        public static void AfficherCommandesClient(Client client)
        {
            var commandes = CommandeManager.ChargerCommandes();

            var commandesClient = commandes
                .Where(c => c.Client.AdresseMail == client.AdresseMail);

            if (!commandesClient.Any())
            {
                Console.WriteLine("Aucune commande trouvée.");
                return;
            }
            
            foreach (var commande in commandesClient)
            {
                Console.WriteLine($"{commande.Client.Nom} {commande.Client.Prenom} - {commande.DateCommande:yyyy-MM-dd} - Prix : {commande.Prix:F2}e");
            }
        }
    }

}
