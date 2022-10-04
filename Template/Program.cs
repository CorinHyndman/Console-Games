using System;
using System.Text;

const int WIDTH = 30;
const int HEIGHT = 15;

// By Datatype
bool gameRunning;
string borderFill;
char[,] screenBuffer;

try
{   // By Alphabet
    borderFill = new string('═', WIDTH);
    gameRunning = true;
    screenBuffer = new char[HEIGHT, WIDTH];

    Console.OutputEncoding = Encoding.UTF8;
    Console.CursorVisible = false;

    FlushBuffer();
    while (gameRunning)
    {
        RenderBuffer();
    }

    void Methods()
    {

    }
   
    void RenderBuffer()
    {
        StringBuilder screenGraphic = new();
        screenGraphic.AppendLine($"╔{borderFill}╗");
        for (int row = 0; row < HEIGHT; row++)
        {
            screenGraphic.Append('║');
            for (int column = 0; column < WIDTH; column++)
            {
                screenGraphic.Append(screenBuffer[row,column]);
            }
            screenGraphic.AppendLine("║");
        }
        screenGraphic.AppendLine($"╚{borderFill}╝");

        Console.SetCursorPosition(0,0);
        Console.WriteLine(screenGraphic.ToString());
    }
    void FlushBuffer()
    {
        for (int row = 0; row < HEIGHT; row++)
        {
            for (int column = 0; column < WIDTH; column++)
            {
                screenBuffer[row, column] = ' ';
            }
        }
    }
}
finally
{
    Console.CursorVisible = true;
    Console.ResetColor();
    Console.Clear();
}

struct Struct
{

}

class Class
{

}

enum Enum
{
    
}