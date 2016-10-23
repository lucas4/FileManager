using FileManagerEngine;
using System;
using System.Collections.Generic;
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
                    SetView(0);
                }
                else if (keyinfo.Key == ConsoleKey.D2)
                {
                    SetView(1);
                }
                else if (keyinfo.Key == ConsoleKey.D3)
                {
                    SetView(2);
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
            Console.Write("C:\\Program Files\\Visual Studio");

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

            DrawIcon2(enable, 4, 8);
            DrawIcon(enable, 15, 8);
            DrawIcon3(enable, 26, 8);
            DrawIcon(enable, 37, 8);
            DrawIcon3(enable, 48, 8);
            DrawIcon(enable, 59, 8);
            DrawIcon3(enable, 70, 8);
            DrawIcon(enable, 81, 8);

            DrawIcon2(enable, 4, 15);
            DrawIcon(enable, 15, 15);
            DrawIcon3(enable, 26, 15);
            DrawIcon(enable, 37, 15);
            DrawIcon3(enable, 48, 15);
            DrawIcon(enable, 59, 15);
            DrawIcon3(enable, 70, 15);
            DrawIcon(enable, 81, 15);

            DrawIcon2(enable, 4, 22);
            DrawIcon(enable, 15, 22);
            DrawIcon3(enable, 26, 22);
            DrawIcon(enable, 37, 22);
            DrawIcon3(enable, 48, 22);
            DrawIcon(enable, 59, 22);
            DrawIcon3(enable, 70, 22);
            DrawIcon(enable, 81, 22);
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

        private void DrawIcon(bool enable, int left, int top)
        {
            if (enable)
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
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
            Console.Write("ZIP");

            Console.SetCursorPosition(left - 1, top + 5);//3,9
            Console.Write("   file");
        }

        private void DrawIcon2(bool enable, int left, int top)
        {
            if (enable)
                Console.ForegroundColor = ConsoleColor.Yellow;
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


            Console.SetCursorPosition(left - 1, top + 5);//3,9
            //Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("Program...");
            //Console.BackgroundColor = ConsoleColor.DarkBlue;
        }

        private void DrawIcon3(bool enable, int left, int top)
        {
            if (enable)
                Console.ForegroundColor = ConsoleColor.DarkCyan;
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
            Console.Write("DLL");

            Console.SetCursorPosition(left - 1, top + 5);//3,9
            Console.Write("   file");
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
    }
}
