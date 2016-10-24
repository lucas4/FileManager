using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace FileManagerEngine.Interfaces
{
    interface IFileManager
    {
        FileInfo CreateFile(string path, string name);
        DirectoryInfo CreateDirectory(string path, string name);
        void Rename(FileInfo sourceFile, string newFileName);
        void Rename(DirectoryInfo sourceDirectory, string newDirectoryName);
        void Delete(FileSystemInfo file);
        
        void SetCurrentDirectory(string directoryPath);
        DirectoryInfo GetCurrentDirectory();
        ObservableCollection<FileSystemInfo> GetFilesAndDirectories();
        ObservableCollection<FileInfo> GetFiles();
        ObservableCollection<DirectoryInfo> GetDirectories();

        bool CanDirectoryGoUp();
        void DirectoryGoUp();

        bool CanDirectoryGoBack();
        void DirectoryGoBack();
        bool CanDirectoryGoForward();
        void DirectoryGoForward();
        ObservableCollection<DirectoryInfo> GetHistory();
        int GetCurrentIndex();
    }
}
