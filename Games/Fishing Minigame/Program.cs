using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

/// W.I.P

try
{
	int frame = 0;
	int score = 0;
	int spawnFrame = 175;
	bool gameOver = false;
	List<Fish> allFish = new();

	string[] PENGUIN_SPRITE =
	{
		@"╭\   _  ",
		@"│ \◄(o) ",
		@"│  Ѥ( ) ",
		@"│ ]/__)>",
	};

	Hook.X = Console.WindowWidth / 2;
	Hook.Y = Pond.YOffset;
	Hook.Sprite = Hook.EMPTY_SPRITE;
	Hook.Color = ConsoleColor.White;

	Console.OutputEncoding = System.Text.Encoding.UTF8;
	Console.CursorVisible = false;

	Console.Write(new string('\n', 3) + new string('_', Console.WindowWidth));
	for (int i = 0; i < PENGUIN_SPRITE.Length; i++)
	{
		Console.SetCursorPosition(Console.WindowWidth / 2, i);
		Console.Write(PENGUIN_SPRITE[i]);
	}

	while (!gameOver)
	{
		if (Console.KeyAvailable)
		{
			switch (Console.ReadKey(true).Key)
			{
				case ConsoleKey.Escape: gameOver = true; break;
				case ConsoleKey.UpArrow when Hook.Y > Pond.YOffset: Hook.Y--; break;
				case ConsoleKey.DownArrow when Hook.Y < Console.WindowHeight - 1: Hook.Y++; break;
			}
		}
		while (Console.KeyAvailable)
		{
			Console.ReadKey(true);
		}

		if (Hook.Sprite == Hook.CAUGHT_SPRITE && Hook.Y == Pond.YOffset)
		{
			score += 50;
			Hook.Sprite = Hook.EMPTY_SPRITE;

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
				Console.Write(Random.Shared.Next(0, 2) is 1 ? Fish.SPRITE_LEFT[0] : Fish.SPRITE_RIGHT[0]);
			}
		}

		Hook.Draw();

        if (frame % spawnFrame is 0)
		{
			int x;
			int y = Random.Shared.Next(1, Pond.Columns);
			switch (Random.Shared.Next(0,2))
            {
                case 0:
					x = Random.Shared.Next(-10, -5);
					allFish.Add(new()
					{
						X = x,
						Y = y,
						Sprite = Fish.SPRITE_RIGHT[0],
						Color = ConsoleColor.DarkYellow,
						Direction = Direction.Right,
					});
					break;
				case 1:
					x = Random.Shared.Next(Pond.Rows, Pond.Rows + 10);
					allFish.Add(new()
					{
						X = x,
						Y = y,
						Sprite = Fish.SPRITE_LEFT[0],
						Color = ConsoleColor.DarkYellow,
						Direction = Direction.Left,
					}); ;
					break;
			}
        }

        if (spawnFrame > 75)
        {
			spawnFrame--;
        }

		for (int i = 0; i < allFish.Count; i++)
		{
			if (allFish[i].Sprite is null || 
			   (allFish[i].Direction is Direction.Left && allFish[i].X < -Fish.SPRITE_LEFT.Length) ||
			   (allFish[i].Direction is Direction.Right && allFish[i].X > Fish.SPRITE_LEFT.Length + Pond.Rows))
			{
				allFish.RemoveAt(i);
				i--;
				continue;
			}
			if (Hook.Sprite == Hook.EMPTY_SPRITE && 
				allFish[i].X == Hook.X - ((int)allFish[i].Direction * (Fish.SPRITE_LENGTH / 2)) && 
				allFish[i].Y == Hook.Y - PENGUIN_SPRITE.Length)
			{
				Hook.Sprite = Hook.CAUGHT_SPRITE;
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
		frame++;
	}
}
finally
{
	Console.CursorVisible = true;
	Console.ResetColor();
	Console.Clear();
}

static class Pond
{
	public const int YOffset = 4;
	public const int XOffset = 6;

	public static int Rows;
	public static int Columns;
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

		for (int i = 0; i < Fish.SPRITE_LENGTH; i++)
		{
			if (fish.X + i < Rows && Sprite[fish.Y, fish.X + i] is not ' ')
			{
				return false;
			}
		}

		for (int i = 0; i < Fish.SPRITE_LENGTH; i++)
		{
            if (fish.X + i < Rows)
            {
				Sprite[fish.Y, fish.X + i] = fish.Sprite[i];
			}
		}
		return true;
	}
}
static class Hook
{
	public static readonly char CAUGHT_SPRITE = 'ö';
	public static readonly char EMPTY_SPRITE = 'ʖ';

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
			Console.Write('│');
		}

		Console.ForegroundColor = Sprite == CAUGHT_SPRITE
			? ConsoleColor.DarkYellow
			: ConsoleColor.DarkGray;
		Console.SetCursorPosition(Console.WindowWidth / 2, Y);
		Console.Write(Sprite);
		Console.ForegroundColor = ConsoleColor.White;
	}
}
class Fish
{
	public static readonly int SPRITE_LENGTH = 5;
	public static readonly string[] SPRITE_LEFT  = { "ϵ(°)<", "ϵ(°)-" };
	public static readonly string[] SPRITE_RIGHT = { ">(°)϶", "-(°)϶" };

	public int Frame = 0;

	public int X { get; set; }
	public int Y { get; set; }
	public int Damage { get; set; }
	public Hitbox Hitbox { get; set; }
	public Direction Direction { get; set; }
	public ConsoleColor Color { get; set; }
	public string Sprite 
	{
		get { return Direction is Direction.Left ? SPRITE_LEFT[Frame] : SPRITE_RIGHT[Frame]; }
		set { }
	}
	public void Draw()
	{
		Console.ForegroundColor = Color;
		Pond.Add(this);
	}
	public void Update()
	{
		X += (int)Direction;

		Frame++;
        if (Frame >= SPRITE_LEFT.Length)
        {
			Frame = 0;
        }

		if (Sprite.Length is 1)
		{
			Sprite = null;
			return;
		}

		if (X < 0)
		{
			Sprite = Sprite[1..];
		}
		if (X + Sprite.Length > Console.WindowWidth)
		{
			Sprite = Sprite[..^1];
		}
	}
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