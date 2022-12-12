using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Brute_Rar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string? File_Path { get; set; }
        
        public string? Directory_Path { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Close_MouseEnter(object sender, MouseEventArgs e)
        {
            Close.Background = Brushes.Red;
        }

        private void Close_MouseLeave(object sender, MouseEventArgs e)
        {
            Close.Background = Brushes.PaleVioletRed;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_Path_File_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            string? tmp = openFileDialog.FileName;
            if(tmp == null && (File_Path == null || File_Path == ""))
            {
                Log.Text = "Veiller choisir un fichier";
            }
            else if(tmp != null)
            {
                File_Path = tmp;
                Path_File.Text = File_Path;
            }
        }

        private void Btn_Path_Folder_Click(object sender, RoutedEventArgs e)
        {
            
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath);

                    System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
                }
        }

        private void Mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Demarrer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
