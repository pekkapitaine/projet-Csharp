using Projet_C__A3;
using Projet_C__A3.Personne;
using System.Globalization;

public static class SalarieManager
{
    private const string FichierCSV = "Stockage/salaries.csv";

    public static void SauvegarderSalaries(List<Salarie> salaries, bool onlyIfNoFile = false)
    {
        if (onlyIfNoFile && File.Exists(FichierCSV))
        {
            return;
        }

        string? dossier = Path.GetDirectoryName(FichierCSV);
        if (!string.IsNullOrEmpty(dossier) && !Directory.Exists(dossier))
        {
            Directory.CreateDirectory(dossier);
        }

        using (var writer = new StreamWriter(FichierCSV, false))
        {
            writer.WriteLine("NumeroSS;Nom;Prenom;DateNaissance;AdressePostale;AdresseMail;Telephone;DateEntree;Poste;MailSuperieur;Salaire");

            foreach (var s in salaries)
            {
                writer.WriteLine(string.Join(";", new string[]
                {
                s.NumeroSS,
                s.Nom,
                s.Prenom,
                s.DateNaissance?.ToString("yyyy-MM-dd"),
                s.AdressePostale,
                s.AdresseMail,
                s.Telephone,
                s.DateEntree?.ToString("yyyy-MM-dd"),
                s.Poste?.ToString(),
                s.Superieur?.AdresseMail ?? "",
                s.Salaire?.ToString(CultureInfo.InvariantCulture)
                }));
            }
        }
    }
    public static void SauvegarderSalarie(Salarie s)
    {
        string dossier = Path.GetDirectoryName(FichierCSV)!;
        if (!string.IsNullOrEmpty(dossier) && !Directory.Exists(dossier))
        {
            Directory.CreateDirectory(dossier);
        }

        bool existe = File.Exists(FichierCSV);

        using (var writer = new StreamWriter(FichierCSV, true))
        {
            if (!existe)
            {
                writer.WriteLine("NumeroSS;Nom;Prenom;DateNaissance;AdressePostale;AdresseMail;Telephone;DateEntree;Poste;MailSuperieurSuperieur;Salaire");
            }

            writer.WriteLine(string.Join(";", new string[]
            {
            s.NumeroSS,
            s.Nom,
            s.Prenom,
            s.DateNaissance?.ToString("yyyy-MM-dd"),
            s.AdressePostale,
            s.AdresseMail,
            s.Telephone,
            s.DateEntree?.ToString("yyyy-MM-dd"),
            s.Poste.ToString(),
            s.Superieur?.AdresseMail ?? "",
            s.Salaire?.ToString(CultureInfo.InvariantCulture)
            }));
        }
    }


    public static List<Salarie> ChargerSalaries()
    {
        var lines = File.ReadAllLines(FichierCSV).Skip(1); // Ignore header
        var tempSuperieurs = new Dictionary<string, string?>();
        var salaries = new List<Salarie>();

        foreach (var line in lines)
        {
            var champs = line.Split(';');

            // Parse poste (enum Role?), accepte valeur vide
            Role? poste = null;
            string posteStr = champs.Length > 8 ? champs[8] : null;
            if (!string.IsNullOrWhiteSpace(posteStr) && Enum.TryParse<Role>(posteStr, out var parsedPoste))
            {
                poste = parsedPoste;
            }

            // Parse dateNaissance
            DateTime dateNaissance = DateTime.MinValue;
            if (champs.Length > 3 && !string.IsNullOrWhiteSpace(champs[3]))
            {
                DateTime.TryParse(champs[3], out dateNaissance);
            }

            // Parse dateEntree
            DateTime dateEntree = DateTime.MinValue;
            if (champs.Length > 7 && !string.IsNullOrWhiteSpace(champs[7]))
            {
                DateTime.TryParse(champs[7], out dateEntree);
            }

            // Parse salaire decimal, default 0
            decimal salaire = 0;
            if (champs.Length > 10 && !string.IsNullOrWhiteSpace(champs[10]))
            {
                decimal.TryParse(champs[10], NumberStyles.Any, CultureInfo.InvariantCulture, out salaire);
            }

            var salarie = new Salarie(
                numeroSS: champs.Length > 0 ? champs[0] : "",
                nom: champs.Length > 1 ? champs[1] : "",
                prenom: champs.Length > 2 ? champs[2] : "",
                dateNaissance: dateNaissance,
                adressePostale: champs.Length > 4 ? champs[4] : "",
                adresseMail: champs.Length > 5 ? champs[5] : "",
                telephone: champs.Length > 6 ? champs[6] : "",
                dateEntree: dateEntree,
                poste: poste,
                superieur: null,
                subordonnes: new List<Salarie>(),
                salaire: salaire
            );

            salaries.Add(salarie);

            // Stockage temporaire du mail du supérieur (peut être vide)
            tempSuperieurs[salarie.AdresseMail] = (champs.Length > 9 && !string.IsNullOrWhiteSpace(champs[9])) ? champs[9] : null;
        }

        // 2ème passe : affectation des supérieurs et subordonnés
        foreach (var salarie in salaries)
        {
            if (tempSuperieurs.TryGetValue(salarie.AdresseMail, out var numeroSuperieur) && !string.IsNullOrWhiteSpace(numeroSuperieur))
            {
                var sup = salaries.FirstOrDefault(s => s.AdresseMail == numeroSuperieur);
                if (sup != null)
                {
                    salarie.Superieur = sup;
                    sup.Subordonnes.Add(salarie);
                }
            }
        }

        return salaries.Where(s => s.Superieur == null).ToList();
    }

