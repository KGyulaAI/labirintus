using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Labyrinth
{
    internal class Settings
    {
        private static int kivalasztottOpcio;
        private string[] opciok = { LangHelper.GetString("language"), LangHelper.GetString("back") };
        private void BeallitasokOpciok()
        {
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
        public int SettingsPage()
        {
            ConsoleKey keyInfo;
            do
            {
                Console.Clear();
                Fomenu fomenu = new Fomenu();
                fomenu.Cim();
                BeallitasokOpciok();
                keyInfo = Console.ReadKey(true).Key;

                if (keyInfo == ConsoleKey.DownArrow)
                {
                    kivalasztottOpcio++;
                    if (kivalasztottOpcio == opciok.Length)
                    {
                        kivalasztottOpcio = 0;
                    }

                }
                else if (keyInfo == ConsoleKey.UpArrow)
                {
                    kivalasztottOpcio--;
                    if (kivalasztottOpcio == -1)
                    {
                        kivalasztottOpcio = opciok.Length - 1;
                    }
                }
            } while (keyInfo != ConsoleKey.Enter);
            if (keyInfo == ConsoleKey.Enter)
            {
                Fomenu fomenu = new Fomenu();
                switch (kivalasztottOpcio)
                {
                    case 0:
                        Console.Clear();
                        fomenu.Cim();
                        SettingsLanguage settingsLanguage = new SettingsLanguage();
                        settingsLanguage.SettingsLanguagePage();
                        break;
                    case 1:
                        Console.Clear();
                        fomenu.Cim();
                        fomenu.MainMenu();
                        break;
                }
            }
            return kivalasztottOpcio;
        }
    }
    internal class SettingsLanguage
    {
        private static int kivalasztottOpcio;
        private string[] opciok = { LangHelper.GetString("english"), LangHelper.GetString("magyar"), LangHelper.GetString("back") };
        private void BeallitasokOpciok()
        {
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
        public int SettingsLanguagePage()
        {
            ConsoleKey keyInfo;
            do
            {
                Console.Clear();
                Fomenu fomenu = new Fomenu();
                fomenu.Cim();
                BeallitasokOpciok();
                keyInfo = Console.ReadKey(true).Key;

                if (keyInfo == ConsoleKey.DownArrow)
                {
                    kivalasztottOpcio++;
                    if (kivalasztottOpcio == opciok.Length)
                    {
                        kivalasztottOpcio = 0;
                    }

                }
                else if (keyInfo == ConsoleKey.UpArrow)
                {
                    kivalasztottOpcio--;
                    if (kivalasztottOpcio == -1)
                    {
                        kivalasztottOpcio = opciok.Length - 1;
                    }
                }
            } while (keyInfo != ConsoleKey.Enter);
            if (keyInfo == ConsoleKey.Enter)
            {
                Fomenu fomenu = new Fomenu();
                switch (kivalasztottOpcio)
                {
                    case 0:
                        LangHelper.ChangeLanguage("en");
                        Console.Clear();
                        fomenu.Cim();
                        SettingsLanguagePage();
                        break;
                    case 1:
                        LangHelper.ChangeLanguage("hu");
                        Console.Clear();
                        fomenu.Cim();
                        SettingsLanguagePage();
                        break;
                    case 2:
                        Console.Clear();
                        fomenu.Cim();
                        Settings settings = new Settings();
                        settings.SettingsPage();
                        break;
                }
            }
            return kivalasztottOpcio;
        }
    }
}