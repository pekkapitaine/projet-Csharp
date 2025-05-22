using Projet_C__A3.Personne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_C__A3
{
    class BaseData
    {
        public static List<Salarie> Salaries()
        {
            var dupond = new Salarie("001", "Dupond", "Jean", new DateTime(1970, 1, 1), "Paris", "dupond@mail.com", "07 81 56 79 77",
                new DateTime(2000, 1, 1), Role.Directeur_General, null, new List<Salarie>(), 10000);
            // Directeurs
            var fiesta = new Salarie("002", "Fiesta", "Marie", new DateTime(1980, 1, 1), "Lyon", "fiesta@mail.com", "0100000002",
                new DateTime(2005, 1, 1), Role.Directeur_Commercial, dupond, new List<Salarie>(), 8000);
            var fetard = new Salarie("003", "Fetard", "Luc", new DateTime(1975, 1, 1), "Bordeaux", "fetard@mail.com", "0100000003",
                new DateTime(2005, 1, 1), Role.Directeur_des_Operations, dupond, new List<Salarie>(), 8000);
            var joyeuse = new Salarie("004", "Joyeuse", "Claire", new DateTime(1982, 1, 1), "Rennes", "joyeuse@mail.com", "0100000004",
                new DateTime(2010, 1, 1), Role.Directeur_RH, dupond, new List<Salarie>(), 8000);
            var gripsous = new Salarie("005", "GripSous", "Marc", new DateTime(1968, 1, 1), "Toulouse", "gripsous@mail.com", "0100000005",
                new DateTime(2003, 1, 1), Role.Directeur_Financier, dupond, new List<Salarie>(), 8000);
            // Subordonnés de Mme Fiesta
            var forge = new Salarie("006", "Forge", "Paul", new DateTime(1990, 1, 1), "Lille", "forge@mail.com", "0100000006",
                new DateTime(2015, 1, 1), Role.Commercial, fiesta, new List<Salarie>(), 3000);
            var fermi = new Salarie("007", "Fermi", "Anne", new DateTime(1992, 1, 1), "Nice", "fermi@mail.com", "0100000007",
                new DateTime(2016, 1, 1), Role.Commercial, fiesta, new List<Salarie>(), 3000);
            // Subordonnés de M. Fetard
            var royal = new Salarie("008", "Royal", "Michel", new DateTime(1985, 1, 1), "Strasbourg", "royal@mail.com", "0100000008",
                new DateTime(2012, 1, 1), Role.Chef_Equipe, fetard, new List<Salarie>(), 5000);
            var prince = new Salarie("009", "Prince", "Sophie", new DateTime(1986, 1, 1), "Clermont-Ferrand", "prince@mail.com", "0100000009",
                new DateTime(2013, 1, 1), Role.Chef_Equipe, fetard, new List<Salarie>(), 5000);
            // Chauffeurs de M. Royal
            var romu = new Salarie("010", "Romu", "Jean", new DateTime(1995, 1, 1), "Vannes", "romu@mail.com", "0100000010",
                new DateTime(2020, 1, 1), Role.Chauffeur, royal, new List<Salarie>(), 2000);
            var romi = new Salarie("011", "Romi", "Julie", new DateTime(1994, 1, 1), "Quimper", "romi@mail.com", "0100000011",
                new DateTime(2020, 1, 1), Role.Chauffeur, royal, new List<Salarie>(), 2000);
            var roma = new Salarie("012", "Roma", "Lucas", new DateTime(1996, 1, 1), "Brest", "roma@mail.com", "0100000012",
                new DateTime(2021, 1, 1), Role.Chauffeur, royal, new List<Salarie>(), 2000);
            // Chauffeurs de Mme Prince
            var rome = new Salarie("013", "Rome", "Nina", new DateTime(1993, 1, 1), "Aurillac", "rome@mail.com", "0100000013",
                new DateTime(2020, 1, 1), Role.Chauffeur, prince, new List<Salarie>(), 2000);
            var rimou = new Salarie("014", "Rimou", "Léo", new DateTime(1993, 1, 1), "Annecy", "rimou@mail.com", "0100000014",
                new DateTime(2020, 1, 1), Role.Chauffeur, prince, new List<Salarie>(), 2000);
            // Subordonnés de Mme Joyeuse
            var couleur = new Salarie("015", "Couleur", "Laura", new DateTime(1988, 1, 1), "Mulhouse", "couleur@mail.com", "0100000015",
                new DateTime(2017, 1, 1), Role.Formation, joyeuse, new List<Salarie>(), 3500);
            var toutlemonde = new Salarie("016", "ToutleMonde", "Max", new DateTime(1990, 1, 1), "Avignon", "toutlemonde@mail.com", "0100000016",
                new DateTime(2018, 1, 1), Role.Contrats, joyeuse, new List<Salarie>(), 3500);
            // Subordonnés de Mr GripSous
            var picsou = new Salarie("017", "Picsou", "Donald", new DateTime(1975, 1, 1), "Grenoble", "picsou@mail.com", "0100000017",
                new DateTime(2005, 1, 1), Role.Direction_Comptable, gripsous, new List<Salarie>(), 7000);
            var grossous = new Salarie("018", "GrosSous", "Franck", new DateTime(1973, 1, 1), "Tarbes", "grossous@mail.com", "0100000018",
                new DateTime(2004, 1, 1), Role.Controleur_de_Gestion, gripsous, new List<Salarie>(), 7000);
            // Comptables de M. Picsou
            var fournier = new Salarie("019", "Fournier", "Alice", new DateTime(1991, 1, 1), "Pau", "fournier@mail.com", "0100000019",
                new DateTime(2019, 1, 1), Role.Comptable, picsou, new List<Salarie>(), 3000);
            var gautier = new Salarie("020", "Gautier", "Bruno", new DateTime(1992, 1, 1), "Angers", "gautier@mail.com", "0100000020",
                new DateTime(2019, 1, 1), Role.Comptable, picsou, new List<Salarie>(), 3000);

            // Ajout des subordonnés
            dupond.Subordonnes.AddRange(new[] { fiesta, fetard, joyeuse, gripsous });
            fiesta.Subordonnes.AddRange(new[] { forge, fermi });
            fetard.Subordonnes.AddRange(new[] { royal, prince });
            royal.Subordonnes.AddRange(new[] { romu, romi, roma });
            prince.Subordonnes.AddRange(new[] { rome, rimou });
            joyeuse.Subordonnes.AddRange(new[] { couleur, toutlemonde });
            gripsous.Subordonnes.AddRange(new[] { picsou, grossous });
            picsou.Subordonnes.AddRange(new[] { fournier, gautier });

            var listEmployee = new List<Salarie>
        {
            dupond, fiesta, fetard, joyeuse, gripsous,
            forge, fermi, royal, prince, romu, romi, roma,
            rome, rimou, couleur, toutlemonde,
            picsou, grossous, fournier, gautier
        };

            return listEmployee;
        }

        public static List<Client> Clients()
        {
            return new List<Client>
            {
                new Client("Durand", "Alice", new DateTime(1985, 4, 12), "12 rue de Paris, Lyon", "alice.durand@mail.com", "0601020304"),
                new Client("Martin", "Bob", new DateTime(1990, 1, 22), "45 avenue Victor Hugo, Marseille", "bob.martin@mail.com", "0605060708"),
                new Client("Lemoine", "Claire", new DateTime(1978, 7, 3), "78 boulevard Haussmann, Paris", "claire.lemoine@mail.com", "0611223344"),
                new Client("Nguyen", "David", new DateTime(1995, 11, 15), "9 chemin des Lilas, Toulouse", "david.nguyen@mail.com", "0655443322"),
                new Client("Rossi", "Emma", new DateTime(1988, 9, 9), "23 rue des Fleurs, Nice", "emma.rossi@mail.com", "0677889900")
            };
        }


        public static List<Vehicule> Vehicules()
        {
            return new List<Vehicule>
            {
                new Voiture("AB-123-CD", 0.45m, 5),
                new Voiture("EF-456-GH", 0.50m, 4),
                new Camionnette("IJ-789-KL", 0.60m, "Livraison urbaine"),
                new Camionnette("MN-012-OP", 0.65m, "Transport de matériel"),
                new CamionBenne("QR-345-ST", 1.20m, 12.5, 2, true),
                new CamionBenne("UV-678-WX", 1.10m, 10.0, 1, false),
                new CamionCiterne("YZ-901-AB", 1.30m, 15.0, "gaz"),
                new CamionCiterne("CD-234-EF", 1.25m, 18.0, "liquide"),
                new CamionFrigorifique("GH-567-IJ", 1.40m, 16.0, 2),
                new CamionFrigorifique("KL-890-MN", 1.35m, 14.0, 1)
            };
        }
    }
}
