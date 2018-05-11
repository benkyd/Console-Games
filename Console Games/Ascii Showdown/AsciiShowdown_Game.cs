using System;
using System.Collections.Generic;
using System.Threading;

namespace Console_Games {
    class AsciiShowdown_Game {
        //Dictionary<Point, char> dic = new Dictionary<Point, char>();

        Random rnd = new Random();
        Ascii_Showdown.AsciiShowdown_Draw draw = new Ascii_Showdown.AsciiShowdown_Draw();
        Ascii_Showdown.AsciiShowdown_AI ai = new Ascii_Showdown.AsciiShowdown_AI();

        int uhealth = 10, chealth = 30, bullets = 500, walls = 20;
        bool boost = false;
        int boostime = 100, cooldown = 200, boostimeon = 0, cooldowntime = 200;

        int width = 40;
        int height = 20;
        int indent = 5;

        int uX;
        int uY;

        public int cX;
        public int cY;

        bool HAULT = false;
        int WINNER = 0;
        char[,] grid;
        char[,] oldGrid;
        static readonly string defaultGrid =
                       "████████████████████████████████████████" + // █: wall
                       "█          ☻                           █" + // ☻: cpu
                       "█                                      █" + // ☺: user
                       "█                                      █" + // ▲: user's bullet
                       "█                                      █" + // ▼: cpu's bullet
                       "█                                      █" + //boomerang(array): boomerang
                       "█                                      █" + //wall(array): user's wall
                       "█                                      █" +
                       "█                                      █" +
                       "█                                      █" +
                       "█                                      █" +
                       "█                                      █" +
                       "█                                      █" +
                       "█                                      █" +
                       "█                                      █" +
                       "█                                      █" +
                       "█                                      █" +
                       "█                                      █" +
                       "█                         ☺            █" +
                       "████████████████████████████████████████";


        public void initializeGame() {
            Console.Clear();
            Console.SetWindowSize(80, 30);
            HAULT = false;

            dispose();

            draw.width = width;
            draw.height = height;
            draw.indent = indent;

            grid = new char[width, height];

            bool boost = false;

            grid = stringToArray(defaultGrid);

            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (grid[i, j] == '☺') {
                        uX = i;
                        uY = j;
                    } else if (grid[i, j] == '☻') {
                        cX = i;
                        cY = j;
                    }
                }
            }

            Console.WriteLine("The game has successfully loaded!");
            Console.WriteLine("To play:");
            Console.WriteLine("W or up arrow to shoot");
            Console.WriteLine("A or left arrow to move left");
            Console.WriteLine("D or right arrow to move right");
            Console.WriteLine("O to use your boost powerup this will only last for 5 seconds with a 10 second cooldown");
            Console.WriteLine("P to place a wall this will only be able to survive 4 enemy or freindly shots before breaking");
            Console.WriteLine("Press Q to Quit at any time");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();

