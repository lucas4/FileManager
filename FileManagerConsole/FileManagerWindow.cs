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
        private int Page;
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
            //FileManager.SetCurrentDirectory(@"C:\");
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
                    SetView(SelectedPanel);
                }
                else if (keyinfo.Key == ConsoleKey.D1)
                    SetView(0);
                else if (keyinfo.Key == ConsoleKey.D2)
                    SetView(1);
                else if (keyinfo.Key == ConsoleKey.D3)
                    SetView(2);
                else if (SelectedPanel == 1 && (keyinfo.Key == ConsoleKey.Spacebar))
                {
                    ReadAddress();
                }
                else if (SelectedPanel == 2 && keyinfo.Key == ConsoleKey.Backspace)
                {
                    if (FileManager.CanDirectoryGoUp())
                    {
                        FileManager.DirectoryGoUp();
                        SelectedItem = null;
                        DrawAddressBar(false, true);
                        DrawContentPanel(true);
                    }
                }
                else if (SelectedPanel == 2 && keyinfo.Key == ConsoleKey.Oem4)
                {
                    if (FileManager.CanDirectoryGoBack())
                    {
                        FileManager.DirectoryGoBack();
                        SelectedItem = null;
                        DrawAddressBar(false, true);
                        DrawContentPanel(true);
                    }
                }
                else if (SelectedPanel == 2 && keyinfo.Key == ConsoleKey.Oem6)
                {
                    if (FileManager.CanDirectoryGoForward())
                    {
                        FileManager.DirectoryGoForward();
                        SelectedItem = null;
                        DrawAddressBar(false, true);
                        DrawContentPanel(true);
                    }
                }
                else if (SelectedPanel == 2 && keyinfo.Key == ConsoleKey.Enter)
                {
                    if (SelectedItem != null && SelectedItem.Exists)
                    {
                        if (SelectedItem is DirectoryInfo)
                        {
                            FileManager.SetCurrentDirectory(SelectedItem.FullName);
                            SelectedItem = null;
                            DrawAddressBar(false, true);
                            DrawContentPanel(true);
                        }
                        else if (SelectedItem is FileInfo)
                        {

                        }
                    }
                }
                else if (SelectedPanel == 2 && (keyinfo.Key == ConsoleKey.DownArrow || keyinfo.Key == ConsoleKey.LeftArrow 
                                               || keyinfo.Key == ConsoleKey.RightArrow || keyinfo.Key == ConsoleKey.UpArrow))
                {
                    NavigateInDirectory(keyinfo.Key);
                }
                else if (SelectedPanel == 2 && keyinfo.Key == ConsoleKey.PageUp)
                {
                    if (Page > 0)
                    {
                        Page--;
                        SelectedItem = null;
                        DrawContentPanel(true);
                    }
                }
                else if (SelectedPanel == 2 && keyinfo.Key == ConsoleKey.PageDown)
                {
                    
                    int count = FileManager.GetFilesAndDirectories().Count;
                    int totalPage = (count + 23) / 24;
                    if ((Page + 1) < totalPage)
                    {
                        Page++;
                        SelectedItem = null;
                        DrawContentPanel(true);
                    }
                }
            }
        }

        private void ReadAddress()
        {
            Console.CursorVisible = true;
            Console.SetCursorPosition(3, 4);
            string path = ReadPath(FileManager.GetCurrentDirectory().FullName);
            Console.CursorVisible = false;

            DirectoryInfo dir;
            try
            {
                dir = new DirectoryInfo(path);
            }
            catch
            {
                dir = new DirectoryInfo(FileManager.GetCurrentDirectory().FullName);
            }

            if (dir.Exists)
            {
                FileManager.SetCurrentDirectory(dir.FullName);
                SelectedItem = null;
                SetView(2);
            }
            else
                SetView(1);
        }

        private void NavigateInDirectory(ConsoleKey key)
        {
            if (SelectedItem == null)
            {
                SelectedItem = GetTable(Page)[0, 0];
                DrawContentPanel(true);
            }
            else
            {
                var files = GetTable(Page);
                Tuple<int?, int?> item = FindItemInTable(GetTable(Page));
                if (item.Item1.HasValue && item.Item2.HasValue)
                {
                    int i, j;
                    switch (key)
                    {
                        case ConsoleKey.DownArrow:
                            i = item.Item1.Value + 1;
                            j = item.Item2.Value;
                            if (i >= 0 && i < 3)
                            {
                                FileSystemInfo newitem = files[i, j];
                                if (newitem != null)
                                {
                                    SelectedItem = newitem;
                                    DrawContentPanel(true);
                                }
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            i = item.Item1.Value;
                            j = item.Item2.Value - 1;
                            if (j >= 0 && j < 8)
                            {
                                FileSystemInfo newitem = files[i, j];
                                if (newitem != null)
                                {
                                    SelectedItem = newitem;
                                    DrawContentPanel(true);
                                }
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            i = item.Item1.Value;
                            j = item.Item2.Value + 1;
                            if (j >= 0 && j < 8)
                            {
                                FileSystemInfo newitem = files[i, j];
                                if (newitem != null)
                                {
                                    SelectedItem = newitem;
                                    DrawContentPanel(true);
                                }
                            }
                            break;
                        case ConsoleKey.UpArrow:
                            i = item.Item1.Value - 1;
                            j = item.Item2.Value;
                            if (i >= 0 && i < 3)
                            {
                                FileSystemInfo newitem = files[i, j];
                                if (newitem != null)
                                {
                                    SelectedItem = newitem;
                                    DrawContentPanel(true);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void SetView(int panel)
        {
            SelectedPanel = panel % 3;
            if (panel == 0)
            {
                DrawAddressBar(false);
                DrawContentPanel(false);
                DrawNavbar(true);
            }
            else if (panel == 1)
            {
                DrawNavbar(false);
                DrawContentPanel(false);
                DrawAddressBar(true, true);
            }
            else if (panel == 2)
            {
                DrawContentPanel(true);
                DrawNavbar(false);
                DrawAddressBar(false, true); 
            }
            else
            {
                DrawNavbar(true);
                DrawAddressBar(false);
                DrawContentPanel(false);
            }
        }

        private void DrawAddressBar(bool enable, bool enableText = false)
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

            Console.SetCursorPosition(3, 4);
            Console.Write(new string(' ', 115));

            if (enableText)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(3, 4);
            Console.Write(FileManager.GetCurrentDirectory().FullName);
        }

        private void DrawContentPanel(bool enable, bool clearDirectory = true)
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

            if (clearDirectory)
            {
                for (int j = 8; j < 29; j++)
                {
                    Console.SetCursorPosition(3, j);
                    Console.Write(new string(' ', 115));
                }
            }

            Console.SetCursorPosition(100, 28);
            int totalPage = (FileManager.GetFilesAndDirectories().Count + 23) / 24;
            string pageof = "Strona " + (Page + 1) + "/" + totalPage;
            Console.Write(pageof);

            DrawProperties(enable);

            var table = GetTable(Page);
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
            
            if (SelectedItem != null)
            {
                var icon = FindItemInTable(table);
                if (icon.Item1.HasValue && icon.Item2.HasValue)
                {
                    int left = (icon.Item2.Value * 11) + 4;
                    int top = (icon.Item1.Value * 7) + 8;
                    DrawIconSelected(SelectedItem, enable, left, top);
                }
                    
            }
        }

        private void DrawProperties(bool enable)
        {
            if (enable)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else
                Console.ForegroundColor = ConsoleColor.Blue;

            Console.SetCursorPosition(91, 6);
            Console.Write('┬');
            Console.SetCursorPosition(91, 29);
            Console.Write('┴');
            for (int i = 7; i < 29; i++)
            {
                Console.SetCursorPosition(91, i);
                Console.Write('│');
            }

            if (SelectedItem != null)
            {
                Console.SetCursorPosition(100, 7);
                Console.Write("Właściwości");

                Console.SetCursorPosition(93, 10);
                Console.Write("Nazwa:");

                Console.SetCursorPosition(93, 11);
                if (SelectedItem.Name.Length > 24)
                {
                    string name = SelectedItem.Name;
                    for (int i = 0; i < 3; i++)
                    {
                        Console.SetCursorPosition(93, 11 + i);
                        if (name.Length > 24)
                        {
                            Console.Write(name.Substring(0, 24));
                            name = name.Substring(24, name.Length - 24);
                        }
                        else
                        {
                            Console.Write(name);
                            break;
                        }
                    }
                }
                else
                    Console.Write(SelectedItem.Name);

                Console.SetCursorPosition(93, 16);
                Console.Write("Data utworzenia:");

                Console.SetCursorPosition(93, 17);
                Console.Write(SelectedItem.CreationTime);

                Console.SetCursorPosition(93, 19);
                Console.Write("Data modyfkiacji:");

                Console.SetCursorPosition(93, 20);
                Console.Write(SelectedItem.LastWriteTime);

                if (SelectedItem is FileInfo)
                {
                    Console.SetCursorPosition(93, 22);
                    Console.Write("Rozmiar:");

                    Console.SetCursorPosition(93, 23);
                    Console.Write((((SelectedItem as FileInfo).Length / 1024f) / 1024f).ToString("0.00") + " MB");
                }

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

        private void DrawIconSelected(FileSystemInfo item, bool enable, int left, int top)
        {
            DrawIcon(item, enable, left, top);
            if (enable)
            {
                Console.SetCursorPosition(left - 1, top + 5);
                Console.BackgroundColor = ConsoleColor.Red;
                // poprawić to:
                if (item.Name.Length > 10)
                    Console.Write(item.Name.Substring(0, 7) + "...");
                else
                    Console.Write(item.Name);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
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

        private string ReadPath(string Default)
        {
            int pos = Console.CursorLeft;
            Console.Write(Default);
            ConsoleKeyInfo info;
            List<char> chars = new List<char>();
            if (string.IsNullOrEmpty(Default) == false)
            {
                chars.AddRange(Default.ToCharArray());
            }

            while (true)
            {
                info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Backspace && Console.CursorLeft > pos)
                {
                    chars.RemoveAt(chars.Count - 1);
                    Console.CursorLeft -= 1;
                    Console.Write(' ');
                    Console.CursorLeft -= 1;

                }
                else if (info.Key == ConsoleKey.Enter) { Console.Write(Environment.NewLine); break; }
                //Here you need create own checking of symbols
                else if (info.KeyChar != '\b' && info.KeyChar != '\n' && info.KeyChar != '\t' && info.KeyChar != '\r') //if (char.IsLetterOrDigit(info.KeyChar)) 
                {
                    if (chars.ToArray().Length < 115)
                    {
                        Console.Write(info.KeyChar);
                        chars.Add(info.KeyChar);
                    }
                }
            }
            return new string(chars.ToArray());
        }

        private FileSystemInfo[,] GetTable(int page)
        {
            var files = FileManager.GetFilesAndDirectories();
            FileSystemInfo[,] table = new FileSystemInfo[3, 8];

            int col = 0;
            int row = 0;
            for (int i = (page * 24); i < files.Count; i++)
            {
                if (i == ((page * 24) + 24 ))
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

        private Tuple<int?, int?> FindItemInTable(FileSystemInfo[,] table)
        {
            int? x = null;
            int? y = null;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    FileSystemInfo item = table[i, j];
                    if (item != null && item.FullName.Equals(SelectedItem.FullName))
                    {
                        x = i;
                        y = j;
                        break;
                    }
                }
                if (x.HasValue && y.HasValue)
                    break;
            }
            return new Tuple<int?, int?>(x, y);
        }
    }
}
