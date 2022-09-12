using System;
using System.Text;
using System.Threading;

try
{
    const int BOARD_ROWS = 6;
    const int BOARD_COLUMNS = 7;

    bool gameRunning = true;
    Chip[,] board = new Chip[BOARD_ROWS, BOARD_COLUMNS]; // BOARD_WIDTH, BOARD_HEIGHT
    StringBuilder boardAscii = new();
    TimeSpan gameDelay = TimeSpan.FromMilliseconds(120);

    Console.OutputEncoding = Encoding.UTF8;
    Console.CursorVisible = false;

    string boardTopAscii    = new('─', BOARD_COLUMNS);
    string boardMiddleAscii = new('○', BOARD_COLUMNS);
    string boardBottomAscii = new('━', BOARD_COLUMNS);

    boardAscii.AppendLine($"╭{boardTopAscii}╮");
    for (int i = 0; i < BOARD_ROWS; i++)
    {
        boardAscii.AppendLine($"│{boardMiddleAscii}│");
    }
    boardAscii.AppendLine($"┷{boardBottomAscii}┷");

    board[BOARD_ROWS - 1, 0] = Chip.Red;
    board[BOARD_ROWS - 1, 1] = Chip.Red;
    board[BOARD_ROWS - 1, 2] = Chip.Blue;
    board[BOARD_ROWS - 1, 3] = Chip.Red;
    board[BOARD_ROWS - 1, 4] = Chip.Blue;
    board[BOARD_ROWS - 1, 5] = Chip.Red;
    board[BOARD_ROWS - 1, 6] = Chip.Blue;

    while (gameRunning)
    {
        Console.SetCursorPosition(0, 1);
        Console.Write(boardAscii);

        for (int row = 0; row < BOARD_ROWS; row++) 
        { 
            for (int column = 0; column < BOARD_COLUMNS; column++)
            {
                if (board[row,column] is Chip.None)
                {
                    continue;
                }

                Console.ForegroundColor = board[row, column] is Chip.Blue
                    ? ConsoleColor.Blue
                    : ConsoleColor.Red;

                (int x, int y) = ConvertBoardToConsoleCoordinates(column,row);
                Console.SetCursorPosition(x, y);
                Console.Write('●');

                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        Thread.Sleep(gameDelay);
    }

    (int x, int y) ConvertBoardToConsoleCoordinates(int column, int row) => (column + 1, row + 2);
}
finally
{
    Console.CursorVisible = true;
    Console.ResetColor();
    Console.Clear();
}
enum Chip
{
    None,
    Blue,
    Red,
}