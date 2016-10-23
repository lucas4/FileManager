using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace FileManagerEngine
{
    /// <summary>
    /// Provides properties and instance methods for read information about drives.
    /// </summary>
    public static class DriveManager
    {
        private static Dictionary<string, DriveInfo> Disks { get; set; }
        private static ManagementEventWatcher watcher { get; set; }
        /// <summary>
        /// Event occurs when we detect a change in drives.
        /// </summary>
        public static EventHandler OnDriveFound { get; set; }

        static DriveManager()
        {
            Disks = new Dictionary<String, DriveInfo>();
            
            watcher = new ManagementEventWatcher();
            watcher.Query = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2 or EventType = 3");
            watcher.EventArrived += new EventArrivedEventHandler(DriveFoundEvent);
            watcher.Start();
            
            RefreshDrives();
        }
        
        private static void RefreshDrives()
        {
            Disks.Clear();

            foreach (var drive in DriveInfo.GetDrives())
            {
                Disks.Add(drive.Name, drive);
                // to się przyda potem:
                //double freeSpace = drive.TotalFreeSpace;
                //double totalSpace = drive.TotalSize;
                //double percentFree = (freeSpace / totalSpace) * 100;
                //float num = (float)percentFree;

                //Console.WriteLine("Drive:{0} With {1} % free", drive.Name, num);
                //Console.WriteLine("Space Remaining:{0}", drive.AvailableFreeSpace);
                //Console.WriteLine("Percent Free Space:{0}", percentFree);
                //Console.WriteLine("Space used:{0}", drive.TotalSize);
                //Console.WriteLine("Type: {0}", drive.DriveType);
                //Console.WriteLine("\n\n");
            }
        }

        private static void DriveFoundEvent(object sender, EventArrivedEventArgs e)
        {
            RefreshDrives();
            
            if (OnDriveFound != null)
                OnDriveFound(null, EventArgs.Empty);
        }

        /// <summary>
        /// Returns information about drive.
        /// </summary>
        /// <param name="name">Drive name. For example: C:\\</param>
        public static DriveInfo GetDrive(string name)
        {
            if (Disks.ContainsKey(name))
                return Disks[name];
            else
                return null;
        }
    }
}
