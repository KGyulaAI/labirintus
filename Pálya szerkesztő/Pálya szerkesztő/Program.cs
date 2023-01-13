using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Palya_szerkeszto
{
    class Palya_szerkeszto
    {
        static int sorSzam = 10;
        static int oszlopSzam = 10;
        static char[,]? map;
        static bool language = false;
        static List<char> palyaElemek = new List<char>() { '.', '╬', '═', '╦', '╩', '║', '╣', '╠', '╗', '╝', '╚', '╔', '█' };
        //Kijáratok száma
        static int GetSuitableEntrance(char[,] map)
        {
            int exits = 0;
            for (int sorIndex = 0; sorIndex < map.GetLength(0); sorIndex++)
            {
                //Bal oszlop
                char balOszlopChar = map[sorIndex, 0];
                switch (balOszlopChar)
                {
                    case '╬':
                    case '═':
                    case '╦':
                    case '╩':
                    case '╣':
                    case '╗':
                    case '╝':
                        exits++;
                        break;
                }
                //Jobb oszlop
                char jobbOszlopChar = map[sorIndex, map.GetLength(1) - 1];
                switch (jobbOszlopChar)
                {
                    case '╬':
                    case '═':
                    case '╦':
                    case '╩':
                    case '╠':
                    case '╚':
                    case '╔':
                        exits++;
                        break;
                }
            }
            for (int oszlopIndexe = 1; oszlopIndexe < map.GetLength(1) - 1; oszlopIndexe++)
            {
                //Felső sor
                char felsoSorChar = map[0, oszlopIndexe];
                switch (felsoSorChar)
                {
                    case '╬':
                    case '╩':
                    case '║':
                    case '╣':
                    case '╠':
                    case '╝':
                    case '╚':
                        exits++;
                        break;
                }
                //Alsó sor
                char alsoSorChar = map[map.GetLength(0) - 1, oszlopIndexe];
                switch (alsoSorChar)
                {
                    case '╬':
                    case '╦':
                    case '║':
                    case '╣':
                    case '╠':
                    case '╗':
                    case '╔':
                        exits++;
                        break;
                }
            }
            return exits;
        }
        //Termek száma
        public static int GetRoomNumber(char[,] map)
        {
            int roomNumber = 0;
            foreach (char c in map)
            {
                if (c == '█')
                {
                    roomNumber++;
                }
            }
            return roomNumber;
        }
        //Szabálytalan karakterek száma
        static bool IsInvalidElement(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] != '.' && map[i, j] != '╔' && map[i, j] != '╗' && map[i, j] != '╚' && map[i, j] != '╝' && map[i, j] != '╬' && map[i, j] != '║' && map[i, j] != '█')
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        //Elérhetetlen karakterek
        static List<string> GetUnavailableElements(char[,] map)
        {
            List<string> elerhetetlen = new List<string>();

            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            for (int i = 1; i < rows - 1; i++)
            {
                for (int j = 1; j < cols - 1; j++)
                {
                    if (map[i, j] != '.' && map[i - 1, j] == '.' && map[i + 1, j] == '.' && map[i, j - 1] == '.' && map[i, j + 1] == '.')
                    {
                        elerhetetlen.Add((i + 1) + ":" + (j + 1));
                    }
                }
            }
            return elerhetetlen;
        }
        //Program címe
        static void Title()
        {
            Console.Write(@"
  __  __                _____                           _             
 |  \/  |              / ____|                         | |            
 | \  / | __ _ _ __   | |  __  ___ _ __   ___ _ __ __ _| |_ ___  _ __ 
 | |\/| |/ _` | '_ \  | | |_ |/ _ \ '_ \ / _ \ '__/ _` | __/ _ \| '__|
 | |  | | (_| | |_) | | |__| |  __/ | | |  __/ | | (_| | || (_) | |   
 |_|  |_|\__,_| .__/   \_____|\___|_| |_|\___|_|  \__,_|\__\___/|_|   
              | |                                                     
              |_|                                                     
");
        }
        //Program főmenüje
        static void FoMenu()
        {
            //ALAPÉRTELMEZETT MAP 10x10 méret
            map = Generate(10, 10);
            do
            {
                PrintMap(map, true);
                for (int index = 0; index < GetUnavailableElements(map).Count; index++)
                {
                    Console.Write($"{GetUnavailableElements(map)[index]}, ");
                }
                if (language == false)
                {
                    Console.WriteLine($"\nTermek száma: {GetRoomNumber(map)}");
                    Console.WriteLine($"Kijáratok száma: {GetSuitableEntrance(map)}");
                }
                else
                {
                    Console.WriteLine($"\nNumber of rooms: {GetRoomNumber(map)}");
                    Console.WriteLine($"Number of exits: {GetSuitableEntrance(map)}");
                }
                Title();
                if (language == false)
                {
                    Console.WriteLine("\t[m] map készítése");
                    Console.WriteLine("\t[s] beállítások");
                    Console.WriteLine("\t[d] map mentése");
                    Console.WriteLine("\t[l] map betöltése");
                    Console.WriteLine("\t[e] jelenlegi map szerkesztése");
                    Console.WriteLine("\t[q] kilépés a programból");
                    Console.WriteLine("Kérem válasszon!: ");
                }
                else
                {
                    Console.WriteLine("\t[m] make map");
                    Console.WriteLine("\t[s] settings");
                    Console.WriteLine("\t[d] save map");
                    Console.WriteLine("\t[l] load map");
                    Console.WriteLine("\t[e] edit current map");
                    Console.WriteLine("\t[q] quit");
                    Console.WriteLine("Please choose an option!: ");
                }
                ConsoleKey billentyuBekeres;
                billentyuBekeres = Console.ReadKey(true).Key;
                switch (billentyuBekeres)
                {
                    //PÁLYA GENERÁLÁS
                    //M betű lenyomása
                    case ConsoleKey.M:
                        Console.Clear();
                        Title();
                        if (language == false)
                        {
                            Console.Write("\nSorok száma: ");
                        }
                        else
                        {
                            Console.Write("\nNumber of rows: ");
                        }
                        sorSzam = Convert.ToInt32(Console.ReadLine());
                        if (language == false)
                        {
                            Console.Write("\nOszlopok száma: ");
                        }
                        else
                        {
                            Console.Write("\nNumber of collums: ");
                        }
                        oszlopSzam = Convert.ToInt32(Console.ReadLine());
                        map = Generate(sorSzam, oszlopSzam);
                        PrintMap(map, true);
                        ElemLerakas();
                        break;
                    //BEÁLLÍTÁSOK MENÜ
                    //S betű lenyomása
                    case ConsoleKey.S:
                        Console.Clear();
                        SettingsMenu();
                        billentyuBekeres = Console.ReadKey(true).Key;
                        switch (billentyuBekeres)
                        {
                            //English
                            case ConsoleKey.E:
                                language = true;
                                Console.Clear();
                                break;
                            //Magyar
                            case ConsoleKey.M:
                                language = false;
                                Console.Clear();
                                break;
                            //Vissza
                            case ConsoleKey.B:
                                Console.Clear();
                                FoMenu();
                                break;
                        }
                        break;
                    //MENTÉS
                    //D betű lenyomása
                    case ConsoleKey.D:
                        if (GetRoomNumber(map) < 1 && GetSuitableEntrance(map) < 1 && IsInvalidElement(map) == false)
                        {
                            if (language == false)
                            {
                                Console.WriteLine("Nem mentheted el így!");
                            }
                            else
                            {
                                Console.WriteLine("You can't save it like that!");
                            }
                            Thread.Sleep(1000);
                            FoMenu();
                        }
                        string bekeres;
                        if (language == false)
                        {
                            Console.WriteLine("Kérem a pálya elérési útvonalát: ");
                        }
                        else
                        {
                            Console.WriteLine("Please add the map's path: ");
                        }
                        bekeres = Console.ReadLine();
                        MapMentes(map, bekeres);
                        break;
                    //BETÖLTÉS
                    //L betű lenyomása
                    case ConsoleKey.L:
                        string bekeres1;
                        if (language == false)
                        {
                            Console.WriteLine("Kérem a pálya elérési útvonalát: ");
                        }
                        else
                        {
                            Console.WriteLine("Please add the map's path: ");
                        }
                        bekeres1 = Console.ReadLine();
                        do
                        {
                            if (!File.Exists(bekeres1))
                            {
                                if (language == false)
                                {
                                    Console.WriteLine("Nem létezik, add meg újra: ");
                                }
                                else
                                {
                                    Console.WriteLine("It doesn't exist, give it again: ");
                                }
                                bekeres1 = Console.ReadLine();
                            }
                        } while (!File.Exists(bekeres1));
                        map = MapBetoltes(bekeres1);
                        PrintMap(map, true);
                        break;
                    //SZERKESZTÉS
                    //E betű lenyomása
                    case ConsoleKey.E:

                        PrintMap(map, true);
                        ElemLerakas();
                        break;
                    //KILÉPÉS
                    //Q betű lenyomása
                    case ConsoleKey.Q:
                        System.Environment.Exit(1);
                        break;
                    default:
                        if (language == false)
                        {
                            Console.WriteLine("Helytelen opció, próbálja újra: ");
                        }
                        else
                        {
                            Console.WriteLine("Incorrect option, try again: ");
                        }
                        Thread.Sleep(1000);
                        break;
                }
            } while (true);
        }
        //Program beállítások menüje
        static void SettingsMenu()
        {
            Title();
            if (language == false)
            {
                Console.WriteLine("\t[e] english");
                Console.WriteLine("\t[m] magyar");
                Console.WriteLine("\t[b] vissza");
                Console.WriteLine("Kérem válasszon!: ");
            }
            else
            {
                Console.WriteLine("\t[e] english");
                Console.WriteLine("\t[m] magyar");
                Console.WriteLine("\t[b] back");
                Console.WriteLine("Please choose an option!: ");
            }
        }
        //Elem lerakás
        static void ElemLerakas()
        {
            string sorIndex;
            string oszlopIndex;
            string elemIndex;
            bool quit = false;
            do
            {
                try
                {
                    if (language == false)
                    {
                        Console.WriteLine("Add meg a sor koordinátáját!: ");
                    }
                    else
                    {
                        Console.WriteLine("Add the coordinate of the row!: ");
                    }
                    do
                    {
                        sorIndex = Console.ReadLine();
                        if (Convert.ToInt32(sorIndex) > map.GetLength(0) || Convert.ToInt32(sorIndex) < 0)
                        {
                            if (language == false)
                            {
                                Console.WriteLine("Helytelen koordináta, adj meg másikat: ");
                            }
                            else
                            {
                                Console.WriteLine("Incorrect coordinate, give another one: ");
                            }
                        }
                    } while (Convert.ToInt32(sorIndex) > map.GetLength(0) || Convert.ToInt32(sorIndex) < 0);
                    if (language == false)
                    {
                        Console.WriteLine("Add meg az oszlop koordinátáját!: ");
                    }
                    else
                    {
                        Console.WriteLine("Add the coordinate of the collumn!: ");
                    }
                    do
                    {
                        oszlopIndex = Console.ReadLine();
                        if (Convert.ToInt32(oszlopIndex) > map.GetLength(1) || Convert.ToInt32(oszlopIndex) < 0)
                        {
                            if (language == false)
                            {
                                Console.WriteLine("Helytelen koordináta, adj meg másikat: ");
                            }
                            else
                            {
                                Console.WriteLine("Incorrect coordinate, give another one: ");
                            }
                        }
                    } while (Convert.ToInt32(oszlopIndex) > map.GetLength(1) || Convert.ToInt32(oszlopIndex) < 0);
                    if (language == false)
                    {
                        Console.WriteLine("Add meg az elem indexét!: ");
                    }
                    else
                    {
                        Console.WriteLine("Add the index of the item!: ");
                    }
                    Console.WriteLine("(0). (1)╬ (2)═ (3)╦ (4)╩ (5)║ (6)╣ (7)╠ (8)╗ (9)╝ (10)╚ (11)╔ (12)█");
                    do
                    {
                        elemIndex = Console.ReadLine();
                        if (Convert.ToInt32(elemIndex) > palyaElemek.Count - 1 || Convert.ToInt32(elemIndex) < 0)
                        {
                            if (language == false)
                            {
                                Console.WriteLine("Helytelen index, adj meg másikat: ");
                            }
                            else
                            {
                                Console.WriteLine("Incorrect index, give another: ");
                            }
                        }
                    } while (Convert.ToInt32(elemIndex) > palyaElemek.Count - 1 || Convert.ToInt32(elemIndex) < 0);
                    map[Convert.ToInt32(sorIndex) - 1, Convert.ToInt32(oszlopIndex) - 1] = palyaElemek[Convert.ToInt32(elemIndex)];
                    PrintMap(map, true);
                }
                catch (System.FormatException)
                {
                    break;
                }
            } while (quit == false);
        }
        //Pályát generál a megadott méretek alapján
        static char[,] Generate(int sorSzam, int oszlopSzam)
        {
            char[,] ujMap = new char[sorSzam, oszlopSzam];
            AlapertelmezettMap(ujMap);
            return ujMap;
        }
        //Feltölti a pályát pontokkal
        static void AlapertelmezettMap(char[,] palya)
        {
            for (int row = 0; row < palya.GetLength(0); row++)
            {
                for (int col = 0; col < palya.GetLength(1); col++)
                {
                    palya[row, col] = '.';
                }
            }
        }
        //Betölti a pályát fájlból
        static char[,] MapBetoltes(string eleresiUtvonal)
        {
            string[] lines = File.ReadAllLines(eleresiUtvonal);
            char[,] map = new char[lines.Length, lines[0].Length];
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    map[row, col] = lines[row][col];
                }
            }
            return map;
        }
        //Elmenti a pályát a megadott helyre
        static void MapMentes(char[,] map, string eleresiUtvonal)
        {
            //ACCESS DENIED LÉPHET FEL!!!

            string[] sorok = new string[map.GetLength(0)];

            for (int sorIndex = 0; sorIndex < map.GetLength(0); sorIndex++)
            {
                for (int oszlopIndex = 0; oszlopIndex < map.GetLength(1); oszlopIndex++)
                {
                    sorok[sorIndex] += map[sorIndex, oszlopIndex];
                }
            }
            File.WriteAllLines(eleresiUtvonal, sorok);
        }
        //Map megjelenítése
        static void PrintMap(char[,] map, bool border = false)
        {
            Console.Clear();
            if (border)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(' ');
                for (int col = 1; col <= map.GetLength(1); col++)
                {
                    if (col % 10 == 0)
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        Console.Write(col % 10);
                    }
                }
            }
            Console.WriteLine();
            for (int row = 0; row < map.GetLength(0); row++)
            {
                if (border)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    if ((row + 1) % 10 == 0)
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        Console.Write((row + 1) % 10);
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                for (int coli = 0; coli < map.GetLength(1); coli++)
                {
                    Console.Write(map[row, coli]);
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        static void Main(string[] args)
        {
            FoMenu();
        }
    }
}