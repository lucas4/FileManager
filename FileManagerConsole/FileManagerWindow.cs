using FileManagerEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerConsole
{
    class FileManagerWindow
    {
        #region console window settings
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        #endregion

        private FileManager FileManager;
        private int SelectedPanel;
        private FileSystemInfo SelectedItem;

        public FileManagerWindow()
        {
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);
            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.SetWindowSize(120, 30);
            Console.SetBufferSize(120, 30);
            Console.CursorVisible = false;
            Console.Clear();

            FileManager = new FileManager();
            SelectedPanel = 0;
            SelectedItem = null;
            FileManager.SetCurrentDirectory(@"C:\Program Files (x86)");
        }

        public void Run()
        {
            DrawNavbar(true);
            DrawAddressBar(false, false);
            DrawContentPanel(false);

            Console.SetCursorPosition(0, 0);
            Console.SetWindowPosition(0, 0);

            ConsoleKeyInfo keyinfo;
            while (true)
            {
                keyinfo = Console.ReadKey(true);

                if (keyinfo.Key == ConsoleKey.Tab)
                {
                    SelectedPanel = (++SelectedPanel) % 3;
                    switch (SelectedPanel)
                    {
                        case 0:
                            SetView(0);
                            break;
                        case 1:
                            SetView(1);
                            break;
                        case 2:
                            SetView(2);
                            break;
                        default:
                            SetView(0);
                            break;
                    }
                }
                else if(keyinfo.Key == ConsoleKey.D1)
                {
                    SelectedPanel = 0;
                    SetView(0);
                }
                else if (keyinfo.Key == ConsoleKey.D2)
                {
                    SelectedPanel = 1;
                    SetView(1);
                }
                else if (keyinfo.Key == ConsoleKey.D3)
                {
                    SelectedPanel = 2;
                    SetView(2);
                }
                else if (SelectedPanel == 2)
                {
                    if (keyinfo.Key == ConsoleKey.DownArrow)
                    {
                        if (SelectedItem != null)
                        {
                            var files = FileManager.GetFilesAndDirectories();

                        }
                        else
                            SelectedItem = FileManager.GetFilesAndDirectories()[0];
                    }
                    else if (keyinfo.Key == ConsoleKey.LeftArrow)
                    {
                        if (SelectedItem != null)
                        {

                        }
                        else
                            SelectedItem = FileManager.GetFilesAndDirectories()[0];
                    }
                    else if (keyinfo.Key == ConsoleKey.RightArrow)
                    {
                        if (SelectedItem != null)
                        {

                        }
                        else
                            SelectedItem = FileManager.GetFilesAndDirectories()[0];
                    }
                    else if (keyinfo.Key == ConsoleKey.UpArrow)
                    {
                        if (SelectedItem != null)
                        {

                        }
                        else
                            SelectedItem = FileManager.GetFilesAndDirectories()[0];
                    }
                }
            }
        }

        private void SetView(int panel)
        {
            if (panel == 0)
            {
                DrawNavbar(true);
                DrawAddressBar(false, false);
                DrawContentPanel(false);
            }
            else if (panel == 1)
            {
                DrawNavbar(false);
                DrawAddressBar(true, true);
                DrawContentPanel(false);
            }
            else if (panel == 2)
            {
                DrawNavbar(false);
                DrawAddressBar(false, true);
                DrawContentPanel(true);
            }
            else
            {
                DrawNavbar(true);
                DrawAddressBar(false, false);
                DrawContentPanel(false);
            }
        }

        private void DrawAddressBar(bool enable, bool enableText)
        {
            if (enable)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else
                Console.ForegroundColor = ConsoleColor.Blue;

            for (int i = 2; i < 118; i++)
            {
                Console.SetCursorPosition(i, 3);
                Console.Write('─');
            }
            for (int i = 2; i < 118; i++)
            {
                Console.SetCursorPosition(i, 5);
                Console.Write('─');
            }

            Console.SetCursorPosition(1, 3);
            Console.Write('┌');
            Console.SetCursorPosition(1, 4);
            Console.Write('│');
            Console.SetCursorPosition(1, 5);
            Console.Write('└');

            Console.SetCursorPosition(118, 3);
            Console.Write('┐');
            Console.SetCursorPosition(118, 4);
            Console.Write('│');
            Console.SetCursorPosition(118, 5);
            Console.Write('┘');

            if (enableText)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(3, 4);
            Console.Write(FileManager.GetCurrentDirectory().FullName);

        }

        private void DrawContentPanel(bool enable)
        {
            if (enable)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else
                Console.ForegroundColor = ConsoleColor.Blue;

            for (int i = 2; i < 118; i++)
            {
                Console.SetCursorPosition(i, 6);
                Console.Write('─');
            }
            for (int i = 2; i < 118; i++)
            {
                Console.SetCursorPosition(i, 29);
                Console.Write('─');
            }
            for (int i = 7; i < 29; i++)
            {
                Console.SetCursorPosition(1, i);
                Console.Write('│');
            }
            for (int i = 7; i < 29; i++)
            {
                Console.SetCursorPosition(118, i);
                Console.Write('│');
            }
            Console.SetCursorPosition(1, 6);
            Console.Write('┌');
            Console.SetCursorPosition(1, 29);
            Console.Write('└');
            Console.SetCursorPosition(118, 6);
            Console.Write('┐');
            Console.SetCursorPosition(118, 29);
            Console.Write('┘');

            //Console.SetCursorPosition(1, 3);
            //Console.Write('│');
            //Console.SetCursorPosition(118, 3);
            //Console.Write('│');

            DrawProperties(enable);

            //var files = FileManager.GetFilesAndDirectories();
            //foreach (FileSystemInfo item in files)
            //{
            //    if (item is DirectoryInfo)
            //        DrawIcon(enable, 4, 8);
            //}
            var table = GetTable(0);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (table[i, j] != null)
                    {
                        var icon = table[i, j];
                        DrawIcon(icon, enable, (j * 11) + 4, (i * 7) + 8);
                    }
                    else
                        break;
                }
            }
        }

        private void DrawProperties(bool enable)
        {
            Console.SetCursorPosition(91, 6);
            Console.Write('┬');
            Console.SetCursorPosition(91, 29);
            Console.Write('┴');
            for (int i = 7; i < 29; i++)
            {
                Console.SetCursorPosition(91, i);
                Console.Write('│');
            }
        }

        private void DrawIcon(FileSystemInfo item, bool enable, int left, int top)
        {
            string extension = null;
            if (enable)
            {
                if (item is DirectoryInfo)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else
                {
                    extension = item.Extension;
                    switch (extension)
                    {
                        case ".txt":
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                        case ".rar":
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            break;
                        case ".zip":
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            break;
                        case ".dll":
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;
                        case ".exe":
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                    }
                    
                }
            }
            else
                Console.ForegroundColor = ConsoleColor.Blue;
            //x,y = 4 
            //Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(left + 1, top);//5,4
            Console.Write('─');
            Console.Write('─');
            Console.Write('─');
            Console.Write('─');
            Console.Write('─');
            Console.Write('─');

            Console.SetCursorPosition(left, top);//4,4
            Console.Write('╓');
            Console.SetCursorPosition(left, top + 1);//4,5
            Console.Write('║');
            Console.SetCursorPosition(left, top + 2);//4,6
            Console.Write('║');
            Console.SetCursorPosition(left, top + 3);//4,7
            Console.Write('║');
            Console.SetCursorPosition(left, top + 4);//4,8
            Console.Write('╙');
            Console.SetCursorPosition(left + 7, top);//11,4
            Console.Write('┐');
            Console.SetCursorPosition(left + 7, top + 1);//11,5
            Console.Write('│');
            Console.SetCursorPosition(left + 7, top + 2);//11,6
            Console.Write('│');
            Console.SetCursorPosition(left + 7, top + 3);//11,7
            Console.Write('│');
            Console.SetCursorPosition(left + 7, top + 4);//11,8
            Console.Write('┘');
            Console.SetCursorPosition(left + 1, top + 4);//5,8
            Console.Write('─');
            Console.Write('─');
            Console.Write('─');
            Console.Write('─');
            Console.Write('─');
            Console.Write('─');
            Console.SetCursorPosition(left + 1, top);//4,6
            
            if (item is FileSystemInfo && !string.IsNullOrEmpty(extension))
                Console.Write(extension);

            Console.SetCursorPosition(left - 1, top + 5);//3,9
            // poprawić to:
            if (item.Name.Length > 10)
                Console.Write(item.Name.Substring(0, 7) + "...");
            else
                Console.Write(item.Name);
        }

        private void DrawNavbar(bool enable)
        {
            if (enable)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else
                Console.ForegroundColor = ConsoleColor.Blue;

            for (int i = 2; i < 118; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write('─');
            }
            for (int i = 2; i < 118; i++)
            {
                Console.SetCursorPosition(i, 2);
                Console.Write('─');
            }
            Console.SetCursorPosition(1, 0);
            Console.Write('┌');
            Console.SetCursorPosition(1, 1);
            Console.Write('│');
            Console.SetCursorPosition(1, 2);
            Console.Write('└');

            Console.SetCursorPosition(118, 0);
            Console.Write('┐');
            Console.SetCursorPosition(118, 1);
            Console.Write('│');
            Console.SetCursorPosition(118, 2);
            Console.Write('┘');

            Console.SetCursorPosition(4, 1);
            Console.Write("Plik");
            if (enable)
            {
                Console.SetCursorPosition(4, 1);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("P");
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }

            Console.SetCursorPosition(10, 1);
            Console.Write("Edycja");
            if (enable)
            {
                Console.SetCursorPosition(10, 1);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("E");
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }

        }

        private FileSystemInfo[,] GetTable(int page)
        {
            var files = FileManager.GetFilesAndDirectories();
            FileSystemInfo[,] table = new FileSystemInfo[3, 8];

            int col = 0;
            int row = 0;
            for (int i = (page * 24); i < files.Count; i++)
            {
                if (i == 24)
                    break;
                table[row, col] = files[i];
                col++;
                if (col == 8)
                {
                    row++;
                    col = 0;
                }
            }

            return table;
        }
    }
}
