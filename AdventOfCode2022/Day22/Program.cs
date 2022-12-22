using System.Diagnostics;
using System.Net.Security;
using static System.Net.Mime.MediaTypeNames;

Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day22.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 6032;
    long supposedanswer2 = 000;
    
    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

  
    int i = 0;
    var instruction = S[^1];
    int x = 0;
    int y = 0;
    int dir = 0;
    int xCnt = 0;
    for (int j = 0; j < S.Count - 2; j++) xCnt = Math.Max(xCnt, S[j].Length);
    while (S[0][x] != '.') x++;
    while (i < instruction.Length)
    {
        string a = "";
        while (i < instruction.Length && instruction[i] <= '9' && instruction[i] >= '0')
        {
            a += instruction[i];
            i++;
        }
        char b = ' ';
        if (i < instruction.Length)
        {
            b = instruction[i];
            i++;
        }

        for (int j = 0; j < int.Parse(a); j++)
        {
            if (dir == 0)
            {
                bool around = false;
                int nextX = x + 1;
                if (nextX == S[y].Length) around = true;
                else if (S[y][nextX] == ' ') around = true;
                if (around)
                {
                    nextX = 0;
                    while (S[y][nextX] == ' ') nextX++;
                }
                if( S[y][nextX] == '.') x = nextX;                     
            }
            else if (dir == 1)
                {
                    bool around = false;
                    int nextY = y + 1;
                    if (nextY == S.Count - 2) around = true;
                    else if (x > S[nextY].Length || S[nextY][x] == ' ') around = true;
                    if (around)
                    {
                        nextY = 0;
                        while (x >= S[nextY].Length || S[nextY][x] == ' ') nextY++;
                    }
                    if (S[nextY][x] == '.') y = nextY;
                }
            else if (dir == 2)
            {
                bool around = false;
                int nextX = x - 1;
                if (nextX < 0) around = true;
                else if (S[y][nextX] == ' ') around = true;
                if (around)
                {
                    nextX = S[y].Length - 1;
                    while (S[y][nextX] == ' ') nextX--;
                }
                if (S[y][nextX] == '.') x = nextX;
            }
            else if (dir == 3)
            {
                bool around = false;
                int nextY = y - 1;
                if (nextY < 0 ) around = true;
                else if (x > S[nextY].Length || S[nextY][x] == ' ') around = true;
                if (around)
                {
                    nextY = S.Count - 3;
                    while (x > S[nextY].Length || S[nextY][x] == ' ') nextY--;
                }
                if (S[nextY][x] == '.') y = nextY;
            }
        }
        if (b == 'L') dir--;
        else if (b == 'R') dir++;
        if (dir < 0) dir = 3;
        if (dir > 3) dir = 0;
    }

    answer1 = 1000 * (y+1) + 4 * (x+1) + dir;

    stopwatch.Stop();
    Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
    Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);
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
