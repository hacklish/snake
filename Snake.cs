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

    public enum TailType
    {
        EMPTY,
        HEAD,
        SNAKE,
        WALL,
        FOOD,
    }

    class GameRender
    {
        private ConsoleColor _defaultColor;

        public GameRender()
        {
            Console.Clear();
            _defaultColor = Console.BackgroundColor;
        }

        public void SetAt(Point at, TailType to)
        {
            Console.SetCursorPosition(at.X, at.Y);

            var color = _defaultColor;
            String body = "■";
            switch (to)
            {
                case TailType.EMPTY:
                    color = _defaultColor;
                    body = " ";
                    break;

                case TailType.FOOD:
                    color = ConsoleColor.Red;
                    break;

                case TailType.WALL: /* FALLTHROUGH */
                case TailType.SNAKE:
                    color = ConsoleColor.White;
                    break;

                case TailType.HEAD:
                    color = ConsoleColor.Green;
                    break;
            }

            Console.ForegroundColor = color;
            Console.Write(body);
        }
    }

    class GameBoard
    {
        private int _width;
        private int _height;
        private TailType[,] _tiles;
        private Random _random;
        private GameRender _render;

        public GameBoard(int width, int height)
        {
            _tiles = new TailType[width, height];
            _width = width;
            _height = height;
            _random = new Random();
            _render = new GameRender();

            ClearBoard();
            GenerateBerry();
        }

        private void ClearBoard()
        {
            for (int i = 0; i < _width; ++i)
            {
                for (int j = 0; j < _height; ++j)
                {
                    if (i == 0 || j == 0
                        || i == (_width - 1) || j == (_height - 1))
                    {
                        SetAt(new Point(i, j), TailType.WALL);
                    }
                    else
                    {
                        ClearAt(new Point(i, j));
                    }
                }
            }
        }

        public void ClearAt(Point clearPosition)
        {
            SetAt(clearPosition, TailType.EMPTY);
        }

        public void SetAt(Point at, TailType to)
        {
            _tiles[at.X, at.Y] = to;
            _render.SetAt(at, to);
        }

        public bool IsObstacleAt(Point evalPosition)
        {
            TailType obstacle = _tiles[evalPosition.X, evalPosition.Y];
            return (obstacle == TailType.SNAKE) ||
                   (obstacle == TailType.WALL);
        }

        public bool IsConsumableAt(Point evalPosition)
        {
            TailType tail = _tiles[evalPosition.X, evalPosition.Y];
            return tail == TailType.FOOD;
        }

        public Point GenerateBerry()
        {
            int x = _random.Next(1, _width - 2);
            int y = _random.Next(1, _height - 2);
            var berry = new Point(x, y);

            SetAt(berry, TailType.FOOD);
            return berry;
        }
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

        public List<Point> Move(Direction where, int howMuch)
        {
            Point diff = DirectionToOffset(where);

            Point head = GetHeadPosition();
            for (int i = 0; i < howMuch; ++i)
            {
                head.Offset(diff);
                _body.Insert(0, head);
            }

            return ClearExcess();
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

        private List<Point> ClearExcess()
        {
            List<Point> removed = new List<Point>();

            int excess = _body.Count - _length;
            for (int i = 0; i < excess; ++i)
            {
                int index = _body.Count - 1;
                Point bodyElement = _body[index];
                removed.Add(bodyElement);
                _body.RemoveAt(index);
            }

            return removed;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var movement = Direction.EAST;

            int screenWidth = 32;
            int screenHeight = 16;
            Console.WindowHeight = Math.Max(screenHeight, Console.WindowHeight);
            Console.WindowWidth = Math.Max(screenWidth, Console.WindowWidth);

            var board = new GameBoard(screenWidth, screenHeight);
            Snake snake = new Snake(screenWidth / 2, screenHeight / 2);

            while (true)
            {
                var headPosition = snake.GetHeadPosition();
                if (board.IsObstacleAt(headPosition))
                {
                    // GameOver
                    break;
                }

                if (board.IsConsumableAt(headPosition))
                {
                    board.GenerateBerry();
                    snake.Grow();
                }

                board.SetAt(headPosition, TailType.HEAD);
                List<Point> tail = snake.GetTail();
                foreach (Point tailPoint in tail)
                {
                    board.SetAt(tailPoint, TailType.SNAKE);
                }

                DateTime loopStart = DateTime.Now;
                while (true)
                {
                    DateTime currentTime = DateTime.Now;
                    if (currentTime.Subtract(loopStart).TotalMilliseconds > 500) { break; }

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

                var toClean = snake.Move(movement, 1);
                foreach (Point clearedPoint in toClean)
                {
                    board.ClearAt(clearedPoint);
                }
            }

            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + snake.GetScore());
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 +1);
        }
    }
}
