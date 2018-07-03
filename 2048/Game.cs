using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2048
{
    public class Game
    {
        private static Random rnd = new Random();

        private readonly int WIDTH;
        private readonly int HEIGHT;

        private int score = 0;

        private int[][] grid;

        public Game(int width, int height)
        {
            Console.CursorVisible = false;
            WIDTH = width;
            HEIGHT = height;

           grid = new int[HEIGHT][];
        }

        public void Run()
        {
            InitGrid();

            while (true)
            {
                ClearScreen();
                ShowGrid();
                CheckHotkeys();

            }
        }

        private void InitGrid()
        {
            for(int y = 0; y < HEIGHT; y++)
            {
                grid[y] = new int[WIDTH];
            }
            SpawnNumber();
            SpawnNumber();
        }

        private void SpawnNumber()
        {
            if (!IsEmptySpaceAvailable())
            {
                return;
            }

            int x = 0;
            int y = 0;
            do
            {
                x = rnd.Next(0, WIDTH);
                y = rnd.Next(0, HEIGHT);

            } while (grid[y][x] != 0);

            grid[y][x] = rnd.Next(0, 100) < 95 ? 2 : 4;
        }

        private bool IsEmptySpaceAvailable()
        {
            for(int i  = 0; i < HEIGHT; i++)
            {
                for(int j = 0; j < WIDTH; j++)
                {
                    if (grid[i][j] == 0) return true;
                }
            }
            return false;
        }

        private void ShowGrid()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    WriteColor(grid[y][x]);
                }
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.WriteLine($"  Score: {score}");


            for (int x = 0; x < WIDTH * 6 + 4; x++)
            {
                Console.SetCursorPosition(x, 0);
                Console.Write("#");

                Console.SetCursorPosition(x, HEIGHT * 3 + 4);
                Console.Write("#");
            }
            Console.Write("#");


            for (int y = 0; y < HEIGHT * 3 + 4; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write("#");

                Console.SetCursorPosition(WIDTH * 6 + 4, y);
                Console.Write("#");
            }
        }

        private bool SlideArrayLeft()
        {
            bool modified = false;
            foreach (var row in grid)
            {
                // Phase 1: merge numbers
                var col = -1;
                var length = row.Length;

                for (int y = 0; y < length; y++)
                {
                    if (row[y] == 0)
                        continue;
                    if(col == -1)
                    {
                        col = y; // remember current col
                        continue;
                    }
                    if(row[col] != row[y])
                    {
                        col = y; // update
                        continue;
                    }
                    if(row[col] == row[y])
                    {
                        row[col] += row[y]; // merge same numbers
                        score += row[col];
                        row[y] = 0;
                        col = -1; // reset
                        modified = true;
                    }
                }

                // Phase 2: move numbers
                for(int i = 0; i<length * length; i++)
                {
                    int y = i % length;

                    if (y == length - 1) continue;
                    if (row[y] == 0 && row[y + 1] != 0)
                    {
                        row[y] = row[y + 1];
                        row[y + 1] = 0;
                        modified = true;
                    }
                }
            }
            return modified;
        }

        private void RotateMatrixCounterClockwise()
        {
            int[][] newMatrix = grid.Select(a => a.ToArray()).ToArray(); // copy array
            int newColumn, newRow = 0;
            for (int oldColumn = HEIGHT - 1; oldColumn >= 0; oldColumn--)
            {
                newColumn = 0;
                for (int oldRow = 0; oldRow < WIDTH; oldRow++)
                {
                    newMatrix[newRow][newColumn] = grid[oldRow][oldColumn];
                    newColumn++;
                }
                newRow++;
            }
            grid = newMatrix;
        }

        private void CheckHotkeys()
        {
            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (SlideArrayLeft())
                    {
                        SpawnNumber();
                    }
                    
                    break;
                case ConsoleKey.DownArrow:
                    RotateMatrixCounterClockwise();
                    RotateMatrixCounterClockwise();
                    RotateMatrixCounterClockwise();
                    if (SlideArrayLeft())
                    {
                        SpawnNumber();
                    }
                    RotateMatrixCounterClockwise();
                    break;
                case ConsoleKey.RightArrow:
                    RotateMatrixCounterClockwise();
                    RotateMatrixCounterClockwise();
                    if (SlideArrayLeft())
                    {
                        SpawnNumber();
                    }
                    RotateMatrixCounterClockwise();
                    RotateMatrixCounterClockwise();
                    break;
                case ConsoleKey.UpArrow:
                    RotateMatrixCounterClockwise();
                    if (SlideArrayLeft())
                    {
                        SpawnNumber();
                    }
                    RotateMatrixCounterClockwise();
                    RotateMatrixCounterClockwise();
                    RotateMatrixCounterClockwise();
                    break;
                default:
                    break;
            }
        }

        private void ClearScreen()
        {
            Console.SetCursorPosition(0, 0);
        }

        private void WriteColor(int num)
        {
            switch (num)
            {
                case 2:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case 8:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 16:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case 32:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case 64:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case 128:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case 256:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case 512:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case 1024:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case 2048:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 4096:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case 8192:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 16384:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    break;
            }
            if(num != 0)
            {
                Console.Write($"{num,6}");
            }
            else
            {
                Console.Write($"{".",6}");
            }
            Console.ResetColor();
        }
    }

}
