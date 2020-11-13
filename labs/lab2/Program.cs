using System;
using static System.Console;
using static System.Math;

namespace lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            double minPoint = -10.0;
            double maxPoint = 10.0;
            while (minPoint <= maxPoint)
            {
                WriteLine("y({0}) = {1}",minPoint, Fx(minPoint));
                minPoint += 0.5;
            }
            WriteLine("Input xMin");
            double xMin = double.Parse(ReadLine());
            WriteLine("Input xMax");
            double xMax = double.Parse(ReadLine());
            WriteLine("Input nSteps");
            int nSteps = int.Parse(ReadLine());
            if (nSteps <= 0)
            {
                WriteLine("Wrong input for nSteps");
            }
            else if (xMin >= xMax)
            {
                WriteLine("Wrong input for xMin or/and for xMax");
            }
            else if((xMin <= 2 && xMax > 2) || (xMin < 2 && xMax >=2))
            {
                WriteLine("Impossible to count integral");
            }
            else // looking for the closest breakpoint to xMin for the Hx function and comparing it with xMax
            {
                double i = (xMin + 2 - PI / 2) / PI;
                i = Ceiling(i);
                double xi = PI / 2 + PI * i - 2;
                if (xi <= xMax)
                {
                    WriteLine("Impossible to count integral");
                }
                else
                {
                    WriteLine(intFx(xMin, xMax, nSteps));
                }
            }
        }
        static double Gx(double x)
        {
            double y = 0;
            if (x == 2)
            {
                y = double.NaN;
            }
            else
            {
                y = -Pow(x,2) + 3 / (2 - x);
            }
            return y;
        }
        static double Hx(double x)
        {
            double y = 0;
            if (Cos(x + 2) == 0)
            {
                y = double.NaN;
            }
            else
            {
                y = 0.5 * Tan(x + 2);
            }
            return y;
        }  
        static double Fx(double x)
        {
            double y = 0;
            if (x > -5 && x <= 3)
            {
                y = Gx(x);
            }
            else
            {
                y = Hx(x);
            }
            return y;
        }
        static double intFx(double xMin, double xMax, int nSteps)
        {
            double integral = 0;
            int counter = 0;
            double step = (xMax - xMin) / nSteps;
            while (counter <= nSteps - 1)
            {
                integral += Fx(xMin + counter * step) + Fx(xMin + (counter + 1) * step);
                counter += 1;
            }
            integral = integral * (step / 2);
            return integral;
        }
    }
}
