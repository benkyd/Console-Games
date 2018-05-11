using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Games.Ascii_Showdown {
    class AsciiShowdown_Draw {
        public int width { get; set; }
        public int height { get; set; }
        public int indent { get; set; }

        public void drawGrid(char[,] grid, int indent) {
            try {
                Console.SetCursorPosition(indent, indent - 3);

                for (int j = 0; j < height; j++) {
                    for (int i = 0; i < width; i++) {
                        if (grid[i, j] == '☺') {
                            Console.ForegroundColor = ConsoleColor.Green;
                        } else if (grid[i, j] == '☻') {
                            Console.ForegroundColor = ConsoleColor.Red;
                        } else {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.Write(grid[i, j]);
                    }
                    Console.WriteLine();
                    Console.SetCursorPosition(indent, j + indent - 2);
                }
            } catch { }
        }
        public void drawStats(int chealth, int uhealth, int bullets, int walls, bool boost) {
            try {
                int drawX = width + indent + 10;

                Console.SetCursorPosition(drawX, 6);
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine("╔════════════════╗");
                Console.SetCursorPosition(drawX, 7);
                if (chealth > 9) {
                    Console.WriteLine("║CPU healh: {0}   ║", chealth);
                } else {
                    Console.WriteLine("║CPU healh: {0}    ║", chealth);
                }
                Console.SetCursorPosition(drawX, 8);
                Console.WriteLine("╚════════════════╝");

                Console.SetCursorPosition(drawX, 14);
                Console.WriteLine("╔════════════════╗");
                Console.SetCursorPosition(drawX, 15);
                if (uhealth > 9) {
                    Console.WriteLine("║Health:    {0}   ║", uhealth);
                } else {
                    Console.WriteLine("║Health:    {0}    ║", uhealth);
                }
                Console.SetCursorPosition(drawX, 16);
                if (bullets > 99) {
                    Console.WriteLine("║Bullets:   {0}  ║", bullets);
                } else if (bullets > 9) {
                    Console.WriteLine("║Bullets:   {0}   ║", bullets);
                } else {
                    Console.WriteLine("║Bullets:   {0}    ║", bullets);
                }
                Console.SetCursorPosition(drawX, 17);
                if (walls > 9) {
                    Console.WriteLine("║Walls:     {0}   ║", walls);
                } else {
                    Console.WriteLine("║Walls:     {0}    ║", walls);
                }
                Console.SetCursorPosition(drawX, 18);
                if (boost) {
                    Console.Write("║Boost:     ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("ON");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("   ║");
                    Console.WriteLine();
                } else {
                    Console.Write("║Boost:     ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("OFF");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("  ║");
                    Console.WriteLine();
                }
                Console.SetCursorPosition(drawX, 19);
                Console.WriteLine("╚════════════════╝");
            } catch { }
        }

        public void updateGrid(char[,] grid, char[,] oldGrid, int indent) {
            try {
                for (int j = 0; j < height; j++) {
                    for (int i = 0; i < width; i++) {
                        if (grid[i, j] != oldGrid[i, j]) {
                            if (grid[i, j] == '☺') {
                                Console.ForegroundColor = ConsoleColor.Green;
                            } else if (grid[i, j] == '☻') {
                                Console.ForegroundColor = ConsoleColor.Red;
                            } else {
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            Console.SetCursorPosition(i + indent, j + indent - 3);
                            Console.Write(grid[i, j]);
                        }
                    }
                }
            } catch { }
        }

        public void updateStats(int chealth, int uhealth, int bullets, int walls, bool boost) {
            int drawX = width + indent + 10;

        }
    }
}
