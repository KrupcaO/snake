using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new SnakeGame(32, 16);
            game.Start();
        }
    }

    class SnakeGame
    {
        private int _screenWidth;
        private int _screenHeight;
        private int _score;
        private bool _gameOver;
        private Random _random;
        private Snake _snake;
        private Berry _berry;
        private string _direction;
        private string _buttonPressed;

        public SnakeGame(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _score = 5;
            _gameOver = false;
            _random = new Random();
            _snake = new Snake(screenWidth / 2, screenHeight / 2);
            _berry = new Berry(_random.Next(1, screenWidth - 1), _random.Next(1, screenHeight - 1));
            _direction = "RIGHT";
            _buttonPressed = "no";
        }

        public void Start()
        {
            while (!_gameOver)
            {
                Console.Clear();
                DrawBorder();
                _gameOver = CheckCollision();

                if (_gameOver)
                {
                    break;
                }

                DrawGameObjects();
                HandleBerryCollision();
                HandleInput();
                MoveSnake();
                RemoveTailIfNeeded();
            }

            EndGame();
        }

        private void DrawBorder()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int i = 0; i < _screenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, _screenHeight - 1);
                Console.Write("■");
            }
            for (int i = 0; i < _screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(_screenWidth - 1, i);
                Console.Write("■");
            }
        }

        private void DrawGameObjects()
        {
            _snake.DrawSnake();
            _berry.DrawBerry(_snake.HeadX, _snake.HeadY);
        }

        private void HandleBerryCollision()
        {
            if (_snake.HeadX == _berry.X && _snake.HeadY == _berry.Y)
            {
                _score++;
                _berry.RespawnBerry(_random.Next(1, _screenWidth - 1), _random.Next(1, _screenHeight - 1));
            }
        }

        private void HandleInput()
        {
            DateTime startTime = DateTime.Now;
            _buttonPressed = "no";

            while (true)
            {
                if (DateTime.Now.Subtract(startTime).TotalMilliseconds > 500) { break; }

                if (Console.KeyAvailable)
                {
                    _direction = UpdateDirection();
                }
            }
        }

        private string UpdateDirection()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyboardInput = Console.ReadKey(true);
                if (keyboardInput.Key.Equals(ConsoleKey.UpArrow) && _direction != "DOWN" && _buttonPressed == "no")
                {
                    _direction = "UP";
                    _buttonPressed = "yes";
                }
                if (keyboardInput.Key.Equals(ConsoleKey.DownArrow) && _direction != "UP" && _buttonPressed == "no")
                {
                    _direction = "DOWN";
                    _buttonPressed = "yes";
                }
                if (keyboardInput.Key.Equals(ConsoleKey.LeftArrow) && _direction != "RIGHT" && _buttonPressed == "no")
                {
                    _direction = "LEFT";
                    _buttonPressed = "yes";
                }
                if (keyboardInput.Key.Equals(ConsoleKey.RightArrow) && _direction != "LEFT" && _buttonPressed == "no")
                {
                    _direction = "RIGHT";
                    _buttonPressed = "yes";
                }
            }
            return _direction;
        }

        private void MoveSnake()
        {
            _snake.MoveSnake(_direction);
        }

        private void RemoveTailIfNeeded()
        {
            if (_snake.BodyCount > _score)
            {
                _snake.RemoveTail();
            }
        }

        private bool CheckCollision()
        {
            if (_snake.HeadX == _screenWidth - 1 || _snake.HeadX == 0 || _snake.HeadY == _screenHeight - 1 || _snake.HeadY == 0)
            {
                return true;
            }
            if (_snake.HasBodyCollision())
            {
                return true;
            }
            return false;
        }

        private void EndGame()
        {
            Console.SetCursorPosition(_screenWidth / 5, _screenHeight / 2);
            Console.WriteLine($"Game over, Score: {_score}");
        }
    }

    class Snake
    {
        private List<int> _bodyX;
        private List<int> _bodyY;
        public int HeadX => _bodyX.Last();
        public int HeadY => _bodyY.Last();
        public int BodyCount => _bodyX.Count;

        public Snake(int startX, int startY)
        {
            _bodyX = new List<int> { startX };
            _bodyY = new List<int> { startY };
        }

        public void DrawSnake()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < _bodyX.Count - 1; i++)
            {
                Console.SetCursorPosition(_bodyX[i], _bodyY[i]);
                Console.Write("■");
            }


            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(HeadX, HeadY);
            Console.Write("■");
        }

        public void MoveSnake(string direction)
        {
            int newHeadX = HeadX;
            int newHeadY = HeadY;

            switch (direction)
            {
                case "UP": newHeadY--; break;
                case "DOWN": newHeadY++; break;
                case "LEFT": newHeadX--; break;
                case "RIGHT": newHeadX++; break;
            }

            _bodyX.Add(newHeadX);
            _bodyY.Add(newHeadY);
        }

        public void RemoveTail()
        {
            _bodyX.RemoveAt(0);
            _bodyY.RemoveAt(0);
        }

        public bool HasBodyCollision()
        {
            for (int i = 0; i < _bodyX.Count - 1; i++)
            {
                if (_bodyX[i] == HeadX && _bodyY[i] == HeadY)
                {
                    return true;
                }
            }
            return false;
        }
    }

    class Berry
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Berry(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void DrawBerry(int snakeHeadX, int snakeHeadY)
        {
            if (X == snakeHeadX && Y == snakeHeadY)
            {
                return;
            }

            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("■");
        }

        public void RespawnBerry
            (int newX, int newY)
        {
            X = newX;
            Y = newY;
        }
    }
}
