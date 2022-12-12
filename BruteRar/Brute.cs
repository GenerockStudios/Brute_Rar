using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;

namespace BruteRar
{
    public enum Mode
    {
        Lettres_Min,
        Lettres_Maj,
        Lettres_Min_Maj,
        Chiffres,
        Caracteres_Speciaux,
        Lettres_Min_Chiffres,
        Lettres_Maj_Chiffres,
        Lettres_Chiffres,
        Lettres_Caracteres_Speciaux,
        Lettres_Min_Caracteres_Speciaux,
        Lettres_Maj_Caracteres_Speciaux,
        Lettres_Min_Chiffres_Caracteres_Speciaux,
        Lettres_Maj_Chiffres_Caracteres_Speciaux,
        Lettres_Chiffres_Caracteres_Speciaux
    }

    public class Brute
    {

        private string Lettres_Min =  "abcdefghijklmnopqrstuvwxyz";
        private string Lettres_Maj = "ABDEFGHIJKLMNOPQRSTUVWXYZ";

        private string Chiffres = "0123456789";

        private string Caracteres_Speciaux = "/\\()[]{}& \"'~²àçéè-_=*$^:!;@|`^@#ù";
        /// <summary>
        /// tableau contenant l'ensemble des caractères 
        /// </summary>
        public string TableauFinal { get; set; }
        /// <summary>
        /// définit si l'état en pause ou pas
        /// </summary>
        public bool Pause { get; set; }
        /// <summary>
        /// liste des positions des différants caractères
        /// </summary>
        public List<int> liste_position { get; set; }
        /// <summary>
        /// le nombre de caractères de départ
        /// </summary>
        public int De { get; set; }
        /// <summary>
        /// le nombres de caractères finaux
        /// </summary>
        public int A { get; set;}
        /// <summary>
        /// barre de progression
        /// </summary>
        public ProgressBar Progression { get; set;  }
        /// <summary>
        /// Représente la textbox ou les informations sont affichés
        /// </summary>
        public TextBlock Sortie { get; set; }
        /// <summary>
        /// Représente le chemin du fichier à extraire
        /// </summary>
        public string Fichier { get; set; }
        /// <summary>
        /// Représente le chemin de sortie du fichier
        /// </summary>
        public string DossierSortie { get; set; }
        /// <summary>
        /// nombre de mot de passe
        /// </summary>
        public long NbMdp { get; set; }
        /// <summary>
        /// Nombres actuel de caractères des mots de passe générés
        /// </summary>
        public int LongueurActuel { get; set; }

        public int Mdpgeneres { get; set; }

        public Thread trd { get; set; }
        /// <summary>
        /// Définit si le mot de passe à été trouvé
        /// </summary>
        public bool Trouver { get; set; }

        public ProcessStartInfo rar { get; set; }

