using Projet_C__A3;
using Projet_C__A3.Graphes;
using Projet_C__A3.Manager;
using Projet_C__A3.Personne;

class Program
{
    public static Graphe graphe = new Graphe("ressources/distances2.csv");

    static void Main()
    {
        // Sauvegarder les salariés dans le CSV
        SalarieManager.SauvegarderSalaries(BaseData.Salaries(), true);

        // Sauvegarder les clients dans le CSV
        ClientManager.SauvegarderClients(BaseData.Clients(), true);

        // Sauvegarder les vehicules dans le CSV
        VehiculeManager.SauvegarderVehicules(BaseData.Vehicules(), true);

        graphe.AfficherMatriceAdjacence();

        graphe.AfficherListeAdjacence();

        Console.WriteLine("=== PARCOURS ===");

        graphe.ParcoursLargeur(Ville.Paris);
        graphe.ParcoursProfondeur(Ville.Paris);
        graphe.VisualiserGraphe();

        var (cheminDjikstra, distanceDjikstra) = graphe.FindShortestPathDjikstra(nameof(Ville.Paris), nameof(Ville.Nice));
        Console.WriteLine("distance Paris - Nice, Djikstra: " + distanceDjikstra + ", CHEMIN: " +
                          string.Join(" => ", cheminDjikstra));

        var (cheminBellmanFord, distanceBellmanFord) = graphe.FindShortestPathBellmanFord(nameof(Ville.Paris), nameof(Ville.Nice));
        Console.WriteLine("distance Paris - Nice, BellmanFord: " + distanceBellmanFord + ", CHEMIN: " +
                          string.Join(" => ", cheminBellmanFord));
        
        var (cheminFloydWarshall, distanceFloydWarshall) = graphe.FindShortestPathFloydWarshall(nameof(Ville.Paris), nameof(Ville.Nice));
        Console.WriteLine("distance Paris - Nice, FloydWarshall: " + distanceFloydWarshall + ", CHEMIN: " +
                          string.Join(" => ", cheminFloydWarshall));
        
        graphe.VisualiserGraphe(cheminFloydWarshall);

        Console.WriteLine("Connexe? : " + graphe.EstConnexe());
        Console.WriteLine("Cycle? : " + graphe.ContientCycle());
        
        Console.WriteLine("Appuyez sur une touche pour lancer le programme...");
        Console.ReadKey(true);
        AfficherMenuPrincipal();
    }

    public static void AfficherMenuPrincipal()
    {
        ConsoleManager.Hello();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("======= MENU PRINCIPAL =======");
            Console.WriteLine("1. Gérer les salariés");
            Console.WriteLine("2. Gérer les clients");
            Console.WriteLine("3. Gérer les commandes");
            Console.WriteLine("4. Gérer les vehicules");
            Console.WriteLine("5. Statistiques");
            Console.WriteLine("0. Quitter");
            Console.Write("Votre choix : ");
            var choix = Console.ReadKey(true);
            Console.WriteLine();

            switch (choix.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    AfficherMenuSalaries();
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    AfficherMenuClients();
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    AfficherMenuCommandes();
                    break;
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    AfficherMenuVehicule();
                    break;
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    AfficherMenuStatistiques();
                    break;
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    return;
                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }
    }

    public static void AfficherMenuSalaries()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== GESTION DES SALARIÉS =====");
            Console.WriteLine("1. Ajouter un salarié");
            Console.WriteLine("2. Modifier un salarié");
            Console.WriteLine("3. Supprimer un salarié");
            Console.WriteLine("4. Afficher l’arborescence des salariés");
            Console.WriteLine("0. Retour");
            Console.Write("Choix : ");
            var choix = Console.ReadKey(true);
            Console.WriteLine();

