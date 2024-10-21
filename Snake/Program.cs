using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Snake
{
    internal class Program
    {
        private static int screenWidth = 40;
        private static int screenHeight = 20;
        private static List<Position> snake = new List<Position>();
        private static Position food;
        private static int score = 0;
        private static Direction currentDirection = Direction.Right;
        private static bool gameOver = false;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            InitializeGame();

            while (!gameOver)
            {
                Draw();
                Input();
                Logic();
                Thread.Sleep(100); 
            }

            Console.Clear();
            Console.WriteLine($"Game Over! Your score: {score}");
        }

        private static void InitializeGame()
        {
            snake.Add(new Position(screenWidth / 2, screenHeight / 2)); 
            PlaceFood();
        }

        private static void PlaceFood()
        {
            Random rand = new Random();
            food = new Position(rand.Next(0, screenWidth), rand.Next(0, screenHeight));
        }

        private static void Draw()
        {
            Console.Clear();

            // Draw borders
            for (int i = 0; i < screenWidth + 2; i++)
                Console.Write("#");
            Console.WriteLine();

            for (int y = 0; y < screenHeight; y++)
            {
                for (int x = 0; x < screenWidth; x++)
                {
                    if (x == 0)
                        Console.Write("#"); 

                    if (snake.Any(s => s.X == x && s.Y == y))
                        Console.Write("O"); 
                    else if (food.X == x && food.Y == y)
                        Console.Write("F"); 
                    else
                        Console.Write(" ");

                    if (x == screenWidth - 1)
                        Console.Write("#"); 
                }
                Console.WriteLine();
            }

            for (int i = 0; i < screenWidth + 2; i++)
                Console.Write("#");
            Console.WriteLine($"Score: {score}");
        }

        private static void Input()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentDirection != Direction.Down) currentDirection = Direction.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentDirection != Direction.Up) currentDirection = Direction.Down;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (currentDirection != Direction.Right) currentDirection = Direction.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        if (currentDirection != Direction.Left) currentDirection = Direction.Right;
                        break;
                }
            }
        }

        private static void Logic()
        {
            Position head = snake.First();
            Position newHead = new Position(head.X, head.Y);

            switch (currentDirection)
            {
                case Direction.Up:
                    newHead.Y--;
                    break;
                case Direction.Down:
                    newHead.Y++;
                    break;
                case Direction.Left:
                    newHead.X--;
                    break;
                case Direction.Right:
                    newHead.X++;
                    break;
            }

            // Check for collisions
            if (newHead.X < 0 || newHead.X >= screenWidth || newHead.Y < 0 || newHead.Y >= screenHeight || snake.Skip(1).Any(s => s.X == newHead.X && s.Y == newHead.Y))
            {
                gameOver = true;
                return;
            }

            snake.Insert(0, newHead);

            if (newHead.X == food.X && newHead.Y == food.Y)
            {
                score++;
                PlaceFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1); 
            }
        }

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        private struct Position
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