    public static void AjouterSalarie(Salarie nouveau)
    {
        var tous = ChargerSalaries().SelectMany(GetTousLesSalaries).ToList();
        tous.Add(nouveau);
        SauvegarderSalaries(tous);
    }

    public static void SupprimerSalarie(string mail)
    {
        var salaries = ChargerSalariesPlats();

        var cible = salaries.FirstOrDefault(s => s.AdresseMail == mail);
        if (cible == null) { 
            Console.WriteLine("Aucun salarié trouvé avec ce mail.");
            Thread.Sleep(2000);
            return;
        }

        var superieurDeCible = cible.Superieur;

        // Supprimer la cible
        salaries.Remove(cible);

        // Mettre à jour les supérieurs des subordonnés directs
        foreach (var s in salaries)
        {
            if (s.Superieur != null && s.Superieur.AdresseMail == mail)
            {
                s.Superieur = superieurDeCible;
                s.MailSuperieurTemp = superieurDeCible?.AdresseMail;
                if (superieurDeCible != null && !superieurDeCible.Subordonnes.Contains(s))
                {
                    superieurDeCible.Subordonnes.Add(s);
                }
            }
        }

        // Nettoyer l'ancienne liste de subordonnés du supérieur supprimé
        if (superieurDeCible != null)
        {
            superieurDeCible.Subordonnes.Remove(cible);
        }
        Console.WriteLine($"Suppression avec succès de {cible.Nom} {cible.Prenom} ({cible.AdresseMail}) ");
        Thread.Sleep(2000);
        SauvegarderSalaries(salaries);
    }

    public static List<Salarie> ObtenirChauffeursDisponibles(DateTime date) 
    {
        var chauffeurs = ChargerSalaries()
            .Where(s => s.Poste == Role.Chauffeur)
            .ToList();

        var commandes = CommandeManager.ChargerCommandes();

        var chauffeursDisponibles = chauffeurs
            .Where(ch => !commandes.Any(c => c.Chauffeur.AdresseMail == ch.AdresseMail && c.DateCommande.Date == date.Date))
            .ToList();
        return chauffeursDisponibles;
    }

