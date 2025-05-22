using Projet_C__A3.Personne;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_C__A3
{
    class Utils
    {

        public static DateTime LireDateObligatoire(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine()!;

                if (DateTime.TryParse(input, out var result))
                    return result;

                Console.WriteLine("Format invalide. Réessayez (format attendu : yyyy-MM-dd).");
            }
        }


        public static DateTime? LireDate(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine()!;
                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (DateTime.TryParse(input, out var result))
                    return result;

                Console.WriteLine("Format invalide. Réessayez.");
            }
        }

        public static decimal? LireDecimal(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine()!;
                if (string.IsNullOrWhiteSpace(input))
                    return null;

                // Remplace la virgule par un point pour la compatibilité FR/EN
                input = input.Replace(',', '.');

                if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var montant))
                    return montant;

                Console.WriteLine("Montant invalide. Exemple : 2500,00 ou 2500.00");
            }
        }


        public static Role? LireRole()
        {
            var roles = Enum.GetValues(typeof(Role)).Cast<Role>().ToList();

            Console.WriteLine("Poste : ");
            for (int i = 0; i < roles.Count; i++)
                Console.WriteLine($"{i + 1}. {roles[i]}");

            while (true)
            {
                Console.Write("Choix : ");
                string input = Console.ReadLine()!;
                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (int.TryParse(input, out int choix) && choix >= 1 && choix <= roles.Count)
                    return roles[choix - 1];

                Console.WriteLine("Choix invalide. Entrez un numéro valide.");
            }
        }
    }
}
