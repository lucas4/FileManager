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
        private int index = 0;
        public ObservableCollection<DirectoryInfo> ClearHistory()
        {
            historyList.Clear();
            return historyList;
        }

        public void AddDirectory(DirectoryInfo directory)
        {
            if (index < historyList.Count && (historyList.Count - index > 1))
            {
                if (historyList[index + 1] == directory)
                {
                    index++;
                }
                else
                {
                    for (int i = historyList.Count - 1; i >= index; i--)
                    {
                        historyList.RemoveAt(i);
                    }
                    historyList.Add(directory);
                    index = ((historyList.Count) - 1);
                }
            }
            else
            {
                historyList.Add(directory);
                index++;
            }

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
            if ((index - 1) > 0)
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
            return historyList[index - 1];
        }

        public DirectoryInfo DirectoryGoForward()
        {
            if (CanDirectoryGoForward()) index++;
            return historyList[index];
        }
    }
}
