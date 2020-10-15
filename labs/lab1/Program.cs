using System;
using static System.Console;
using static System.Math;

namespace lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Enter number a");
            double a = double.Parse(ReadLine());
            WriteLine("Enter number b");
            double b = double.Parse(ReadLine());
            WriteLine("Enter number c");
            double c = double.Parse(ReadLine());
            double d0 = ((Pow(a + 3, c + 1)) - 10) / (a - b);
            double d1 = (6 * b + (c / a));
            double d2 = Pow(6 + Sin(b), Cos(a) / c);
            double d = d0 + d1 + d2;
            WriteLine(a);
            WriteLine(b);
            WriteLine(c);
            WriteLine(d0);
            WriteLine(d1);
            WriteLine(d2);
            WriteLine(d);
            //part 2
            double y;
            WriteLine("Enter X");
            double x = double.Parse(ReadLine());
            if ((x > -10 && x <= -5) || (5 <= x && x < 10))
            {
                if (2-x != 0)// ОДЗ не имеет смысла, т.к в указаном выше промежутке не существует такого х, при котором не выполнится данное условие
                {
                    y = (-Pow(x, 2) + 3) / (2 - x);
                }
                else
                {
                y = double.NaN; 
                }
            }
            else
            {
                int calcCos = Convert.ToInt32(Cos((x+2) * (PI / 180)));
                if (calcCos == 0)
                {
                    y = double.NaN;
                }
                else
                {
                    y = 0.5 * Tan(calcCos);
                }
            }
            WriteLine(x);
            WriteLine(y); 
        }  
    }
}