            switch (choix.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    SalarieManager.CreerSalarieDepuisConsole();
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    Console.Write("Mail de l'employé à modifier: ");
                    string? employeAModifier = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(employeAModifier))
                    {
                        SalarieManager.ModifieSalarie(employeAModifier);
                    }

                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    Console.Write("Mail de l'employé à supprimer: ");
                    string? employeASupprimer = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(employeASupprimer))
                    {
                        SalarieManager.SupprimerSalarie(employeASupprimer);
                    }

                    break;
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    var racine = SalarieManager
                        .ChargerSalaries()
                        .FirstOrDefault(s => s.Superieur == null);
                    if (racine == null) return;
                    racine.AfficherArborescenceGraphique();
                    Console.Clear();
                    Console.WriteLine("=== ARBORESCENCE DES SALARIÉS ===\n");
                    racine.AfficherArborescence();
                    Console.WriteLine();
                    Console.WriteLine("### Arborescence aussi disponible graphiquement, fichier arborescence.png");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey(true);
                    break;
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    return;
                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }
    }


    public static void AfficherMenuClients()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== GESTION DES CLIENTS =====");
            Console.WriteLine("1. Ajouter un client");
            Console.WriteLine("2. Modifier un client");
            Console.WriteLine("3. Supprimer un client");
            Console.WriteLine("4. Afficher tous les clients");
            Console.WriteLine("0. Retour");
            Console.Write("Choix : ");
            var choix = Console.ReadKey(true);
            Console.WriteLine();

            switch (choix.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    ClientManager.CreerClientDepuisConsole();
                    break;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    Console.Write("Mail du client à modifier : ");
                    string numModif = Console.ReadLine()!;
                    ClientManager.ModifierClient(numModif);
                    Thread.Sleep(1000);
                    break;

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    Console.Write("Mail du client à supprimer : ");
                    string numSup = Console.ReadLine()!;
                    ClientManager.SupprimerClient(numSup);
                    Thread.Sleep(1000);
                    break;

                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    var clients = ClientManager.ChargerClients();
                    Console.WriteLine("Trier les clients par :");
                    Console.WriteLine("1. Nom");
                    Console.WriteLine("2. Prénom");
                    Console.WriteLine("3. Ville");

                    var triChoix = Console.ReadKey(true).Key;
                    IEnumerable<Client> clientsTries = clients;

                    switch (triChoix)
                    {
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            clientsTries = clients.OrderBy(c => c.Nom).ThenBy(c => c.Prenom);
                            break;

                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2:
                            clientsTries = clients.OrderBy(c => c.Prenom).ThenBy(c => c.Nom);
                            break;

                        case ConsoleKey.D3:
                        case ConsoleKey.NumPad3:
                            clientsTries = clients.OrderBy(c => c.AdressePostale).ThenBy(c => c.Prenom);
                            break;

                        default:
                            Console.WriteLine("Choix invalide. Affichage par nom.");
                            clientsTries = clients.OrderBy(c => c.Nom).ThenBy(c => c.Prenom);
                            break;
                    }

                    Console.Clear();
                    Console.WriteLine("=== Liste des clients  : ===");
                    foreach (var c in clientsTries)
                    {
                        Console.WriteLine($"- {c.Prenom} {c.Nom} | {c.AdresseMail} | {c.AdressePostale}");
                    }

                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey(true);
                    break;


                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    return;

                default:
                    Console.WriteLine("Choix invalide.");
                    Thread.Sleep(1000);
                    break;
            }
        }
    }


    public static void AfficherMenuCommandes()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== GESTION DES COMMANDES =====");
            Console.WriteLine("1. Ajouter une commande");
            Console.WriteLine("2. Modifier une commande");
            Console.WriteLine("3. Supprimer une commande");
            Console.WriteLine("4. Afficher toutes les commandes");
            Console.WriteLine("0. Retour");
            Console.Write("Choix : ");
            var choix = Console.ReadKey(true);
            Console.WriteLine();

            switch (choix.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    CommandeManager.AjouterCommandeDepuisConsole(graphe);
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    CommandeManager.ModifierCommandeDepuisConsole(graphe);
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    CommandeManager.SupprimerCommandeDepuisConsole();
                    break;
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    CommandeManager.AfficherCommandesDepuisConsole();
                    break;
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    return;
                default:
                    Console.WriteLine("Choix invalide.");
                    Thread.Sleep(1000);
                    break;
            }
        }
    }


    public static void AfficherMenuStatistiques()
    {
        Console.Clear();
        Console.WriteLine("===== STATISTIQUES =====");
        Console.WriteLine("1. Nombre de livraison par chauffeur");
        Console.WriteLine("2. Nombre de commandes par client");
        Console.WriteLine("3. Prix moyen des commandes");
        Console.WriteLine("4. Prix moyen par clients");
        Console.WriteLine("0. Retour");
        Console.Write("Choix : ");
        var choix = Console.ReadKey(true);
        Console.WriteLine();

        switch (choix.Key)
        {
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
                StatistiquesManager.AfficherNombreLivraisonsParChauffeur();
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey(false);
                break;
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                StatistiquesManager.AfficherCommandesParPeriode();
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey(false);
                break;
            case ConsoleKey.D3:
            case ConsoleKey.NumPad3:
                StatistiquesManager.AfficherMoyennePrixCommandes();
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey(false);
                break;
            case ConsoleKey.D4:
            case ConsoleKey.NumPad4:
                StatistiquesManager.AfficherMoyenneCommandesParClient();
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey(false);
                break;
            case ConsoleKey.D5:
            case ConsoleKey.NumPad5:
                var client = ClientManager.ChoisirClient();
                StatistiquesManager.AfficherCommandesClient(client);
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey(false);
                break;
            case ConsoleKey.D0:
            case ConsoleKey.NumPad0:
                return;
            default:
                Console.WriteLine("Choix invalide.");
                break;
        }
    }

    public static void AfficherMenuVehicule()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== GESTION DES VEHICULES =====");
            Console.WriteLine("1. Ajouter un véhicule");
            Console.WriteLine("2. Modifier un véhicule");
            Console.WriteLine("3. Supprimer un véhicule");
            Console.WriteLine("4. Afficher tous les véhicules");
            Console.WriteLine("0. Retour");
            Console.Write("\nChoix : ");

            var choix = Console.ReadKey(true);
            Console.WriteLine();

            switch (choix.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    VehiculeManager.AjouterVehiculeDepuisConsole();
                    break;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    Console.Write("Immatriculation du véhicule à modifier : ");
                    string immatModif = Console.ReadLine()!;
                    VehiculeManager.ModifierVehicule(immatModif);
                    break;

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    Console.Write("Immatriculation du véhicule à supprimer : ");
                    string immatSupp = Console.ReadLine()!;
                    VehiculeManager.SupprimerVehicule(immatSupp);
                    Console.WriteLine("Véhicule supprimé si trouvé.");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey(true);
                    break;

                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    VehiculeManager.AfficherTousVehicules();
                    break;

                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    return;

                default:
                    Console.WriteLine("Choix invalide. Appuyez sur une touche pour réessayer...");
                    Console.ReadKey(true);
                    break;
            }
        }
    }
}