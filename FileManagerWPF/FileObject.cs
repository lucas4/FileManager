using System;
using System.Collections.Generic;
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
            
            this.ImageSource = new Uri("Images/Extensions/zip.png", UriKind.Relative);
            this.Image = new BitmapImage(this.ImageSource);
        }
    }
    public class DirectoryObject : FileManagerObject
    {
        public DirectoryInfo File { get; set; }
        
        private DirectoryObject() { }
        public DirectoryObject(string Path)
        {
            this.Name = "fdff";
            this.Path = Path;
            this.ImageSource = new Uri("Images/Folders/folder.png", UriKind.Relative);
            this.Image = new BitmapImage(this.ImageSource);
        }
    }
}
