using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace FileManagerEngine
{
    interface IHistory
    {
        void AddDirectory(DirectoryInfo directory);
        ObservableCollection<DirectoryInfo> GetHistory();
        ObservableCollection<DirectoryInfo> ClearHistory();
        int GetCurrentIndex();
        bool CanDirectoryGoBack();
        bool CanDirectoryGoForward();
        DirectoryInfo DirectoryGoBack();
        DirectoryInfo DirectoryGoForward();
    }
}
