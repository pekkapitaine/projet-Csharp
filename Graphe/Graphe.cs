using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Projet_C__A3.Graphes
{
    public class Graphe
    {
        private Dictionary<Noeud, List<Lien>> _adjacence;
        private Dictionary<string, Noeud> _noeuds;
        private double[,] _matriceAdjacence;
        private const string CHEMIN_IMG = "Graphe_villes.png";

        public Graphe(string cheminCsv)
        {
            _adjacence = new Dictionary<Noeud, List<Lien>>();
            _noeuds = new Dictionary<string, Noeud>();
            ChargerDepuisCsv(cheminCsv);
            ConstruireMatriceAdjacence();
        }

        private void ChargerDepuisCsv(string cheminCsv)
        {
            var lignes = File.ReadAllLines(cheminCsv);

            foreach (var ligne in lignes.Skip(1)) // On saute l'en-tête
            {
                var parties = ligne.Split(';');
                if (parties.Length != 3) continue;

                var ville1 = parties[0].Trim();
                var ville2 = parties[1].Trim();
                if (!double.TryParse(parties[2].Trim(), out var distance)) continue;

                var noeud1 = ObtenirOuCreerNoeud(ville1);
                var noeud2 = ObtenirOuCreerNoeud(ville2);

                AjouterLien(noeud1, noeud2, distance);
                AjouterLien(noeud2, noeud1, distance);
            }
        }

        private Noeud ObtenirOuCreerNoeud(string nom)
        {
            if (_noeuds.ContainsKey(nom)) return _noeuds[nom];
            var noeud = new Noeud(nom);
            _noeuds[nom] = noeud;
            _adjacence[noeud] = new List<Lien>();
            return noeud;
        }

        private void AjouterLien(Noeud source, Noeud destination, double distance)
        {
            _adjacence[source].Add(new Lien(destination, distance));
        }

        private void ConstruireMatriceAdjacence()
        {
            int n = _noeuds.Count;
            _matriceAdjacence = new double[n, n];
            var listeNoeuds = _noeuds.Values.ToList();

            for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                _matriceAdjacence[i, j] = double.PositiveInfinity;

            for (int i = 0; i < listeNoeuds.Count; i++)
            {
                var noeud = listeNoeuds[i];
                foreach (var lien in _adjacence[noeud])
                {
                    int j = listeNoeuds.IndexOf(lien.Destination);
                    _matriceAdjacence[i, j] = lien.Distance;
                }
            }
        }

        public (List<string> chemin, double distance) FindShortestPathDjikstra(string villeDepart, string villeArrivee)
        {
            if (!_noeuds.ContainsKey(villeDepart) || !_noeuds.ContainsKey(villeArrivee))
                return (new List<string>(), double.PositiveInfinity);
            

            var depart = _noeuds[villeDepart];
            var arrivee = _noeuds[villeArrivee];

            var distances = new Dictionary<Noeud, double>();
            var precedent = new Dictionary<Noeud, Noeud>();
            var visite = new HashSet<Noeud>();

            foreach (var noeud in _noeuds.Values)
                distances[noeud] = double.PositiveInfinity;

            distances[depart] = 0;

            var queue = new SortedSet<(double, Noeud)>(
                Comparer<(double, Noeud)>.Create((a, b) =>
                {
                    int cmp = a.Item1.CompareTo(b.Item1);
                    return cmp == 0 ? a.Item2.Nom.CompareTo(b.Item2.Nom) : cmp;
                })
            );

            queue.Add((0, depart));

            while (queue.Count > 0)
            {
                var (distanceActuelle, courant) = queue.Min;
                queue.Remove(queue.Min);

                if (visite.Contains(courant)) continue;
                visite.Add(courant);

                if (courant == arrivee) break;

                foreach (var lien in _adjacence[courant])
                {
                    var voisin = lien.Destination;
                    var nouvelleDistance = distanceActuelle + lien.Distance;

                    if (nouvelleDistance < distances[voisin])
                    {
                        distances[voisin] = nouvelleDistance;
                        precedent[voisin] = courant;
                        queue.Add((nouvelleDistance, voisin));
                    }
                }
            }

            var chemin = new List<string>();
            var noeudActuel = arrivee;

            if (!precedent.ContainsKey(noeudActuel) && arrivee != depart)
                return (new List<string>(), double.PositiveInfinity); // Pas de chemin

            while (noeudActuel != null)
            {
                chemin.Insert(0, noeudActuel.Nom);
                precedent.TryGetValue(noeudActuel, out noeudActuel);
            }

            // distances[arrivee] contient la distance totale du chemin le plus court
            return (chemin, distances[arrivee]);
        }

        public (List<string> chemin, double distance) FindShortestPathBellmanFord(string villeDepart,
            string villeArrivee)
        {
            if (!_noeuds.ContainsKey(villeDepart) || !_noeuds.ContainsKey(villeArrivee))
                return (new List<string>(), double.PositiveInfinity);

            var depart = _noeuds[villeDepart];
            var arrivee = _noeuds[villeArrivee];

            var distances = new Dictionary<Noeud, double>();
            var precedent = new Dictionary<Noeud, Noeud?>();

            foreach (var noeud in _noeuds.Values)
                distances[noeud] = double.PositiveInfinity;
            distances[depart] = 0;

            int nbNoeuds = _noeuds.Count;

            for (int i = 0; i < nbNoeuds - 1; i++)
            {
                bool modif = false;

                foreach (var courant in _noeuds.Values)
                {
                    if (distances[courant] == double.PositiveInfinity)
                        continue;

                    if (!_adjacence.ContainsKey(courant))
                        continue;

                    foreach (var lien in _adjacence[courant])
                    {
                        var voisin = lien.Destination;
                        double nouvelleDistance = distances[courant] + lien.Distance;

                        if (nouvelleDistance < distances[voisin])
                        {
                            distances[voisin] = nouvelleDistance;
                            precedent[voisin] = courant;
                            modif = true;
                        }
                    }
                }

                if (!modif)
                    break;
            }

            var chemin = new List<string>();
            var noeudActuel = arrivee;

            if (!precedent.ContainsKey(noeudActuel) && arrivee != depart)
                return (new List<string>(), double.PositiveInfinity);

            while (noeudActuel != null)
            {
                chemin.Insert(0, noeudActuel.Nom);
                precedent.TryGetValue(noeudActuel, out noeudActuel);
            }

            return (chemin, distances[arrivee]);
        }


        public (List<string> chemin, double distance) FindShortestPathFloydWarshall(string villeDepart,
            string villeArrivee)
        {
            var listeVilles = _noeuds.Keys.ToList();
            
            var start = listeVilles.IndexOf(villeDepart);
            var end = listeVilles.IndexOf(villeArrivee);
            if (!_noeuds.ContainsKey(villeDepart) || !_noeuds.ContainsKey(villeArrivee))
                return (new List<string>(), double.PositiveInfinity);

            var n = _matriceAdjacence.GetLength(0);
            double[,] dist = new double[n, n];
            int[,] next = new int[n, n];
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        dist[i, j] = 0;
                        next[i, j] = i;
                    }
                    else if (_matriceAdjacence[i, j] != 0)
                    {
                        dist[i, j] = _matriceAdjacence[i, j];
                        next[i, j] = j;
                    }
                    else
                    {
                        dist[i, j] = double.PositiveInfinity;
                        next[i, j] = -1;
                    }
                }
            }

            for (var k = 0; k < n; k++)
            {
                for (var i = 0; i < n; i++)
                {
                    for (var j = 0; j < n; j++)
                    {
                        if (dist[i, k] + dist[k, j] < dist[i, j])
                        {
                            dist[i, j] = dist[i, k] + dist[k, j];
                            next[i, j] = next[i, k];
                        }
                    }
                }
            }

            List<string> chemin = new List<string>();
            if (next[start, end] == -1)
            {
                return (chemin, double.PositiveInfinity);
            }

            int u = start;
            chemin.Add(villeDepart);
            while (u != end)
            {
                u = next[u, end];
                chemin.Add(listeVilles[u]);
            }

            return (chemin, dist[start, end]);
        }


        public List<string> ParcoursLargeur(Ville departName)
        {
            var visite = new HashSet<Noeud>();
            var resultat = new List<string>();
            var file = new Queue<Noeud>();
            string departString = nameof(departName);

            if (!_noeuds.ContainsKey(departString))
                return resultat;

            var depart = _noeuds[departString];
            visite.Add(depart);
            file.Enqueue(depart);

            Console.WriteLine("Parcours en largeur :");

            while (file.Count > 0)
            {
                var courant = file.Dequeue();
                resultat.Add(courant.Nom);

                // Affichage à chaque visite de noeud
                Console.WriteLine($"Visite : {courant.Nom}");

                foreach (var lien in _adjacence[courant])
                {
                    if (!visite.Contains(lien.Destination))
                    {
                        visite.Add(lien.Destination);
                        file.Enqueue(lien.Destination);
                    }
                }
            }

            return resultat;
        }

        public List<string> ParcoursProfondeur(Ville departName)
        {
            var visite = new HashSet<Noeud>();
            var resultat = new List<string>();
            string departString = nameof(departName);

            if (!_noeuds.ContainsKey(departString))
                return resultat;

            Console.WriteLine("Parcours en profondeur :");

            DFS(_noeuds[departString], visite, resultat);
            return resultat;
        }

        private void DFS(Noeud noeud, HashSet<Noeud> visite, List<string> resultat)
        {
            if (visite.Contains(noeud))
                return;

            visite.Add(noeud);
            resultat.Add(noeud.Nom);

            Console.WriteLine($"Visite : {noeud.Nom}");

            foreach (var lien in _adjacence[noeud])
            {
                DFS(lien.Destination, visite, resultat);
            }
        }


        public bool EstConnexe()
        {
            if (_noeuds.Count == 0) return true;

            var visite = new HashSet<Noeud>();
            var premier = _noeuds.Values.First();
            DFS(premier, visite, new List<string>());

            return visite.Count == _noeuds.Count;
        }

        public bool ContientCycle()
        {
            var visite = new HashSet<Noeud>();
            foreach (var noeud in _noeuds.Values)
            {
                if (!visite.Contains(noeud))
                {
                    if (CycleDFS(noeud, visite, null))
                        return true;
                }
            }

            return false;
        }

        private bool CycleDFS(Noeud courant, HashSet<Noeud> visite, Noeud parent)
        {
            visite.Add(courant);

            foreach (var lien in _adjacence[courant])
            {
                var voisin = lien.Destination;
                if (!visite.Contains(voisin))
                {
                    if (CycleDFS(voisin, visite, courant))
                        return true;
                }
                else if (!voisin.Equals(parent))
                {
                    return true;
                }
            }

            return false;
        }

        public void AfficherMatriceAdjacence()
        {
            var noms = _noeuds.Keys.ToList();
            int largeurColonne = noms.Max(n => n.Length);

            Console.Write("".PadRight(largeurColonne));

            foreach (var nom in noms)
                Console.Write(nom.PadRight(largeurColonne));

            Console.WriteLine();

            for (var i = 0; i < noms.Count; i++)
            {
                Console.Write(noms[i].PadRight(largeurColonne));
                for (var j = 0; j < noms.Count; j++)
                {
                    var val = _matriceAdjacence[i, j];
                    var texte = double.IsInfinity(val) ? "0" : val.ToString("F1");
                    Console.Write(texte.PadRight(largeurColonne));
                }

                Console.WriteLine();
            }
        }

        public void AfficherListeAdjacence()
        {
            Console.WriteLine("Liste d'adjacence :");
            foreach (var kvp in _adjacence)
            {
                Console.Write($"{kvp.Key}: ");
                foreach (var lien in kvp.Value)
                {
                    Console.Write($"-> {lien.Destination} ({lien.Distance} km) ");
                }

                Console.WriteLine();
            }
        }


        public void VisualiserGraphe(string nameFile, List<string> chemin = null)
        {
            const int largeur = 1000;
            const int hauteur = 1000;
            const int rayon = 25;
            const int marge = 100;
            
            if (chemin == null)
            {
                chemin = new List<string>();
            }

            var bitmap = new SKBitmap(largeur, hauteur);
            var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paintNoeud = new SKPaint
            {
                Color = SKColors.SteelBlue,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };
            
            var paintNoeudChemin = new SKPaint
            {
                Color = SKColors.Red,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            var paintTexte = new SKPaint
            {
                Color = SKColors.White,
                TextSize = 16,
                IsAntialias = true,
                TextAlign = SKTextAlign.Center
            };

            var paintLien = new SKPaint
            {
                Color = SKColors.Gray,
                IsAntialias = true,
                StrokeWidth = 2
            };
            
            var paintLienChemin = new SKPaint
            {
                Color = SKColors.DarkRed,
                IsAntialias = true,
                StrokeWidth = 2
            };

            var noeuds = _noeuds.Values.ToList();
            var positions = new Dictionary<Noeud, SKPoint>();

            double angleStep = 2 * Math.PI / noeuds.Count;
            float centerX = largeur / 2;
            float centerY = hauteur / 2;
            float rayonPlacement = Math.Min(largeur, hauteur) / 2 - marge;

            for (int i = 0; i < noeuds.Count; i++)
            {
                float x = centerX + (float)(rayonPlacement * Math.Cos(i * angleStep));
                float y = centerY + (float)(rayonPlacement * Math.Sin(i * angleStep));
                positions[noeuds[i]] = new SKPoint(x, y);
            }

            foreach (var kvp in _adjacence)
            {
                var pointA = positions[kvp.Key];
                foreach (var lien in kvp.Value)
                {
                    var pointB = positions[lien.Destination];
                    if (chemin.Contains(kvp.Key.Nom) && chemin.Contains(lien.Destination.Nom))
                    {
                        canvas.DrawLine(pointA, pointB, paintLienChemin);
                    }
                    else
                    {
                        canvas.DrawLine(pointA, pointB, paintLien);
                    }
                }
            }

            foreach (var noeud in noeuds)
            {
                var point = positions[noeud];
                if (chemin.Contains(noeud.Nom))
                {
                    canvas.DrawCircle(point, rayon, paintNoeudChemin);
                }
                else
                {
                    canvas.DrawCircle(point, rayon, paintNoeud);
                }
                canvas.DrawText(noeud.Nom, point.X, point.Y + 6, paintTexte);
            }

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = File.OpenWrite($"{nameFile}.png");
            data.SaveTo(stream);
        }

        public List<string> ObtenirListeVilles()
        {
            return _noeuds.Keys.ToList();
        }
    }
}