    public static List<Salarie> ChargerSalariesPlats()
    {
        var lines = File.ReadAllLines(FichierCSV).Skip(1);
        var salaries = new List<Salarie>();

        // 1ère passe : création sans supérieur (superieur=null)
        foreach (var line in lines)
        {
            var champs = line.Split(';');

            DateTime.TryParse(champs[3], out var dateNaissance);
            DateTime.TryParse(champs[7], out var dateEntree);

            Role? poste = null;
            if (!string.IsNullOrWhiteSpace(champs[8]))
            {
                if (Enum.TryParse<Role>(champs[8], out var roleParsed))
                    poste = roleParsed;
            }

            decimal salaire = 0;
            if (!string.IsNullOrWhiteSpace(champs[10]))
            {
                var champSalaire = champs[10].Replace(',', '.');
                decimal.TryParse(champSalaire, NumberStyles.Any, CultureInfo.InvariantCulture, out salaire);
            }
            var salarie = new Salarie(
                numeroSS: champs[0],
                nom: champs[1],
                prenom: champs[2],
                dateNaissance: dateNaissance,
                adressePostale: champs[4],
                adresseMail: champs[5],
                telephone: champs[6],
                dateEntree: dateEntree,
                poste: poste,
                superieur: null,
                subordonnes: new List<Salarie>(),
                salaire: salaire
            );

            // Stocker temporairement le numéro SS du supérieur
            salarie.MailSuperieurTemp = string.IsNullOrWhiteSpace(champs[9]) ? null : champs[9];

            salaries.Add(salarie);
        }

        // 2ème passe : rattacher les supérieurs et construire les subordonnés
        foreach (var salarie in salaries)
        {
            if (!string.IsNullOrEmpty(salarie.MailSuperieurTemp))
            {
                var superieur = salaries.FirstOrDefault(s => s.AdresseMail == salarie.MailSuperieurTemp);
                if (superieur != null)
                {
                    salarie.Superieur = superieur;
                    superieur.Subordonnes.Add(salarie);
                }
            }
        }

        return salaries;
    }

    public static IEnumerable<Salarie> GetTousLesSalaries(Salarie s)
    {
        yield return s;
        foreach (var sub in s.Subordonnes)
        {
            foreach (var desc in GetTousLesSalaries(sub))
            {
                yield return desc;
            }
        }
    }


    public static void ViderCSV()
    {
        string dossier = Path.GetDirectoryName(FichierCSV)!;
        if (!string.IsNullOrEmpty(dossier) && !Directory.Exists(dossier))
            Directory.CreateDirectory(dossier);

        using (var writer = new StreamWriter(FichierCSV, false))
        {
            writer.WriteLine("NumeroSS;Nom;Prenom;DateNaissance;Adresse;Email;Telephone;DateEntree;Poste;SuperieurSS;Salaire");
        }
    }

    public static void CreerSalarieDepuisConsole()
    {
        var tous = ChargerSalaries().SelectMany(GetTousLesSalaries).ToList();
        Console.Clear();
        Console.WriteLine("=== AJOUT D'UN NOUVEAU SALARIÉ ===");

        string numeroSS;
        while (true)
        {
            Console.Write("Numéro de Sécurité Sociale : ");
            numeroSS = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(numeroSS)) break;
            if (tous.Any(s => s.NumeroSS == numeroSS))
            {
                Console.WriteLine("Un salarié avec ce numéro de SS existe déjà.");
                continue;
            }
            break;
        }
        Console.Write("Nom : ");
        string nom = Console.ReadLine()!;

        Console.Write("Prénom : ");
        string prenom = Console.ReadLine()!;

        DateTime? dateNaissance = Utils.LireDate("Date de naissance (yyyy-MM-dd, vide = non renseignée) : ");

        Console.Write("Adresse postale : ");
        string adressePostale = Console.ReadLine()!;

