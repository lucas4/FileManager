using FileManagerEngine;
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
using System.Windows.Shapes;

namespace FileManagerWPF
{
    /// <summary>
    /// Interaction logic for AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        FileManager FileManager;
        private byte Type;
        private string Path;
        public AddEditWindow(byte type, string path)
        {
            InitializeComponent();
            FileManager = new FileManager();
            FileManager.Error += ShowError;
            this.Type = type;
            this.Path = path;
            Init();

        }

        private void ShowError(object sender, ErrorEvent e)
        {
            MessageBox.Show(e.Value, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }

        private void Init()
        {
            switch (this.Type)
            {
                case 0:
                    this.Title = "Nowy plik";
                    this.Icon = new BitmapImage(new Uri("pack://application:,,,/FileManagerWPF;component/Images/Extensions/File.png", UriKind.Absolute));
                    break;
                case 1:
                    this.Title = "Edytuj plik";
                    this.Icon = new BitmapImage(new Uri("pack://application:,,,/FileManagerWPF;component/Images/Icons/Rename.png", UriKind.Absolute));
                    var file = new FileInfo(this.Path);
                    FileNameTextBox.Text = file.Name;
                    break;
                case 2:
                    this.Title = "Nowy folder";
                    this.Icon = new BitmapImage(new Uri("pack://application:,,,/FileManagerWPF;component/Images/Folders/Folder2.png", UriKind.Absolute));
                    break;
                case 3:
                    this.Title = "Edytuj folder";
                    this.Icon = new BitmapImage(new Uri("pack://application:,,,/FileManagerWPF;component/Images/Icons/Rename.png", UriKind.Absolute));
                    var dir = new DirectoryInfo(this.Path);
                    FileNameTextBox.Text = dir.Name;
                    break;
                default:
                    this.Title = "Nowy folder";
                    this.Icon = new BitmapImage(new Uri("pack://application:,,,/FileManagerWPF;component/Images/Folders/Folder2.png", UriKind.Absolute));
                    break;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            switch (this.Type)
            {
                case 0:
                    if (FileManager.CreateFile(Path, FileNameTextBox.Text) != null)
                        this.Close();
                    break;
                case 1:
                    FileManager.Rename(new FileInfo(Path), FileNameTextBox.Text);
                    this.Close();
                    break;
                case 2:
                    if (FileManager.CreateDirectory(Path, FileNameTextBox.Text) != null)
                        this.Close();
                    break;
                case 3:
                    FileManager.Rename(new DirectoryInfo(Path), FileNameTextBox.Text);
                    this.Close();
                    break;
                default:
                    break;
            }
        }
    }
}
