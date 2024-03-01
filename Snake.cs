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
    class Program
    {
        static void Main(string[] args)
        {
            int screenwidth = 32;
            int screenheight = 16;
            Console.WindowHeight = Math.Max(screenheight, Console.WindowHeight);
            Console.WindowWidth = Math.Max(screenwidth, Console.WindowWidth);

            Random randomnummer = new Random();
            int score = 5;
            int gameover = 0;
            var startPosition = new Point(screenwidth / 2, screenheight / 2);
            var headPosition = new Pixel();
            headPosition.xpos = startPosition.X;
            headPosition.ypos = startPosition.Y;
            headPosition.schermkleur = ConsoleColor.Red;
            string movement = "RIGHT";
            var tail = new List<Point>();
            var berryPosition = new Pixel();
            berryPosition.xpos = randomnummer.Next(1, screenwidth - 2);
            berryPosition.ypos = randomnummer.Next(1, screenheight - 2);
            berryPosition.schermkleur = ConsoleColor.Cyan;
            DateTime tijd = DateTime.Now;
            DateTime tijd2 = DateTime.Now;
            string buttonpressed = "no";
            while (true)
            {
                Console.Clear();
                if (headPosition.xpos == screenwidth-1 || headPosition.xpos == 0 ||headPosition.ypos == screenheight-1 || headPosition.ypos == 0)
                { 
                    gameover = 1;
                }

                String wallLine = new String('■', screenwidth);
                Console.SetCursorPosition(0, 0);
                Console.Write(wallLine);
                Console.SetCursorPosition(0, screenheight - 1);
                Console.Write(wallLine);

                for (int i = 0; i < screenheight; i++)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write("■");
                    Console.SetCursorPosition(screenwidth - 1, i);
                    Console.Write("■");
                }
                Console.ForegroundColor = ConsoleColor.Green;
                if (berryPosition.xpos == headPosition.xpos && berryPosition.ypos == headPosition.ypos)
                {
                    score++;
                    berryPosition.xpos = randomnummer.Next(1, screenwidth-2);
                    berryPosition.ypos = randomnummer.Next(1, screenheight-2);
                }

                foreach (Point tailPoint in tail)
                {
                    Console.SetCursorPosition(tailPoint.X, tailPoint.Y);
                    Console.Write("■");
                    if (tailPoint.X == headPosition.xpos && tailPoint.Y == headPosition.ypos)
                    {
                        gameover = 1;
                    }
                }
                if (gameover == 1)
                {
                    break;
                }
                Console.SetCursorPosition(headPosition.xpos, headPosition.ypos);
                Console.ForegroundColor = headPosition.schermkleur;
                Console.Write("■");
                Console.SetCursorPosition(berryPosition.xpos , berryPosition.ypos);
                Console.ForegroundColor = berryPosition.schermkleur;
                Console.Write("■");
                tijd = DateTime.Now;
                buttonpressed = "no";
                while (true)
                {
                    tijd2 = DateTime.Now;
                    if (tijd2.Subtract(tijd).TotalMilliseconds > 500) { break; }
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo toets = Console.ReadKey(true);
                        //Console.WriteLine(toets.Key.ToString());
                        if (toets.Key.Equals(ConsoleKey.UpArrow) && movement != "DOWN" && buttonpressed == "no")
                        {
                            movement = "UP";
                            buttonpressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.DownArrow) && movement != "UP" && buttonpressed == "no")
                        {
                            movement = "DOWN";
                            buttonpressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.LeftArrow) && movement != "RIGHT" && buttonpressed == "no")
                        {
                            movement = "LEFT";
                            buttonpressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.RightArrow) && movement != "LEFT" && buttonpressed == "no")
                        {
                            movement = "RIGHT";
                            buttonpressed = "yes";
                        }
                    }
                }
                tail.Add(new Point(headPosition.xpos, headPosition.ypos));
                switch (movement)
                {
                    case "UP":
                        headPosition.ypos--;
                        break;
                    case "DOWN":
                        headPosition.ypos++;
                        break;
                    case "LEFT":
                        headPosition.xpos--;
                        break;
                    case "RIGHT":
                        headPosition.xpos++;
                        break;
                }
                if (tail.Count() > score)
                {
                    tail.RemoveAt(0);
                }
            }
            Console.SetCursorPosition(screenwidth / 5, screenheight / 2);
            Console.WriteLine("Game over, Score: "+ score);
            Console.SetCursorPosition(screenwidth / 5, screenheight / 2 +1);
        }
        class Pixel
        {
            public int xpos { get; set; }
            public int ypos { get; set; }
            public ConsoleColor schermkleur { get; set; }
        }
    }
}
//¦
