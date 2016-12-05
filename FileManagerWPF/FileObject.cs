using FileManagerEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FileManagerWPF
{
    public class FileManagerObject
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public Uri ImageSource { get; set; }
        public BitmapImage Image { get; set; }
    }
    public class FileObject : FileManagerObject
    {
        public FileInfo File { get; set; }

        private FileObject() { }
        public FileObject(string Path)
        {
            this.Name = "name";
            this.Path = Path;
            this.ImageSource = new Uri("/Images/Extensions/File.png", UriKind.Relative);
            this.Image = new BitmapImage(this.ImageSource);
        }
        public FileObject(FileInfo obj)
        {

            this.File = obj;
            this.ImageSource = new Uri("/Images/Extensions/File.png", UriKind.Relative);
            this.Image = new BitmapImage(this.ImageSource);
        }
    }
    public class DirectoryObject : FileManagerObject
    {
        public DirectoryInfo File { get; set; }
        
        private DirectoryObject() { }
        public DirectoryObject(string Path)
        {
            this.File = new DirectoryInfo(Path);
            this.ImageSource = new Uri("/Images/Folders/Folder2.png", UriKind.Relative);
            this.Image = new BitmapImage(this.ImageSource);
        }
        public DirectoryObject(DirectoryInfo obj)
        {
            this.File = obj;
            this.ImageSource = new Uri("/Images/Folders/Folder2.png", UriKind.Relative);
            this.Image = new BitmapImage(this.ImageSource);
        }
    }
    public class MenuItem
    {
        public ObservableCollection<MenuItem> Items { get; set; }
        public bool HasItems { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public Uri ImageSource { get; set; }
        public BitmapImage Image { get; set; }

        protected MenuItem() { }
        public MenuItem(string Path)
        {
            DirectoryInfo dir = new DirectoryInfo(Path);
            FileManager manager = new FileManager();
            this.Items = new ObservableCollection<MenuItem>();
            this.Name = dir.Name;
            this.Path = Path;
            this.ImageSource = new Uri("/Images/Folders/Folder2.png", UriKind.Relative);
            this.Image = new BitmapImage(this.ImageSource);
            this.HasItems = (manager.GetDirectories(Path).Count > 0) ? true : false;
        }
    }
    public class DriveMenuItem : MenuItem
    {
        private DriveInfo drive;
        public DriveMenuItem(string key)
        {
            this.drive = new DriveInfo(key);
            FileManager manager = new FileManager();
            this.Items = new ObservableCollection<MenuItem>();
            this.Name = drive.Name;
            this.Path = drive.RootDirectory.FullName;
            this.HasItems = (manager.GetDirectories(Path).Count > 0) ? true : false;
            SetImage(drive.DriveType);
        }
        private void SetImage(DriveType type)
        {
            switch (type)
            {
                case DriveType.Removable:
                    Name = drive.VolumeLabel;
                    ImageSource = new Uri("/Images/Icons/USB2.png", UriKind.Relative);
                    break;
                case DriveType.Fixed:
                    Name = "Dysk lokalny " + drive.Name;
                    ImageSource = new Uri("/Images/Icons/HDD.png", UriKind.Relative);
                    break;
                case DriveType.Network:
                    ImageSource = new Uri("/Images/Icons/Cloud-Storage.png", UriKind.Relative);
                    break;
                case DriveType.CDRom:
                    ImageSource = new Uri("/Images/Icons/CD.png", UriKind.Relative);
                    break;
                default:
                    ImageSource = new Uri("/Images/Icons/HDD.png", UriKind.Relative);
                    break;
            }
            this.Image = new BitmapImage(this.ImageSource);
        }
    }
    public class ComputerMenuItem : MenuItem
    {
        public ComputerMenuItem()
        {
            this.Items = new ObservableCollection<MenuItem>();
            this.Name = "Ten Komputer";
            this.Path = @"/";
            this.HasItems = true;
            SetImage();
        }
        private void SetImage()
        {
            ImageSource = new Uri("/Images/Icons/Windows-Client.png", UriKind.Relative);
            this.Image = new BitmapImage(this.ImageSource);
        }
    }
}