        string adresseMail;
        while (true)
        {
            Console.Write("Adresse email : ");
            adresseMail = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(adresseMail))
            {
                Console.WriteLine("Ce champ est obligatoire.");
                continue;
            }
            else if (tous.Any(s => s.AdresseMail?.Equals(adresseMail, StringComparison.OrdinalIgnoreCase) == true))
            {
                Console.WriteLine("Un salarié avec cette adresse email existe déjà.");
                continue;
            }
            else break;
        }

        Console.Write("Téléphone : ");
        string telephone = Console.ReadLine()!;

        DateTime? dateEntree = Utils.LireDate("Date d'entrée (yyyy-MM-dd, vide = non renseignée) : ");

        Role? poste = Utils.LireRole();

        decimal? salaire = Utils.LireDecimal("Salaire (en euros) : ");

        // Gestion du supérieur
        Salarie? superieur = null;
        string mailSup;
        do
        {
            Console.Write("Mail du supérieur: ");
            mailSup = Console.ReadLine()!;
           // if (string.IsNullOrWhiteSpace(mailSup))
           // {
                // L'utilisateur ne veut pas de supérieur
               // break;
            //}
            superieur = tous.FirstOrDefault(s => s.AdresseMail.Equals(mailSup, StringComparison.OrdinalIgnoreCase));

            if (superieur == null)
            {
                Console.WriteLine("Supérieur non trouvé. Veuillez réessayer ou laisser vide si aucun.");
            }
        } while (superieur == null && !string.IsNullOrWhiteSpace(mailSup));


        var nouveau = new Salarie(
            numeroSS, nom, prenom, dateNaissance ?? DateTime.MinValue,
            adressePostale, adresseMail, telephone,
            dateEntree ?? DateTime.MinValue,
            poste, superieur, null, salaire
        );

        AjouterSalarie(nouveau);
        Console.WriteLine("### Salarié ajouté avec succès ! ###");
        Thread.Sleep(1000);

    }

    public static void ModifieSalarie(string mail)
    {
        var salaries = ChargerSalaries();
        var tous = salaries.SelectMany(GetTousLesSalaries).Distinct().ToList();
        var salarie = salaries.SelectMany(GetTousLesSalaries).FirstOrDefault(s => s.AdresseMail == mail);

        if (salarie == null)
        {
            Console.WriteLine("Aucun salarié trouvé avec cet mail.");
            Thread.Sleep(1500);
            return;
        }

        Console.WriteLine($"\nModifier salarié : {salarie.Prenom} {salarie.Nom} ({salarie.AdresseMail})");
        Console.WriteLine("1. Nom");
        Console.WriteLine("2. Prénom");
        Console.WriteLine("3. Adresse mail");
        Console.WriteLine("4. Téléphone");
        Console.WriteLine("5. Adresse postale");
        Console.WriteLine("6. Poste");
        Console.WriteLine("7. Salaire");
        Console.WriteLine("8. Date de naissance");
        Console.WriteLine("9. Date d’entrée");
        Console.WriteLine("10. Terminer");

        while (true)
        {
            Console.Write("Choix (1-10) : ");
            var choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    Console.WriteLine($"Ancien nom : {salarie.Nom ?? "aucun"}");
                    Console.Write("Nouveau nom : ");
                    var nom = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nom)) salarie.Nom = nom;
                    break;

                case "2":
                    Console.WriteLine($"Ancien prénom : {salarie.Prenom ?? "aucun"}");
                    Console.Write("Nouveau prénom : ");
                    var prenom = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(prenom)) salarie.Prenom = prenom;
                    break;

                case "3":
                    Console.WriteLine($"Ancienne adresse mail : {salarie.AdresseMail}");
                    Console.Write("Nouvelle adresse mail : ");
                    var email = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        if (salaries.Any(s => s.AdresseMail == email))
                            Console.WriteLine("Erreur : cet email est déjà utilisé.");
                        else
                            salarie.AdresseMail = email;
                    }
                    break;

                case "4":
                    Console.WriteLine($"Ancien téléphone : {salarie.Telephone ?? "aucun"}");
                    Console.Write("Nouveau téléphone : ");
                    var tel = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(tel)) salarie.Telephone = tel;
                    break;

                case "5":
                    Console.WriteLine($"Ancienne adresse postale : {salarie.AdressePostale ?? "aucun"}");
                    Console.Write("Nouvelle adresse postale : ");
                    var adresse = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(adresse)) salarie.AdressePostale = adresse;
                    break;

                case "6":
                    Console.WriteLine($"Ancien role: {salarie.Poste}");
                    var role = Utils.LireRole();
                    if (role != null) salarie.Poste = role;
                    break;

                case "7":
                    Console.WriteLine($"Ancien salaire: {salarie.Salaire}");
                    var salaire = Utils.LireDecimal("Nouveau salaire : ");
                    if (salaire.HasValue) salarie.Salaire = salaire.Value;
                    break;

                case "8":
                    Console.WriteLine($"Ancienne date de naissance: {salarie.DateNaissance}");
                    var dateNaissance = Utils.LireDate("Nouvelle date de naissance (format JJ/MM/AAAA) : ");
                    if (dateNaissance.HasValue) salarie.DateNaissance = dateNaissance.Value;
                    break;

                case "9":
                    Console.WriteLine($"Ancienne date d'entrée: {salarie.DateEntree}");
                    var dateEntree = Utils.LireDate("Nouvelle date d’entrée (format JJ/MM/AAAA) : ");
                    if (dateEntree.HasValue) salarie.DateEntree = dateEntree.Value;
                    break;

                case "10":
                    SauvegarderSalaries(tous);
                    Console.WriteLine("### Modifications enregistrées. ###");
                    Thread.Sleep(1000);
                    return;

                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }
    }
}
