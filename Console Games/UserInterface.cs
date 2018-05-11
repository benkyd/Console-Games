using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Games {
    class UserInterface {
        public int startHeight;
        public int startWidth;

        public void atLaunch() {
            startHeight = Console.WindowHeight;
            startWidth = Console.WindowWidth;
            programStart();
        }

        public void programStart() {
            Console.Clear();
            Console.CursorVisible = false;

            int offsetFromLeft = 1;
            int offsetFromTop = 2;

            string[] gameList = new string[3];
            gameList[0] = "Ascii Showdown";
            gameList[1] = "Space Invaders";
            gameList[2] = "Tetris";

            int numberOfGames = gameList.Length;

            while (true) {
                selectGame(numberOfGames, offsetFromLeft, offsetFromTop, gameList);
            }
        }

        private void setHUD(int GameAmmount) {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Select a console game to play:");

            for (int i = 2; i < GameAmmount + 2; i++) {
                Console.SetCursorPosition(0, i);
                Console.Write("|");
            }

            Console.SetCursorPosition(0, GameAmmount + 3);
            Console.WriteLine("Copyright™ Benjamin Kyd 2018");
            Console.WriteLine("Press Q to quit...");
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
        }

        private void selectGame(int numGames, int offsetLeft, int offsetTop, string[] gameList) {

            if (Console.WindowHeight == startHeight || Console.WindowWidth == startWidth) {
                Console.SetWindowSize(startWidth, startHeight);
            }

            Console.Clear();
            setHUD(numGames);
            for (int i = 0; i < gameList.Length; i++) {
                Console.SetCursorPosition(offsetLeft, offsetTop + i);

                if (gameList[i].Length <= 6) {
                    Console.Write(" " + gameList[i] + "\t\t (" + (i + 1) + ")");
                } else {
                    Console.Write(" " + gameList[i] + "\t (" + (i + 1) + ")");

                }
            }

            //List<AsciiShowdown_Game> showdown = new List<AsciiShowdown_Game>();

            //Console.WriteLine("\n█ █ █ █ █ \n👾 👾 👾 👾 👾");

            var response = Console.ReadKey();
            switch (response.KeyChar) {
                case 'Q':
                case 'q':
                    Environment.Exit(0);
                    break;
                case '1':
                    AsciiShowdown_Game showdown = new AsciiShowdown_Game();
                    showdown.initializeGame();
                    break;
                case '2':
                    break;
                case '3':
                    Tetris_Game tetris = new Tetris_Game();
                    tetris.initializeGame();
                    break;
                default:
                    break;
            }

        }
    }
}
