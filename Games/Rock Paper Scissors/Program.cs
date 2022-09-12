using System;
using System.Threading;

Random rng;
bool gameOver;
Move playerMove;
Move opponentMove;
(int player, int opponent) score;

try
{
	rng = new();
	gameOver = false;
	playerMove = Move.None;
	opponentMove = Move.None;
	score = (0, 0);

	Console.WriteLine("Console Rock Paper Scissors");
	Console.WriteLine("A countdown from 3 will start and you will have to press either:");
	Console.WriteLine(Environment.NewLine + "[R]ock [P]aper or [S]cissors" + Environment.NewLine);
	Console.WriteLine("If no input is detected the bot will think you are cheating!");
	Console.WriteLine(Environment.NewLine + "Press [Enter] to start");
	Console.ReadLine();

	Console.CursorVisible = false;
	Console.ResetColor();
	Console.Clear();

	while (!gameOver)
	{
		Console.WriteLine("Get Ready!");
		for (int i = 3; i > 0; i--)
		{
			Console.ForegroundColor = i switch
			{
				3 => ConsoleColor.DarkRed,
				2 => ConsoleColor.Red,
				1 => ConsoleColor.Yellow,
				_ => ConsoleColor.White,
			};

			Console.WriteLine(i);
			Thread.Sleep(TimeSpan.FromSeconds(1));
			Console.CursorTop--;
		}
		while (Console.KeyAvailable)
		{
			Console.ReadKey(true);
		}

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine("Go!");
		Console.ForegroundColor = ConsoleColor.White;

		opponentMove = (Move)rng.Next(3);
		Thread.Sleep(TimeSpan.FromMilliseconds(750));
		if (Console.KeyAvailable)
		{
			playerMove = Console.ReadKey(true).Key switch
			{
				ConsoleKey.R or ConsoleKey.D1 => Move.Rock,
				ConsoleKey.P or ConsoleKey.D2 => Move.Paper,
				ConsoleKey.S or ConsoleKey.D3 => Move.Scissors,
				_ => Move.None,
			};
		}
		else
		{
			Console.WriteLine("No cheating!" + Environment.NewLine);
			continue;
		}

		Console.WriteLine(Environment.NewLine + $"Your Move: {playerMove} vs Opponent Move: {opponentMove}");

		switch (playerMove, opponentMove)
		{
			case (Move.Paper, Move.Rock):
			case (Move.Rock, Move.Scissors):
			case (Move.Scissors, Move.Paper):
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("You win!");
				score.player++;
				break;
			case (Move.Rock, Move.Paper):
			case (Move.Scissors, Move.Rock):
			case (Move.Paper, Move.Scissors):
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("You lose!");
				score.opponent++;
				break;
			default:
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("It's a draw!");
				break;
		}

		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine($"Score: {score.player} - {score.opponent}");

		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine(Environment.NewLine + "Press [Enter] to continue");
		Console.ReadLine();
	}
}
finally 
{
	Console.CursorVisible = true;
	Console.ResetColor();
	Console.Clear();
}
enum Move
{
	Rock,
	Paper,
	Scissors,
	None,
}