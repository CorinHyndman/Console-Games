using System;
using System.Linq;
using System.Text;

int input;
bool gameOver;
string clearLine;
int[] validBotPositions;
Player[] board;
Random rng;

try
{
	rng = new();
	gameOver = false;
	board = new Player[9];
	clearLine = new(' ', Console.WindowWidth);

	Console.CursorVisible = false;
	Console.ResetColor();
	Console.Clear();

	while (!gameOver)
	{
		DrawBoard();

		input = GetRangedIntInput(minInclusive: 1, maxInclusive: 9) - 1;

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

		validBotPositions = Enumerable.Range(0, board.Length)
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

		boardString.AppendLine();
		boardString.AppendFormat("  {0} | {1} | {2} \n",
			board[0] is Player.None ? 1 : board[0].ToString(),
			board[1] is Player.None ? 2 : board[1].ToString(),
			board[2] is Player.None ? 3 : board[2].ToString());

		boardString.AppendLine(" ───┼───┼───");

		boardString.AppendFormat("  {0} | {1} | {2} \n",
			board[3] is Player.None ? 4 : board[3].ToString(),
			board[4] is Player.None ? 5 : board[4].ToString(),
			board[5] is Player.None ? 6 : board[5].ToString());

		boardString.AppendLine(" ───┼───┼───");

		boardString.AppendFormat("  {0} | {1} | {2} \n",
			board[6] is Player.None ? 7 : board[6].ToString(),
			board[7] is Player.None ? 8 : board[7].ToString(),
			board[8] is Player.None ? 9 : board[8].ToString());

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