            draw.drawGrid(grid, indent);
            draw.drawStats(chealth, uhealth, bullets, walls, boost);
            gameLoop();
        }

        private void gameLoop() {
            while (WINNER == 0 && HAULT == false) {
                oldGrid = getOldGrid(grid);

                if (boost) {
                    boostimeon++;
                    if (boostimeon == boostime) {
                        boost = false;
                        boostimeon = 0;
                        cooldown = 0;
                    }
                } else if (cooldown < cooldowntime) {
                    cooldown++;
                }

                List<ConsoleKeyInfo> keys = new List<ConsoleKeyInfo>();
                while (Console.KeyAvailable) {
                    var key = Console.ReadKey(true);
                    keys.Add(key);
                }


                moveColideandStuff();

                ai.cX = cX;
                ai.cY = cY;
                grid = ai.moveCPU(grid, width, uX, uY, boost);
                cX = ai.cX;
                cY = ai.cY;

                keyPressed(keys);

                //game logic calls

                if (bullets == 0 || uhealth == 0) {
                    WINNER = 2;
                    break;
                } else if (chealth == 0) {
                    WINNER = 1;
                    break;
                }

                CheckAndResetWindowSize();
                draw.updateGrid(grid, oldGrid, indent);
                draw.drawStats(chealth, uhealth, bullets, walls, boost);
                Thread.Sleep(50);
            }

            if (WINNER == 1) {
                Console.Clear();
                Console.WriteLine("You win!");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                initializeGame();
            } else if (WINNER == 2) {
                Console.Clear();
                Console.WriteLine("You loose :(");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                initializeGame();
            }

            dispose();
            gameLoop();
        }

        private void keyPressed(List<ConsoleKeyInfo> keys) {
            try {
                if (keys.Count >= 1) {
                    var key = keys[keys.Count - 1];

                    switch (key.KeyChar) {
                        case 'W':
                        case 'w':
                            if (boost) {
                                grid[uX - 1, uY - 1] = '▲';
                                grid[uX, uY - 1] = '▲';
                                grid[uX + 1, uY - 1] = '▲';
                                bullets += -3;
                            } else {
                                grid[uX, uY - 1] = '▲';
                                bullets--;
                            }
                            break;
                        case 'D':
                        case 'd':
                            if (uX + 1 < width - 1) {
                                grid[uX, uY] = ' ';
                                grid[uX + 1, uY] = '☺';
                                uX++;
                            }
                            break;
                        case 'A':
                        case 'a':
                            if (uX - 1 > 0) {
                                grid[uX, uY] = ' ';
                                grid[uX - 1, uY] = '☺';
                                uX--;
                            }
                            break;
                        case 'P':
                        case 'p':
                            if (walls > 0 && grid[uX, uY - 2] != '█') {
                                grid[uX, uY - 2] = '█';
                                walls--;
                            }
                            break;
                        case 'O':
                        case 'o':
                            if (boost) {
                                boost = false;
                                boostimeon = 0;
                            } else if (!boost && cooldown == cooldowntime) {
                                cooldown = 0;

                                boost = true;
                            }
                            break;
                        case 'Q':
                        case 'q':
                            dispose();
                            HAULT = true;
                            UserInterface ui = new UserInterface();
                            ui.programStart();
                            break;
                    }
                    switch (key.Key) {
                        case ConsoleKey.UpArrow:
                            if (boost) {
                                grid[uX - 1, uY - 1] = '▲';
                                grid[uX, uY - 1] = '▲';
                                grid[uX + 1, uY - 1] = '▲';
                                bullets += -3;
                            } else {
                                grid[uX, uY - 1] = '▲';
                                bullets--;
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            if (uX + 1 < width - 1) {
                                grid[uX, uY] = ' ';
                                grid[uX + 1, uY] = '☺';
                                uX++;
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            if (uX - 1 > 0) {
                                grid[uX, uY] = ' ';
                                grid[uX - 1, uY] = '☺';
                                uX--;
                            }
                            break;
                        case ConsoleKey.Spacebar:
                            if (boost) {
                                grid[uX - 1, uY - 1] = '▲';
                                grid[uX, uY - 1] = '▲';
                                grid[uX + 1, uY - 1] = '▲';
                                bullets += -3;
                            } else {
                                grid[uX, uY - 1] = '▲';
                                bullets--;
                            }
                            break;
                    }
                }
            } catch { }
        }

        private void moveColideandStuff() {
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (grid[i, j] == '▲') {                        //if the target is a user bullet
                        if (j > 1) {                                //if the target is not on a border
                            if (grid[i, j - 1] == '█' && j > 1) {   //if the target has hit a wall which isnt the border, damage wall
                                grid[i, j] = ' ';
                                grid[i, j - 1] = '▓';
                            } else if (grid[i, j - 1] == '▓') {     //wall dameging
                                grid[i, j] = ' ';
                                grid[i, j - 1] = '▒';
                            } else if (grid[i, j - 1] == '▒') {
                                grid[i, j] = ' ';
                                grid[i, j - 1] = '░';
                            } else if (grid[i, j - 1] == '░') {
                                grid[i, j] = ' ';
                                grid[i, j - 1] = '*';
                            } else if (grid[i, j - 1] == '☻') {     //if the target has hit the CPU
                                chealth--;
                                grid[i, j] = ' ';
                                flashScreenEnemyHit();
                            } else {                                //if else, move forward
                                grid[i, j] = ' ';
                                grid[i, j - 1] = '▲';
                            }
                        } else {                                    //if the target is on a wall, destroy it
                            grid[i, j] = ' ';
                        }
                    } else if (grid[i, j] == '▼') {                 //if the target is a CPU bullet
                        if (j < height - 2) {                       //if the target is not on a lower border
                            if (grid[i, j + 1] == '█' && j > 1) {   //if the target has hit a wall which isnt the border, damage wall
                                grid[i, j] = ' ';
                                grid[i, j + 1] = '▓';
                            } else if (grid[i, j + 1] == '▓') {     //wall dameging
                                grid[i, j] = ' ';
                                grid[i, j + 1] = '▒';
                            } else if (grid[i, j + 1] == '▒') {
                                grid[i, j] = ' ';
                                grid[i, j + 1] = '░';
                            } else if (grid[i, j + 1] == '░') {
                                grid[i, j] = ' ';
                                grid[i, j + 1] = '*';
                            } else if (grid[i, j + 1] == '☺') {     //if the target has hit the user
                                uhealth--;
                                grid[i, j] = ' ';
                                flashScreenPlayerHit();
                            } else {                                //if else, move forward
                                grid[i, j] = ' ';
                                grid[i, j + 1] = 'v';
                            }
                        } else {                                    //if the target is on a wall, destroy it
                            grid[i, j] = ' ';
                        }
                    } else if (grid[i, j] == '*') {                 //if its an explosion, delete it
                        grid[i, j] = ' ';
                    }
                }
            }
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (grid[i, j] == 'v') {
                        grid[i, j] = '▼';
                    }
                }
            }
        }

        private char[,] stringToArray(string toload) {
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    grid[j, i] = toload[i * width + j];
                }
            }
            return grid;
        }

        private char[,] getOldGrid(char[,] currentGrid) {
            char[,] oldGrid = new char[width, height];
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    oldGrid[i, j] = currentGrid[i, j];
                }
            }
            return oldGrid;
        }

        private void flashScreenEnemyHit() {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            draw.drawGrid(grid, indent);
            draw.drawStats(chealth, uhealth, bullets, walls, boost);
        }

        private void flashScreenPlayerHit() {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            draw.drawGrid(grid, indent);
            draw.drawStats(chealth, uhealth, bullets, walls, boost);
        }

        private void CheckAndResetWindowSize() {
            try {
                if (Console.WindowWidth != 80 || Console.WindowHeight != 30) {
                    Console.SetWindowSize(80, 30);
                    Console.Clear();
                    draw.drawGrid(grid, indent);
                    draw.drawStats(chealth, uhealth, bullets, walls, boost);
                }
            } catch { }
        }
        
        public void dispose() {
            ai.dispose();

            uhealth = 10;
            chealth = 30;
            bullets = 500;
            walls = 20;
            boost = false;
            boostime = 100;
            cooldown = 200;
            boostimeon = 0;
            cooldowntime = 200;

            width = 40;
            height = 20;
            indent = 5;

            HAULT = false;
            WINNER = 0;
        }
    }
}
