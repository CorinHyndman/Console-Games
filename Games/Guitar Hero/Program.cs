using System;
using System.Threading;
using System.Collections.Generic;

const int WIDTH = 22;
const int HEIGHT = 15;

try
{
	string[] arrows =
	{
		 "{─",
		@"\/",
		@"/\",
		 "─}"
	};
	string board =
		"│    │    │    │    │" + '\n' +
		"│    │    │    │    │" + '\n' +
		"│    │    │    │    │" + '\n' +
		"│    │    │    │    │" + '\n' +
		"│    │    │    │    │" + '\n' +
		"│    │    │    │    │" + '\n' +
		"│    │    │    │    │" + '\n' +
		"│    │    │    │    │" + '\n' +
		"│    │    │    │    │" + '\n' +
		"│    │    │    │    │" + '\n' +
		"│    │    │    │    │" + '\n' +
		"│    │    │    │    │" + '\n' +
		"│    │    │    │    │" + '\n' +
		"│    │    │    │    │" + '\n' +
	   @"│ {─ │ \/ │ /\ │ ─} │";

	int score = 0;
	int frame = 0;
	Random rng = new();
	int fallSpeed = 120;
	bool gameOver = false;
	var allArrows = new List<Arrow>();
	var inputDirection = Direction.Neutral;

	Console.CursorVisible = false;

	while (!gameOver)
	{
		if (frame++ % 4 is 0)
		{
			int temp = rng.Next(0, 4);
			allArrows.Add(new Arrow(2 + (5 * temp), arrows[temp], (Direction)temp));
		}

		Thread.Sleep(fallSpeed);
		if (Console.KeyAvailable)
		{
			inputDirection = (Console.ReadKey(true).Key) switch
			{
				ConsoleKey.UpArrow or ConsoleKey.W => Direction.Up,
				ConsoleKey.DownArrow or ConsoleKey.A => Direction.Down,
				ConsoleKey.LeftArrow or ConsoleKey.S => Direction.Left,
				ConsoleKey.RightArrow or ConsoleKey.D => Direction.Right,
				_ => Direction.Neutral
			};
		}
		while (Console.KeyAvailable)
		{
			Console.ReadKey(true);
		}

		DrawBoard();
		Update();
	}

	void Update()
	{
		int? removeAt = null;
		foreach (var arrow in allArrows)
		{
			if (arrow.Y == HEIGHT)
			{
				removeAt = allArrows.IndexOf(arrow);
				if (inputDirection == arrow.Direction)
				{
					score += 100;
				}
				else
				{
					score -= 100;
				}
			}
			arrow.Update();
		}
		if (removeAt.HasValue)
		{
			allArrows.RemoveAt(removeAt.Value);
		}
	}
	void DrawBoard()
	{
		Console.SetCursorPosition(0, 0);
		Console.WriteLine($"Score: {score}   ");
		Console.WriteLine(board);

		foreach (var arrow in allArrows)
		{
			Console.ForegroundColor = arrow.Direction switch
			{
				Direction.Up => ConsoleColor.Yellow,
				Direction.Down => ConsoleColor.Red,
				Direction.Left => ConsoleColor.Green,
				Direction.Right => ConsoleColor.Blue,
				_ => ConsoleColor.White
			};
			Console.SetCursorPosition(arrow.X, arrow.Y);
			Console.WriteLine(arrow.Sprite);
		}
		Console.ResetColor();
	}
}
finally
{
	Console.CursorVisible = true;
	Console.ResetColor();
	Console.Clear();
}
class Arrow
{
	public int X;
	public int Y;
	public string Sprite;
	public Direction Direction;

	public Arrow(int x, string sprite, Direction direction)
	{
		X = x;
		Y = 1;
		Sprite = sprite;
		Direction = direction;
	}
	public void Update()
	{
		Y++;
	}
}
enum Direction
{
	Left, Down,
	Up, Right,
	Neutral
}