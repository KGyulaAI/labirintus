using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Labyrinth
{
    internal class Endings
    {
        private static int kivalasztottOpcio;
        private string[] opciok = { LangHelper.GetString("yes"), LangHelper.GetString("mainMenu") };

        private void EndingOpciok()
        {
            Console.ResetColor();
            Console.WriteLine(LangHelper.GetString("opcioValasztas"));
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
        public int Win()
        {
            Fomenu fomenu = new Fomenu();
            ConsoleKey consoleKey;
            do
            {

                Console.Clear();
                fomenu.Cim();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(LangHelper.GetString("win"));
                Console.WriteLine();
                Console.WriteLine(LangHelper.GetString("playAgain"));
                Console.WriteLine();
                EndingOpciok();
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
                        fomenu.MainMenu();
                        break;
                }
            }
            return kivalasztottOpcio;
        }
        public int LoseIfNotExploredRooms()
        {
            Fomenu fomenu = new Fomenu();
            ConsoleKey consoleKey;
            do
            {
                Console.Clear();
                fomenu.Cim();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(LangHelper.GetString("lose"));
                Console.WriteLine(LangHelper.GetString("loseIfNotEnoughRooms"));
                Console.WriteLine();
                Console.WriteLine(LangHelper.GetString("playAgain"));
                Console.WriteLine();
                EndingOpciok();
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
                        fomenu.MainMenu();
                        break;
                }
            }
            return kivalasztottOpcio;
        }
        public int LoseIfStuckInWall()
        {
            Fomenu fomenu = new Fomenu();
            ConsoleKey consoleKey;
            do
            {
                Console.Clear();
                fomenu.Cim();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(LangHelper.GetString("lose"));
                Console.WriteLine(LangHelper.GetString("loseIfStuckInWall"));
                Console.WriteLine();
                Console.WriteLine(LangHelper.GetString("playAgain"));
                Console.WriteLine();
                EndingOpciok();
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
                        fomenu.MainMenu();
                        break;
                }
            }
            return kivalasztottOpcio;
        }
    }
}