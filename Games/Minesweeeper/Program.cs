using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

const int ROWS = 15;
const int COLUMNS = 55;
const int MINES_COUNT = 55;
const int MINE = -1;

bool gameRunning;
string borderFill;
int[,] board = new int[ROWS, COLUMNS];
char[,] screenBuffer;
(int Row, int Col) cursor;
List<(int Row, int Col)> revealed;
List<(int Row, int Col)> flagged;
Random rng;

try
{
    borderFill = new string('═', COLUMNS);
    cursor = (ROWS / 2, COLUMNS / 2);
    flagged = new List<(int X, int Y)>();
    gameRunning = true;
    revealed = new List<(int X, int Y)>();
    rng = new Random();
    screenBuffer = new char[ROWS, COLUMNS];

    Console.OutputEncoding = Encoding.Unicode;
    Console.CursorVisible = false;

    FlushBuffer();
    RenderBuffer();
    Console.SetCursorPosition(0, 1);
    Console.WriteLine("║Console Minesweeper  ");
    Console.WriteLine("║     Controls        " + Environment.NewLine);
    Console.WriteLine("║        ▲            ");
    Console.WriteLine("║      ◄ ▼ ►          " + Environment.NewLine);
    Console.WriteLine("║[F] to flag a mine   ");
    Console.WriteLine("║Flag all mines to win");
    Console.WriteLine("║Press [ENTER] to start");
    while (Console.ReadKey(true).Key != ConsoleKey.Enter) continue;

    for (int i = 0; i < MINES_COUNT; i++)
    {
        int col = rng.Next(0, COLUMNS);
        int row = rng.Next(0, ROWS);
        if (board[row, col] is not MINE)
        {
            board[row, col] = MINE;
        }
        else
        {
            i--;
        }
    }

    for (int row = 0; row < ROWS; row++)
    {
        for (int col = 0; col < COLUMNS; col++)
        {
            if (board[row, col] is MINE)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if(InBounds(row + i, col + j) && board[row + i, col + j] is not MINE)
                        {
                            board[row + i, col + j]++;
                        }
                    }
                }
            }
        }
    }

    FlushBuffer();
    while (gameRunning)
    {
        Console.SetCursorPosition(0, 0);
        foreach ((int Row, int Col) in revealed)
        {
            screenBuffer[Row, Col] = board[Row, Col] switch
            {
             MINE => '☼',
                0 => '░',
                _ => (char)(board[Row, Col] + 48),
            };
        }
        foreach ((int Row, int Col) in flagged)
        {
            screenBuffer[Row, Col] = '¶';
        }

        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.SetCursorPosition(cursor.Col + 1, cursor.Row + 1);

        if (flagged.Contains((cursor.Row, cursor.Col)))
        {
            Console.Write('¶');
        }
        else
        {
            if (revealed.Contains((cursor.Row, cursor.Col)))
            {
                if (board[cursor.Row,cursor.Col] is 0)
                {
                    Console.Write('░');
                }
                else
                {
                    Console.Write((char)(board[cursor.Row, cursor.Col] + 48));
                }
            }
            else
            {
                Console.Write(' ');
            }
        }
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(33));
        if (Console.KeyAvailable)
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.F:
                    if (flagged.Contains((cursor.Row, cursor.Col)))
                    {
                        flagged.Remove((cursor.Row, cursor.Col));
                    }
                    else
                    {
                        flagged.Add((cursor.Row, cursor.Col));
                    }
                    break;

                case ConsoleKey.Enter when !flagged.Contains((cursor.Row,cursor.Col)):
                    if (board[cursor.Row,cursor.Col] is MINE)
                    {
                        EndScreen("Lose");
                        return;
                    }
                    RevealSelected(cursor.Row, cursor.Col); 
                    break;

                case ConsoleKey.UpArrow when cursor.Row > 0: cursor.Row--; break;
                case ConsoleKey.DownArrow when cursor.Row < ROWS - 1: cursor.Row++; break;

                case ConsoleKey.LeftArrow when cursor.Col > 0: cursor.Col--; break;
                case ConsoleKey.RightArrow when cursor.Col < COLUMNS - 1: cursor.Col++; break;
            }
        }
        while (Console.KeyAvailable)
        {
            Console.ReadKey(true);
        }

        if (flagged.Count is MINES_COUNT && flagged.All(x => board[x.Row, x.Col] is MINE))
        {
            EndScreen("Win");
            return;
        }

        RenderBuffer();
        FlushBuffer();
    }

    void RevealSelected(int Row, int Col)
    {
        Stack<(int Row, int Col, Exits Exits)> path = new();
        path.Push((Row, Col, Exits.All));
        while (path.Count > 0)
        {
            (int Row, int Col, Exits Exits) currentPos = path.Pop();

            if (!revealed.Contains((currentPos.Row, currentPos.Col)))
            {
                revealed.Add((currentPos.Row, currentPos.Col));
            }
            if (board[currentPos.Row,currentPos.Col] > 0)
            {
                continue;
            }

            if (currentPos.Exits.HasFlag(Exits.North) && 
                InBounds(currentPos.Row - 1, currentPos.Col) &&
                !revealed.Contains((currentPos.Row - 1, currentPos.Col)))
            {
                path.Push((currentPos.Row - 1, currentPos.Col, Exits.All ^ Exits.South));                
            }

            if (currentPos.Exits.HasFlag(Exits.South) &&
                InBounds(currentPos.Row + 1, currentPos.Col) &&
                !revealed.Contains((currentPos.Row + 1, currentPos.Col)))
            {
                path.Push((currentPos.Row + 1, currentPos.Col, Exits.All ^ Exits.North));
            }

            if (currentPos.Exits.HasFlag(Exits.East) &&
                InBounds(currentPos.Row, currentPos.Col + 1) &&
                !revealed.Contains((currentPos.Row, currentPos.Col + 1)))
            {
                path.Push((currentPos.Row, currentPos.Col + 1, Exits.All ^ Exits.West));
            }

            if (currentPos.Exits.HasFlag(Exits.West) &&
                InBounds(currentPos.Row, currentPos.Col - 1) &&
                !revealed.Contains((currentPos.Row, currentPos.Col - 1)))
            {
                path.Push((currentPos.Row, currentPos.Col - 1, Exits.All ^ Exits.East));
            }
        }
    }

    void RenderBuffer()
    {
        StringBuilder screenGraphic = new();
        screenGraphic.AppendLine($"╔{borderFill}╗");
        for (int row = 0; row < ROWS; row++)
        {
            screenGraphic.Append('║');
            for (int column = 0; column < COLUMNS; column++)
            {
                screenGraphic.Append(screenBuffer[row, column]);
            }
            screenGraphic.AppendLine("║");
        }
        screenGraphic.AppendLine($"╚{borderFill}╝");

        Console.SetCursorPosition(0, 0);
        Console.WriteLine(screenGraphic.ToString());
    }

    void FlushBuffer()
    {
        for (int row = 0; row < ROWS; row++)
        {
            for (int column = 0; column < COLUMNS; column++)
            {
                screenBuffer[row, column] = ' ';
            }
        }
    }

    bool InBounds(int row,int col) => 
        row >= 0 && row < ROWS &&
        col >= 0 && col < COLUMNS;

    void EndScreen(string result)
    {
        Console.SetCursorPosition(0, 1);
        Console.WriteLine("║{0,-22}", $"You {result}!");
        Console.WriteLine("║{0,-22}", $"Mine Count: {MINES_COUNT}");
        Console.WriteLine("║{0,-22}", "Press [ENTER] to exit");
        while (Console.ReadKey(true).Key != ConsoleKey.Enter) continue;
    }
}
finally
{
    Console.CursorVisible = true;
    Console.ResetColor();
    Console.Clear();
}
[Flags]
enum Exits
{
    None  = 0,
    North = 1,
    South = 2,
    East  = 4,
    West  = 8,
    All   = 15,
}