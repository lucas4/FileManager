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

        public DirectoryInfo CreateDirectory(string path)
        {
           return Directory.CreateDirectory(path);
        }

        public FileInfo CreateFile(string path)
        {
            FileInfo file = new FileInfo(path);
            file.Create();
            return file;
        }

        public void Delete(DirectoryInfo directory)
        {
            directory.Delete();
        }

        public void Delete(FileInfo file)
        {
            file.Delete();
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
            if(CanDirectoryGoUp())
            {
                DirectoryInfo directory = GetCurrentDirectory().Parent;
                SetCurrentDirectory(directory.FullName);
            }
        }

        public DirectoryInfo GetCurrentDirectory()
        {
            return new DirectoryInfo(Directory.GetCurrentDirectory());
        }

        public int GetCurrentIndex()
        {
            return history.GetCurrentIndex();
        }

        public ObservableCollection<DirectoryInfo> GetDirectories()
        {
            ObservableCollection<DirectoryInfo> directorieslist = new ObservableCollection<DirectoryInfo>();
            DirectoryInfo current = GetCurrentDirectory();
            if (!current.Exists)
                return directorieslist;

            var directories = Directory.GetDirectories(current.FullName);
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

            var files = Directory.GetFiles(current.FullName);
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
            foreach (var directory in GetDirectories())
            {
                filesAndDirectories.Add(directory);
            }
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
            {
                if (sourceDirectory == null)
                {
                    throw new ArgumentNullException("sourceDirectory", "Directory info to rename cannot be null");
                }

                if (string.IsNullOrWhiteSpace(newDirectoryName))
                {
                    throw new ArgumentException("New name cannot be null or blank", "name");
                }

                sourceDirectory.MoveTo(Path.Combine(sourceDirectory.Parent.FullName, newDirectoryName));
            }
        }

        public void Rename(FileInfo sourceFile, string newFileName)
        {
            {
                if (sourceFile == null)
                {
                    throw new ArgumentNullException("sourceFile", "File info to rename cannot be null");
                }

                if (string.IsNullOrWhiteSpace(newFileName))
                {
                    throw new ArgumentException("New name cannot be null or blank", "name");
                }

                sourceFile.MoveTo(Path.Combine(sourceFile.FullName, newFileName));
            }
        }

        public void SetCurrentDirectory(string directoryPath)
        {
            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            if (dir.Exists)
            {

                Directory.SetCurrentDirectory(dir.FullName);
                history.AddDirectory(dir);
            }
        }
    }
}