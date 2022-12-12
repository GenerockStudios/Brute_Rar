using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace BruteRar.Doc
{
    /// <summary>
    /// Logique d'interaction pour Informations.xaml
    /// </summary>
    public partial class Informations : Window
    {
        public Informations()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(IsLoaded == true)
            {
                var tab = (TabItem)ConteneurOptions.SelectedItem;
                Title = tab.Header.ToString();
                if(tab.Header.ToString() == "Aide")
                {
                    Aide.Text = ReadFile(System.IO.Path.GetFullPath("help.txt"));
                }
                else
                {
                    Contrat.Text = ReadFile(System.IO.Path.GetFullPath("contrat.txt"));
                }
            }
        }

        private string ReadFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            string text = "";
            foreach(string line in lines)
            {
                text += line + "\n";
            }
            return text;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var tab = (TabItem)ConteneurOptions.SelectedItem;
            Title = tab.Header.ToString();
            if (tab.Header.ToString() == "Aide")
            {
                Aide.Text = ReadFile(System.IO.Path.GetFullPath("help.txt"));
            }
            else
            {
                Contrat.Text = ReadFile(System.IO.Path.GetFullPath("contrat.txt"));
            }
        }
    }
}
