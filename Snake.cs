using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;

namespace Snake
{
    public enum Direction
    {
        NORTH,
        SOUTH,
        WEST,
        EAST,
    }

    class Snake
    {
        private List<Point> _body = new List<Point>();
        private const int _startLength = 5;
        private int _length = _startLength;

        public Snake(Point pt)
        {
            _body.Add(pt);
        }

        public Snake(int startX, int startY)
        {
            _body.Add(new Point(startX, startY));
        }

        public int GetScore()
        {
            return _length - _startLength;
        }

        public Point GetHeadPosition()
        {
            return _body[0];
        }

        public List<Point> GetTail()
        {
            return _body.GetRange(1, _body.Count - 1);
        }

        public void Grow()
        {
            ++_length;
        }

        public void Move(Direction where, int howMuch)
        {
            Point diff = DirectionToOffset(where);

            Point head = GetHeadPosition();
            for (int i = 0; i < howMuch; ++i)
            {
                head.Offset(diff);
                _body.Insert(0, head);
            }

            ClearExcess();
        }

        private Point DirectionToOffset(Direction stepDirection)
        {
            var diff = new Point(0, 0);
            switch (stepDirection)
            {
                case Direction.NORTH:
                    diff.Y = -1;
                    break;

                case Direction.SOUTH:
                    diff.Y = 1;
                    break;

                case Direction.EAST:
                    diff.X = 1;
                    break;

                case Direction.WEST:
                    diff.X = -1;
                    break;
            }

            return diff;
        }

        private void ClearExcess()
        {
            int excess = _body.Count - _length;
            for (int i = 0; i < excess; ++i)
            {
                // FIXME: console clean-up, should be somwhere else
                Point bodyElement = _body[_body.Count - 1];
                Console.SetCursorPosition(bodyElement.X, bodyElement.Y);
                Console.Write(' ');

                _body.RemoveAt(_body.Count - 1);
            }
        }
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
            int gameover = 0;
            var startPosition = new Point(screenWidth / 2, screenHeight / 2);
            var headColor = ConsoleColor.Red;
            var movement = Direction.EAST;
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

            Snake snake = new Snake(startPosition);

            while (true)
            {
                var headPosition = snake.GetHeadPosition();
                if (headPosition.X == screenWidth-1 || headPosition.X == 0 ||headPosition.Y == screenHeight-1 || headPosition.Y == 0)
                {
                    gameover = 1;
                }

                if (berryPosition.X == headPosition.X && berryPosition.Y == headPosition.Y)
                {
                    snake.Grow();
                    berryPosition.X = randomNummer.Next(1, screenWidth-2);
                    berryPosition.Y = randomNummer.Next(1, screenHeight-2);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                List<Point> tail = snake.GetTail();
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
                Console.ForegroundColor = headColor;
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

                snake.Move(movement, 1);
            }

            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + snake.GetScore());
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