        public Process process { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fichier">Chemin fichier à extraire</param>
        /// <param name="dossier_sortie">Chemin du dossier de sortie</param>
        /// <param name="mode">mode de génération des mots de passes</param>
        /// <param name="de">taille de départ des mots de passes</param>
        /// <param name="a">taille finale des mots de passes</param>
        /// <param name="sortie">La textbox lié à l'interface graphique</param>
        /// <param name="progression">La progressbar qui permet de suivre la progression</param>
        public Brute(string fichier, string dossier_sortie, int mode, int de, int a, TextBlock sortie, ProgressBar progression)
        {
            ConstructTable(mode);
            NbMdp = CalculateurCombinaisons(de, a, TableauFinal.Length);
            De = de;
            A = a;
            Fichier = fichier;
            DossierSortie = dossier_sortie;
            Progression = progression;
            Sortie = sortie;
            LongueurActuel = De;
            liste_position = new List<int>();
            string chemin = Path.GetFullPath(@"Tools\UnRAR.exe");
            rar = new ProcessStartInfo(chemin);
            rar.WindowStyle = ProcessWindowStyle.Hidden;
            process = Process.Start(rar);
        }
        /// <summary>
        /// Construis l'objet pour obtenir la taille du tableau final
        /// </summary>
        /// <param name="mode"></param>
        public Brute(int mode, int de , int a)
        {
            ConstructTable(mode);
            NbMdp = CalculateurCombinaisons(de, a, TableauFinal.Length);
        }

        //détermine le nombre combinaisons totale
        public long CalculateurCombinaisons(int de, int a, int taille_tb)
        {
            long resultat = 0;
            for(int i = de; i <= a; i++)
            {
                resultat += (long)Math.Pow(taille_tb, i);
            }
            return resultat;
        }

        /// <summary>
        /// Construis le tableau qui servira de base au brute force et au mixage de combinaison
        /// </summary>
        /// <param name="mode">mode chosi en entier</param>
        public void ConstructTable(int mode)
        {
            if (mode == ((int)Mode.Lettres_Min)) TableauFinal = Lettres_Min;
            else if (mode == ((int)Mode.Lettres_Maj)) TableauFinal = Lettres_Maj;
            else if (mode == ((int)Mode.Lettres_Min_Maj)) TableauFinal = Lettres_Min + Lettres_Maj;
            else if (mode == ((int)Mode.Chiffres)) TableauFinal = Chiffres;
            else if (mode == ((int)Mode.Caracteres_Speciaux)) TableauFinal = Caracteres_Speciaux;
            else if (mode == ((int)Mode.Lettres_Min_Chiffres)) TableauFinal = Lettres_Min + Chiffres;
            else if (mode == ((int)Mode.Lettres_Maj_Chiffres)) TableauFinal = Lettres_Maj + Chiffres;
            else if (mode == ((int)Mode.Lettres_Chiffres)) TableauFinal = Lettres_Min + Lettres_Maj + Chiffres;
            else if (mode == ((int)Mode.Lettres_Caracteres_Speciaux)) TableauFinal = Lettres_Min + Lettres_Maj + Caracteres_Speciaux;
            else if (mode == ((int)Mode.Lettres_Min_Caracteres_Speciaux)) TableauFinal = Lettres_Min + Caracteres_Speciaux;
            else if (mode == ((int)Mode.Lettres_Maj_Caracteres_Speciaux)) TableauFinal = Lettres_Maj + Caracteres_Speciaux;
            else if (mode == ((int)Mode.Lettres_Min_Chiffres_Caracteres_Speciaux)) TableauFinal = Lettres_Min + Chiffres + Caracteres_Speciaux;
            else if (mode == ((int)Mode.Lettres_Maj_Chiffres_Caracteres_Speciaux)) TableauFinal = Lettres_Maj + Chiffres + Caracteres_Speciaux;
            else if (mode == ((int)Mode.Lettres_Chiffres_Caracteres_Speciaux)) TableauFinal = Lettres_Min + Lettres_Maj + Chiffres + Caracteres_Speciaux;
            else TableauFinal = Lettres_Min;
        }

        public void Reprendre()
        {
            Pause = false;
            trd = new Thread(new ThreadStart(Start));
            trd.IsBackground = true;
            trd.Start();
        }

        public void Start()
        {
            /*
             * Vérification d'état de pause
             * génération des mots de passe
             * capacité de reprise
             * tests des mots de passes
             * si le mot de passe est trouvé affichage du mot de passe
             */
            char[] combinaisons = new char[A];
            bool test = false;


            // on part de la longueur depart ou de la longueur sauvegardée
            for (LongueurActuel = LongueurActuel; LongueurActuel <= A; LongueurActuel++)
            {
                combinaisons = new char[LongueurActuel];
                for (int i = 0; i < LongueurActuel; i++)
                {
                    liste_position.Add(0);
                    combinaisons[i] = TableauFinal[liste_position[i]];
                }
                test = true;
                for (int i = 0; i <= TableauFinal.Length && test == true; i++)
                {
                    if (i != TableauFinal.Length)
                    {
                        combinaisons[LongueurActuel - 1] = TableauFinal[i];
                    }
                    if (i == TableauFinal.Length)
                    {
                        if (liste_position[LongueurActuel - 2] < TableauFinal.Length)
                        {
                            liste_position[LongueurActuel - 2] = liste_position[LongueurActuel - 2] + 1;
                        }

                        i = 0;
                        //mettre a jour la liste des positions
                        for (int j = 0; j <= LongueurActuel; j++)
                        {
                            if (j < LongueurActuel && liste_position[j] == TableauFinal.Length)
                            {
                                if (liste_position[0] > TableauFinal.Length)
                                {
                                    liste_position[0] = 0;
                                    test = false;
                                }
                                if (liste_position[j] <= TableauFinal.Length)
                                {
                                    liste_position[j] = liste_position[j] + 1;
                                    liste_position[j + 1] = 0;
                                }
                                if (liste_position[0] > TableauFinal.Length)
                                {
                                    liste_position[0] = 0;
                                    test = false;
                                }
                                if (liste_position[j] >= TableauFinal.Length)
                                {
                                    if ((j - 1) >= 0)
                                    {
                                        liste_position[j - 1] = liste_position[j - 1] + 1;
                                        liste_position[j] = 0;
                                    }
                                }
                            }

                            if (j < LongueurActuel)
                            {
                                if (liste_position[0] > TableauFinal.Length)
                                {
                                    liste_position[0] = 0;
                                    test = false;
                                }
                                combinaisons[j] = TableauFinal[liste_position[j]];
                            }

                        }

                    }
                    if(Pause == true)
                    {
                        break;
                    }
                    Mdpgeneres++;
                    string password = new string(combinaisons);
                    if(Mdpgeneres % 2 == 0)
                    {
                        Sortie.Dispatcher.Invoke(() =>
                        {
                            Sortie.Text = Mdpgeneres.ToString() + " / " + NbMdp.ToString() + " mots de passes testés";
                        });

                        Progression.Dispatcher.Invoke(() =>
                        {
                            Progression.Value = (Mdpgeneres * 100) / NbMdp;
                        });
                    }
                    if(Extract(password))
                    {
                        Pause = true;
                        Sortie.Dispatcher.Invoke(() =>
                        {
                            Sortie.Text = "Mot de passe trouvé :  " + password;
                            Sortie.Foreground = Brushes.Blue;
                            var stack = (StackPanel)Sortie.Parent;
                            var main = (MainWindow)stack.Parent;
                            main.Finish();
                        });
                        break;
                    }
                }
                if (Pause == true)
                {
                    trd.Abort();
                    break;
                }

            }
        }

        public bool Extract(string password)
        {
            string arg = string.Format("e \"{0}\" -p\"{1}\" -ad \"{2}\"", Fichier, password, DossierSortie);
            process.StartInfo.Arguments = arg;
            process.Start();
            process.WaitForExit();
            if(process.ExitCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}