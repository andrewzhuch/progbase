using System;
using static System.Console;

namespace task2
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] arrayOfEarth = new int[,]{
                {0, 0, 1, 1, 1, 0, 1, 1, 1},
                {0, 1, 0, 1, 0, 1, 0, 0, 1},
                {0, 1, 0, 0, 0, 0, 1, 0, 1},
                {1, 1, 1, 1, 1, 0, 1, 1, 0},
                {0, 0, 0, 1, 1, 1, 0, 0, 0},
                {1, 1, 1, 1, 0, 1, 1, 0, 1},
                {0, 1, 1, 0, 0, 0, 1, 0, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1}
            };
            WriteLine(biggestIsland(arrayOfEarth));
            print(arrayOfEarth);
        }
        static int biggestIsland(int[,] arrayofEarth)//looking for the biggest island
        {
            int counterForChanges = 0;//counting how many changes have done in one iteration
            int newNumber = 1;//change all 1 for new numbers strarting from 1 to quantity of 1 in array
            for(int i = 0; i < arrayofEarth.GetLength(0); i++)//hereinafter i - raw index, j - column index
            {
                for(int j = 0; j < arrayofEarth.GetLength(1); j++)
                {
                    if(arrayofEarth[i, j] == 1)
                    {
                        arrayofEarth[i, j] = newNumber;
                        newNumber++;
                    }
                }
            }
            int[] counters = new int[newNumber];
            for(int i = 1; i < counters.Length; i++)
            {
                counters[i] = 1;
            }
            do//connecting earth parts
            {
                counterForChanges = 0;
                for(int i = 0; i < arrayofEarth.GetLength(0); i++)
                {
                    for(int j = 0; j < arrayofEarth.GetLength(1); j++)
                    {
                        if(arrayofEarth[i, j] != 0 && i != 0 && arrayofEarth[i - 1 , j] != 0 && arrayofEarth[i, j] > arrayofEarth[i - 1, j])//for upper cell
                        {
                            counters[arrayofEarth[i, j]] = 0;
                            arrayofEarth[i,j] = arrayofEarth[i - 1, j];
                            counters[arrayofEarth[i - 1, j]]++;
                            counterForChanges++;
                        }
                        if(arrayofEarth[i, j] != 0 && i != arrayofEarth.GetLength(0) - 1 && arrayofEarth[i + 1, j] != 0 && arrayofEarth[i, j] > arrayofEarth[i + 1, j])//for bottom sell
                        {
                            counters[arrayofEarth[i, j]] = 0;
                            arrayofEarth[i, j] = arrayofEarth[i + 1, j];
                            counters[arrayofEarth[i + 1, j]]++;
                            counterForChanges++; 
                        }
                        if(arrayofEarth[i, j] != 0 && j != 0 && arrayofEarth[i, j - 1] != 0 && arrayofEarth[i, j] > arrayofEarth[i, j - 1])//for left cell
                        {
                            counters[arrayofEarth[i, j]] = 0;
                            arrayofEarth[i, j] = arrayofEarth[i , j - 1];
                            counters[arrayofEarth[i, j - 1]]++;
                            counterForChanges++;
                        }
                        if(arrayofEarth[i, j] != 0 && j != arrayofEarth.GetLength(1) - 1 && arrayofEarth[i, j + 1] != 0 && arrayofEarth[i, j + 1] < arrayofEarth[i, j])//for rigt cell
                        {
                            counters[arrayofEarth[i, j]] = 0;
                            arrayofEarth[i, j] = arrayofEarth[i, j + 1];
                            counters[arrayofEarth[i, j + 1]]++;
                            counterForChanges++;
                        }
                    }
                }
            }
            while(counterForChanges != 0);
            int volumeOfBiggestIsland = -1;
            for(int i = 1; i < counters.Length; i++)//comparing sizes of islands
            {
                if(counters[i] > volumeOfBiggestIsland)
                {
                    volumeOfBiggestIsland = counters[i];
                }
            }
            return volumeOfBiggestIsland;
        }
        static void print(int[,] arrayofEarth)
        {
            Write("+");
            int counter = 0;
            while(counter < arrayofEarth.GetLength(1))
            {
                Write("-");
                counter++;
            }
            Write("+");
            WriteLine();
            for(int i = 0; i < arrayofEarth.GetLength(0); i++)
            {
                Write("|");
                for(int j = 0; j < arrayofEarth.GetLength(1); j++)
                {
                    if(arrayofEarth[i,j] == 0)
                    {
                        Write(" ");
                    }
                    else if(arrayofEarth[i, j] == 7)
                    {
                        ForegroundColor = ConsoleColor.Green;
                        Write("N");
                        ResetColor();
                    }
                    else
                    {
                        Write("N");
                    }
                }
                Write("|");
                WriteLine();
            }
            Write("+");
            int counter1 = 0;
            while(counter1 < arrayofEarth.GetLength(1))
            {
                Write("-");
                counter1++;
            }
            Write("+");
            WriteLine();
        }
    }
}
