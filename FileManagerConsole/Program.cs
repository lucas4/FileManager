﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using FileManagerEngine;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace FileManagerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            FileManagerWindow manager = new FileManagerWindow();
            manager.Run();
        }
    }
}
