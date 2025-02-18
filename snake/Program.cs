using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
///█ ■
////https://www.youtube.com/watch?v=SGZgvMwjq2U
namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 16;
            Console.WindowWidth = 32;
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;
            Random randomNumber = new Random();
            int score = 5;
            Boolean gameOver = false;
            pixel snakeBodyPixel = new pixel();
            snakeBodyPixel.xPosition = screenWidth/2;
            snakeBodyPixel.yPosition = screenHeight/2;
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
                if (snakeBodyPixel.xPosition == screenWidth-1 || snakeBodyPixel.xPosition == 0 ||snakeBodyPixel.yPosition == screenHeight-1 || snakeBodyPixel.yPosition == 0)
                { 
                    gameOver = true;
                }
                for (int i = 0;i< screenWidth; i++)
                {
                    Console.SetCursorPosition(i, 0);
                    Console.Write("■");
                }
                for (int i = 0; i < screenWidth; i++)
                {
                    Console.SetCursorPosition(i, screenHeight -1);
                    Console.Write("■");
                }
                for (int i = 0; i < screenHeight; i++)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write("■");
                }
                for (int i = 0; i < screenHeight; i++)
                {
                    Console.SetCursorPosition(screenWidth - 1, i);
                    Console.Write("■");
                }
                Console.ForegroundColor = ConsoleColor.Green;
                if (berryPositionX == snakeBodyPixel.xPosition && berryPositionY == snakeBodyPixel.yPosition)
                {
                    score++;
                    berryPositionX = randomNumber.Next(1, screenWidth-2);
                    berryPositionY = randomNumber.Next(1, screenHeight-2);
                } 
                for (int i = 0; i < snakeBodyPositionX.Count(); i++)
                {
                    Console.SetCursorPosition(snakeBodyPositionX[i], snakeBodyPositionY[i]);
                    Console.Write("■");
                    if (snakeBodyPositionX[i] == snakeBodyPixel.xPosition && snakeBodyPositionY[i] == snakeBodyPixel.yPosition)
                    {
                        gameOver = true;
                    }
                }
                if (gameOver == true)
                {
                    break;
                }
                Console.SetCursorPosition(snakeBodyPixel.xPosition, snakeBodyPixel.yPosition);
                Console.ForegroundColor = snakeBodyPixel.snakeHead;
                Console.Write("■");
                Console.SetCursorPosition(berryPositionX, berryPositionY);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("■");
                startTime = DateTime.Now;
                buttonPressed = "no";
                while (true)
                {
                    refreshTime = DateTime.Now;
                    if (refreshTime.Subtract(startTime).TotalMilliseconds > 500) { break; }
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo keyboardInput = Console.ReadKey(true);
                        //Console.WriteLine(toets.Key.ToString());
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
                }
                snakeBodyPositionX.Add(snakeBodyPixel.xPosition);
                snakeBodyPositionY.Add(snakeBodyPixel.yPosition);
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
                if (snakeBodyPositionX.Count() > score)
                {
                    snakeBodyPositionX.RemoveAt(0);
                    snakeBodyPositionY.RemoveAt(0);
                }
            }
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: "+ score);
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 +1);
        }
        class pixel
        {
            public int xPosition { get; set; }
            public int yPosition { get; set; }
            public ConsoleColor snakeHead { get; set; }
        }
    }
}
//¦