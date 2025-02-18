using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            int screenWidth = 32;
            int screenHeight = 16;
            int score = 5;
            bool gameOver = false;

            Random randomNumber = new Random();
            pixel snakeBodyPixel = new pixel();
            snakeBodyPixel.xPosition = screenWidth / 2;
            snakeBodyPixel.yPosition = screenHeight / 2;
            snakeBodyPixel.snakeHead = ConsoleColor.Red;
            string direction = "RIGHT";
            List<int> snakeBodyPositionX = new List<int>();
            List<int> snakeBodyPositionY = new List<int>();
            int berryPositionX = randomNumber.Next(0, screenWidth);
            int berryPositionY = randomNumber.Next(0, screenHeight);
            DateTime startTime;
            DateTime refreshTime;
            string buttonPressed = "no";

            while (true)
            {
                Console.Clear();

                DrawBorder(screenWidth, screenHeight);

                gameOver = CheckCollision(snakeBodyPixel.xPosition, snakeBodyPixel.yPosition, screenWidth, screenHeight, snakeBodyPositionX, snakeBodyPositionY);

                if (gameOver)
                {
                    break;
                }

                DrawGameObjects(snakeBodyPixel, snakeBodyPositionX, snakeBodyPositionY, berryPositionX, berryPositionY);

                if (berryPositionX == snakeBodyPixel.xPosition && berryPositionY == snakeBodyPixel.yPosition)
                {
                    score++;
                    berryPositionX = randomNumber.Next(1, screenWidth - 2);
                    berryPositionY = randomNumber.Next(1, screenHeight - 2);
                }

                startTime = DateTime.Now;
                buttonPressed = "no";
                while (true)
                {
                    refreshTime = DateTime.Now;
                    if (refreshTime.Subtract(startTime).TotalMilliseconds > 500) { break; }
                    if (Console.KeyAvailable)
                    {
                        direction = UpdateDirection(direction, ref buttonPressed);
                    }
                }

                snakeBodyPositionX.Add(snakeBodyPixel.xPosition);
                snakeBodyPositionY.Add(snakeBodyPixel.yPosition);

                snakeBodyPixel = MoveSnake(snakeBodyPixel, direction);

                if (snakeBodyPositionX.Count() > score)
                {
                    snakeBodyPositionX.RemoveAt(0);
                    snakeBodyPositionY.RemoveAt(0);
                }
            }

            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
        }

        static void DrawBorder(int screenWidth, int screenHeight)
        {
            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, screenHeight - 1);
                Console.Write("■");
            }
            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(screenWidth - 1, i);
                Console.Write("■");
            }
        }

        static void DrawGameObjects(pixel snakeBodyPixel, List<int> snakeBodyPositionX, List<int> snakeBodyPositionY, int berryPositionX, int berryPositionY)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < snakeBodyPositionX.Count(); i++)
            {
                Console.SetCursorPosition(snakeBodyPositionX[i], snakeBodyPositionY[i]);
                Console.Write("■");
            }

            Console.SetCursorPosition(snakeBodyPixel.xPosition, snakeBodyPixel.yPosition);
            Console.ForegroundColor = snakeBodyPixel.snakeHead;
            Console.Write("■");

            Console.SetCursorPosition(berryPositionX, berryPositionY);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("■");
        }

        static bool CheckCollision(int xPosition, int yPosition, int screenWidth, int screenHeight, List<int> snakeBodyPositionX, List<int> snakeBodyPositionY)
        {
            if (xPosition == screenWidth - 1 || xPosition == 0 || yPosition == screenHeight - 1 || yPosition == 0)
            {
                return true;
            }
            for (int i = 0; i < snakeBodyPositionX.Count(); i++)
            {
                if (snakeBodyPositionX[i] == xPosition && snakeBodyPositionY[i] == yPosition)
                {
                    return true;
                }
            }
            return false;
        }

        static string UpdateDirection(string direction, ref string buttonPressed)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyboardInput = Console.ReadKey(true);
                if (keyboardInput.Key.Equals(ConsoleKey.UpArrow) && direction != "DOWN" && buttonPressed == "no")
                {
                    direction = "UP";
                    buttonPressed = "yes";
                }
                if (keyboardInput.Key.Equals(ConsoleKey.DownArrow) && direction != "UP" && buttonPressed == "no")
                {
                    direction = "DOWN";
                    buttonPressed = "yes";
                }
                if (keyboardInput.Key.Equals(ConsoleKey.LeftArrow) && direction != "RIGHT" && buttonPressed == "no")
                {
                    direction = "LEFT";
                    buttonPressed = "yes";
                }
                if (keyboardInput.Key.Equals(ConsoleKey.RightArrow) && direction != "LEFT" && buttonPressed == "no")
                {
                    direction = "RIGHT";
                    buttonPressed = "yes";
                }
            }
            return direction;
        }

        static pixel MoveSnake(pixel snakeBodyPixel, string direction)
        {
            switch (direction)
            {
                case "UP":
                    snakeBodyPixel.yPosition--;
                    break;
                case "DOWN":
                    snakeBodyPixel.yPosition++;
                    break;
                case "LEFT":
                    snakeBodyPixel.xPosition--;
                    break;
                case "RIGHT":
                    snakeBodyPixel.xPosition++;
                    break;
            }
            return snakeBodyPixel;
        }

        class pixel
        {
            public int xPosition { get; set; }
            public int yPosition { get; set; }
            public ConsoleColor snakeHead { get; set; }
        }
    }
}
