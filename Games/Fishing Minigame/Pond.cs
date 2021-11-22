using System;
using System.Text;

namespace Fishing_Minigame
{
	static class Pond
	{
		public const int YOffset = 4;
		public const int XOffset = 6; // Keep Even >:]

		private static int Rows;
		private static int Columns;
		private static char[,] Array;
		static Pond()
		{
			Rows = Console.WindowWidth + XOffset;
			Columns = Console.WindowHeight - YOffset;
			Array = new char[Columns,Rows];
			
			for (int i = 0; i < Columns; i++)
			{
				for (int j = 0; j < Rows; j++)
				{
					Array[i, j] = ' ';
				}
			}
		}
		public static void Draw()
		{
			StringBuilder sb = new();

			for (int i = 0; i < Columns; i++)
			{
				for (int j = XOffset / 2; j < Rows - (XOffset / 2); j++)
				{
					sb.Append(Array[i, j]);
				}
				if (i != Columns - 1)
				{
					sb.AppendLine();
				}
			}

			Console.SetCursorPosition(0, YOffset);
			Console.Write(sb.ToString());
		}
		public static void Clear()
		{
			Array = new char[Columns, Rows];

			for (int i = 0; i < Columns; i++)
			{
				for (int j = 0; j < Rows; j++)
				{
					Array[i, j] = ' ';
				}
			}
		}
		/// <returns> Bool Indicating Whether Fish Has Been Added</returns>
		public static bool Add(Fish fish)
		{
			if (fish.X < 1 || fish.X > Rows ||
				fish.Y < 1 || fish.Y > Columns)
			{
				return false;
			}

			for (int i = 0; i < fish.Sprite.Length; i++)
			{
				if (Array[fish.Y,fish.X + i] is not ' ')
				{
					return false;
				}
			}

			for (int i = 0; i < fish.Sprite.Length; i++)
			{
				Array[fish.Y, fish.X + i] = fish.Sprite[i];
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
			for (int i = Pond.YOffset -1; i < Y; i++)
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
}
