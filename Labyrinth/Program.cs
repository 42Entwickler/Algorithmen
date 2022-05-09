/*
 * Code As is. Not quality checked and not well documented - sorry for that.
 * Just for demonstration for the youtube videos implemented.
 * Like, Subscribe, Comment and Follow at https://www.youtube.com/c/42Entwickler
 * Code run can be found here: https://www.youtube.com/watch?v=39oIOTBvUX8
 * And here: https://www.youtube.com/watch?v=EShCwiHlnBg
 */
using System;
using System.Collections.Generic;

namespace Labyrinth {
    class Program
    {
        const char EMPTY = ' ';
        const char PATH_INDICATOR = '+';
        const int LAB_COUNT = 8;
        const int SPEED = 110;
        const int INFO_SPEED_FACTOR = 15;
        const int INFO_LINE_COUNT = 2;
        const int START_ROW = 7;
        const int START_COL = 9;

        static void Main(string[] args)
        {
            Console.ReadKey();
            Console.Title = "Searching Strategies";
            Console.WriteLine("Depth-First- & Breadth-First- & Dijkstra- & A*-Search Labyrinth Solving");
            for (int i = 1; i <= LAB_COUNT; i++)
            {
                char[,] lab;
                bool solutionFound;
                //// Depth First Search
                System.Threading.Thread.Sleep(SPEED * INFO_SPEED_FACTOR);
                Console.Clear();
                Console.WriteLine("Labyrinth " + i.ToString());
                Console.WriteLine("Depth First Search");
                lab = GetLabyrinth(i);
                Print(lab);
                System.Threading.Thread.Sleep(SPEED * INFO_SPEED_FACTOR);
                solutionFound = DepthFirstSearch(lab, START_ROW, START_COL);
                InfoSolution(solutionFound, lab.GetLength(0));

                // Breath First Search
                System.Threading.Thread.Sleep(SPEED * INFO_SPEED_FACTOR);
                Console.Clear();
                Console.WriteLine("Labyrinth " + i.ToString());
                Console.WriteLine("Breadth First Search");
                lab = GetLabyrinth(i);
                Print(lab);
                System.Threading.Thread.Sleep(SPEED * INFO_SPEED_FACTOR);
                solutionFound = BreadthFirstSearch(lab, START_ROW, START_COL);
                System.Threading.Thread.Sleep(SPEED * INFO_SPEED_FACTOR);
                InfoSolution(solutionFound, lab.GetLength(0));

                // Dijkstra
                System.Threading.Thread.Sleep(SPEED * INFO_SPEED_FACTOR);
                Console.Clear();
                Console.WriteLine("Labyrinth " + i.ToString());
                Console.WriteLine("Dijkstra");
                lab = GetLabyrinth(i);
                Print(lab);
                System.Threading.Thread.Sleep(SPEED * INFO_SPEED_FACTOR);
                solutionFound = Dijkstra(lab, START_ROW, START_COL);
                System.Threading.Thread.Sleep(SPEED * INFO_SPEED_FACTOR);
                InfoSolution(solutionFound, lab.GetLength(0));

                // A*
                System.Threading.Thread.Sleep(SPEED * INFO_SPEED_FACTOR);
                Console.Clear();
                Console.WriteLine("Labyrinth " + i.ToString());
                Console.WriteLine("A*");
                lab = GetLabyrinth(i);
                Print(lab);
                System.Threading.Thread.Sleep(SPEED * INFO_SPEED_FACTOR);
                solutionFound = AStar(lab, START_ROW, START_COL);
                System.Threading.Thread.Sleep(SPEED * INFO_SPEED_FACTOR);
                InfoSolution(solutionFound, lab.GetLength(0));
            }
            Console.Clear();
            Console.WriteLine("Done");
        }

