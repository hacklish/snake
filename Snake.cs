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
            Console.WindowHeight = 16;
            Console.WindowWidth = 32;
            int screenwidth = Console.WindowWidth;
            int screenheight = Console.WindowHeight;
            Random randomnummer = new Random();
            int score = 5;
            int gameover = 0;
            var startPosition = new Point(screenwidth / 2, screenheight / 2);
            var headPosition = new Pixel();
            headPosition.xpos = startPosition.X;
            headPosition.ypos = startPosition.Y;
            headPosition.schermkleur = ConsoleColor.Red;
            string movement = "RIGHT";
            List<int> xposlijf = new List<int>();
            List<int> yposlijf = new List<int>();
            var berryPosition = new Pixel();
            berryPosition.xpos = randomnummer.Next(0, screenwidth);
            berryPosition.ypos = randomnummer.Next(0, screenheight);
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
                for (int i = 0;i< screenwidth; i++)
                {
                    Console.SetCursorPosition(i, 0);
                    Console.Write("■");
                }
                for (int i = 0; i < screenwidth; i++)
                {
                    Console.SetCursorPosition(i, screenheight -1);
                    Console.Write("■");
                }
                for (int i = 0; i < screenheight; i++)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write("■");
                }
                for (int i = 0; i < screenheight; i++)
                {
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
                for (int i = 0; i < xposlijf.Count(); i++)
                {
                    Console.SetCursorPosition(xposlijf[i], yposlijf[i]);
                    Console.Write("■");
                    if (xposlijf[i] == headPosition.xpos && yposlijf[i] == headPosition.ypos)
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
                Console.ForegroundColor = ConsoleColor.Cyan;
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
                xposlijf.Add(headPosition.xpos);
                yposlijf.Add(headPosition.ypos);
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
                if (xposlijf.Count() > score)
                {
                    xposlijf.RemoveAt(0);
                    yposlijf.RemoveAt(0);
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
