using Microsoft.VisualBasic.FileIO;
using System.Net;

Run(fromFile(@"..\..\..\example_input.txt"), true);
Run(fromFile(@"E:\develop\advent-of-code-input\2022\day10.txt"), false);

void Run(string[] S, bool isTest)
{ 
    long supposedanswer1 = 13140;
   
    long answer1 = 0;
    
    var screen = (new string('.', 40)).ToCharArray();
    int X = 1;
    int cycle = 0;
    int i = 0;
    int readyCycle = 1;
    while (i <  S.Length)
    {
        cycle++;

        int Xnew = X;
        if (cycle > readyCycle)
        {
            var s = S[i].Split(' ');
            if (s[0] == "noop")
            {
                readyCycle++;
            }
            else if (s[0] == "addx")
            {
                Xnew += int.Parse(s[1]);
                readyCycle += 2;
            }
            i++;
        }

        int position = (cycle - 1) % 40;
        if (position == 0)
        {
            Console.WriteLine(new string(screen));
            screen = (new string('.', 40)).ToCharArray();
        }
        if (position < X+2 && position >= X-1)
        {
            screen[position] = '#';
        }
        if (cycle == 20 || cycle == 60 || cycle == 100 || cycle == 140 || cycle == 180 || cycle == 220)
        {
            answer1 += cycle * X;
        }

        X = Xnew;

    }

    w(1, answer1, supposedanswer1, isTest);
    w(2, "zie scherm", "", isTest);
}



static void w<T>(int number, T val, T supposedval, bool isTest)
{
    string? v = (val == null) ? "(null)" : val.ToString();
    string? sv = (supposedval == null) ? "(null)" : supposedval.ToString();

    var previouscolour = Console.ForegroundColor;
    Console.Write("Answer Part " + number + ": ");
    Console.ForegroundColor = (v == sv) ? ConsoleColor.Green : ConsoleColor.White;
    Console.Write(v);
    Console.ForegroundColor = previouscolour;
    if (isTest)
    {
        Console.Write(" ... supposed (example) answer: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(sv);
        Console.ForegroundColor = previouscolour;
    }
    else
        Console.WriteLine();
}

static string[] fromFile(string fileName)
{
    return  File.ReadAllLines(fileName).ToArray();
}
