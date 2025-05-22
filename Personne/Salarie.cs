using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using SkiaSharp;

namespace Projet_C__A3.Personne
{
    public enum Role
    {
        Directeur_General,
        Directeur_Commercial,
        Directeur_des_Operations,
        Directeur_RH,
        Directeur_Financier,
        Controleur_de_Gestion,
        Direction_Comptable,
        Comptable,
        Formation,
        Contrats,
        Chauffeur,
        Commercial,
        Chef_Equipe
    }

    public class Salarie : Personne
    {
        public const double tarifDeBase = 15; // tarif de base en euros
        public string? MailSuperieurTemp { get; set; } // temporaire

        public string? NumeroSS { get; set; }
        public DateTime? DateEntree { get; set; }
        public Role? Poste { get; set; }
        public decimal? Salaire { get; set; }

        public Salarie? Superieur { get; set; }
        public List<Salarie> Subordonnes { get; set; }

        public int Anciennete => DateEntree != null && Poste == Role.Chauffeur ? DateTime.Now.Year - DateEntree.Value.Year : 0;

        public int TarifHoraire => (int)(tarifDeBase * (1 + Math.Log(1 + Anciennete) / Math.Log(10)));


        public Salarie(string numeroSS, string nom, string prenom, DateTime dateNaissance, string adressePostale,
                       string adresseMail, string telephone, DateTime dateEntree, Role? poste, Salarie? superieur = null, List<Salarie>? subordonnes = null, decimal? salaire = 0)
            : base(nom, prenom, dateNaissance, adressePostale, adresseMail, telephone)
        {
            NumeroSS = numeroSS;
            DateEntree = dateEntree;
            Poste = poste ?? null;
            Salaire = salaire;
            Superieur = superieur;
            Subordonnes = subordonnes ?? new List<Salarie>();
        }

        public override string ToString()
        {
            return $"[Salarie] {base.ToString()}, Poste : {Poste}, Entré(e) le : {DateEntree?.ToShortDateString()}, Ancienneté : {Anciennete} an(s), Salaire : {Salaire}e, Tarif horaire : {TarifHoraire}e/h";
        }

        public void AjoutSubordonnes(Salarie s)
        {
            if (s is null) return;
            Subordonnes.Add(s);
            s.Superieur = this;
        }


        public void AfficherArborescence(int niveau = 0)
        {
            Console.WriteLine($"{new string(' ', niveau * 2)}- {Prenom} {Nom} {Poste} ({AdresseMail})");

            foreach (var sub in Subordonnes.OrderBy(s => s.Nom))
            {
                sub.AfficherArborescence(niveau + 1);
            }
        }


        private int CalculerPositions(Salarie salarie, int x, int y, Dictionary<Salarie, SKPoint> positions, ref int maxX, ref int maxY)
        {
            int spacingX = 40;
            int spacingY = 80;
            int boxWidth = 200;

            int currentX = x;

            if (salarie.Subordonnes.Count == 0)
            {
                positions[salarie] = new SKPoint(currentX, y);
                maxX = Math.Max(maxX, currentX + boxWidth);
                maxY = Math.Max(maxY, y + spacingY);
                return currentX + boxWidth + spacingX;
            }

            int subX = currentX;
            foreach (var sub in salarie.Subordonnes)
            {
                subX = CalculerPositions(sub, subX, y + spacingY, positions, ref maxX, ref maxY);
            }

            int totalWidth = subX - x - spacingX;
            int centerX = x + totalWidth / 2;
            positions[salarie] = new SKPoint(centerX, y);
            maxX = Math.Max(maxX, centerX + boxWidth);
            maxY = Math.Max(maxY, y + spacingY);

            return subX;
        }

        public void AfficherArborescenceGraphique(string cheminImage = "arborescence.png")
        {
            // Trouver la racine
            var racine = this;
            while (racine.Superieur != null)
                racine = racine.Superieur;

            // Calcule les positions de chaque salarié
            var positions = new Dictionary<Salarie, SKPoint>();
            int maxX = 0, maxY = 0;
            CalculerPositions(racine, 0, 0, positions, ref maxX, ref maxY);

            // Taille image + marges
            int largeur = maxX + 200;
            int hauteur = maxY + 100;

            using var surface = SKSurface.Create(new SKImageInfo(largeur, hauteur));
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

            var boxPaint = new SKPaint
            {
                Color = SKColors.LightBlue,
                IsAntialias = true
            };

            var textPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 18,
                IsAntialias = true
            };

            var linePaint = new SKPaint
            {
                Color = SKColors.DarkGray,
                StrokeWidth = 2,
                IsAntialias = true
            };

            int boxWidth = 120;
            int boxHeight = 40;

            // Dessine les lignes de liaison
            foreach (var (salarie, point) in positions)
            {
                foreach (var sub in salarie.Subordonnes)
                {
                    if (positions.TryGetValue(sub, out var subPoint))
                    {
                        var start = new SKPoint(point.X + boxWidth / 2, point.Y + boxHeight);
                        var end = new SKPoint(subPoint.X + boxWidth / 2, subPoint.Y);
                        canvas.DrawLine(start, end, linePaint);
                    }
                }
            }

            // Dessine les boîtes + textes
            foreach (var (salarie, point) in positions)
            {
                var rect = new SKRect(point.X, point.Y, point.X + boxWidth, point.Y + boxHeight);
                canvas.DrawRect(rect, boxPaint);

                string texte = $"{salarie.Prenom} {salarie.Nom}\n({salarie.Poste})";
                var lines = texte.Split('\n');

                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    float textWidth = textPaint.MeasureText(line);
                    float xText = point.X + (boxWidth - textWidth) / 2;
                    float yText = point.Y + 20 + i * 15;
                    canvas.DrawText(line, xText, yText, textPaint);
                }
            }

            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = File.OpenWrite(cheminImage);
            data.SaveTo(stream);

            Console.WriteLine("Image générée dans : " + cheminImage);
        }




        public void LicencierSalarie()
        {
            if (Superieur != null)
            {
                Subordonnes.ForEach(s =>
                {
                    s.Superieur = Superieur;
                    Superieur.Subordonnes.Add(s);
                });

                Superieur.Subordonnes.Remove(this);
            }

            Subordonnes.Clear();
            this.Superieur?.AfficherArborescenceGraphique();
            Superieur = null;
            SalarieManager.SupprimerSalarie(this.AdresseMail);
        }
    }
}
