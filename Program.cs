using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace DrawChart
{
    class Program
    {
        private const string BigNumber = "960939379918958884971672962127852754715004339660129306651505519271702802395266424689642842174350718121267153782770623355993237280874144307891325963941337723487857735749823926629715517173716995165232890538221612403238855866184013235585136048828693337902491454229288667081096184496091705183454067827731551705405381627380967602565625016981482083418783163849115590225610003652351370343874461848378737238198224849863465033159410054974700593138339226497249461751545728366702369745461014655997933798537483143786841806593422227898388722980000748404719";
        public static List<Point> chartList = new List<Point>();

        //y - 100, x - 300, for good resolution
        //y - 40, x - 100, for notmal resolution
        public static int ScreenHeight = 100; 
        public static int ScreenWight = 300;

        public struct Point 
        {            
            public int X { get; set; }
            public int Y { get; set; }

            public Point(int x, int y) 
            {
                this.X = x;
                this.Y = y;
            }
        }

        static void Main(string[] args)
        {
            Console.Clear();
            Console.CursorVisible= false;

            //linear
                // int n2 = 1;
            //parabola
                // float x1;
            //logistics map
                // float n1 = 0.8f;
                // float GrowthRate = 3.7f;
            //mandelbrot set
                float a;
                float b;
                float aa;
                float bb;
                float ca;
                float cb;
                bool isGoingToInfinity= false;
                float zoom;

            
            for (int x = 0; x < ScreenWight; x++)
            {
                //logistics map
                    // x1 = x
                    // chartList.Add(new Point(x1, (int)(n1*100)));
                    // n1 = GrowthRate * n1 * (1 - n1);
                //linear
                    // chartList.Add(new Point(n2, n2));
                    // n2 += 1;
                //parabola
                    // x1 = ((float)x).map(0, ScreenWight, -(ScreenWight/2), ScreenWight/2);
                    // chartList.Add(new Point((int)(x1*10), (int)(x1*x1*100)));
                //mandelbrot set
                    for (int y = 0; y <= ScreenHeight; y++)
                    {
                        a = ca = ((float)x).map(0, ScreenWight, -2, 2);
                        b = cb = ((float)y).map(0, ScreenHeight, -2, 2);

                        for (int i = 0; i < 100; i++)
                        {
                            aa = a * a - b * b;
                            bb = 2 * a * b;
                            a = aa + ca;
                            b = bb + cb;
                            if(MathF.Abs(a + b) > 20)
                            {
                                isGoingToInfinity = true;
                                break;
                            }
                            isGoingToInfinity = false;
                        }                            
                        if(!isGoingToInfinity)
                        {
                            chartList.Add(new Point(x, y));
                        }
                    }
            }
            DrawChart(chartList);
        }

        public static void DrawChart(List<Point> dict)
        {
            int consoleWidth = ScreenWight;
            int consoleHeight = ScreenHeight;
            int actualConsoleHeight = consoleHeight * 2;
            var minX = dict.Min(c => c.X);
            var minY = dict.Min(c => c.Y);            
            var maxX = dict.Max(c => c.X);
            var maxY = dict.Max(c => c.Y);

            Console.WriteLine(maxY);
            // normalize points to new coordinates
            var normalized = dict.
                Select(c => new Point(c.X - minX, c.Y - minY)).
                Select(c => new Point((int)Math.Round((float) (c.X) / (maxX - minX) * (consoleWidth - 1)), (int)Math.Round((float) (c.Y) / (maxY - minY) * (actualConsoleHeight - 1)))).ToArray();
            Func<int, int, bool> IsHit = (hx, hy) => {
                return normalized.Any(c => c.X == hx && c.Y == hy);
            };

            for (int y = actualConsoleHeight - 1; y > 0; y -= 2)
            {
                Console.Write(y == actualConsoleHeight - 1 ? '┌' : '│');
                for (int x = 0; x < consoleWidth; x++)
                {
                    bool hitTop = IsHit(x, y);
                    bool hitBottom = IsHit(x, y - 1);                    
                    if (hitBottom && hitTop)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write('█');
                    }
                    else if (hitTop)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write('▀');
                    }
                    else if (hitBottom)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write('▀');
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write('▀');
                    }                    
                }                
                Console.ResetColor();
                Console.WriteLine();
            }
            Console.WriteLine('└' + new string('─', (consoleWidth / 2) - 1) + '┴' + new string('─', (consoleWidth / 2) - 1) + '┘');
            Console.Write((dict.Min(x => x.X) + "/" + dict.Min(x => x.Y)).PadRight(consoleWidth / 3));
            Console.Write((dict.Max(x => x.X) / 2).ToString().PadLeft(consoleWidth / 3 / 2).PadRight(consoleWidth / 3));
            Console.WriteLine(dict.Max(x => x.X).ToString().PadLeft(consoleWidth / 3));
        }    
        public static BigFloat modBF(BigFloat x, BigFloat y)
        {
            return x % y;
        }
        public static BigFloat modINT(BigFloat x, BigFloat y)
        {
            return x % y;
        }
    }


    public static class ExtensionMethods 
    {

        public static float map (this float value, float fromSource, float toSource, float fromTarget, float toTarget) 
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        public static int map (this int value, int fromSource, int toSource, int fromTarget, int toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }
    
    }
}