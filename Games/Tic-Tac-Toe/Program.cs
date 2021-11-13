using System;
using System.Linq;
using System.Text;

try
{
	Random rng = new();
	bool gameOver = false;
	Player[] board = new Player[9];

	Console.CursorVisible = false;
	Console.ResetColor();
	Console.Clear();

	while (!gameOver)
	{
		DrawBoard();

		int input = GetIntInput(min: 1, max: 9) - 1;

		while (board[input] is not Player.None)
		{
			Console.WriteLine($"Input position is already taken");
			Console.Write("Please try again: ");

			input = GetIntInput(min: 1, max: 9) - 1;

			Console.CursorTop -= 3;
			Console.WriteLine(new string(' ', Console.WindowWidth));
			Console.WriteLine(new string(' ', Console.WindowWidth));
			Console.WriteLine(new string(' ', Console.WindowWidth));
			Console.CursorTop -= 2;
		}

		board[input] = Player.X;

		int[] validBotPositions = Enumerable.Range(0, board.Length)
			.Where(i => board[i] is Player.None)
			.ToArray();

		if (validBotPositions.Length != 0)
		{
			board[validBotPositions[rng.Next(0, validBotPositions.Length)]] = Player.O;
		}

		gameOver = CheckForGameOver(Player.X) || CheckForGameOver(Player.O);
	}

	DrawBoard();
	Console.WriteLine(Environment.NewLine + "Game Over!");

	bool CheckForGameOver(Player player) =>
		board[0] == player && board[1] == player && board[2] == player ||
		board[3] == player && board[4] == player && board[5] == player ||
		board[6] == player && board[7] == player && board[8] == player ||
		board[0] == player && board[3] == player && board[6] == player ||
		board[1] == player && board[4] == player && board[7] == player ||
		board[2] == player && board[5] == player && board[8] == player ||
		board[0] == player && board[4] == player && board[8] == player ||
		board[2] == player && board[4] == player && board[6] == player;	 
	int GetIntInput(int min, int max)
	{
		bool success = int.TryParse(Console.ReadLine(), out int result)
			&& result >= min
			&& result <= max;

		while (!success)
		{
			Console.WriteLine($"Input must be a valid integer between ({min}-{max})");
			Console.Write("Please retry: ");
			success = int.TryParse(Console.ReadLine(), out result)
				&& result >= min
				&& result <= max;

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

}
enum Player
{
	None,
	X,
	O,
}