using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Projet_C__A3.Manager
{
    public static class VehiculeManager
    {
        private static readonly string FichierCSV = "ressources/vehicules.csv";

        public static void NettoyerCSV()
        {
            if (!File.Exists(FichierCSV))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FichierCSV)!);
                File.WriteAllText(FichierCSV, "Immatriculation;TarifParKm;Type;Places\n");
            }
            else
            {
                File.WriteAllText(FichierCSV, "Immatriculation;TarifParKm;Type;Places\n");
            }
        }

        public static void SauvegarderVehicule(Vehicule vehicule)
        {
            bool fichierExiste = File.Exists(FichierCSV);

            using (var writer = new StreamWriter(FichierCSV, append: true))
            {
                if (!fichierExiste)
                {
                    writer.WriteLine("Immatriculation;TarifParKm;Type;Propriete1;Propriete2;Propriete3");
                }

                string[] ligne;

                switch (vehicule)
                {
                    case Voiture v:
                        ligne = new string[] {
                    v.Immatriculation,
                    v.TarifParKm.ToString(CultureInfo.InvariantCulture),
                    nameof(Voiture),
                    v.NombrePassagers.ToString(),
                    "", ""
                };
                        break;

                    case Camionnette c:
                        ligne = new string[] {
                    c.Immatriculation,
                    c.TarifParKm.ToString(CultureInfo.InvariantCulture),
                    nameof(Camionnette),
                    c.Usage,
                    "", ""
                };
                        break;

                    case CamionBenne b:
                        ligne = new string[] {
                    b.Immatriculation,
                    b.TarifParKm.ToString(CultureInfo.InvariantCulture),
                    nameof(CamionBenne),
                    b.Volume.ToString(CultureInfo.InvariantCulture),
                    b.NombreBennes.ToString(),
                    b.GrueAuxiliaire.ToString()
                };
                        break;

                    case CamionCiterne cit:
                        ligne = new string[] {
                    cit.Immatriculation,
                    cit.TarifParKm.ToString(CultureInfo.InvariantCulture),
                    nameof(CamionCiterne),
                    cit.Volume.ToString(CultureInfo.InvariantCulture),
                    cit.TypeProduit,
                    ""
                };
                        break;

                    case CamionFrigorifique frig:
                        ligne = new string[] {
                    frig.Immatriculation,
                    frig.TarifParKm.ToString(CultureInfo.InvariantCulture),
                    nameof(CamionFrigorifique),
                    frig.Volume.ToString(CultureInfo.InvariantCulture),
                    frig.GroupesElectrogenes.ToString(),
                    ""
                };
                        break;

                    default:
                        ligne = new string[] {
                    vehicule.Immatriculation,
                    vehicule.TarifParKm.ToString(CultureInfo.InvariantCulture),
                    "Inconnu", "", "", ""
                };
                        break;
                }

                writer.WriteLine(string.Join(";", ligne));
            }
        }


        public static void SauvegarderVehicules(List<Vehicule> vehicules, bool onlyIfNoFile = false)
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

            using (var writer = new StreamWriter(FichierCSV, false)) // false pour overwrite
            {
                writer.WriteLine("Immatriculation;TarifParKm;Type;Propriete1;Propriete2;Propriete3");

                foreach (var vehicule in vehicules)
                {
                    string[] ligne;

                    switch (vehicule)
                    {
                        case Voiture v:
                            ligne = new string[] {
                        v.Immatriculation,
                        v.TarifParKm.ToString(CultureInfo.InvariantCulture),
                        nameof(Voiture),
                        v.NombrePassagers.ToString(),
                        "", ""
                    };
                            break;

                        case Camionnette c:
                            ligne = new string[] {
                        c.Immatriculation,
                        c.TarifParKm.ToString(CultureInfo.InvariantCulture),
                        nameof(Camionnette),
                        c.Usage,
                        "", ""
                    };
                            break;

                        case CamionBenne b:
                            ligne = new string[] {
                        b.Immatriculation,
                        b.TarifParKm.ToString(CultureInfo.InvariantCulture),
                        nameof(CamionBenne),
                        b.Volume.ToString(CultureInfo.InvariantCulture),
                        b.NombreBennes.ToString(),
                        b.GrueAuxiliaire.ToString()
                    };
                            break;

                        case CamionCiterne cit:
                            ligne = new string[] {
                        cit.Immatriculation,
                        cit.TarifParKm.ToString(CultureInfo.InvariantCulture),
                        nameof(CamionCiterne),
                        cit.Volume.ToString(CultureInfo.InvariantCulture),
                        cit.TypeProduit,
                        ""
                    };
                            break;

                        case CamionFrigorifique frig:
                            ligne = new string[] {
                        frig.Immatriculation,
                        frig.TarifParKm.ToString(CultureInfo.InvariantCulture),
                        nameof(CamionFrigorifique),
                        frig.Volume.ToString(CultureInfo.InvariantCulture),
                        frig.GroupesElectrogenes.ToString(),
                        ""
                    };
                            break;

                        default:
                            ligne = new string[] {
                        vehicule.Immatriculation,
                        vehicule.TarifParKm.ToString(CultureInfo.InvariantCulture),
                        "Inconnu", "", "", ""
                    };
                            break;
                    }

                    writer.WriteLine(string.Join(";", ligne));
                }
            }
        }


    public static List<Vehicule> ChargerVehicules()
        {
            var vehicules = new List<Vehicule>();

            if (!File.Exists(FichierCSV))
                return vehicules;

            var lignes = File.ReadAllLines(FichierCSV).Skip(1);

            foreach (var ligne in lignes)
            {
                var parts = ligne.Split(';');
                if (parts.Length < 4) continue;

                string immat = parts[0];
                if (!decimal.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal tarif))
                    continue;

                string type = parts[2];

                try
                {
                    switch (type)
                    {
                        case "Voiture":
                            int places = int.Parse(parts[3]);
                            vehicules.Add(new Voiture(immat, tarif, places));
                            break;

                        case "Camionnette":
                            string usage = parts[3];
                            vehicules.Add(new Camionnette(immat, tarif, usage));
                            break;

                        case "CamionBenne":
                            double volumeBenne = double.Parse(parts[3], CultureInfo.InvariantCulture);
                            int nbBennes = int.Parse(parts[4]);
                            bool grue = bool.Parse(parts[5]);
                            vehicules.Add(new CamionBenne(immat, tarif, volumeBenne, nbBennes, grue));
                            break;

                        case "CamionCiterne":
                            double volumeCiterne = double.Parse(parts[3], CultureInfo.InvariantCulture);
                            string produit = parts[4];
                            vehicules.Add(new CamionCiterne(immat, tarif, volumeCiterne, produit));
                            break;

                        case "CamionFrigorifique":
                            double volumeFrigo = double.Parse(parts[3], CultureInfo.InvariantCulture);
                            int groupes = int.Parse(parts[4]);
                            vehicules.Add(new CamionFrigorifique(immat, tarif, volumeFrigo, groupes));
                            break;
                    }
                }
                catch
                {
                    continue; // Ignore les lignes mal formées
                }
            }

            return vehicules;
        }


        public static Vehicule? ChercherVehiculeParImmat(string immatriculation)
        {
            return ChargerVehicules().FirstOrDefault(v => v.Immatriculation.Equals(immatriculation, StringComparison.OrdinalIgnoreCase));
        }

        public static void SupprimerVehicule(string immatriculation)
        {
            if (!File.Exists(FichierCSV)) return;

            var lignes = File.ReadAllLines(FichierCSV).ToList();
            lignes = lignes.Where((ligne, index) =>
            {
                if (index == 0) return true;
                var parts = ligne.Split(';');
                return !parts[0].Equals(immatriculation, StringComparison.OrdinalIgnoreCase);
            }).ToList();

            File.WriteAllLines(FichierCSV, lignes);
        }

        public static void ModifierVehicule(string immatriculation)
        {
            var vehicules = ChargerVehicules();
            var vehicule = vehicules.FirstOrDefault(v => v.Immatriculation.Equals(immatriculation, StringComparison.OrdinalIgnoreCase));
            if (vehicule == null)
            {
                Console.WriteLine("Véhicule non trouvé.");
                return;
            }

            Console.WriteLine($"Modification du véhicule {immatriculation} :");

            Console.Write("Nouveau tarif par km (laisser vide pour garder l'actuel) : ");
            string inputTarif = Console.ReadLine()!;
            if (decimal.TryParse(inputTarif.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal tarif))
            {
                vehicule.TarifParKm = tarif;
            }

            if (vehicule is Voiture voiture)
            {
                Console.Write("Nouveau nombre de passagers (laisser vide pour garder l'actuel) : ");
                string inputPlaces = Console.ReadLine()!;
                if (int.TryParse(inputPlaces, out int places))
                {
                    voiture.NombrePassagers = places;
                }
            }

            // Réécriture complète du fichier
            NettoyerCSV();
            foreach (var v in vehicules)
            {
                SauvegarderVehicule(v);
            }

            Console.WriteLine("Véhicule modifié avec succès.");
        }

        public static void AfficherTousVehicules()
        {
            var vehicules = ChargerVehicules();

            Console.WriteLine("=== Liste des véhicules ===");
            foreach (var v in vehicules)
            {
                string description = $"Immat: {v.Immatriculation}, Tarif/km: {v.TarifParKm}, Type: {v.GetType().Name}";

                switch (v)
                {
                    case Voiture voiture:
                        description += $", Places: {voiture.NombrePassagers}";
                        break;

                    case Camionnette camionnette:
                        description += $", Usage: {camionnette.Usage}";
                        break;

                    case CamionBenne benne:
                        description += $", Volume: {benne.Volume} m3, Bennes: {benne.NombreBennes}, Grue: {(benne.GrueAuxiliaire ? "Oui" : "Non")}";
                        break;

                    case CamionCiterne citerne:
                        description += $", Volume: {citerne.Volume} m3, Produit: {citerne.TypeProduit}";
                        break;

                    case CamionFrigorifique frigo:
                        description += $", Volume: {frigo.Volume} m3, Groupes électrogènes: {frigo.GroupesElectrogenes}";
                        break;
                }

                Console.WriteLine(description);
            }
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey(true);
        }

        public static void AfficherVehiculesDisponible(DateTime date, int nbToShow = int.MaxValue)
        {
            var commandes = CommandeManager.ChargerCommandes();
            var vehiculesDisponibles = ChargerVehicules()
                .Where(v => !commandes.Any(c => c.Vehicule.Immatriculation == v.Immatriculation && c.DateCommande.Date == date.Date))
                .OrderBy(v => v.TarifParKm)
                .Take(nbToShow)
                .ToList();

            Console.WriteLine("=== Véhicules disponibles ===");

            if (!vehiculesDisponibles.Any())
            {
                Console.WriteLine("Aucun véhicule disponible.");
            }

            foreach (var v in vehiculesDisponibles)
            {
                string description = $"Immat: {v.Immatriculation}, Tarif/km: {v.TarifParKm}, Type: {v.GetType().Name}";

                switch (v)
                {
                    case Voiture voiture:
                        description += $", Places: {voiture.NombrePassagers}";
                        break;

                    case Camionnette camionnette:
                        description += $", Usage: {camionnette.Usage}";
                        break;

                    case CamionBenne benne:
                        description += $", Volume: {benne.Volume} m3, Bennes: {benne.NombreBennes}, Grue: {(benne.GrueAuxiliaire ? "Oui" : "Non")}";
                        break;

                    case CamionCiterne citerne:
                        description += $", Volume: {citerne.Volume} m3, Produit: {citerne.TypeProduit}";
                        break;

                    case CamionFrigorifique frigo:
                        description += $", Volume: {frigo.Volume} m3, Groupes électrogènes: {frigo.GroupesElectrogenes}";
                        break;
                }

                Console.WriteLine(description);
            }
        }


        public static void AjouterVehiculeDepuisConsole()
        {
            Console.WriteLine("=== Ajout d'un nouveau véhicule ===");

            Console.Write("Immatriculation : ");
            string immat = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(immat))
            {
                Console.WriteLine("Immatriculation obligatoire.");
                return;
            }

            if (ChercherVehiculeParImmat(immat) != null)
            {
                Console.WriteLine("Un véhicule avec cette immatriculation existe déjà.");
                return;
            }

            var tarifOpt = Utils.LireDecimal("Tarif par km : ");
            if (tarifOpt == null)
            {
                Console.WriteLine("Tarif invalide.");
                return;
            }
            decimal tarif = tarifOpt.Value;

            Console.WriteLine("Type de véhicule :");
            Console.WriteLine("1. Voiture");
            Console.WriteLine("2. Camionnette");
            Console.WriteLine("3. Camion Benne");
            Console.WriteLine("4. Camion Citerne");
            Console.WriteLine("5. Camion Frigorifique");
            Console.Write("Choix : ");
            string choix = Console.ReadLine()!;

            switch (choix)
            {
                case "1":
                    Console.Write("Nombre de places : ");
                    if (!int.TryParse(Console.ReadLine(), out int places))
                    {
                        Console.WriteLine("Nombre de places invalide.");
                        return;
                    }
                    SauvegarderVehicule(new Voiture(immat, tarif, places));
                    Console.WriteLine("Voiture ajoutée.");
                    break;

                case "2":
                    Console.Write("Usage de la camionnette : ");
                    string usage = Console.ReadLine()!;
                    SauvegarderVehicule(new Camionnette(immat, tarif, usage));
                    Console.WriteLine("Camionnette ajoutée.");
                    break;

                case "3":
                    Console.Write("Volume (m3) : ");
                    if (!double.TryParse(Console.ReadLine(), out double volumeBenne))
                    {
                        Console.WriteLine("Volume invalide.");
                        return;
                    }
                    Console.Write("Nombre de bennes : ");
                    if (!int.TryParse(Console.ReadLine(), out int nbBennes))
                    {
                        Console.WriteLine("Nombre invalide.");
                        return;
                    }
                    Console.Write("Grue auxiliaire (o/n) : ");
                    bool grue = Console.ReadLine()!.Trim().ToLower() == "o";
                    SauvegarderVehicule(new CamionBenne(immat, tarif, volumeBenne, nbBennes, grue));
                    Console.WriteLine("Camion benne ajouté.");
                    break;

                case "4":
                    Console.Write("Volume (m3) : ");
                    if (!double.TryParse(Console.ReadLine(), out double volumeCiterne))
                    {
                        Console.WriteLine("Volume invalide.");
                        return;
                    }
                    Console.Write("Type de produit transporté : ");
                    string typeProduit = Console.ReadLine()!;
                    SauvegarderVehicule(new CamionCiterne(immat, tarif, volumeCiterne, typeProduit));
                    Console.WriteLine("Camion citerne ajouté.");
                    break;

                case "5":
                    Console.Write("Volume (m3) : ");
                    if (!double.TryParse(Console.ReadLine(), out double volumeFrigo))
                    {
                        Console.WriteLine("Volume invalide.");
                        return;
                    }
                    Console.Write("Nombre de groupes électrogènes : ");
                    if (!int.TryParse(Console.ReadLine(), out int groupes))
                    {
                        Console.WriteLine("Nombre invalide.");
                        return;
                    }
                    SauvegarderVehicule(new CamionFrigorifique(immat, tarif, volumeFrigo, groupes));
                    Console.WriteLine("Camion frigorifique ajouté.");
                    break;

                default:
                    Console.WriteLine("Type non supporté.");
                    break;
            }
        }
        public static List<Vehicule> ObtenirVehiculesDisponibles(DateTime date)
        {
            var commandes = CommandeManager.ChargerCommandes();
            var vehicules = ChargerVehicules();

            var vehiculesDisponibles = vehicules
                .Where(v => !commandes.Any(c =>
                    c.Vehicule.Immatriculation.Equals(v.Immatriculation, StringComparison.OrdinalIgnoreCase)
                    && c.DateCommande.Date == date.Date))
                .OrderBy(v => v.TarifParKm)
                .ToList();

            return vehiculesDisponibles;
        }

    }
}
