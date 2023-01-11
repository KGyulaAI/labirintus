using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Labyrinth
{
    internal class Play
    {
        private static int MAP_POZICIO_X = MapPosition.MAP_POZICIO_X;
        private static int MAP_POZICIO_Y = MapPosition.MAP_POZICIO_Y;
        private static int currentXCoordinate;
        private static int currentYCoordinate;
        private int exploredRooms;
        private static string? mapEleresiUtja;
        private string? difficulty;
        private int kivalasztottOpcio;
        private string[] difficultyOptions = { LangHelper.GetString("easy"), LangHelper.GetString("hard") };

        //METÓDUSOK A JÁTÉKHOZ
        /// <summary>
        /// Betölti a térképet a fájlból.
        /// </summary>
        /// <param name="mapNeve">A térkép elérési útvonala.</param>
        /// <returns>Térkép</returns>
        public char[,] BetoltTerkepet(string mapNeve)
        {
            string[] sorok = File.ReadAllLines(mapNeve);
            char[,] map = new char[sorok.Length, sorok[0].Length];
            for (int sorIndex = 0; sorIndex < map.GetLength(0); sorIndex++)
            {
                for (int oszlopIndexe = 0; oszlopIndexe < map.GetLength(1); oszlopIndexe++)
                {
                    map[sorIndex, oszlopIndexe] = sorok[sorIndex][oszlopIndexe];
                }
            }
            return map;
        }
        /// <summary>
        /// Könnyű térkép megjelenítése.
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        static void EasyMap(char[,] map)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            for (int sorIndex = 0; sorIndex < map.GetLength(0); sorIndex++)
            {
                for (int oszlopIndexe = 0; oszlopIndexe < map.GetLength(1); oszlopIndexe++)
                {
                    if (map[sorIndex, oszlopIndexe] == '.')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else if (map[sorIndex, oszlopIndexe] == '█')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.SetCursorPosition(MAP_POZICIO_X + oszlopIndexe, MAP_POZICIO_Y + sorIndex);
                    Console.WriteLine(map[sorIndex, oszlopIndexe]);
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }
        /// <summary>
        /// Nehéz térkép megjelenítése.
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        static void HardMap(char[,] map)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            for (int sorIndex = 0; sorIndex < map.GetLength(0); sorIndex++)
            {
                for (int oszlopIndexe = 0; oszlopIndexe < map.GetLength(1); oszlopIndexe++)
                {
                    Console.SetCursorPosition(MAP_POZICIO_X + oszlopIndexe, MAP_POZICIO_Y + sorIndex);
                    Console.WriteLine(map[sorIndex, oszlopIndexe]);
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }
        /// <summary>
        /// Kiírja a térkép hosszúságát és szélességét.
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        static void SizeOfTheMap(char[,] map)
        {
            Console.WriteLine($"{LangHelper.GetString("sizeOfTheMap")} {map.GetLength(0)} X {map.GetLength(1)}");
        }
        //Kötelező metódus
        /// <summary>
        /// Megadja, hogy hány termet tartamaz a térkép
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        /// <returns>Termek száma</returns>
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
        //Kötelező metódus
        /// <summary>
        /// A kapott térkép széleit végignézve megállapítja, hogy hány kijárat van.
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        /// <returns>Az alkalmas kijáratok száma</returns>
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
        private void DifficultyOpciok()
        {
            SettingsLanguage settingsLanguage = new SettingsLanguage();
            Console.ResetColor();
            Console.WriteLine(LangHelper.GetString("gameDifficulty"));
            Console.WriteLine($"{LangHelper.GetString("opcioValasztas")}");
            Console.WriteLine();
            for (int i = 0; i < difficultyOptions.Count(); i++)
            {
                if (i == kivalasztottOpcio)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($">>{difficultyOptions[i]}<<");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine($"  {difficultyOptions[i]}  ");
                }
            }
            Console.ResetColor();
        }
        private int DifficultyMenu()
        {
            ConsoleKey consoleKey;
            do
            {
                Console.Clear();
                Fomenu fomenu = new Fomenu();
                fomenu.Cim();
                DifficultyOpciok();
                consoleKey = Console.ReadKey(true).Key;

                if (consoleKey == ConsoleKey.DownArrow)
                {
                    kivalasztottOpcio++;
                    if (kivalasztottOpcio == difficultyOptions.Length)
                    {
                        kivalasztottOpcio = 0;
                    }

                }
                else if (consoleKey == ConsoleKey.UpArrow)
                {
                    kivalasztottOpcio--;
                    if (kivalasztottOpcio == -1)
                    {
                        kivalasztottOpcio = difficultyOptions.Length - 1;
                    }
                }
            } while (consoleKey != ConsoleKey.Enter);
            if (consoleKey == ConsoleKey.Enter)
            {
                switch (kivalasztottOpcio)
                {
                    case 0:
                        difficulty = "Easy";
                        Console.Clear();
                        break;
                    case 1:
                        difficulty = "Hard";
                        Console.Clear();
                        break;
                }
            }
            return kivalasztottOpcio;
        }
        /// <summary>
        /// A játék elején random elhelyezi a játékost az egyik kijárathoz.
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        public void PlacePlayerRandomly(char[,] map)
        {
            //RANDOM BEJÁRAT VÁLASZTÁS
            List<int> exitXCoordinates = new List<int>();
            List<int> exitYCoordinates = new List<int>();
            List<char> exitCharacter = new List<char>();
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
                        exitXCoordinates.Add(sorIndex);
                        exitYCoordinates.Add(0);
                        exitCharacter.Add(balOszlopChar);
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
                        exitXCoordinates.Add(sorIndex);
                        exitYCoordinates.Add(map.GetLength(1) - 1);
                        exitCharacter.Add(jobbOszlopChar);
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
                        exitXCoordinates.Add(0);
                        exitYCoordinates.Add(oszlopIndexe);
                        exitCharacter.Add(felsoSorChar);
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
                        exitXCoordinates.Add(map.GetLength(0) - 1);
                        exitYCoordinates.Add(oszlopIndexe);
                        exitCharacter.Add(alsoSorChar);
                        break;
                }
            }
            Console.BackgroundColor = ConsoleColor.Green;
            Random rnd = new Random();
            int randomExit = rnd.Next(exitXCoordinates.Count);
            Console.SetCursorPosition(MAP_POZICIO_X + exitYCoordinates[randomExit], MAP_POZICIO_Y + exitXCoordinates[randomExit]);
            Console.Write(exitCharacter[randomExit]);
            currentXCoordinate = exitXCoordinates[randomExit];
            currentYCoordinate = exitYCoordinates[randomExit];
        }
        /// <summary>
        /// A játékos és a pályaelemek vezérlése.
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        public void ManageGame(char[,] map)
        {
            ConsoleKey billentyuLeutes;
            //Kilépés
            bool quit = false;
            //Teleport
            bool teleport = false;
            //Irányok listája
            List<string> directions = new List<string>();
            //Terem koordináták hozzáadása listákhoz
            List<int> RoomsX = new List<int>();
            List<int> RoomsY = new List<int>();
            for (int sorIndex = 0; sorIndex < map.GetLength(0); sorIndex++)
            {
                for (int oszlopIndex = 0; oszlopIndex < map.GetLength(1); oszlopIndex++)
                {
                    if (map[sorIndex, oszlopIndex] == '█')
                    {
                        RoomsX.Add(oszlopIndex);
                        RoomsY.Add(sorIndex);
                    }
                }
            }

            do
            {
                //Játékos irányítása
                billentyuLeutes = Console.ReadKey(true).Key;
                switch (billentyuLeutes)
                {
                    //UP
                    case ConsoleKey.W:
                        //Kilépés a labirintusból
                        if (currentXCoordinate == 0)
                        {
                            switch (map[currentXCoordinate, currentYCoordinate])
                            {
                                case '╬':
                                case '╩':
                                case '║':
                                case '╣':
                                case '╠':
                                case '╝':
                                case '╚':
                                case '█':
                                    Console.SetCursorPosition(0, 17);
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                    Console.Write(LangHelper.GetString("quitLabyrinth"));
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    billentyuLeutes = Console.ReadKey(true).Key;

                                    if (billentyuLeutes == ConsoleKey.W)
                                    {
                                        quit = true;
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(0, 17);
                                        Console.WriteLine("                                            ");
                                        continue;
                                    }
                                    break;
                            }
                        }
                        //Mozgás
                        else if (map[currentXCoordinate - 1, currentYCoordinate] != '.' && map[currentXCoordinate - 1, currentYCoordinate] != '═' && map[currentXCoordinate - 1, currentYCoordinate] != '╩' && map[currentXCoordinate - 1, currentYCoordinate] != '╝' && map[currentXCoordinate - 1, currentYCoordinate] != '╚')
                        {
                            switch (map[currentXCoordinate, currentYCoordinate])
                            {
                                case '╬':
                                case '╩':
                                case '║':
                                case '╣':
                                case '╠':
                                case '╝':
                                case '╚':
                                case '█':
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                    currentXCoordinate--;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.BackgroundColor = ConsoleColor.Green;
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                    break;
                            }
                        }
                        break;
                    //LEFT
                    case ConsoleKey.A:
                        //Kilépés a labirintusból
                        if (currentYCoordinate == 0)
                        {
                            switch (map[currentXCoordinate, currentYCoordinate])
                            {
                                case '╬':
                                case '═':
                                case '╦':
                                case '╩':
                                case '╣':
                                case '╗':
                                case '╝':
                                case '█':
                                    Console.SetCursorPosition(0, 17);
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                    Console.Write(LangHelper.GetString("quitLabyrinth"));
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    billentyuLeutes = Console.ReadKey(true).Key;

                                    if (billentyuLeutes == ConsoleKey.A)
                                    {
                                        quit = true;
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(0, 17);
                                        Console.WriteLine("                                            ");
                                        continue;
                                    }
                                    break;
                            }
                            break;
                        }
                        //Mozgás
                        else if (map[currentXCoordinate, currentYCoordinate - 1] != '.' && map[currentXCoordinate, currentYCoordinate - 1] != '║' && map[currentXCoordinate, currentYCoordinate - 1] != '╣' && map[currentXCoordinate, currentYCoordinate - 1] != '╗' && map[currentXCoordinate, currentYCoordinate - 1] != '╝')
                        {
                            switch (map[currentXCoordinate, currentYCoordinate])
                            {
                                case '╬':
                                case '═':
                                case '╦':
                                case '╩':
                                case '╣':
                                case '╗':
                                case '╝':
                                case '█':
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                    currentYCoordinate--;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.BackgroundColor = ConsoleColor.Green;
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                    break;
                            }
                        }
                        break;
                    //DOWN
                    case ConsoleKey.S:
                        //Kilépés a labirintusból
                        if (currentXCoordinate == map.GetLength(0) - 1)
                        {
                            switch (map[currentXCoordinate, currentYCoordinate])
                            {
                                case '╬':
                                case '╦':
                                case '║':
                                case '╣':
                                case '╠':
                                case '╗':
                                case '╔':
                                case '█':
                                    Console.SetCursorPosition(0, 17);
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                    Console.Write(LangHelper.GetString("quitLabyrinth"));
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    billentyuLeutes = Console.ReadKey(true).Key;

                                    if (billentyuLeutes == ConsoleKey.S)
                                    {
                                        quit = true;
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(0, 17);
                                        Console.WriteLine("                                            ");
                                        continue;
                                    }
                                    break;
                            }
                            break;
                        }
                        //Mozgás
                        else if (map[currentXCoordinate + 1, currentYCoordinate] != '.' && map[currentXCoordinate + 1, currentYCoordinate] != '═' && map[currentXCoordinate + 1, currentYCoordinate] != '╦' && map[currentXCoordinate + 1, currentYCoordinate] != '╗' && map[currentXCoordinate + 1, currentYCoordinate] != '╔')
                        {
                            switch (map[currentXCoordinate, currentYCoordinate])
                            {
                                case '╬':
                                case '╦':
                                case '║':
                                case '╣':
                                case '╠':
                                case '╗':
                                case '╔':
                                case '█':
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                    currentXCoordinate++;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.BackgroundColor = ConsoleColor.Green;
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                    break;
                            }
                        }
                        break;
                    //RIGHT
                    case ConsoleKey.D:
                        //Kilépés a labirintusból
                        if (currentYCoordinate == map.GetLength(1) - 1)
                        {
                            switch (map[currentXCoordinate, currentYCoordinate])
                            {
                                case '╬':
                                case '═':
                                case '╦':
                                case '╩':
                                case '╠':
                                case '╚':
                                case '╔':
                                case '█':
                                    Console.SetCursorPosition(0, 17);
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                    Console.Write(LangHelper.GetString("quitLabyrinth"));
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    billentyuLeutes = Console.ReadKey(true).Key;

                                    if (billentyuLeutes == ConsoleKey.D)
                                    {
                                        quit = true;
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(0, 17);
                                        Console.WriteLine("                                            ");
                                        continue;
                                    }
                                    break;
                            }
                            break;
                        }
                        //Mozgás
                        else if (map[currentXCoordinate, currentYCoordinate + 1] != '.' && map[currentXCoordinate, currentYCoordinate + 1] != '║' && map[currentXCoordinate, currentYCoordinate + 1] != '╠' && map[currentXCoordinate, currentYCoordinate + 1] != '╚' && map[currentXCoordinate, currentYCoordinate + 1] != '╔')
                        {
                            switch (map[currentXCoordinate, currentYCoordinate])
                            {
                                case '╬':
                                case '═':
                                case '╦':
                                case '╩':
                                case '╠':
                                case '╚':
                                case '╔':
                                case '█':
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                    currentYCoordinate++;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.BackgroundColor = ConsoleColor.Green;
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                    break;
                            }
                        }
                        break;
                    //TELEPORT
                    case ConsoleKey.Spacebar:
                        teleport = true;
                        //Szín váltás
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                        Console.Write(map[currentXCoordinate, currentYCoordinate]);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.SetCursorPosition(0, 19);
                        Console.Write(LangHelper.GetString("ghostMode"));
                        //MOZGÁS SZELLEM MÓDBAN
                        do
                        {
                            billentyuLeutes = Console.ReadKey(true).Key;
                            //FEL
                            if (billentyuLeutes == ConsoleKey.W && currentXCoordinate != 0)
                            {
                                if (map[currentXCoordinate, currentYCoordinate] == '.')
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                    currentXCoordinate--;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.BackgroundColor = ConsoleColor.Yellow;
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                    currentXCoordinate--;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.BackgroundColor = ConsoleColor.Yellow;
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                }
                            }
                            //BAL
                            if (billentyuLeutes == ConsoleKey.A && currentYCoordinate != 0)
                            {
                                if (map[currentXCoordinate, currentYCoordinate] == '.')
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                    currentYCoordinate--;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.BackgroundColor = ConsoleColor.Yellow;
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                    currentYCoordinate--;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.BackgroundColor = ConsoleColor.Yellow;
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                }
                            }
                            //LE
                            if (billentyuLeutes == ConsoleKey.S && currentXCoordinate != map.GetLength(0) - 1)
                            {
                                if (map[currentXCoordinate, currentYCoordinate] == '.')
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                    currentXCoordinate++;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.BackgroundColor = ConsoleColor.Yellow;
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                    currentXCoordinate++;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.BackgroundColor = ConsoleColor.Yellow;
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                }
                            }
                            //JOBB
                            if (billentyuLeutes == ConsoleKey.D && currentYCoordinate != map.GetLength(1) - 1)
                            {
                                if (map[currentXCoordinate, currentYCoordinate] == '.')
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                    currentYCoordinate++;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.BackgroundColor = ConsoleColor.Yellow;
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                    currentYCoordinate++;
                                    Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.BackgroundColor = ConsoleColor.Yellow;
                                    Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                }
                            }
                            if (billentyuLeutes == ConsoleKey.Spacebar)
                            {
                                teleport = false;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.SetCursorPosition(MAP_POZICIO_X + currentYCoordinate, MAP_POZICIO_Y + currentXCoordinate);
                                Console.Write(map[currentXCoordinate, currentYCoordinate]);
                                Console.SetCursorPosition(0, 19);
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.Write("                                      ");
                            }
                        } while (teleport == true);
                        break;
                }
                //Továbblépési lehetőségek
                switch (map[currentXCoordinate, currentYCoordinate])
                {
                    case '╬':
                        //ÉSZAK
                        if (currentXCoordinate == 0)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate - 1, currentYCoordinate] == '╬' || map[currentXCoordinate - 1, currentYCoordinate] == '╦' || map[currentXCoordinate - 1, currentYCoordinate] == '║' || map[currentXCoordinate - 1, currentYCoordinate] == '╣' || map[currentXCoordinate - 1, currentYCoordinate] == '╠' || map[currentXCoordinate - 1, currentYCoordinate] == '╗' || map[currentXCoordinate - 1, currentYCoordinate] == '╔')
                        {
                            directions.Add(LangHelper.GetString("north"));
                        }
                        else if (map[currentXCoordinate - 1, currentYCoordinate] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //NYUGAT
                        if (currentYCoordinate == 0)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate - 1] == '╬' || map[currentXCoordinate, currentYCoordinate - 1] == '═' || map[currentXCoordinate, currentYCoordinate - 1] == '╦' || map[currentXCoordinate, currentYCoordinate - 1] == '╩' || map[currentXCoordinate, currentYCoordinate - 1] == '╠' || map[currentXCoordinate, currentYCoordinate - 1] == '╚' || map[currentXCoordinate, currentYCoordinate - 1] == '╔')
                        {
                            directions.Add(LangHelper.GetString("west"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate - 1] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //DÉL
                        if (currentXCoordinate == map.GetLength(0) - 1)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate + 1, currentYCoordinate] == '╬' || map[currentXCoordinate + 1, currentYCoordinate] == '╩' || map[currentXCoordinate + 1, currentYCoordinate] == '║' || map[currentXCoordinate + 1, currentYCoordinate] == '╣' || map[currentXCoordinate + 1, currentYCoordinate] == '╠' || map[currentXCoordinate + 1, currentYCoordinate] == '╝' || map[currentXCoordinate + 1, currentYCoordinate] == '╚')
                        {
                            directions.Add(LangHelper.GetString("south"));
                        }
                        else if (map[currentXCoordinate + 1, currentYCoordinate] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //KELET
                        if (currentYCoordinate == map.GetLength(1) - 1)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate + 1] == '╬' || map[currentXCoordinate, currentYCoordinate + 1] == '═' || map[currentXCoordinate, currentYCoordinate + 1] == '╦' || map[currentXCoordinate, currentYCoordinate + 1] == '╩' || map[currentXCoordinate, currentYCoordinate + 1] == '╣' || map[currentXCoordinate, currentYCoordinate + 1] == '╗' || map[currentXCoordinate, currentYCoordinate + 1] == '╝')
                        {
                            directions.Add(LangHelper.GetString("east"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate + 1] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        break;
                    case '█':
                        //ÉSZAK
                        if (currentXCoordinate == 0)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate - 1, currentYCoordinate] == '╬' || map[currentXCoordinate - 1, currentYCoordinate] == '╦' || map[currentXCoordinate - 1, currentYCoordinate] == '║' || map[currentXCoordinate - 1, currentYCoordinate] == '╣' || map[currentXCoordinate - 1, currentYCoordinate] == '╠' || map[currentXCoordinate - 1, currentYCoordinate] == '╗' || map[currentXCoordinate - 1, currentYCoordinate] == '╔')
                        {
                            directions.Add(LangHelper.GetString("north"));
                        }
                        else if (map[currentXCoordinate - 1, currentYCoordinate] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //NYUGAT
                        if (currentYCoordinate == 0)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate - 1] == '╬' || map[currentXCoordinate, currentYCoordinate - 1] == '═' || map[currentXCoordinate, currentYCoordinate - 1] == '╦' || map[currentXCoordinate, currentYCoordinate - 1] == '╩' || map[currentXCoordinate, currentYCoordinate - 1] == '╠' || map[currentXCoordinate, currentYCoordinate - 1] == '╚' || map[currentXCoordinate, currentYCoordinate - 1] == '╔')
                        {
                            directions.Add(LangHelper.GetString("west"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate - 1] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //DÉL
                        if (currentXCoordinate == map.GetLength(0) - 1)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate + 1, currentYCoordinate] == '╬' || map[currentXCoordinate + 1, currentYCoordinate] == '╩' || map[currentXCoordinate + 1, currentYCoordinate] == '║' || map[currentXCoordinate + 1, currentYCoordinate] == '╣' || map[currentXCoordinate + 1, currentYCoordinate] == '╠' || map[currentXCoordinate + 1, currentYCoordinate] == '╝' || map[currentXCoordinate + 1, currentYCoordinate] == '╚')
                        {
                            directions.Add(LangHelper.GetString("south"));
                        }
                        else if (map[currentXCoordinate + 1, currentYCoordinate] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //KELET
                        if (currentYCoordinate == map.GetLength(1) - 1)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate + 1] == '╬' || map[currentXCoordinate, currentYCoordinate + 1] == '═' || map[currentXCoordinate, currentYCoordinate + 1] == '╦' || map[currentXCoordinate, currentYCoordinate + 1] == '╩' || map[currentXCoordinate, currentYCoordinate + 1] == '╣' || map[currentXCoordinate, currentYCoordinate + 1] == '╗' || map[currentXCoordinate, currentYCoordinate + 1] == '╝')
                        {
                            directions.Add(LangHelper.GetString("east"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate + 1] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        break;
                    case '═':
                        //NYUGAT
                        if (currentYCoordinate == 0)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate - 1] == '╬' || map[currentXCoordinate, currentYCoordinate - 1] == '═' || map[currentXCoordinate, currentYCoordinate - 1] == '╦' || map[currentXCoordinate, currentYCoordinate - 1] == '╩' || map[currentXCoordinate, currentYCoordinate - 1] == '╠' || map[currentXCoordinate, currentYCoordinate - 1] == '╚' || map[currentXCoordinate, currentYCoordinate - 1] == '╔')
                        {
                            directions.Add(LangHelper.GetString("west"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate - 1] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //KELET
                        if (currentYCoordinate == map.GetLength(1) - 1)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate + 1] == '╬' || map[currentXCoordinate, currentYCoordinate + 1] == '═' || map[currentXCoordinate, currentYCoordinate + 1] == '╦' || map[currentXCoordinate, currentYCoordinate + 1] == '╩' || map[currentXCoordinate, currentYCoordinate + 1] == '╣' || map[currentXCoordinate, currentYCoordinate + 1] == '╗' || map[currentXCoordinate, currentYCoordinate + 1] == '╝')
                        {
                            directions.Add(LangHelper.GetString("east"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate + 1] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        break;
                    case '╦':
                        //NYUGAT
                        if (currentYCoordinate == 0)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate - 1] == '╬' || map[currentXCoordinate, currentYCoordinate - 1] == '═' || map[currentXCoordinate, currentYCoordinate - 1] == '╦' || map[currentXCoordinate, currentYCoordinate - 1] == '╩' || map[currentXCoordinate, currentYCoordinate - 1] == '╠' || map[currentXCoordinate, currentYCoordinate - 1] == '╚' || map[currentXCoordinate, currentYCoordinate - 1] == '╔')
                        {
                            directions.Add(LangHelper.GetString("west"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate - 1] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //DÉL
                        if (currentXCoordinate == map.GetLength(0) - 1)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate + 1, currentYCoordinate] == '╬' || map[currentXCoordinate + 1, currentYCoordinate] == '╩' || map[currentXCoordinate + 1, currentYCoordinate] == '║' || map[currentXCoordinate + 1, currentYCoordinate] == '╣' || map[currentXCoordinate + 1, currentYCoordinate] == '╠' || map[currentXCoordinate + 1, currentYCoordinate] == '╝' || map[currentXCoordinate + 1, currentYCoordinate] == '╚')
                        {
                            directions.Add(LangHelper.GetString("south"));
                        }
                        else if (map[currentXCoordinate + 1, currentYCoordinate] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //KELET
                        if (currentYCoordinate == map.GetLength(1) - 1)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate + 1] == '╬' || map[currentXCoordinate, currentYCoordinate + 1] == '═' || map[currentXCoordinate, currentYCoordinate + 1] == '╦' || map[currentXCoordinate, currentYCoordinate + 1] == '╩' || map[currentXCoordinate, currentYCoordinate + 1] == '╣' || map[currentXCoordinate, currentYCoordinate + 1] == '╗' || map[currentXCoordinate, currentYCoordinate + 1] == '╝')
                        {
                            directions.Add(LangHelper.GetString("east"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate + 1] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        break;
                    case '╩':
                        //ÉSZAK
                        if (currentXCoordinate == 0)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate - 1, currentYCoordinate] == '╬' || map[currentXCoordinate - 1, currentYCoordinate] == '╦' || map[currentXCoordinate - 1, currentYCoordinate] == '║' || map[currentXCoordinate - 1, currentYCoordinate] == '╣' || map[currentXCoordinate - 1, currentYCoordinate] == '╠' || map[currentXCoordinate - 1, currentYCoordinate] == '╗' || map[currentXCoordinate - 1, currentYCoordinate] == '╔')
                        {
                            directions.Add(LangHelper.GetString("north"));
                        }
                        else if (map[currentXCoordinate - 1, currentYCoordinate] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //NYUGAT
                        if (currentYCoordinate == 0)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate - 1] == '╬' || map[currentXCoordinate, currentYCoordinate - 1] == '═' || map[currentXCoordinate, currentYCoordinate - 1] == '╦' || map[currentXCoordinate, currentYCoordinate - 1] == '╩' || map[currentXCoordinate, currentYCoordinate - 1] == '╠' || map[currentXCoordinate, currentYCoordinate - 1] == '╚' || map[currentXCoordinate, currentYCoordinate - 1] == '╔')
                        {
                            directions.Add(LangHelper.GetString("west"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate - 1] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //KELET
                        if (currentYCoordinate == map.GetLength(1) - 1)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate + 1] == '╬' || map[currentXCoordinate, currentYCoordinate + 1] == '═' || map[currentXCoordinate, currentYCoordinate + 1] == '╦' || map[currentXCoordinate, currentYCoordinate + 1] == '╩' || map[currentXCoordinate, currentYCoordinate + 1] == '╣' || map[currentXCoordinate, currentYCoordinate + 1] == '╗' || map[currentXCoordinate, currentYCoordinate + 1] == '╝')
                        {
                            directions.Add(LangHelper.GetString("east"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate + 1] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        break;
                    case '║':
                        //ÉSZAK
                        if (currentXCoordinate == 0)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate - 1, currentYCoordinate] == '╬' || map[currentXCoordinate - 1, currentYCoordinate] == '╦' || map[currentXCoordinate - 1, currentYCoordinate] == '║' || map[currentXCoordinate - 1, currentYCoordinate] == '╣' || map[currentXCoordinate - 1, currentYCoordinate] == '╠' || map[currentXCoordinate - 1, currentYCoordinate] == '╗' || map[currentXCoordinate - 1, currentYCoordinate] == '╔')
                        {
                            directions.Add(LangHelper.GetString("north"));
                        }
                        else if (map[currentXCoordinate - 1, currentYCoordinate] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //DÉL
                        if (currentXCoordinate == map.GetLength(0) - 1)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate + 1, currentYCoordinate] == '╬' || map[currentXCoordinate + 1, currentYCoordinate] == '╩' || map[currentXCoordinate + 1, currentYCoordinate] == '║' || map[currentXCoordinate + 1, currentYCoordinate] == '╣' || map[currentXCoordinate + 1, currentYCoordinate] == '╠' || map[currentXCoordinate + 1, currentYCoordinate] == '╝' || map[currentXCoordinate + 1, currentYCoordinate] == '╚')
                        {
                            directions.Add(LangHelper.GetString("south"));
                        }
                        else if (map[currentXCoordinate + 1, currentYCoordinate] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        break;
                    case '╣':
                        //ÉSZAK
                        if (currentXCoordinate == 0)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate - 1, currentYCoordinate] == '╬' || map[currentXCoordinate - 1, currentYCoordinate] == '╦' || map[currentXCoordinate - 1, currentYCoordinate] == '║' || map[currentXCoordinate - 1, currentYCoordinate] == '╣' || map[currentXCoordinate - 1, currentYCoordinate] == '╠' || map[currentXCoordinate - 1, currentYCoordinate] == '╗' || map[currentXCoordinate - 1, currentYCoordinate] == '╔')
                        {
                            directions.Add(LangHelper.GetString("north"));
                        }
                        else if (map[currentXCoordinate - 1, currentYCoordinate] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //NYUGAT
                        if (currentYCoordinate == 0)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate - 1] == '╬' || map[currentXCoordinate, currentYCoordinate - 1] == '═' || map[currentXCoordinate, currentYCoordinate - 1] == '╦' || map[currentXCoordinate, currentYCoordinate - 1] == '╩' || map[currentXCoordinate, currentYCoordinate - 1] == '╠' || map[currentXCoordinate, currentYCoordinate - 1] == '╚' || map[currentXCoordinate, currentYCoordinate - 1] == '╔')
                        {
                            directions.Add(LangHelper.GetString("west"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate - 1] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //DÉL
                        if (currentXCoordinate == map.GetLength(0) - 1)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate + 1, currentYCoordinate] == '╬' || map[currentXCoordinate + 1, currentYCoordinate] == '╩' || map[currentXCoordinate + 1, currentYCoordinate] == '║' || map[currentXCoordinate + 1, currentYCoordinate] == '╣' || map[currentXCoordinate + 1, currentYCoordinate] == '╠' || map[currentXCoordinate + 1, currentYCoordinate] == '╝' || map[currentXCoordinate + 1, currentYCoordinate] == '╚')
                        {
                            directions.Add(LangHelper.GetString("south"));
                        }
                        else if (map[currentXCoordinate + 1, currentYCoordinate] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        break;
                    case '╠':
                        //ÉSZAK
                        if (currentXCoordinate == 0)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate - 1, currentYCoordinate] == '╬' || map[currentXCoordinate - 1, currentYCoordinate] == '╦' || map[currentXCoordinate - 1, currentYCoordinate] == '║' || map[currentXCoordinate - 1, currentYCoordinate] == '╣' || map[currentXCoordinate - 1, currentYCoordinate] == '╠' || map[currentXCoordinate - 1, currentYCoordinate] == '╗' || map[currentXCoordinate - 1, currentYCoordinate] == '╔')
                        {
                            directions.Add(LangHelper.GetString("north"));
                        }
                        else if (map[currentXCoordinate - 1, currentYCoordinate] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //DÉL
                        if (currentXCoordinate == map.GetLength(0) - 1)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate + 1, currentYCoordinate] == '╬' || map[currentXCoordinate + 1, currentYCoordinate] == '╩' || map[currentXCoordinate + 1, currentYCoordinate] == '║' || map[currentXCoordinate + 1, currentYCoordinate] == '╣' || map[currentXCoordinate + 1, currentYCoordinate] == '╠' || map[currentXCoordinate + 1, currentYCoordinate] == '╝' || map[currentXCoordinate + 1, currentYCoordinate] == '╚')
                        {
                            directions.Add(LangHelper.GetString("south"));
                        }
                        else if (map[currentXCoordinate + 1, currentYCoordinate] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //KELET
                        if (currentYCoordinate == map.GetLength(1) - 1)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate + 1] == '╬' || map[currentXCoordinate, currentYCoordinate + 1] == '═' || map[currentXCoordinate, currentYCoordinate + 1] == '╦' || map[currentXCoordinate, currentYCoordinate + 1] == '╩' || map[currentXCoordinate, currentYCoordinate + 1] == '╣' || map[currentXCoordinate, currentYCoordinate + 1] == '╗' || map[currentXCoordinate, currentYCoordinate + 1] == '╝')
                        {
                            directions.Add(LangHelper.GetString("east"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate + 1] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        break;
                    case '╗':
                        //NYUGAT
                        if (currentYCoordinate == 0)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate - 1] == '╬' || map[currentXCoordinate, currentYCoordinate - 1] == '═' || map[currentXCoordinate, currentYCoordinate - 1] == '╦' || map[currentXCoordinate, currentYCoordinate - 1] == '╩' || map[currentXCoordinate, currentYCoordinate - 1] == '╠' || map[currentXCoordinate, currentYCoordinate - 1] == '╚' || map[currentXCoordinate, currentYCoordinate - 1] == '╔')
                        {
                            directions.Add(LangHelper.GetString("west"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate - 1] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //DÉL
                        if (currentXCoordinate == map.GetLength(0) - 1)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate + 1, currentYCoordinate] == '╬' || map[currentXCoordinate + 1, currentYCoordinate] == '╩' || map[currentXCoordinate + 1, currentYCoordinate] == '║' || map[currentXCoordinate + 1, currentYCoordinate] == '╣' || map[currentXCoordinate + 1, currentYCoordinate] == '╠' || map[currentXCoordinate + 1, currentYCoordinate] == '╝' || map[currentXCoordinate + 1, currentYCoordinate] == '╚')
                        {
                            directions.Add(LangHelper.GetString("south"));
                        }
                        else if (map[currentXCoordinate + 1, currentYCoordinate] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        break;
                    case '╝':
                        //ÉSZAK
                        if (currentXCoordinate == 0)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate - 1, currentYCoordinate] == '╬' || map[currentXCoordinate - 1, currentYCoordinate] == '╦' || map[currentXCoordinate - 1, currentYCoordinate] == '║' || map[currentXCoordinate - 1, currentYCoordinate] == '╣' || map[currentXCoordinate - 1, currentYCoordinate] == '╠' || map[currentXCoordinate - 1, currentYCoordinate] == '╗' || map[currentXCoordinate - 1, currentYCoordinate] == '╔')
                        {
                            directions.Add(LangHelper.GetString("north"));
                        }
                        else if (map[currentXCoordinate - 1, currentYCoordinate] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //NYUGAT
                        if (currentYCoordinate == 0)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate - 1] == '╬' || map[currentXCoordinate, currentYCoordinate - 1] == '═' || map[currentXCoordinate, currentYCoordinate - 1] == '╦' || map[currentXCoordinate, currentYCoordinate - 1] == '╩' || map[currentXCoordinate, currentYCoordinate - 1] == '╠' || map[currentXCoordinate, currentYCoordinate - 1] == '╚' || map[currentXCoordinate, currentYCoordinate - 1] == '╔')
                        {
                            directions.Add(LangHelper.GetString("west"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate - 1] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        break;
                    case '╚':
                        //ÉSZAK
                        if (currentXCoordinate == 0)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate - 1, currentYCoordinate] == '╬' || map[currentXCoordinate - 1, currentYCoordinate] == '╦' || map[currentXCoordinate - 1, currentYCoordinate] == '║' || map[currentXCoordinate - 1, currentYCoordinate] == '╣' || map[currentXCoordinate - 1, currentYCoordinate] == '╠' || map[currentXCoordinate - 1, currentYCoordinate] == '╗' || map[currentXCoordinate - 1, currentYCoordinate] == '╔')
                        {
                            directions.Add(LangHelper.GetString("north"));
                        }
                        else if (map[currentXCoordinate - 1, currentYCoordinate] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //KELET
                        if (currentYCoordinate == map.GetLength(1) - 1)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate + 1] == '╬' || map[currentXCoordinate, currentYCoordinate + 1] == '═' || map[currentXCoordinate, currentYCoordinate + 1] == '╦' || map[currentXCoordinate, currentYCoordinate + 1] == '╩' || map[currentXCoordinate, currentYCoordinate + 1] == '╣' || map[currentXCoordinate, currentYCoordinate + 1] == '╗' || map[currentXCoordinate, currentYCoordinate + 1] == '╝')
                        {
                            directions.Add(LangHelper.GetString("east"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate + 1] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        break;
                    case '╔':
                        //DÉL
                        if (currentXCoordinate == map.GetLength(0) - 1)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate + 1, currentYCoordinate] == '╬' || map[currentXCoordinate + 1, currentYCoordinate] == '╩' || map[currentXCoordinate + 1, currentYCoordinate] == '║' || map[currentXCoordinate + 1, currentYCoordinate] == '╣' || map[currentXCoordinate + 1, currentYCoordinate] == '╠' || map[currentXCoordinate + 1, currentYCoordinate] == '╝' || map[currentXCoordinate + 1, currentYCoordinate] == '╚')
                        {
                            directions.Add(LangHelper.GetString("south"));
                        }
                        else if (map[currentXCoordinate + 1, currentYCoordinate] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        //KELET
                        if (currentYCoordinate == map.GetLength(1) - 1)
                        {
                            directions.Add(LangHelper.GetString("exitGate"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate + 1] == '╬' || map[currentXCoordinate, currentYCoordinate + 1] == '═' || map[currentXCoordinate, currentYCoordinate + 1] == '╦' || map[currentXCoordinate, currentYCoordinate + 1] == '╩' || map[currentXCoordinate, currentYCoordinate + 1] == '╣' || map[currentXCoordinate, currentYCoordinate + 1] == '╗' || map[currentXCoordinate, currentYCoordinate + 1] == '╝')
                        {
                            directions.Add(LangHelper.GetString("east"));
                        }
                        else if (map[currentXCoordinate, currentYCoordinate + 1] == '█')
                        {
                            directions.Add(LangHelper.GetString("treasureRoom"));
                        }
                        break;
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition(0, 15);
                Console.Write(LangHelper.GetString("possibleDirections"));
                Console.SetCursorPosition(0, 16);
                Console.Write("                                                          ");
                Console.SetCursorPosition(0, 16);
                Console.Write(String.Join(" ", directions));
                directions.Clear();
                Console.ResetColor();

                //Termek felfedezése és kiírása
                if (map[currentXCoordinate, currentYCoordinate] == '█')
                {
                    for (int index = 0; index < RoomsX.Count; index++)
                    {
                        if (currentXCoordinate == RoomsY[index] && currentYCoordinate == RoomsX[index])
                        {
                            exploredRooms++;
                            RoomsX.RemoveAt(index);
                            RoomsY.RemoveAt(index);
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.SetCursorPosition(0, 13);
                            Console.Write(exploredRooms);
                            Console.ResetColor();
                        }
                    }
                }

                if (map[currentXCoordinate, currentYCoordinate] == '.' && teleport == false)
                {
                    quit = true;
                }
            } while (quit == false);
        }

        //JÁTÉK INDULÁS
        public void NewGame()
        {
            //PÁLYA BEKÉRÉSE
            Console.CursorVisible = true;
            Fomenu fomenu = new Fomenu();
            fomenu.Cim();
            Console.ResetColor();
            Console.WriteLine(LangHelper.GetString("path"));
            mapEleresiUtja = Console.ReadLine();
            
            do
            {
                if (!File.Exists(mapEleresiUtja))
                {
                    Console.Write(LangHelper.GetString("fileExists"));
                    mapEleresiUtja = Console.ReadLine();
                }
            } while (!File.Exists(mapEleresiUtja));
            string fileNev = Path.GetFileName(mapEleresiUtja);
            Console.Clear();
            //Nehézség, játékmód
            Console.CursorVisible = false;
            DifficultyMenu();
            //PÁLYA MEGJELENÍTÉSE
            fomenu.Cim();
            //Pálya neve
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{LangHelper.GetString("mapName")}{fileNev}");
            //Pálya mérete
            SizeOfTheMap(BetoltTerkepet(mapEleresiUtja));
            //Termek száma
            Play player = new Play();
            Console.WriteLine(LangHelper.GetString("roomNumber"));
            Console.SetCursorPosition(0, 13);
            Console.WriteLine($"0 / {GetRoomNumber(BetoltTerkepet(mapEleresiUtja))}");
            //Kijáratok száma
            Console.WriteLine($"{LangHelper.GetString("exitNumber")}{GetSuitableEntrance(BetoltTerkepet(mapEleresiUtja))}");
            //Pálya betöltése nehézség szerint
            if (difficulty == "Easy")
            {
                EasyMap(BetoltTerkepet(mapEleresiUtja));
            }
            else if (difficulty == "Hard")
            {
                HardMap(BetoltTerkepet(mapEleresiUtja));
            }
            //Játékos elhelyezése és mozgatása
            player.PlacePlayerRandomly(BetoltTerkepet(mapEleresiUtja));
            player.ManageGame(BetoltTerkepet(mapEleresiUtja));
            //Játék befejezése
            Endings endings = new Endings();
            if (player.exploredRooms == GetRoomNumber(BetoltTerkepet(mapEleresiUtja)) && BetoltTerkepet(mapEleresiUtja)[currentXCoordinate, currentYCoordinate] != '.')
            {
                endings.Win();
            }
            else if (player.exploredRooms < GetRoomNumber(BetoltTerkepet(mapEleresiUtja)) && BetoltTerkepet(mapEleresiUtja)[currentXCoordinate, currentYCoordinate] != '.')
            {
                endings.LoseIfNotExploredRooms();
            }
            else if (BetoltTerkepet(mapEleresiUtja)[currentXCoordinate, currentYCoordinate] == '.')
            {
                endings.LoseIfStuckInWall();
            }
        }
    }
}