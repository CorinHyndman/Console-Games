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

bool gameOver = false;
Queue<(int x, int y)> snakeBody = new();
State snakeState = State.Neutral;
(int x, int y) applePos = (WIDTH / 2, HEIGHT / 2);
(int x, int y) snakeHead = (WIDTH / 2, HEIGHT / 2);
TimeSpan gameDelay = TimeSpan.FromMilliseconds(120);

try
{
	Console.CursorVisible = false;
	while (snakeState is State.Neutral)
	{
		DrawBorder();
		Console.SetCursorPosition(0,1);
		Console.WriteLine("║Console Snake" + Environment.NewLine);
		Console.WriteLine("║   Controls  " + Environment.NewLine);
		Console.WriteLine("║   W      ^  " + Environment.NewLine);
		Console.WriteLine("║ A S D  < V >" + Environment.NewLine);
		Console.WriteLine("║Press Any Direction to start");

		DrawSnake(ConsoleColor.Green);
		snakeState = GetSnakeState();

		Console.ResetColor();
		Console.Clear();
	}

	DrawBorder();
	RelocateApple();

	while (!gameOver)
	{
		if (Console.KeyAvailable)
		{
			snakeState = GetSnakeState();
		}
		while (Console.KeyAvailable)
		{
			Console.ReadKey(true);
		}

		snakeBody.Enqueue(snakeHead);
		DrawSnake(ConsoleColor.Black);

		switch (snakeState)
		{
			case State.Up: snakeHead.y--; break;
			case State.Down: snakeHead.y++; break;
			case State.Left: snakeHead.x--; break;
			case State.Right: snakeHead.x++; break;
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

		if (snakeBody.Contains(snakeHead) ||
			snakeHead.x is 0 || snakeHead.x is WIDTH + 1 ||
			snakeHead.y is 0 || snakeHead.y is HEIGHT + 1)
		{
			Console.ResetColor();
			Console.SetCursorPosition(0, 1);
			Console.WriteLine( "║Game Over");
			Console.WriteLine($"║Score: {snakeBody.Count}");
			Console.WriteLine( "║Press [Enter] To Exit");
			Console.ReadLine();
			return;
		}

		Thread.Sleep(gameDelay);
	}

	void DrawBorder()
	{
		string borderEmptyPadding  = new(' ', WIDTH);
		string borderFilledPadding = new('═', WIDTH);

		Console.WriteLine($"╔{borderFilledPadding}╗");
		for (int i = 0; i < HEIGHT; i++)
		{
			Console.WriteLine($"║{borderEmptyPadding}║");
		}
		Console.WriteLine($"╚{borderFilledPadding}╝");
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
	}
	void RelocateApple()
	{
		Random rng = new();

		applePos.x = rng.Next(1, WIDTH);
		applePos.y = rng.Next(1, HEIGHT);

		while (snakeHead == applePos || snakeBody.Contains(applePos))
		{
			applePos.x = rng.Next(1, WIDTH);
			applePos.y = rng.Next(1, HEIGHT);
		}

		Console.SetCursorPosition(applePos.x, applePos.y);
		Console.ForegroundColor = ConsoleColor.Red;
		Console.Write('@');
	}

	State GetSnakeState()
	{
		return Console.ReadKey(true).Key switch
		{
			ConsoleKey.UpArrow or ConsoleKey.W => State.Up,
			ConsoleKey.DownArrow or ConsoleKey.S => State.Down,
			ConsoleKey.LeftArrow or ConsoleKey.A => State.Left,
			ConsoleKey.RightArrow or ConsoleKey.D => State.Right,
			ConsoleKey.Escape => State.Exit,
			_ => State.Neutral
		};
	}
}
finally
{
	Console.CursorVisible = true;
	Console.ResetColor();
	Console.Clear();
}
enum State
{
	Up,
	Down,
	Left,
	Right,
	Neutral,
	Exit
}