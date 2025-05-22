using Projet_C__A3.Manager;
using Projet_C__A3.Personne;
using System.Globalization;
using static System.Net.WebRequestMethods;
using Projet_C__A3.Graphes;
using File = System.IO.File;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Projet_C__A3
{
    public static class CommandeManager
    {
        private static readonly string cheminFichier = "ressources/commandes.csv";

        public static void StockerCommande(Commande commande)
        {
            string dossier = Path.GetDirectoryName(cheminFichier)!;

            if (!string.IsNullOrEmpty(dossier) && !Directory.Exists(dossier))
            {
                Directory.CreateDirectory(dossier);
            }

            bool existe = File.Exists(cheminFichier);
            using (var writer = new StreamWriter(cheminFichier, true))
            {
                if (!existe)
                {
                    writer.WriteLine("NomClient;PrenomClient;AdresseDepart;AdresseArrivee;Kilometrage;ImmatVehicule;TarifParKm;EmailChauffeur;DateCommande;Prix;PaiementEffectue");
                }

                writer.WriteLine(string.Join(";", new string[]
                {
                    commande.Client.Nom,
                    commande.Client.Prenom,
                    commande.VilleDepart,
                    commande.VilleArrivee,
                    commande.Kilometrage.ToString(CultureInfo.InvariantCulture),
                    commande.Vehicule.Immatriculation,
                    commande.Vehicule.TarifParKm.ToString(CultureInfo.InvariantCulture),
                    commande.Chauffeur.AdresseMail,
                    commande.DateCommande.ToString("yyyy-MM-dd"),
                    commande.Prix.ToString(CultureInfo.InvariantCulture),
                    commande.PaiementEffectue.ToString()
                }));
            }
        }

        public static void SupprimerCommande(string nomClient, string prenomClient, DateTime dateCommande)
        {
            if (!File.Exists(cheminFichier)) return;

            var lignes = File.ReadAllLines(cheminFichier).ToList();
            lignes = lignes.Where((ligne, index) =>
            {
                if (index == 0) return true;

                var parts = ligne.Split(';');
                return !(parts[0] == nomClient &&
                         parts[1] == prenomClient &&
                         DateTime.TryParseExact(parts[8], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) &&
                         date == dateCommande);
            }).ToList();

            File.WriteAllLines(cheminFichier, lignes);
        }

        public static List<Commande> ChargerCommandes()
        {
            var Commandes = new List<Commande>();
            if (!File.Exists(cheminFichier)) return Commandes;

            var lignes = File.ReadAllLines(cheminFichier).Skip(1);
            foreach (var ligne in lignes)
            {
                var parts = ligne.Split(';');
                if (parts.Length < 11) continue;

                var client = new Client(parts[0], parts[1], DateTime.MinValue, "", "", "");
                var adresseDepart = parts[2];
                var adresseArrivee = parts[3];
                var km = double.Parse(parts[4], CultureInfo.InvariantCulture);
                var vehicule = new Voiture(parts[5], decimal.Parse(parts[6], CultureInfo.InvariantCulture), 4);
                var chauffeur = new Salarie("000", "", "", DateTime.MinValue, parts[7], "", "", DateTime.MinValue, Role.Chauffeur);
                var dateCommande = DateTime.ParseExact(parts[8], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var paiement = bool.Parse(parts[10]);

                var Commande = new Commande(client, adresseDepart, adresseArrivee, km, vehicule, chauffeur, dateCommande, paiement);
                Commandes.Add(Commande);
            }

            return Commandes;
        }

        public static void AjouterCommandeDepuisConsole(Graphe graphe)
        {
            Console.Clear();
            Console.WriteLine("=== Ajout d'une commande ===");

            var clients = ClientManager.ChargerClients();
            var chauffeurs = SalarieManager.ChargerSalaries().Where(s => s.Poste == Role.Chauffeur).ToList();
            var vehicules = VehiculeManager.ChargerVehicules();

            Client? client = null;
            string mailClient;

            while (true)
            {
                Console.Write("Adresse mail du client : ");
                mailClient = Console.ReadLine()!;

                client = clients.FirstOrDefault(c => c.AdresseMail.Equals(mailClient, StringComparison.OrdinalIgnoreCase));

                if (client != null)
                {
                    Console.WriteLine($"Client trouvé : {client.Prenom} {client.Nom} ({client.AdresseMail})");
                    break;
                }

                Console.WriteLine("Client introuvable.");
                Console.Write("Souhaitez-vous créer un nouveau client avec cette adresse mail ? (o/n) : ");
                string choix = Console.ReadLine()!.Trim().ToLower();

                if (choix == "o")
                {
                    ClientManager.CreerClientDepuisConsole(mailClient);
                    break;
                }
                else
                {
                    Console.WriteLine("Veuillez ressaisir l'adresse mail.");
                }
            }

            var villesDisponible = graphe.ObtenirListeVilles();

            string adresseDepart;
            do
            {
                Console.Write("Ville de départ : ");
                adresseDepart = Console.ReadLine()!;
                if (!villesDisponible.Any(v => v.Equals(adresseDepart, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Ville inconnue. Veuillez réessayer.");
                }
            } while (!villesDisponible.Any(v => v.Equals(adresseDepart, StringComparison.OrdinalIgnoreCase)));

            string adresseArrivee;
            do
            {
                Console.Write("Ville d'arrivée : ");
                adresseArrivee = Console.ReadLine()!;
                if (!villesDisponible.Any(v => v.Equals(adresseArrivee, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Ville inconnue. Veuillez réessayer.");
                }
            } while (!villesDisponible.Any(v => v.Equals(adresseArrivee, StringComparison.OrdinalIgnoreCase)));
            adresseDepart = villesDisponible.First(v => v.Equals(adresseDepart, StringComparison.OrdinalIgnoreCase));
            adresseArrivee = villesDisponible.First(v => v.Equals(adresseArrivee, StringComparison.OrdinalIgnoreCase));

            DateTime date = Utils.LireDateObligatoire("Date de la commande (yyyy-MM-dd) : ");

            var km = graphe.FindShortestPathDjikstra(adresseDepart, adresseArrivee).Item2;

            var vehiculesDisponibles = VehiculeManager.ObtenirVehiculesDisponibles(date);
            Console.WriteLine($"=== 5 véhicules disponibles le {date:dd/MM/yyyy} ===");
            foreach (var v in vehiculesDisponibles.Take(5))
            {
                Console.WriteLine(v);
            }

            // ==== Sélection du véhicule ====
            Vehicule? vehicule = null;
            while (vehicule == null)
            {
                Console.Write("Immatriculation du véhicule : ");
                string immat = Console.ReadLine()!;
                vehicule = vehiculesDisponibles.FirstOrDefault(v => v.Immatriculation == immat);
                if (vehicule == null)
                {
                    Console.WriteLine("Véhicule indisponible ou introuvable. Veuillez réessayer.");
                }
            }

            // ==== Sélection du chauffeur ====
            var chauffeursDisponibles = SalarieManager.ObtenirChauffeursDisponibles(date);
            Console.WriteLine($"=== 5 chauffeurs disponibles le {date:dd/MM/yyyy} ===");
            foreach (var c in chauffeursDisponibles.Take(5))
            {
                Console.WriteLine(c);
            }

            Salarie? chauffeur = null;
            while (chauffeur == null)
            {
                Console.Write("Adresse mail du chauffeur : ");
                string mail = Console.ReadLine()!;
                chauffeur = chauffeursDisponibles.FirstOrDefault(c => c.AdresseMail == mail);
                if (chauffeur == null)
                {
                    Console.WriteLine("Chauffeur indisponible ou introuvable. Veuillez réessayer.");
                }
            }

            Console.Write("Le paiement est-il effectué ? (o/n) : ");
            bool paiement = Console.ReadLine()!.ToLower() == "o";

            try
            {
                var commande = new Commande(client, adresseDepart, adresseArrivee, graphe.FindShortestPathDjikstra(adresseDepart, adresseArrivee).Item2, vehicule, chauffeur, date, paiement);
                StockerCommande(commande);
                Console.WriteLine("Commande ajoutée avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }

            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        public static void ModifierCommandeDepuisConsole(Graphe graphe)
        {
            Console.Clear();
            Console.WriteLine("=== Modification d'une commande ===");
            Console.Write("Nom du client : ");
            string nom = Console.ReadLine()!;
            Console.Write("Prénom du client : ");
            string prenom = Console.ReadLine()!;
            Console.Write("Date de la commande (yyyy-MM-dd) : ");
            DateTime date = DateTime.Parse(Console.ReadLine()!);

            SupprimerCommande(nom, prenom, date);
            AjouterCommandeDepuisConsole(graphe);  // On recrée une commande modifiée
        }

        public static void SupprimerCommandeDepuisConsole()
        {
            Console.Clear();
            Console.WriteLine("=== Suppression d'une commande ===");
            Console.Write("Nom du client : ");
            string nom = Console.ReadLine()!;
            Console.Write("Prénom du client : ");
            string prenom = Console.ReadLine()!;
            Console.Write("Date de la commande (yyyy-MM-dd) : ");
            DateTime date = DateTime.Parse(Console.ReadLine()!);

            SupprimerCommande(nom, prenom, date);
            Console.WriteLine("Commande supprimée (si elle existait).");
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        public static void AfficherCommandesDepuisConsole()
        {
            Console.Clear();
            Console.WriteLine("=== Liste des commandes ===");

            var commandes = ChargerCommandes();

            if (!commandes.Any())
            {
                Console.WriteLine("Aucune commande enregistrée.");
            }
            else
            {
                foreach (var c in commandes)
                {
                    Console.WriteLine(c);
                }
            }

            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

    }
}