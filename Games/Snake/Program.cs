using System;
using System.Collections.Generic;
using System.Threading;

const int WIDTH = 40;
const int HEIGHT = 15;

if (Console.WindowWidth < WIDTH + 2 ||
	Console.WindowHeight < HEIGHT + 2)
{
	Console.WriteLine("Console size too small.");
	Console.WriteLine("Please resize Console and try again");
	return;
}

try
{
	bool gameOver = false;
	Queue<(int x, int y)> snakeBody = new();
	Direction snakeDirection = Direction.Neutral;
	(int x, int y) applePos = (WIDTH / 2, HEIGHT / 2);
	(int x, int y) snakeHead = (WIDTH / 2, HEIGHT / 2);
	TimeSpan gameDelay = TimeSpan.FromMilliseconds(120);

	while (snakeDirection == Direction.Neutral)
	{
		Console.WriteLine("        Console Snake");
		Console.WriteLine(Environment.NewLine + "          Controls");
		Console.WriteLine(Environment.NewLine + "          W      ^");
		Console.WriteLine("        A S D  < V >");
		Console.WriteLine(Environment.NewLine + "Press Any Direction to start");

		GetSnakeDirection();
		Console.Clear();
	}

	snakeBody.Enqueue(snakeHead);
	Console.CursorVisible = false;
	Console.ResetColor();
	Console.Clear();

	DrawBorder(WIDTH, HEIGHT);
	RelocateApple();

	while (!gameOver)
	{
		if (Console.KeyAvailable)
		{
			GetSnakeDirection();
 		}
		while (Console.KeyAvailable)
		{
			Console.ReadKey(true);
		}

		snakeBody.Enqueue(snakeHead);
		DrawSnake(ConsoleColor.Black);

		switch (snakeDirection)
		{
			case Direction.Up: snakeHead.y--; break;
			case Direction.Down: snakeHead.y++; break;
			case Direction.Left: snakeHead.x--; break;
			case Direction.Right: snakeHead.x++; break;
		}
		
		if (snakeBody.Contains(snakeHead) || 
			snakeHead.x is 0 || snakeHead.x is WIDTH + 1 ||
			snakeHead.y is 0 || snakeHead.y is HEIGHT + 1)
		{
			gameOver = true;
		}

		if (snakeHead == applePos && !gameOver)
		{
			RelocateApple();
		}
		else
		{
			snakeBody.Dequeue();
		}

		DrawSnake(ConsoleColor.Green);
		Thread.Sleep(gameDelay);
	}

	for (int i = 1; i < 15; i++)
	{
		DrawSnake((ConsoleColor)i);

		Console.ForegroundColor = ConsoleColor.White;

		int y = 1;
		Console.SetCursorPosition(1, y);
		Console.Write("Game Over :]");
		Console.SetCursorPosition(1, ++y);
		Console.Write($"Score: {snakeBody.Count}");
		Console.SetCursorPosition(1, ++y);
		Console.Write("Press [Enter] To Exit!");

		Thread.Sleep(TimeSpan.FromMilliseconds(75));
	}

	Console.ReadLine();

	void DrawBorder(int width, int height)
	{
		string borderBlank = new (' ', width);
		string borderFilled = new ('═', width);

		Console.WriteLine($"╔{borderFilled}╗");
		for (int i = 0; i < height; i++)
		{
			Console.WriteLine($"║{borderBlank}║");
		}
		Console.WriteLine($"╚{borderFilled}╝");
	}
	void DrawSnake(ConsoleColor color)
	{
		Console.BackgroundColor = color;
		Console.ForegroundColor = ConsoleColor.White;

		Console.SetCursorPosition(snakeHead.x, snakeHead.y);
		Console.Write(color is ConsoleColor.Black ? ' ' : '"');
		foreach ((int x, int y) in snakeBody)
		{
			Console.SetCursorPosition(x, y);
			Console.Write(' ');
		}

		Console.BackgroundColor = ConsoleColor.Black;
	}
	void GetSnakeDirection()
	{
		snakeDirection = Console.ReadKey(true).Key switch
		{
			ConsoleKey.UpArrow or ConsoleKey.W => Direction.Up,
			ConsoleKey.DownArrow or ConsoleKey.S => Direction.Down,
			ConsoleKey.LeftArrow or ConsoleKey.A => Direction.Left,
			ConsoleKey.RightArrow or ConsoleKey.D => Direction.Right,
			_ => Direction.Neutral
		};
	}
	void RelocateApple()
	{
		Random rng = new();

		applePos.x = rng.Next(1, WIDTH + 1);
		applePos.y = rng.Next(1, HEIGHT + 1);

		while (snakeHead == applePos || snakeBody.Contains(applePos))
		{
			applePos.x = rng.Next(1, WIDTH + 1);
			applePos.y = rng.Next(1, HEIGHT + 1);
		}

		Console.SetCursorPosition(applePos.x, applePos.y);
		Console.ForegroundColor = ConsoleColor.Red;
		Console.Write('@');
	}
}
finally
{
	Console.CursorVisible = true;
	Console.ResetColor();
	Console.Clear();
}
enum Direction
{
	Up,
	Down,
	Left,
	Right,
	Neutral
}