        static void InfoSolution(bool found, int height)
        {
            Console.SetCursorPosition(0, height + INFO_LINE_COUNT);
            if (found)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Solution found");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No Solution for this labyrinth");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        static bool BreadthFirstSearch(char[,] lab, int startRow, int startCol)
        {
            bool isStart = true;
            // This is our storage
            Queue<(int row, int col)> toSearch = new Queue<(int row, int col)>();
            // Add the given element to the storage to analyze
            toSearch.Enqueue((startRow, startCol));
            while (toSearch.Count > 0)
            {
                (int row, int col) = toSearch.Dequeue();

                //Console.Clear();
                lab[row, col] = PATH_INDICATOR;
                //Print(lab);
                PrintAt(lab[row, col], row, col);
                System.Threading.Thread.Sleep(SPEED);

                // Check if we found an exit
                if (!isStart && (row == 0 || col == 0 || row == lab.GetUpperBound(0) || col == lab.GetUpperBound(1)))
                {
                    return true;    // Exist found - > Finished
                }
                isStart = false;

                // Now add each neighbour (that is possible) to the queue to need to be searched.
                // If possible means havn't been visited before and no wall.
                if (row - 1 >= 0 && lab[row - 1, col] == EMPTY && !toSearch.Contains((row - 1, col)))
                    toSearch.Enqueue((row - 1, col));
                if (row + 1 < lab.GetLength(0) && lab[row + 1, col] == EMPTY && !toSearch.Contains((row + 1, col)))
                    toSearch.Enqueue((row + 1, col));
                if (col - 1 >= 0 && lab[row, col - 1] == EMPTY && !toSearch.Contains((row, col - 1)))
                    toSearch.Enqueue((row, col - 1));
                if (col + 1 < lab.GetLength(1) && lab[row, col + 1] == EMPTY && !toSearch.Contains((row, col + 1)))
                    toSearch.Enqueue((row, col + 1));
            }

            return false;
        }

        static bool DepthFirstSearch(char[,] lab, int row, int col, bool isStart = true)
        {
            // If non empty (wall or visited) ignore it
            if (row < 0 || col < 0 || row > lab.GetUpperBound(0) || col > lab.GetUpperBound(1) || lab[row, col] != EMPTY)
                return false;

            //Console.Clear();
            lab[row, col] = PATH_INDICATOR;
            PrintAt(lab[row, col], row, col);
            System.Threading.Thread.Sleep(SPEED);

            // Check if we found solution
            if (!isStart && (row == 0 || col == 0 || row == lab.GetUpperBound(0) || col == lab.GetUpperBound(1)))
            {
                return true;
            }

            // Now let's go ahead by searching each of the 4 neighbours -> Depth First Search
            if (DepthFirstSearch(lab, row - 1, col, false))
                return true;
            if (DepthFirstSearch(lab, row + 1, col, false))
                return true;
            if (DepthFirstSearch(lab, row, col - 1, false))
                return true;
            if (DepthFirstSearch(lab, row, col + 1, false))
                return true;
            return false;
        }

