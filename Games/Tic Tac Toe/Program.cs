using System;
using System.Linq;
using System.Text;

try
{
	Random rng = new();
	bool gameOver = false;
	Player[] board = new Player[9];
	string clearLine = new(' ', Console.WindowWidth);

	Console.CursorVisible = false;
	Console.ResetColor();
	Console.Clear();

	while (!gameOver)
	{
		DrawBoard();

		int input = GetRangedIntInput(minInclusive: 1, maxInclusive: 9) - 1;

		while (board[input] is not Player.None)
		{
			Console.WriteLine($"Input position is already taken");
			Console.Write("Please try again: ");

			input = GetRangedIntInput(minInclusive: 1, maxInclusive: 9) - 1;

			Console.CursorTop -= 3;
			Console.WriteLine(clearLine);
			Console.WriteLine(clearLine);
			Console.WriteLine(clearLine);
			Console.CursorTop -= 2;
		}

		board[input] = Player.X;

		int[] validBotPositions = Enumerable.Range(0, board.Length)
			.Where(i => board[i] is Player.None)
			.ToArray();

		if (validBotPositions.Length != 0 && !CheckForGameOver(Player.X))
		{
			board[validBotPositions[rng.Next(0, validBotPositions.Length)]] = Player.O;
		}

		gameOver = CheckForGameOver(Player.X) || CheckForGameOver(Player.O);
	}

	DrawBoard();
	Console.WriteLine("Game Over!");
	Console.WriteLine("Press [ENTER] to exit");
	Console.ReadLine();

	bool CheckForGameOver(Player player) =>
		board[0] == player && board[1] == player && board[2] == player ||
		board[3] == player && board[4] == player && board[5] == player ||
		board[6] == player && board[7] == player && board[8] == player ||
		board[0] == player && board[3] == player && board[6] == player ||
		board[1] == player && board[4] == player && board[7] == player ||
		board[2] == player && board[5] == player && board[8] == player ||
		board[0] == player && board[4] == player && board[8] == player ||
		board[2] == player && board[4] == player && board[6] == player;

	int GetRangedIntInput(int minInclusive, int maxInclusive)
	{
		bool successfulParse = int.TryParse(Console.ReadLine(), out int result)
			&& result >= minInclusive
			&& result <= maxInclusive;

		while (!successfulParse)
		{
			Console.WriteLine($"Input must be a valid integer between ({minInclusive}-{maxInclusive})");
			Console.Write("Please retry: ");
			successfulParse = int.TryParse(Console.ReadLine(), out result)
				&& result >= minInclusive
				&& result <= maxInclusive;

			Console.CursorTop -= 3;
			Console.WriteLine(new string(' ', Console.WindowWidth));
			Console.WriteLine(new string(' ', Console.WindowWidth));
			Console.WriteLine(new string(' ', Console.WindowWidth));
			Console.CursorTop -= 2;
		}
		return result;
	}
	void DrawBoard()
	{
		StringBuilder boardString = new();

		boardString.AppendFormat("  {0} | {1} | {2} \n",
			board[0] is not Player.None ? board[0].ToString() : 1,
			board[1] is not Player.None ? board[1].ToString() : 2,
			board[2] is not Player.None ? board[2].ToString() : 3);
		boardString.AppendLine(" ───┼───┼───");
		boardString.AppendFormat("  {0} | {1} | {2} \n",
			board[3] is not Player.None ? board[3].ToString() : 4,
			board[4] is not Player.None ? board[4].ToString() : 5,
			board[5] is not Player.None ? board[5].ToString() : 6);
		boardString.AppendLine(" ───┼───┼───");
		boardString.AppendFormat("  {0} | {1} | {2} \n",
			board[6] is not Player.None ? board[6].ToString() : 7,
			board[7] is not Player.None ? board[7].ToString() : 8,
			board[8] is not Player.None ? board[8].ToString() : 9);

		Console.SetCursorPosition(0, 0);
		Console.Write(boardString);
	}
}
finally
{
	Console.CursorVisible = true;
	Console.ResetColor();
	Console.Clear();
}
enum Player
{
	None,
	X,
	O,
}