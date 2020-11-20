using System;
using static System.Console;

namespace lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] startArray = new int[] {-6, -1, -1, 3, -3, 2, -2, -1, -5, -2, 5};//first array
            int minNumber = minimalNumber(startArray); //minimal number in the first array
            int[] groundLevel = calculatingGroundLevel(startArray, minNumber);
            int maxGroundLevel = maximalGroudLevel(groundLevel);
            WriteLine("Input water level");
            int waterLevel = int.Parse(ReadLine());//max water level
            int[] waterLevelArray = waterLevelCounting(groundLevel, startArray, waterLevel);//array with the levels of water
            airVolume(waterLevelArray, maxGroundLevel, groundLevel);
            print(groundLevel, waterLevel, maxGroundLevel);
        }
        static int maximalGroudLevel(int[] groundLevel)
        {
            int maxGroundLevel = 0;
            for(int i = 1; i < groundLevel.Length; i ++)//looking for the highest ground level
            {
                if(groundLevel[i] > maxGroundLevel)
                {
                    maxGroundLevel = groundLevel[i];
                }
            }
            return maxGroundLevel;
        }
        static int[] calculatingGroundLevel(int[] startArray, int minNumber)
        {
            int[] groundLevel = new int[startArray.Length];//array with levels of ground
            for(int i = 0; i < startArray.Length; i++)//making new array for the ground level
            {
                groundLevel[i] = startArray[i] - minNumber;//0, 5, 5, 9, 3, 8, 4, 5, 1, 4, 11
            }
            Write("Ground levels are ");
            for(int i = 0; i < groundLevel.Length; i++)
            {
                Write("{0} ", groundLevel[i]);
            }
            WriteLine();
            return groundLevel;
        }
        static int minimalNumber(int[] startArray)
        {
            int minNumber = startArray[0];//minimal number in the first array
            for(int i = 1; i < startArray.Length; i++)//looking for the minimal number
            {
                if(startArray[i] < minNumber)
                {
                    minNumber = startArray[i];
                }
            }
            return minNumber;
        }
        static int[] waterLevelCounting(int[] groundLevel, int[] startArray, int waterLevel)//calculating water levels
        {
            int[] waterLevelArray = new int[startArray.Length];
            for(int i = 0; i < groundLevel.Length; i++)//maling new array for the water level
            {
                if(groundLevel[i] < waterLevel)
                {
                    waterLevelArray[i] = waterLevel - groundLevel[i];//4, 0, 0, 0, 1, 0, 0, 0, 3, 0, 0
                }
            }
            Write("Water levels are ");
            for(int i = 0; i < waterLevelArray.Length; i++)
            {
                Write("{0} ", waterLevelArray[i]);
            }
            WriteLine();
            return waterLevelArray;
        }
        static void airVolume(int[] waterLevelArray, int maxGroundLevel, int[] groundLevel)//calculating volume of air over water
        {
            int generalAirVolume = 0;
            for(int i = 0; i < waterLevelArray.LongLength; i++)//calculating general air volume over water
            {
                if(waterLevelArray[i] != 0)
                {
                    generalAirVolume = generalAirVolume + (maxGroundLevel - (waterLevelArray[i] + groundLevel[i]));
                }
            }
            WriteLine("General air volume over water = {0}",generalAirVolume); 
        }
        static void print(int[] groundLevel, int waterLevel, int maxGroundLevel)
        //algorithm in general
        //for the first and last raw of our picture printing "-"
        //for the frist and last column printing "|"
        //using our main array where max index == number of culumns for without borders, max ground level == number of raws without borders
        //for the ground, water and air cells I check arrays
        {
            int currentRaw = maxGroundLevel + 1;//print number of raw, for example '"12", "11" etc
            int i = maxGroundLevel + 2;//index for raw
            while(i > 0)//print without water
            {
                int j = -1;//index for column
                while(j < groundLevel.Length + 1)
                {
                    if(i == maxGroundLevel + 2 || i == 1)
                    {
                        Write("-");
                    }
                    else if((j == -1 && i != 1 && i != maxGroundLevel + 2) || (j ==groundLevel.Length && i != 1 && i !=maxGroundLevel + 2))
                    {
                        Write("|");
                    }
                    else
                    {
                        if(groundLevel[j] >= i - 1)
                        {
                            Write("N");
                        }
                        else
                        {
                            Write(" ");
                        }
                    }
                    j++;
                }
                i--;
                Write(" {0}",currentRaw);
                currentRaw--;
                WriteLine();
            }
            int currentRaw1 = maxGroundLevel + 1;//print number of raw, for example '"12", "11" etc
            int i1 = maxGroundLevel + 2;//index for raw
            while(i1 > 0)//print with water
            {
                int j1 = -1;//index for column
                while(j1 < groundLevel.Length + 1)
                {
                    if(i1 == maxGroundLevel + 2 || i1 == 1)
                    {
                        Write("-");
                    }
                    else if((j1 == -1 && i1 != 1 && i1 != maxGroundLevel + 2) || (j1 ==groundLevel.Length && i1 != 1 && i1 !=maxGroundLevel + 2))
                    {
                        Write("|");
                    }
                    else
                    {
                        if(groundLevel[j1] >= i1 - 1)
                        {
                            Write("N");
                        }
                        else if((groundLevel[j1] < i1 - 1) && (waterLevel >= i1 - 1))
                        {
                            Write("~");
                        }
                        else
                        {
                            Write(" ");
                        }
                    }
                    j1++;
                }
                i1--;
                Write(" {0}",currentRaw1);
                if(i1 == waterLevel)
                {
                    Write(" Water Level");
                }
                currentRaw1--;
                WriteLine();
            }
        }
    }
}