        static bool Dijkstra(char[,] lab, int startRow, int startCol, Func<int, int, int> goalEstimation = null)
        {
            const int MAX_VAL = int.MaxValue / 2 - 2;
            // If non empty (wall or visited) ignore it
            if (startRow < 0 || startCol < 0 || startRow > lab.GetUpperBound(0) || startCol > lab.GetUpperBound(1) || lab[startRow, startCol] != EMPTY)
                return false;

            lab[startRow, startCol] = PATH_INDICATOR;
            PrintAt(lab[startRow, startCol], startRow, startCol);
            System.Threading.Thread.Sleep(SPEED);

            int[,] distances = new int[lab.GetLength(0), lab.GetLength(1)];
            int[,] estimates = new int[lab.GetLength(0), lab.GetLength(1)];
            bool[,] done = new bool[lab.GetLength(0), lab.GetLength(1)];

            for (int row = 0; row < lab.GetLength(0); row++)
                for (int col = 0; col < lab.GetLength(1); col++)
                {
                    estimates[row, col] = MAX_VAL;
                    distances[row, col] = MAX_VAL;
                }

            distances[startRow, startCol] = 0;
            int nextRow = startRow;
            int nextCol = startCol;

            bool finished = false;
            while (!finished)
            {
                for (int row = Math.Max(0, nextRow - 1); row < Math.Min(nextRow + 2, distances.GetLength(0)); row++)
                    for (int col = Math.Max(0, nextCol - 1); col < Math.Min(nextCol + 2, distances.GetLength(1)); col++)
                    {
                        if (distances[row, col] == MAX_VAL && !done[row, col] && lab[row, col] == EMPTY)   // avoid walls.
                        {
                            // If we have already a way to this know probably we are faster now...
                            int estimate = 0;
                            if (goalEstimation != null)
                                estimate = goalEstimation(row, col);    // for a*
                            estimates[row, col] = estimate;
                            if (distances[row, col] > distances[nextRow, nextCol] + 1)
                                distances[row, col] = distances[nextRow, nextCol] + 1;
                        }
                    }
                done[nextRow, nextCol] = true;
                // Find neighbour with min value.
                int minRow = int.MaxValue;
                int minCol = int.MaxValue;
                int minVal = int.MaxValue;
                for (int row = 0; row < distances.GetLength(0); row++)
                    for (int col = 0; col < distances.GetLength(1); col++)
                    {
                        if (!done[row, col] && minVal > distances[row, col] + estimates[row, col] && lab[row, col] == EMPTY)
                        {
                            minVal = distances[row, col] + +estimates[row, col];
                            minRow = row;
                            minCol = col;
                        }
                    }
                // Check finished
                if (minRow == 0 || minCol == 0 || minRow == lab.GetUpperBound(0) || minCol == lab.GetUpperBound(1))
                {
                    lab[minRow, minCol] = PATH_INDICATOR;
                    PrintAt(lab[minRow, minCol], minRow, minCol);
                    System.Threading.Thread.Sleep(SPEED);

                    return true;
                }

                nextCol = minCol;
                nextRow = minRow;

                if (minRow == int.MaxValue || minCol == int.MaxValue)
                    return false;

                lab[nextRow, nextCol] = PATH_INDICATOR;
                PrintAt(lab[nextRow, nextCol], nextRow, nextCol);
                System.Threading.Thread.Sleep(SPEED);
            }

            // With the distance and done lists call again...
            return false;
        }
        static bool AStar(char[,] lab, int startRow, int startCol)
        {
            // Find target
            int goalRow = 0;
            int goalCol = 0;
            for (int row = 0; row < lab.GetLength(0); row++)
                for (int col = 0; col < lab.GetLength(1); col++)
                    if ((row, col) != (startRow, startCol) && lab[row, col] == EMPTY && (row == 0 || col == 0 || row == lab.GetUpperBound(0) || col == lab.GetUpperBound(1)))
                    {
                        goalRow = row;
                        goalCol = col;
                        break;
                    }
            return Dijkstra(lab, startRow, startCol, (row, col) => { 
                int dRow = row - goalRow;
                int dCol = col - goalCol;
                int dRowSq = dRow * dRow;
                int dColSq = dCol * dCol;
                int sum = dRowSq + dColSq;
                int res = (int)Math.Sqrt(sum);
                return res;
                });
        }

        static void Print(char[,] lab)
        {
            for (int row = 0; row < lab.GetLength(0); row++)
            {
                for (int col = 0; col < lab.GetLength(1); col++)
                {
                    switch (lab[row, col])
                    {
                        case PATH_INDICATOR:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }
                    Console.Write(lab[row, col]);
                }
                Console.WriteLine();
            }
        }
        static void PrintAt(char sym, int row, int col)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(col, row + INFO_LINE_COUNT);    // 2 Info Lines before....
            Console.Write(sym);
        }

        static char[,] GetLabyrinth(int num)
        {
            string filename = num.ToString() + ".txt";
            string[] data = System.IO.File.ReadAllLines(filename);
            // Create the 2d Array.
            char[,] lab = new char[data.Length, data[0].Length];
            // Now copy the values from the string[] [each line] to the lab.
            for (int row = 0; row < data.Length; row++)
            {
                for (int col = 0; col < data[row].Length; col++)
                {
                    lab[row, col] = data[row][col];
                }
            }
            return lab;
        }
    }
}
