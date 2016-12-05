using FileManagerEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace FileManagerEngine
{
    partial class FileManager : IFileManager
    {
        public EventHandler<ErrorEvent> Error { get; set; }
        History history = new History();

        public bool CanDirectoryGoBack()
        {
            return history.CanDirectoryGoBack();
        }

        public bool CanDirectoryGoForward()
        {
            return history.CanDirectoryGoForward();
        }

        public bool CanDirectoryGoUp()
        {
            DirectoryInfo directory = GetCurrentDirectory();
            if (directory.Parent != null && directory.Parent.Exists)
                return true;
            else
                return false;
        }

        public DirectoryInfo CreateDirectory(string path, string name = "Nowy folder")
        {
            DirectoryInfo dir = null;
            try
            {
                dir = new DirectoryInfo(Path.Combine(path, name));
                dir.Create();
            }
            catch (Exception ex)
            {
                if (Error != null)
                    Error(this, new ErrorEvent { Value = ex.Message });
            }
            return dir;
        }

        public FileInfo CreateFile(string path, string name = "Nowy plik")
        {
            FileInfo file = null;
            try
            {
                file = new FileInfo(Path.Combine(path, name));
                file.Create().Dispose();
            }
            catch (Exception ex)
            {
                if (Error != null)
                    Error(this, new ErrorEvent { Value = ex.Message });
            }
            return file;
        }

        public void Delete(FileSystemInfo file)
        {
            try
            {
                if (file != null && file.Exists)
                    file.Delete();
            }
            catch (Exception ex)
            {
                if (Error != null)
                    Error(this, new ErrorEvent { Value = ex.Message });
            }
        }

        public void DirectoryGoBack()
        {
            Directory.SetCurrentDirectory(history.DirectoryGoBack().FullName);
        }

        public void DirectoryGoForward()
        {
            Directory.SetCurrentDirectory(history.DirectoryGoForward().FullName);
        }

        public void DirectoryGoUp()
        {
            if (CanDirectoryGoUp())
            {
                DirectoryInfo directory = GetCurrentDirectory().Parent;
                SetCurrentDirectory(directory.FullName);
            }
        }

        public bool CheckWritePermissions(string path)
        {
            try
            {
                using (FileStream fs = File.Create( Path.Combine(path, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose) )
                { }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckReadPermissions(string path)
        {
            try
            {
                var directories = Directory.GetDirectories(path);
                var files = Directory.GetFiles(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public DirectoryInfo GetCurrentDirectory()
        {
            DirectoryInfo dir = null;
            try
            {
                dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            }
            catch (Exception ex)
            {
                if (Error != null)
                    Error(this, new ErrorEvent { Value = ex.Message });
            }
            return dir;
        }

        public int GetCurrentIndex()
        {
            return history.GetCurrentIndex();
        }

        public ObservableCollection<DirectoryInfo> GetDirectories()
        {
            return GetDirectories(string.Empty);
        }
        public ObservableCollection<DirectoryInfo> GetDirectories(string path)
        {
            ObservableCollection<DirectoryInfo> directorieslist = new ObservableCollection<DirectoryInfo>();
            DirectoryInfo current;
            if (string.IsNullOrEmpty(path))
                current = GetCurrentDirectory();
            else
                current = new DirectoryInfo(path);
            if (!current.Exists)
                return directorieslist;

            string[] directories = null;
            try
            {
                directories = Directory.GetDirectories(current.FullName);
            }
            catch (UnauthorizedAccessException ex)
            {
                //SetCurrentDirectory(current.Parent.FullName);
                if (Error != null)
                    Error(this, new ErrorEvent { Value = ex.Message });
                return directorieslist;
            }
            catch (Exception ex)
            {
                if (Error != null)
                    Error(this, new ErrorEvent { Value = ex.Message });
                return directorieslist;
            }
            foreach (var item in directories)
            {
                DirectoryInfo directory = new DirectoryInfo(item);
                directorieslist.Add(directory);
            }
            return directorieslist;
        }

        public ObservableCollection<FileInfo> GetFiles()
        {
            ObservableCollection<FileInfo> fileslist = new ObservableCollection<FileInfo>();
            DirectoryInfo current = GetCurrentDirectory();
            if (!current.Exists)
                return fileslist;

            string[] files = null;
            try
            {
                files = Directory.GetFiles(current.FullName);
            }
            catch (Exception ex)
            {
                if (Error != null)
                    Error(this, new ErrorEvent { Value = ex.Message });
                return fileslist;
            }
            foreach (var item in files)
            {
                FileInfo file = new FileInfo(item);
                fileslist.Add(file);
            }
            return fileslist;
        }

        public ObservableCollection<FileSystemInfo> GetFilesAndDirectories()
        {
            ObservableCollection<FileSystemInfo> filesAndDirectories = new ObservableCollection<FileSystemInfo>();
            var dirs = GetDirectories();
            if (dirs != null && dirs.Count > 0)
                foreach (var directory in GetDirectories())
                {
                    filesAndDirectories.Add(directory);
                }
            var files = GetFiles();
            if (files != null && files.Count > 0)
                foreach (var file in GetFiles())
                {
                    filesAndDirectories.Add(file);
                }

            return filesAndDirectories;
        }

        public ObservableCollection<DirectoryInfo> GetHistory()
        {
            return history.GetHistory();
        }

        public void Rename(DirectoryInfo sourceDirectory, string newDirectoryName)
        {
            if (string.IsNullOrWhiteSpace(newDirectoryName))
            {
                if (Error != null)
                    Error(this, new ErrorEvent { Value = "Nazwa nie może być pusta." });
            }
            string path = Path.Combine(sourceDirectory.Parent.FullName, newDirectoryName);
            try
            {
                sourceDirectory.MoveTo(path);
            }
            catch (Exception ex)
            {
                if (Error != null)
                    Error(this, new ErrorEvent { Value = ex.Message });
            }
        }

        public void Rename(FileInfo sourceFile, string newFileName)
        {
            if (string.IsNullOrWhiteSpace(newFileName))
            {
                if (Error != null)
                    Error(this, new ErrorEvent { Value = "Nazwa nie może być pusta." });
            }
            string path = Path.Combine(sourceFile.Directory.FullName, newFileName);
            try
            {
                sourceFile.MoveTo(path);
            }
            catch (Exception ex)
            {
                if (Error != null)
                    Error(this, new ErrorEvent { Value = ex.Message });
            }
        }

        public void SetCurrentDirectory(string directoryPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(directoryPath);
                if (dir.Exists)
                {
                    Directory.SetCurrentDirectory(dir.FullName);
                    history.AddDirectory(dir);


                }
                else
                    if (Error != null)
                    Error(this, new ErrorEvent { Value = String.Format("Nie znaleziono '{0}", directoryPath) });
            }
            catch (Exception ex)
            {
                if (Error != null)
                    Error(this, new ErrorEvent { Value = ex.Message });
            }
        }
    }

    class ErrorEvent : EventArgs
    {
        public string Title { get; set; }
        public string Value { get; set; }

        public ErrorEvent()
        {
            this.Title = "ERROR";
        }
    }

}