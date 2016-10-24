using FileManagerEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace FileManagerEngine
{
    partial class History : IHistory
    {
        ObservableCollection<DirectoryInfo> historyList = new ObservableCollection<DirectoryInfo>();
        private int index = -1;


        public void AddDirectory(DirectoryInfo directory)
        {
            historyList.Add(directory);
            index = historyList.Count + 1;
        }

        public ObservableCollection<DirectoryInfo> GetHistory()
        {
            return historyList;
        }

        public int GetCurrentIndex()
        {
            return index;
        }

        public bool CanDirectoryGoBack()
        {
            if ((index - 1) >= 0)
                return true;
            else
                return false;
        }

        public bool CanDirectoryGoForward()
        {
            if ((index + 1) <= historyList.Count - 1)
                return true;
            else
                return false;
        }

        public DirectoryInfo DirectoryGoBack()
        {
            if (CanDirectoryGoBack()) index--;
            return historyList[index];
        }

        public DirectoryInfo DirectoryGoForward()
        {
            if (CanDirectoryGoForward()) index++;
            return historyList[index];
        }
    }
}
