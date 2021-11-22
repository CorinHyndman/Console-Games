using System;
using System.Collections.Generic;
using System.Threading;
using Fishing_Minigame;

// >< '>
string[] PenguinFishing = // All Ascii Stolen from https://ascii.co.uk/art >:] 
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