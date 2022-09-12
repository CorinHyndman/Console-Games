using System;
using System.Text;
using System.Threading;

const int BORDER_SIZE = 1;
const int BOARD_WIDTH = 41;
const int BOARD_HEIGHT = 9;
const int PADDLE_HEIGHT = 3;
const char BALL_SPRITE = 'o';

int frame;
int ballUpdateFrame;
int opponentUpdateFrame;
string border;
string scoreDisplay;
bool gameOver;
(int X, int Y) ball;
(int X, int Y) ballSpeed;
(int X, int Y) paddlePlayer;
(int X, int Y) paddleOpponent;
(int player, int opponent) score;

try
{
	if (Console.WindowWidth < BOARD_WIDTH || Console.WindowHeight < BOARD_HEIGHT)
	{
		Console.WriteLine("Console Window Too small please resize then retry");
		Console.WriteLine($"Console Minimum Size Width: {BOARD_WIDTH} Height: {BOARD_HEIGHT}");
		return;
	}

	frame = 0;
	gameOver = false;
	ballUpdateFrame = 10;
	opponentUpdateFrame = 7;
	scoreDisplay = "{0,6} - {1,-6}";
	ball = (BORDER_SIZE + BOARD_WIDTH / 2, BORDER_SIZE + BOARD_HEIGHT / 2);
	ballSpeed = (0, 0);
	paddlePlayer = (BORDER_SIZE, BOARD_HEIGHT / 2);
	paddleOpponent = (BOARD_WIDTH, BOARD_HEIGHT / 2);
	score = (0, 0);

	Console.CursorVisible = false;

	border = GenerateBorder();
	Console.Write(border);
	ResetBall();
	
	while (!gameOver)
	{
		Console.SetCursorPosition(0,0);
		Console.Write(border);
		while (Console.KeyAvailable)
		{
			switch(Console.ReadKey(true).Key)
			{
				case ConsoleKey.UpArrow: paddlePlayer.Y--; break;
				case ConsoleKey.DownArrow: paddlePlayer.Y++; break;
			};

			if (paddlePlayer.Y < BORDER_SIZE)
			{
				paddlePlayer.Y = BORDER_SIZE;
			}
			if (paddlePlayer.Y + PADDLE_HEIGHT > BOARD_HEIGHT + BORDER_SIZE)
			{
				paddlePlayer.Y = BOARD_HEIGHT + BORDER_SIZE - PADDLE_HEIGHT;
			}
		}

		if (frame % opponentUpdateFrame is 0 && ball.X > BOARD_WIDTH / 2)
		{
			if (paddleOpponent.Y + PADDLE_HEIGHT / 2 < ball.Y && paddleOpponent.Y < BORDER_SIZE + BOARD_HEIGHT - PADDLE_HEIGHT)
			{
				paddleOpponent.Y++;
			}
			else if (paddleOpponent.Y + PADDLE_HEIGHT / 2 > ball.Y && paddleOpponent.Y > BORDER_SIZE)
			{
				paddleOpponent.Y--;
			}
		}

		if (frame % ballUpdateFrame is 0)
		{
			if (ball.X < BORDER_SIZE)
			{
				score.opponent++;
				DrawScore();
				ResetBall();
				frame = 0;
				continue;
			}
			if (ball.X > BOARD_WIDTH)
			{
				score.player++;
				DrawScore();
				ResetBall();
				frame = 0;
				continue;
			}

			if (ball.Y <= BORDER_SIZE || ball.Y >= BOARD_HEIGHT)
			{
				ballSpeed.Y = -ballSpeed.Y;
			}

			for (int i = 0; i < PADDLE_HEIGHT; i++)
			{
				if ((ball.X + ballSpeed.X == paddlePlayer.X && ball.Y + ballSpeed.Y == paddlePlayer.Y + i) ||
					(ball.X + ballSpeed.X == paddleOpponent.X && ball.Y + ballSpeed.Y == paddleOpponent.Y + i))
				{
					switch (i)
					{
						case 0: ballSpeed.Y = -1; break;
						case 1: ballSpeed.Y = Random.Shared.Next(10) < 5 ? -1 : 1; break;
						case 2: ballSpeed.Y =  1; break;
					}
					ballSpeed.X = -ballSpeed.X;
					
					if (ballUpdateFrame > 1)
					{
						ballUpdateFrame--;
					}

					break;
				}
			}

			ball.X += ballSpeed.X;
			ball.Y += ballSpeed.Y;
		}

		Console.SetCursorPosition(ball.X , ball.Y);
		Console.Write(BALL_SPRITE);
		DrawPaddles();
		DrawScore();

		Thread.Sleep(10);
		frame++;

		if (score.player is 10 || score.opponent is 10)
		{
			gameOver = true;
		}
	}

	Console.WriteLine("Game Over!");
	Console.WriteLine($"{(score.player is 10 ? "Player" : "Opponent")} Wins!");
	Console.WriteLine("Press [ENTER] to exit");
	Console.ReadLine();

	void DrawScore()
	{
		Console.SetCursorPosition((BORDER_SIZE + BOARD_WIDTH / 2) - (scoreDisplay.Length / 2), BOARD_HEIGHT + BORDER_SIZE * 2);
		Console.Write(scoreDisplay, score.player, score.opponent);
	}
	void ResetBall()
	{
		ball = (BORDER_SIZE + BOARD_WIDTH / 2, BORDER_SIZE + BOARD_HEIGHT / 2);
		ballUpdateFrame = 10;
		Console.SetCursorPosition(ball.X,ball.Y);
		Console.WriteLine(BALL_SPRITE);
		DrawPaddles();

		for (int i = 3; i > 0; i--)
		{
			Console.SetCursorPosition(ball.X, ball.Y + 1);
			Console.Write(i);
			Thread.Sleep(TimeSpan.FromSeconds(1));
		}

		ballSpeed.X = Random.Shared.Next(100) < 50 ? -1 : 1;
		ballSpeed.Y = Random.Shared.Next(100) < 50 ? -1 : 1;
		ballSpeed.Y = Random.Shared.Next(100) < 25 ? 0 : ballSpeed.Y;
	}
	void DrawPaddles()
	{
		for (int i = 0; i < PADDLE_HEIGHT; i++)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.SetCursorPosition(paddlePlayer.X, paddlePlayer.Y + i);
			Console.Write('█');

			Console.ForegroundColor = ConsoleColor.Red;
			Console.SetCursorPosition(paddleOpponent.X, paddleOpponent.Y + i);
			Console.Write('█');
		}
		Console.ForegroundColor = ConsoleColor.White;
	}
	string GenerateBorder()
	{
		StringBuilder border = new();
		string borderEmptyPadding = new(' ', BOARD_WIDTH);
		string borderFilledPadding = new('═', BOARD_WIDTH);

		border.AppendLine($"╔{borderFilledPadding}╗");
		for (int i = 0; i < BOARD_HEIGHT; i++)
		{
			border.AppendLine($"║{borderEmptyPadding}║");
		}
		border.AppendLine($"╚{borderFilledPadding}╝");

		return border.ToString();
	}
}
finally
{
	Console.CursorVisible = true;
	Console.ResetColor();
	Console.Clear();
}