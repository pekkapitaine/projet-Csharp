using Projet_C__A3;
using Projet_C__A3.Personne;
using System.Globalization;

public static class ClientManager
{
    private const string FichierCSV = "Stockage/clients.csv";

    public static void SauvegarderClients(List<Client> clients, bool onlyIfNoFile = false)
    {
        if (onlyIfNoFile && File.Exists(FichierCSV))
        {
            return;
        }

        string dossier = Path.GetDirectoryName(FichierCSV)!;
        if (!string.IsNullOrEmpty(dossier) && !Directory.Exists(dossier))
            Directory.CreateDirectory(dossier);

        using var writer = new StreamWriter(FichierCSV, false);
        writer.WriteLine("Nom;Prenom;DateNaissance;AdressePostale;AdresseMail;Telephone");

        foreach (var c in clients)
        {
            writer.WriteLine(string.Join(";", new string[]
            {
                c.Nom,
                c.Prenom,
                c.DateNaissance.ToString(),
                c.AdressePostale,
                c.AdresseMail,
                c.Telephone
            }));
        }
    }
    public static void SauvegarderClient(Client c)
    {
        if (!File.Exists(FichierCSV))
        {
            SauvegarderClients(new List<Client> { c });
            return;
        }

        using var writer = new StreamWriter(FichierCSV, true);
        writer.WriteLine(string.Join(";", new string[]
        {
            c.Nom,
            c.Prenom,
            c.DateNaissance.ToString(),
            c.AdressePostale,
            c.AdresseMail,
            c.Telephone
        }));
    }

    public static List<Client> ChargerClients()
    {
        if (!File.Exists(FichierCSV))
            return new List<Client>();

        var lignes = File.ReadAllLines(FichierCSV).Skip(1);
        var clients = new List<Client>();

        foreach (var ligne in lignes)
        {
            var champs = ligne.Split(';');
            DateTime.TryParse(champs[2], out var dateNaissance);

            clients.Add(new Client(
                champs[0],
                champs[1],
                dateNaissance,
                champs[3],
                champs[4],
                champs[5]
            ));
        }

        return clients;
    }

    public static void AjouterClient(Client client)
    {
        var clients = ChargerClients();
        if (clients.Any(c => c.AdresseMail.Equals(client.AdresseMail, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("Un client avec cette adresse mail existe déjà.");
            return;
        }

        clients.Add(client);
        SauvegarderClients(clients);
    }

    public static void CreerClientDepuisConsole(string mail="" )
    {
        Console.WriteLine("===== AJOUT D'UN CLIENT =====");

        Console.Write("Nom : ");
        string nom = Console.ReadLine()!.Trim();

        Console.Write("Prénom : ");
        string prenom = Console.ReadLine()!.Trim();

        Console.Write("Date de naissance (yyyy-MM-dd) : ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateNaissance))

        Console.Write("Adresse postale : ");
        string adressePostale = Console.ReadLine()!.Trim();
        string adresseMail;
        if (mail == "")
        {
            
            var clients = ChargerClients();
            do
            {
                Console.Write("Adresse mail : ");
                adresseMail = Console.ReadLine()!.Trim();

                if (string.IsNullOrWhiteSpace(adresseMail))
                {
                    Console.WriteLine("L'adresse mail ne peut pas être vide.");
                    continue;
                }

                if (clients.Any(c => c.AdresseMail.Equals(adresseMail, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Cette adresse mail est déjà utilisée par un autre client.");
                    adresseMail = "";
                }

            } while (string.IsNullOrWhiteSpace(adresseMail));
        } else { adresseMail = mail;  Console.WriteLine($"Adresse mail : {adresseMail}"); }



            Console.Write("Téléphone : ");
        string telephone = Console.ReadLine()!.Trim();

        Client nouveauClient = new (nom, prenom, dateNaissance, adressePostale, adresseMail, telephone);

        AjouterClient(nouveauClient);
        Console.WriteLine("Client ajouté avec succès.");
        Thread.Sleep(1500);
    }


    public static void SupprimerClient(string email)
    {
        var clients = ChargerClients();
        var cible = clients.FirstOrDefault(c => c.AdresseMail.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (cible != null)
        {
            clients.Remove(cible);
            SauvegarderClients(clients);
            Console.WriteLine("Client supprimé.");
        }
        else
        {
            Console.WriteLine("Client non trouvé.");
        }
    }
    
    public static Client ChoisirClient()
    {
        var clients = ClientManager.ChargerClients();
            
        if (clients.Count == 0)
        {
            Console.WriteLine("Aucun client disponible.");
            return null;
        }

        Console.WriteLine("===== LISTE DES CLIENTS =====");
        for (var i = 0; i < clients.Count; i++)
        {
            var client = clients[i];
            Console.WriteLine($"{i + 1}. {client.Prenom} {client.Nom} - {client.AdresseMail}");
        }

        int choix;
        do
        {
            Console.Write("Entrez le numéro du client souhaité : ");
            string saisie = Console.ReadLine();

            if (int.TryParse(saisie, out choix) && choix >= 1 && choix <= clients.Count)
            {
                return clients[choix - 1];
            }

            Console.WriteLine("Numéro invalide. Veuillez réessayer.");
        }
        while (true);
    }


    public static void ModifierClient(string email)
    {
        var clients = ChargerClients();
        var client = clients.FirstOrDefault(c => c.AdresseMail.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (client == null)
        {
            Console.WriteLine("Client introuvable.");
            return;
        }

        Console.WriteLine("1. Nom");
        Console.WriteLine("2. Prénom");
        Console.WriteLine("3. Adresse postale");
        Console.WriteLine("4. Adresse mail");
        Console.WriteLine("5. Téléphone");
        Console.WriteLine("6. Date de naissance");
        Console.WriteLine("7. Terminer");

        while (true)
        {
            Console.Write("Choix (1-7) : ");
            var choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    Console.Write("Nouveau nom : ");
                    client.Nom = Console.ReadLine()!;
                    break;
                case "2":
                    Console.Write("Nouveau prénom : ");
                    client.Prenom = Console.ReadLine()!;
                    break;
                case "3":
                    Console.Write("Nouvelle adresse postale : ");
                    client.AdressePostale = Console.ReadLine()!;
                    break;
                case "4":
                    Console.Write("Nouvelle adresse mail : ");
                    var newEmail = Console.ReadLine()!;
                    if (clients.Any(c => c.AdresseMail.Equals(newEmail, StringComparison.OrdinalIgnoreCase) && c != client))
                    {
                        Console.WriteLine("Adresse déjà utilisée.");
                    }
                    else
                    {
                        client.AdresseMail = newEmail;
                    }
                    break;
                case "5":
                    Console.Write("Nouveau téléphone : ");
                    client.Telephone = Console.ReadLine()!;
                    break;
                case "6":
                    var date = Utils.LireDate("Nouvelle date de naissance (yyyy-MM-dd) : ");
                    if (date.HasValue) client.DateNaissance = date.Value;
                    break;
                case "7":
                    SauvegarderClients(clients);
                    Console.WriteLine("Modifications enregistrées.");
                    return;
                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }
    }
}
