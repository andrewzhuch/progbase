using System;
using Progbase.Procedural;
using static System.Console;
using static System.Math;

namespace lab4
{
    struct Point
    {
        public double x;
        public double y;
    }
    struct Circle
    {
        public double x;
        public double y;
        public double radius;
    }
    class Program
    {
        static void Main(string[] args)
        {   
            const int size = 40;//size of canv
            double alpha = PI / 3;
            Point a = new Point();//main point
            a.x = size / 2;
            a.y = size / 2;
            Point b = new Point();//center ot the second circle
            Clear();
            Circle r2 = new Circle();//circle located on the main circle
            r2.x = 0;
            r2.y = 0;
            r2.radius = 3;
            Circle r3 = new Circle();//internal circle
            r3.x = a.x;
            r3.y = a.y;
            r3.radius = 3;
            int r1 = 10;//main circle
            Canvas.InvertYOrientation();//initialization
            Canvas.SetSize(size, size);
            ConsoleKeyInfo keyInfo;
            do//printing
            {
                b.x = r1 * Cos(alpha) + a.x;
                b.y = r1 * Sin(alpha) + a.x;
                Canvas.BeginDraw();
                Canvas.SetColor(255, 0, 0);
                Canvas.StrokeLine(0, 0, size, size);//diagoal L1
                Canvas.SetColor(0, 255, 255);
                Canvas.FillCircle((int)b.x, (int)b.y, (int)r2.radius);//circle located on the main circle
                Canvas.SetColor(255, 255, 0);
                Canvas.PutPixel((int)b.x, (int)b.y);///center ot the second circle
                if(r3.radius > 0)
                {
                    Canvas.SetColor(0, 255, 0);
                    Canvas.FillCircle((int)a.x, (int)a.y, (int)r3.radius);//internal circle
                }
                Canvas.SetColor(0, 0, 255);
                Canvas.PutPixel((int)a.x, (int)a.y);//main point
                Canvas.EndDraw();
                keyInfo = ReadKey();//key for ending programm
                if(keyInfo.Key == ConsoleKey.D)//moving up main point
                {
                    if(a.x < size - r3.radius - 1 && a.x < size - 1 && b.x + r2.radius < size - 1 && b.y + r2.radius < size - 1)//checking borders
                    {
                        a.x++;
                        a.y++;
                    }
                }
                else if(keyInfo.Key == ConsoleKey.A)//moving down main point
                {
                    if(a.x > r3.radius && a.x > 0 && b.x - r2.radius > 1 && b.y - r2.radius > 1)//checking borders
                    {
                        a.x--;
                        a.y--;
                    }
                }
                else if(keyInfo.Key == ConsoleKey.W)//increasing radius of internal circle
                {
                    if((a.x + r3.radius < size - 1) && r3.radius < a.x)//checking borders
                    {
                        r3.radius++;
                    }
                }
                else if(keyInfo.Key == ConsoleKey.S)//reducing radius of internal circle
                {
                    r3.radius--;
                }
                else if(keyInfo.Key == ConsoleKey.Q)//increasing alpha
                {
                    alpha += PI / 10;
                }
                else if(keyInfo.Key == ConsoleKey.E)//reducing alpha
                {
                    alpha -= PI / 10;
                }
                else if(keyInfo.Key == ConsoleKey.R)//inreasing radius of main circle
                {
                    if(b.x + r2.radius  < size - 1 && b.y + r2.radius  < size - 1 && b.x - r2.radius  > 1 && b.y > 0 && b.y - r2.radius  > 1)//checking borders
                    {
                        r1++;
                    }
                }
                else if(keyInfo.Key == ConsoleKey.T)//reducing radius of main circle
                {
                    if(r1 > 1)//checking condition r1 > 0
                    {
                        r1--;
                    }
                }
                else if((keyInfo.Key == ConsoleKey.P))//inreasing radius of circle located on the main circle
                {
                    if(b.x + r2.radius < size - 1 && b.y + r2.radius < size - 1 && b.x - r2.radius > 1 && b.y > 0 && b.y - r2.radius > 1)//checking borders
                    {
                        r2.radius++; 
                    }
                }
                else if(keyInfo.Key == ConsoleKey.L)//reducing radius of circle located on the main circle
                {
                    if(r2.radius > 1)//checking condition r2 > 0
                    {
                        r2.radius--;
                    }
                }
            }
            while(keyInfo.Key != ConsoleKey.Escape);
            WriteLine();
        }
    }
}
