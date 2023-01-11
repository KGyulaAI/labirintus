using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Labyrinth
{
    static class MapPosition
    {
        public static int MAP_POZICIO_X = Console.WindowWidth / 2;
        public static int MAP_POZICIO_Y = Console.WindowHeight / 2;
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;
            Console.Title = "Labyrinth";
            Console.CursorVisible = false;
            Fomenu fomenu = new Fomenu();
            fomenu.MainMenu();
        }
    }
}