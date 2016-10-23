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
        public bool CanDirectoryGoBack()
        {
            throw new NotImplementedException();
        }

        public bool CanDirectoryGoForward()
        {
            throw new NotImplementedException();
        }

        public bool CanDirectoryGoUp()
        {
            throw new NotImplementedException();
        }

        public DirectoryInfo CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public FileInfo CreateFile(string path)
        {
            throw new NotImplementedException();
        }

        public void Delete(DirectoryInfo directory)
        {
            throw new NotImplementedException();
        }

        public void Delete(FileInfo file)
        {
            throw new NotImplementedException();
        }

        public void DirectoryGoBack()
        {
            throw new NotImplementedException();
        }

        public void DirectoryGoForward()
        {
            throw new NotImplementedException();
        }

        public void DirectoryGoUp()
        {
            throw new NotImplementedException();
        }

        public DirectoryInfo GetCurrentDirectory()
        {
            return new DirectoryInfo(Directory.GetCurrentDirectory());
        }

        public int GetCurrentIndex()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Rename(DirectoryInfo sourceDirectory, string newDirectoryName)
        {
            throw new NotImplementedException();
        }

        public void Rename(FileInfo sourceFile, string newFileName)
        {
            throw new NotImplementedException();
        }

        public void SetCurrentDirectory(string directoryPath)
        {
            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            if (dir.Exists)
                Directory.SetCurrentDirectory(dir.FullName);
        }
    }
}
