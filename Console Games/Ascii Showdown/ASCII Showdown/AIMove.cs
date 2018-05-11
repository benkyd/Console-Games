using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCII_Showdown {
    class AIMove {
        //you can have it fire a horizontal line of bullets, or fire every third step, or fire a bunch in a straight line
        //give it shooting patterns that it can cycle through based on situation

        List<int> lastDirection = new List<int>(); //0 left, 1 right
        List<int> lastMode = new List<int>(); //last movetype stored in last index
        Random rnd = new Random();

        int type = 0;
        int iterationsOfType = 0;

        bool lineOfSight = true;
        int lastKnownPos = 0;

        public int cX { get; set; }
        public int cY { get; set; }


        public char[,] moveCPU(char[,] grid, int width, int uX, int uY) {
            if (grid[uX, uY - 2] == '█') {  //checks if user is hiding
                uX = lastKnownPos;
                lineOfSight = false;
            } else {
                lineOfSight = true;
            }

            type = getAItype(grid, width, uX, uY);

            if (type == 0) {
                return grid;
            } else if (type == 1) {         //defensive, retreat and shoot every 8th
                if (iterationsOfType % 8 == 0) {
                    grid[cX, cY + 1] = '▼';
                }
                if (iterationsOfType % 4 == 0) {
                    if (cX < uX) {
                        grid = moveRight(grid, width);
                    } else {
                        grid = moveLeft(grid, width);
                    }
                }
            } else if (type == 2) {         //aggressive 

            } else if (type == 3) {         //try to break walls

            } else if (type == 4) {         //try to block off player

            } else if (type == 5) {         //retreat and not shoot

            } else if (type == 6) {         //blanket shot

            }

            iterationsOfType++;

            /* string lastdir = "";
            for (int i = 0; i < lastDirection.Count; i++) {
                lastdir += lastDirection[i];
            }
            string lastmode = "";
            for (int i = 0; i < lastMode.Count; i++) {
                lastmode += lastMode[i];
            }
            Console.SetCursorPosition(0, 22);
            Console.WriteLine("Last direction: " + lastdir);
            Console.WriteLine("Last mode: " + lastmode);
            Console.WriteLine("Iterations of mode: " + iterationsOfType); */

            return grid;
        }

        private int getAItype(char[,] grid, int width,int uX, int uY) {
            if (lastMode.Count > 1 && lastMode[lastMode.Count - 1] != lastMode[lastMode.Count - 2]) {
                iterationsOfType = 0;
            }
            if (!lineOfSight) {
                wonder(grid, width);
                lastMode.Add(0);
                return 0;
            }
            lastMode.Add(1);
            return 1;
        }

        private char[,] wonder(char[,] grid, int width) {
            if (iterationsOfType % 8 == 0) {
                int shoot = rnd.Next(1, 5);
                int move = rnd.Next(0, 2);
                if (shoot == 1) {
                    grid[cX, cY + 1] = '▼';
                }
                if (move == 1) {
                    grid = moveLeft(grid, width);
                } else {
                    grid = moveRight(grid, width);
                }
            }
            iterationsOfType++;
            return grid;
        }

        private char[,] moveLeft(char[,] grid, int width) {
            if (cX - 1 > 0) {
                lastDirection.Add(0);
                grid[cX, cY] = ' ';
                cX--;
                grid[cX, cY] = '☻';
            }
            return grid;
        }
        private char[,] moveRight(char[,] grid, int width) {
            if (cX + 1 < width - 1) {
                lastDirection.Add(1);
                grid[cX, cY] = ' ';
                cX++;
                grid[cX, cY] = '☻';
            }
            return grid;
        }

        public void reset() {
            lastDirection.Clear();
        }
    }
}
