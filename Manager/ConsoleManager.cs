using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_C__A3.Manager
{
    class ConsoleManager
    {
        public static void Hello() 
        {
            Console.Clear();
            Console.WriteLine("======= BIENVENUE DANS VOTRE OUTIL DE GESTION D'ENTREPRISE =======");
            Console.WriteLine("            ============= CHARGEMENT ==============");
            Console.Write("              [");
             
            int total = 33;

            for (int i = 0; i < total; i++)
                Console.Write(" ");
            Console.Write("]");

            Console.SetCursorPosition(15, Console.CursorTop);


            for (int i = 0; i < total; i++)
            {
                Console.Write("*");
                Thread.Sleep(40);
            }
        }
    }
}
