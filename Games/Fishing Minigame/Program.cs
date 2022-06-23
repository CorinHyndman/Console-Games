using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

///
/// W.I.P
///


string[] PenguinFishing =
{
	@"     _   ",
	@" /\ <`)  ",
	@"|  \(V)  ",
	@"| ]/ __)>",
};

int score = 0;
Random rng = new();
bool gameOver = false;
List<Fish> allFish = new();

Hook.X = Console.WindowWidth / 2;
Hook.Y = Pond.YOffset;
Hook.Sprite = '¿';
Hook.Color = ConsoleColor.White;

Console.CursorVisible = false;

Console.Write("\r\n\r\n\r\n" + new string('_', Console.WindowWidth));
for (int i = 0; i < PenguinFishing.Length; i++)
{
	Console.SetCursorPosition(Console.WindowWidth / 2, i);
	Console.Write(PenguinFishing[i]);
}

while (!gameOver)
{
	if (Console.KeyAvailable)
	{
		switch (Console.ReadKey(true).Key)
		{
			case ConsoleKey.S:
				allFish.Add(new Samon()
				{
					X = Console.WindowWidth / 2,
					Y = 1,
					Sprite = "><'>",
					Color = ConsoleColor.Red,
					Direction = Direction.Right,
					Hitbox = new(x: 0, y: 2, width: 4, height: 1),
				});
				break;
			case ConsoleKey.Escape: gameOver = true; break;
			case ConsoleKey.UpArrow when Hook.Y > Pond.YOffset: Hook.Y--; break;
			case ConsoleKey.DownArrow when Hook.Y < Console.WindowHeight - 1: Hook.Y++; break;
		}
	}
	while (Console.KeyAvailable)
	{
		Console.ReadKey(true);
	}

	if (Hook.Sprite is 'ö' && Hook.Y == Pond.YOffset)
	{
		score += 50;
		Hook.Sprite = '¿';

		if (score < 750)
		{
			int fishX = (Console.WindowWidth / 2) - (score / 50 * 4) - 5;
			int fishY = 3;
			if (score > 250)
			{
				fishY--;
				fishX += 19;
			}
			if (score > 450)
			{
				fishY--;
				fishX += 15;
			}
			if (score > 600)
			{
				fishY--;
				fishX += 11;
			}

			Console.SetCursorPosition(fishX + fishY, fishY);
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.Write(rng.Next(0, 2) is 1 ? "<'><" : "><'>");
		}
	}

	Hook.Draw();

	for (int i = 0; i < allFish.Count; i++)
	{
		if (allFish[i].Sprite is null)
		{
			allFish.RemoveAt(i);
			i--;
			continue;
		}
		if (Hook.Sprite is '¿' &&
			allFish[i].X == Hook.X &&
			allFish[i].Y == Hook.Y - Pond.YOffset)
		{
			Hook.Sprite = 'ö';
			allFish.RemoveAt(i);
			i--;
			continue;
		}
		allFish[i].Draw();
		allFish[i].Update();
	}

	Thread.Sleep(TimeSpan.FromMilliseconds(100));

	Pond.Draw();
	Pond.Clear();
}

static class Pond
{
	public const int YOffset = 4;
	public const int XOffset = 6;

	private static int Rows;
	private static int Columns;
	private static char[,] Sprite;
	static Pond()
	{
		Rows = Console.WindowWidth + XOffset;
		Columns = Console.WindowHeight - YOffset;
		Sprite = new char[Columns, Rows];

		for (int i = 0; i < Columns; i++)
		{
			for (int j = 0; j < Rows; j++)
			{
				Sprite[i, j] = ' ';
			}
		}
	}
	public static void Draw()
	{
		StringBuilder spriteDisplay = new();

		for (int i = 0; i < Columns; i++)
		{
			for (int j = XOffset / 2; j < Rows - (XOffset / 2); j++)
			{
				spriteDisplay.Append(Sprite[i, j]);
			}
			if (i != Columns - 1)
			{
				spriteDisplay.AppendLine();
			}
		}

		Console.SetCursorPosition(0, YOffset);
		Console.Write(spriteDisplay.ToString());
	}
	public static void Clear()
	{
		Sprite = new char[Columns, Rows];

		for (int i = 0; i < Columns; i++)
		{
			for (int j = 0; j < Rows; j++)
			{
				Sprite[i, j] = ' ';
			}
		}
	}
	public static bool Add(Fish fish)
	{
		if (fish.X < 1 || fish.X > Rows ||
			fish.Y < 1 || fish.Y > Columns)
		{
			return false;
		}

		for (int i = 0; i < fish.Sprite.Length; i++)
		{
			if (Sprite[fish.Y, fish.X + i] is not ' ')
			{
				return false;
			}
		}

		for (int i = 0; i < fish.Sprite.Length; i++)
		{
			Sprite[fish.Y, fish.X + i] = fish.Sprite[i];
		}
		return true;
	}
}
static class Hook
{
	public static int X;
	public static int Y;
	public static char Sprite;
	public static ConsoleColor Color;
	public static void Draw()
	{
		Console.ForegroundColor = Color;
		for (int i = Pond.YOffset - 1; i < Y; i++)
		{
			Console.SetCursorPosition(Console.WindowWidth / 2, i);
			Console.Write('|');
		}

		Console.ForegroundColor = Sprite is 'ö'
			? ConsoleColor.DarkYellow
			: ConsoleColor.DarkGray;
		Console.SetCursorPosition(Console.WindowWidth / 2, Y);
		Console.Write(Sprite);
		Console.ForegroundColor = ConsoleColor.White;
	}
}
abstract class Fish
{
	public int X { get; set; }
	public int Y { get; set; }
	public int Damage { get; set; }
	public string Sprite { get; set; }
	public Hitbox Hitbox { get; set; }
	public Direction Direction { get; set; }
	public ConsoleColor Color { get; set; }
	public void Draw()
	{
		Console.ForegroundColor = Color;
		Pond.Add(this);
	}
	public void Update()
	{
		X += (int)Direction;
		Hitbox.X += (int)Direction;

		if (Sprite.Length is 1)
		{
			Sprite = null;
			return;
		}

		if (X < 0)
		{
			Sprite = Sprite[1..];
			Hitbox.Width = Sprite.Length;
		}
		if (X + Sprite.Length > Console.WindowWidth)
		{
			Sprite = Sprite[..^1];
			Hitbox.Width = Sprite.Length;
		}
	}
}
class Samon : Fish
{

}
class Hitbox
{
	public int X { get; set; }
	public int Y { get; set; }
	public int Width { get; set; }
	public int Height { get; set; }
	public Hitbox(int x, int y, int width, int height)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}
	public bool Contains(int x, int y) =>
			x >= X && x <= X + Width &&
			y >= Y && y <= Y + Height;
	public bool Contains(Hitbox hitbox) =>
			hitbox.X >= X && hitbox.X + hitbox.Width <= X + Width &&
			hitbox.Y >= Y && hitbox.Y + hitbox.Height <= Y + Height;
	public bool IntersectsWith(Hitbox hitbox) =>
			hitbox.X < X + Width && X < hitbox.X + hitbox.Width &&
			hitbox.Y < Y + Height && Y < hitbox.Y + hitbox.Height;
}
enum Direction
{
	Left = -1,
	Right = 1,
}