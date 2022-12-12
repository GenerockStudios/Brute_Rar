using System.Windows;
using System.Windows.Controls;

namespace BruteRar
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string File_Path { get; set; }

        public string Directory_Path { get; set; }

        public Brute Brute { get; set; }

        public int De = 3;

        public int A = 4;

        public bool Path_File_OK { get; set; }

        public bool Path_Directory_OK { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            Intervalle_De_TextChanged(null, null);
            Pause.IsEnabled = false;
        }

        private void Btn_Path_File_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.ShowDialog();
            string tmp = openFileDialog.FileName;
            if (tmp == null && (File_Path == null || File_Path == ""))
            {
                Log.Text = "Veiller choisir un fichier .rar";
            }
            else if (tmp != null)
            {
                Path_File.Text = tmp;
                if(Path_Folder.Text == null || Path_Folder.Text == "")
                {
                    Path_Folder.Text = tmp.Replace("\\" + System.IO.Path.GetFileName(tmp), "");
                }
                Path_File_OK = true;
            }
        }

        private void Btn_Path_Folder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = fbd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                Path_Folder.Text = fbd.SelectedPath;
            }
        }

        private void Mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Intervalle_A != null && Intervalle_De != null && Intervalle_Resultat != null)
            {
                Brute brute = new Brute(Mode.SelectedIndex, int.Parse(Intervalle_De.Text), int.Parse(Intervalle_A.Text));
                Intervalle_Resultat.Text = brute.NbMdp.ToString();
            }
        }

        private void Demarrer_Click(object sender, RoutedEventArgs e)
        {
            if(Path_File_OK && long.Parse(Intervalle_Resultat.Text) > 0)
            {
                Progression.Value = 0;
                Path_File.IsEnabled = false;
                Path_Folder.IsEnabled = false;
                Mode.IsEnabled = false;
                Intervalle_A.IsEnabled = false;
                Intervalle_De.IsEnabled = false;
                Demarrer.IsEnabled = false;
                Reprendre.IsEnabled = false;
                Pause.IsEnabled = true;

                Brute = new Brute(Path_File.Text, Path_Folder.Text, Mode.SelectedIndex, int.Parse(Intervalle_De.Text), int.Parse(Intervalle_A.Text), Log, Progression);
                Brute.Reprendre();
            }
            else
            {
                MessageBox.Show("Veuilez renseigner les champs vide");
            }
        }

        private void Intervalle_De_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Intervalle_A != null && Intervalle_De != null && Intervalle_Resultat != null)
            {
                int de = 0;
                if (int.TryParse(Intervalle_De.Text, out de) == false)
                {
                    Intervalle_De.Text = De.ToString();
                }
                else
                {
                    De = de;
                    Intervalle_De.Text = De.ToString();
                }
                Brute brute = new Brute(Mode.SelectedIndex, int.Parse(Intervalle_De.Text), int.Parse(Intervalle_A.Text));
                Intervalle_Resultat.Text = brute.NbMdp.ToString();
            }
        }

        private void Intervalle_A_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Intervalle_A != null && Intervalle_De != null && Intervalle_Resultat != null)
            {
                int a = 0;
                if (int.TryParse(Intervalle_A.Text, out a) == false)
                {
                    Intervalle_A.Text = A.ToString();
                }
                else
                {
                    A = a;
                    Intervalle_A.Text = A.ToString();
                }

                Brute brute = new Brute(Mode.SelectedIndex, int.Parse(Intervalle_De.Text), int.Parse(Intervalle_A.Text));
                Intervalle_Resultat.Text = brute.NbMdp.ToString();
            }
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Doc.Informations informations = new Doc.Informations();
            informations.ShowDialog();

        }

        private void Minimise_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if(Brute.Pause == false)
            {
                Path_File.IsEnabled = true;
                Path_Folder.IsEnabled = true;
                Mode.IsEnabled = true;
                Intervalle_A.IsEnabled = true;
                Intervalle_De.IsEnabled = true;
                Demarrer.IsEnabled = true;
                Reprendre.IsEnabled = true;
                Brute.Pause = true;
            }
        }

        private void Reprendre_Click(object sender, RoutedEventArgs e)
        {
            if(Brute.Pause == true)
            {
                Path_File.IsEnabled = false;
                Path_Folder.IsEnabled = false;
                Mode.IsEnabled = false;
                Intervalle_A.IsEnabled = false;
                Intervalle_De.IsEnabled = false;
                Demarrer.IsEnabled = false;
                Reprendre.IsEnabled = false;
                Brute.Reprendre();
            }
        }

        public void Finish()
        {
            Path_File.IsEnabled = true;
            Path_Folder.IsEnabled = true;
            Mode.IsEnabled = true;
            Intervalle_A.IsEnabled = true;
            Intervalle_De.IsEnabled = true;
            Demarrer.IsEnabled = true;
            Reprendre.IsEnabled = true;
            Pause.IsEnabled = false;
            MessageBox.Show(Log.Text);
            Brute.process.Close();
        }

        private void Mask_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }

}
