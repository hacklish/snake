using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
///█ ■
namespace Snake
{
    public enum Direction
    {
        NORTH,
        SOUTH,
        WEST,
        EAST,
    }

    class Program
    {
        static void Main(string[] args)
        {
            int screenWidth = 32;
            int screenHeight = 16;
            Console.WindowHeight = Math.Max(screenHeight, Console.WindowHeight);
            Console.WindowWidth = Math.Max(screenWidth, Console.WindowWidth);

            Random randomNummer = new Random();
            int snakeLenght = 5;
            int score = 0;
            int gameover = 0;
            var startPosition = new Point(screenWidth / 2, screenHeight / 2);
            var headPosition = new Pixel();
            headPosition.X = startPosition.X;
            headPosition.Y = startPosition.Y;
            headPosition.Color = ConsoleColor.Red;
            var movement = Direction.EAST;
            var tail = new List<Point>();
            var berryPosition = new Pixel();
            berryPosition.X = randomNummer.Next(1, screenWidth - 2);
            berryPosition.Y = randomNummer.Next(1, screenHeight - 2);
            berryPosition.Color = ConsoleColor.Cyan;

            Console.Clear();
            String wallLine = new String('■', screenWidth);
            Console.SetCursorPosition(0, 0);
            Console.Write(wallLine);
            Console.SetCursorPosition(0, screenHeight - 1);
            Console.Write(wallLine);

            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(screenWidth - 1, i);
                Console.Write("■");
            }

            while (true)
            {
                if (headPosition.X == screenWidth-1 || headPosition.X == 0 ||headPosition.Y == screenHeight-1 || headPosition.Y == 0)
                {
                    gameover = 1;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                if (berryPosition.X == headPosition.X && berryPosition.Y == headPosition.Y)
                {
                    score++;
                    snakeLenght++;
                    berryPosition.X = randomNummer.Next(1, screenWidth-2);
                    berryPosition.Y = randomNummer.Next(1, screenHeight-2);
                }

                foreach (Point tailPoint in tail)
                {
                    Console.SetCursorPosition(tailPoint.X, tailPoint.Y);
                    Console.Write("■");
                    if (tailPoint.X == headPosition.X && tailPoint.Y == headPosition.Y)
                    {
                        gameover = 1;
                    }
                }
                if (gameover == 1)
                {
                    break;
                }
                Console.SetCursorPosition(headPosition.X, headPosition.Y);
                Console.ForegroundColor = headPosition.Color;
                Console.Write("■");
                Console.SetCursorPosition(berryPosition.X , berryPosition.Y);
                Console.ForegroundColor = berryPosition.Color;
                Console.Write("■");
                DateTime tijd = DateTime.Now;
                while (true)
                {
                    DateTime tijd2 = DateTime.Now;
                    if (tijd2.Subtract(tijd).TotalMilliseconds > 500) { break; }
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        //Console.WriteLine(toets.Key.ToString());
                        if (key.Equals(ConsoleKey.UpArrow) && movement != Direction.SOUTH)
                        {
                            movement = Direction.NORTH;
                        }
                        if (key.Equals(ConsoleKey.DownArrow) && movement != Direction.NORTH)
                        {
                            movement = Direction.SOUTH;
                        }
                        if (key.Equals(ConsoleKey.LeftArrow) && movement != Direction.EAST)
                        {
                            movement = Direction.WEST;
                        }
                        if (key.Equals(ConsoleKey.RightArrow) && movement != Direction.WEST)
                        {
                            movement = Direction.EAST;
                        }
                    }
                }
                tail.Add(new Point(headPosition.X, headPosition.Y));
                switch (movement)
                {
                    case Direction.NORTH:
                        headPosition.Y--;
                        break;
                    case Direction.SOUTH:
                        headPosition.Y++;
                        break;
                    case Direction.WEST:
                        headPosition.X--;
                        break;
                    case Direction.EAST:
                        headPosition.X++;
                        break;
                }
                if (tail.Count() > snakeLenght)
                {
                    Console.SetCursorPosition(tail[0].X, tail[0].Y);
                    Console.Write(' ');
                    tail.RemoveAt(0);
                }
            }
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: "+ score);
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 +1);
        }
        struct Pixel
        {
            public int X { get; set; }
            public int Y { get; set; }
            public ConsoleColor Color { get; set; }
        }
    }
}
//¦
