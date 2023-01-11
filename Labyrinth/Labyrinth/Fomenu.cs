using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Labyrinth
{
    internal class Fomenu
    {
        private int kivalasztottOpcio;
        public string[] opciok = { LangHelper.GetString("newGame"), LangHelper.GetString("settings"), LangHelper.GetString("exit") };
        public void Cim()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"
            
        ██╗      █████╗ ██████╗ ██╗   ██╗██████╗ ██╗███╗   ██╗████████╗██╗  ██╗
        ██║     ██╔══██╗██╔══██╗╚██╗ ██╔╝██╔══██╗██║████╗  ██║╚══██╔══╝██║  ██║
        ██║     ███████║██████╔╝ ╚████╔╝ ██████╔╝██║██╔██╗ ██║   ██║   ███████║
        ██║     ██╔══██║██╔══██╗  ╚██╔╝  ██╔══██╗██║██║╚██╗██║   ██║   ██╔══██║
        ███████╗██║  ██║██████╔╝   ██║   ██║  ██║██║██║ ╚████║   ██║   ██║  ██║
        ╚══════╝╚═╝  ╚═╝╚═════╝    ╚═╝   ╚═╝  ╚═╝╚═╝╚═╝  ╚═══╝   ╚═╝   ╚═╝  ╚═╝
                                                                       
            ");
        }
        private void FomenuOpciok()
        {
            SettingsLanguage settingsLanguage = new SettingsLanguage();
            Console.ResetColor();
            Console.WriteLine($"{LangHelper.GetString("opcioValasztas")}");
            Console.WriteLine();
            for (int i = 0; i < opciok.Count(); i++)
            {
                if (i == kivalasztottOpcio)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($">>{opciok[i]}<<");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine($"  {opciok[i]}  ");
                }
            }
            Console.ResetColor();
        }
        public int MainMenu()
        {
            ConsoleKey consoleKey;
            do
            {
                Console.Clear();
                Cim();
                FomenuOpciok();
                consoleKey = Console.ReadKey(true).Key;

                if (consoleKey == ConsoleKey.DownArrow)
                {
                    kivalasztottOpcio++;
                    if (kivalasztottOpcio == opciok.Length)
                    {
                        kivalasztottOpcio = 0;
                    }

                }
                else if (consoleKey == ConsoleKey.UpArrow)
                {
                    kivalasztottOpcio--;
                    if (kivalasztottOpcio == -1)
                    {
                        kivalasztottOpcio = opciok.Length - 1;
                    }
                }
            } while (consoleKey != ConsoleKey.Enter);
            if (consoleKey == ConsoleKey.Enter)
            {
                switch (kivalasztottOpcio)
                {
                    case 0:
                        Console.Clear();
                        Play player = new Play();
                        player.NewGame();
                        break;
                    case 1:
                        Console.Clear();
                        Settings settings = new Settings();
                        settings.SettingsPage();
                        break;
                    case 2:
                        Console.Clear();
                        Environment.Exit(0);
                        break;
                }
            }
            return kivalasztottOpcio;
        }
    }
}