using System;
using static System.Console;
using System.IO;

namespace lab3_2
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0 || (args[0] == "console" && args.Length == 1))
            {
                ILogger logger = new ConsoleLogger();
                ProcessSet(logger);
            }
            else if(args.Length == 2 && args[0] == "sql")
            {
                string dataBaseFile = args[1];
                ILogger logger = new SQLLogger(dataBaseFile);
                ProcessSet(logger);
            }
            else
            {
                WriteLine("Wrong command line arguments.");
            }
        }
        static void ProcessSet(ILogger logger)
        {
            WriteLine("Set is based on bool array, so user must set range of possible values himself.");
            WriteLine("Please, input minimal and maximal values that can be putted into set.");
            WriteLine("Input min for set a:");
            string minA = ReadLine();
            WriteLine("Input max for set a:");
            string maxA =  ReadLine();
            WriteLine("Input min for set b:");
            string minB = ReadLine();
            WriteLine("Input max for set b:");
            string maxB = ReadLine();
            if(CheckInput(minA, maxA, minB, maxB) == false)
            {
                WriteLine("Wrong input! Only integer can be inputted. Besides, minimal values shoud be lower than maximal.");
            }
            else
            {
                int minimalA = int.Parse(minA);
                int maximalA = int.Parse(maxA);
                int minimalB = int.Parse(minB);
                int maximalB = int.Parse(maxB);
                IsetInt a = new ArraySetInt(minimalA, maximalA);
                IsetInt b = new ArraySetInt(minimalB, maximalB);
                while(true)
                {
                    WriteLine("Input your command:");
                    string[] command = ReadLine().Split(" ");
                    if(command.Length == 1)
                    {
                        if(command[0] == "exit")
                        {
                            break;
                        }
                        else if(command[0] == "union")
                        {
                            ProcessCommandUnion(a, b, logger);
                        }
                        else if(command[0] == "equals")
                        {
                            ProcessCommandEqual(a, b, logger);
                        }
                        else
                        {
                            logger.LogError("Wrong command.");  
                        }
                    }
                    else if(command.Length == 3)
                    {
                        if(command[1] == "add")
                        {
                            ProcessCommandAdd(command, ref a, ref b, minimalA, maximalA, minimalB, maximalB, logger);
                        }
                        else if(command[1] == "write")
                        {
                            ProcessCommandWrite(command, a, b, logger);
                        }
                        else if(command[1] == "contains")
                        {
                            ProcessCommandContains(command, a, b, minimalA, maximalA, minimalB, maximalB, logger);
                        }
                        else if(command[1] == "read")
                        {
                            ProcessCommandRead(command, a, b, minimalA, maximalA, minimalB, maximalB, logger);
                        }
                        else if(command[1] == "remove")
                        {
                            ProcessCommandRemove(command, a, b, minimalA, maximalA, minimalB, maximalB, logger);
                        }
                        else
                        {
                            logger.LogError("Wrong command.");
                        }
                    }
                    else if(command.Length == 2)
                    {
                        if(command[1] == "log")
                        {
                            ProcessCommandLog(command, a, b, logger);
                        }
                        else if(command[1] == "count")
                        {
                            ProcessCommandCount(command, a, b, logger);
                        }
                        else if(command[1] == "clear")
                        {
                            ProcessCommandClear(command, a, b, logger);
                        }
                        else
                        {
                            logger.LogError("Wrong command.");
                        }
                    }
                    else
                    {
                        logger.LogError("Wrong command.");
                    }
                }
            }
        }
        static bool CheckInput(string minA, string maxA, string minB, string maxB)
        {
            int minimalA;
            int maximalA;
            int minimalB;
            int maximalB;
            bool isInt1 = int.TryParse(minA, out minimalA);
            bool isInt2 = int.TryParse(maxA, out maximalA);
            bool isInt3 = int.TryParse(minB, out minimalB);
            bool isInt4 = int.TryParse(maxB, out maximalB);
            if(isInt1 == false || isInt2 == false || isInt3 == false || isInt4 == false)
            {
                return false;
            }
            if(minimalA >= maximalA || minimalB >= maximalB)
            {
                return false;
            }
            return true;
        }
        static bool CkeckSetValue(string value)
        {
            int a;
            bool isOK = int.TryParse(value, out a);
            return isOK;
        }
        static void WriteSet(string filePath, IsetInt set)
        {
            StreamWriter sw = new StreamWriter(filePath);
            int[] arrayOfSet = new int[set.GetCount()];
            set.CopyTo(arrayOfSet);
            for(int i = 0; i < arrayOfSet.Length; i++)
            {
                sw.Write("{0},", arrayOfSet[i]);
            }
            sw.Close();
        }
        static void ProcessCommandUnion(IsetInt a, IsetInt b, ILogger logger)
        {
            a.UnionWith(b);
            int[] array = new int[a.GetCount()];
            a.CopyTo(array);
            string set = String.Join(',', array);
            logger.Log(set);
        }
        static void ProcessCommandEqual(IsetInt a, IsetInt b, ILogger logger)
        {
            bool Equals = a.SetEquals(b);
            logger.Log(Equals.ToString());
        }
        static void ProcessCommandAdd(string[] command, ref IsetInt setA, ref IsetInt setB, int minA, int maxA, int minB, int maxB, ILogger logger)
        {
            if(CkeckSetValue(command[2]) == false)
            {
                logger.LogError("Only integers can be putted into set.");
                return;
            }
            int newValue = int.Parse(command[2]);
            if(command[0] == "a")
            {
                if(newValue < minA || newValue > maxA)
                {
                    logger.LogError("Value is not in range of possible values.");
                    return;
                }
                bool add = setA.Add(newValue);
                logger.Log(add.ToString());
            }
            else if(command[0] == "b")
            {
                if(newValue < minB || newValue > maxB)
                {
                    logger.LogError("Value is not in range of possible values.");
                    return;
                }
                bool add = setB.Add(newValue);
                logger.Log(add.ToString());
            }
            else
            {   
                logger.LogError("Wrong name of set. There are only set <a> and set <b>.");
            }
        }
        static void ProcessCommandContains(string[] command, IsetInt a, IsetInt b, int minA, int maxA, int minB, int maxB, ILogger logger)
        {
            if(CkeckSetValue(command[2]) == false)
            {
                logger.LogError("Set can contain only integers.");
                return;
            }
            int value = int.Parse(command[2]);
            if(command[0] == "a")
            {
                if(value < minA || value > maxA)
                {
                    logger.LogError("Value is not in range of possible values.");
                    return;
                }
                bool contains = a.Contains(value);
                logger.Log(contains.ToString());
            }
            else if(command[0] == "b")
            {
                if(value < minB || value > maxB)
                {
                    logger.LogError("Value is not in range of possible values.");
                    return;
                }
                bool contains = b.Contains(value);
                logger.Log(contains.ToString());
            }
            else
            {   
                logger.LogError("Wrong name of set. There are only set <a> and set <b>.");
            }
        }
        static void ProcessCommandLog(string[] command, IsetInt a, IsetInt b, ILogger logger)
        {
            if(command[0] == "a")
            {
                int[] array = new int[a.GetCount()];
                a.CopyTo(array);
                string set = String.Join(',', array);
                logger.Log(set);
            }
            else if(command[0] == "b")
            {
                int[] array = new int[b.GetCount()];
                b.CopyTo(array);
                string set = String.Join(',', array);
                logger.Log(set);
            }
            else
            {   
                logger.LogError("Wrong name of set. There are only set <a> and set <b>.");
            }
        }
        static void ProcessCommandRead(string[] command, IsetInt a, IsetInt b, int minA, int maxA, int minB, int maxB, ILogger logger)
        {
            StreamReader sr = new StreamReader(command[2]);
            string line = sr.ReadToEnd();
            string[] array = line.Split(',');
            if(command[0] != "a" && command[0] != "b")
            {
                logger.LogError("Wrong name of set. There are only set <a> and set <b>.");
                return;
            }
            int counter = 0;
            for(int i = 0; i < array.Length; i++)
            {
                if(CkeckSetValue(array[i]) == true)
                {
                    if(command[0] == "a")
                    {
                        if(int.Parse(array[i]) < minA || int.Parse(array[i]) > maxA)
                        {
                            counter++;
                        }
                        else
                        {
                            a.Add(int.Parse(array[i]));
                        }
                    }
                    else if(command[0] == "b")
                    {
                        if(int.Parse(array[i]) < minB || int.Parse(array[i]) > maxB)
                        {
                            counter++;
                        }
                        else
                        {
                            b.Add(int.Parse(array[i]));
                        }
                    }
                }
                else
                {
                    counter++;
                }
            }
            if(counter != 0)
            {
                logger.Log("Not all values were added.");
            }
            else
            {
                logger.Log("All values were added.");
            }
        }
        static void ProcessCommandCount(string[] command, IsetInt a, IsetInt b, ILogger logger)
        {
            if(command[0] == "a")
            {
                string count = a.GetCount().ToString();
                logger.Log(count);
            }
            else if(command[0] == "b")
            {
                string count = b.GetCount().ToString();
                logger.Log(count);
            }
            else
            {   
                logger.LogError("Wrong name of set. There are only set <a> and set <b>.");
            }
        }
        static void ProcessCommandRemove(string[] command, IsetInt a, IsetInt b, int minA, int maxA, int minB, int maxB, ILogger logger)
        {
            if(CkeckSetValue(command[2]) == false)
            {
                logger.LogError("Set can contain only integers.");
                return;
            }
            int value = int.Parse(command[2]);
            if(command[0] == "a")
            {
                if(value < minA || value > maxA)
                {
                    logger.LogError("Value is not in range of possible values.");
                    return;
                }
                bool remove = a.Remove(value);
                logger.Log(remove.ToString());
            }
            else if(command[0] == "b")
            {
                if(value < minB || value > maxB)
                {
                    logger.LogError("Value is not in range of possible values.");
                    return;
                }
                bool remove = b.Remove(value);
                logger.Log(remove.ToString());
            }
            else
            {   
                logger.LogError("Wrong name of set. There are only set <a> and set <b>.");
            }
        }
        static void ProcessCommandClear(string[] command, IsetInt a, IsetInt b, ILogger logger)
        {
            if(command[0] == "a")
            {
                int[] array = new int[a.GetCount()];
                a.CopyTo(array);
                for(int i = 0; i < array.Length; i++)
                {
                    a.Remove(array[i]);
                }
                logger.Log("Set a is empty now");
            }
            else if(command[0] == "b")
            {
                int[] array = new int[b.GetCount()];
                b.CopyTo(array);
                for(int i = 0; i < array.Length; i++)
                {
                    b.Remove(array[i]);
                }
                logger.Log("Set b is empty now");
            }
            else
            {   
                logger.LogError("Wrong name of set. There are only set <a> and set <b>.");
            }
        }
        static void ProcessCommandWrite(string[] command, IsetInt a, IsetInt b, ILogger logger)
        {
            if(command[0] == "a")
            {
                WriteSet(command[2], a);
                logger.Log("Set a writen successfully.");
            }
            else if(command[0] == "b")
            {
                WriteSet(command[2], b);
                logger.Log("Set b writen successfully.");
            }
            else
            {
                logger.LogError("Wrong name of set. There are only set <a> and set <b>.");
            }
        }
    }
}
