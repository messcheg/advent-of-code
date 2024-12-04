using System.Collections.Immutable;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using AocHelper;

Run(@"..\..\..\example.txt", true, 8, 0000);
Run(@"..\..\..\example1.txt", true, 4, 1);
Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day10.txt", false, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var X = S.Select(l => l.Select(k => '.').ToArray()).ToArray();

    // find S
    int x = 0;
    int y = 0;
    bool ready = false;
    while (y<S.Count && !ready)
    {
        while (x < S[0].Length && !ready)
        {
            ready = S[y][x] == 'S';
            if (!ready) x++;
        }
        if (!ready)
        {
            x = 0;
            y++;
        }
    }

    int direction = 0;
    int steps = 0;
    ready = false;
    X[y][x] = 'S';
    if (x < S[0].Length - 1 && "-J7".Contains(S[y][x+1])) direction = 1;
    else if (y > 0 && "|7F".Contains(S[y-1][x])) direction = 2;
    else if (x > 0 && "-FL".Contains(S[y][x-1])) direction = 3;
    while (!ready)
    {
        char c = '.';
        switch (direction)
        {
            case 0:
                c = S[y + 1][x];
                if (c == 'J') direction = 3;
                else if (c == 'L') direction = 1;
                y++;
                break;
            case 1:
                c = S[y][x + 1];
                if (c == 'J') direction = 2;
                else if (c == '7') direction = 0;
                x++;
                break;
            case 2:
                c = S[y - 1][x];
                if (c == 'F') direction = 1;
                else if (c == '7') direction = 3;
                y--;
                break;
            case 3:
                c = S[y][x - 1];
                if (c == 'F') direction = 0;
                else if (c == 'L') direction = 2;
                x--;
                break;
            default: break;
        }
        X[y][x] = c;
        ready = c == 'S';
        steps++;
    }

    answer1 = (long)steps / 2;

    for (int i=0;i<X.Length;i++)
    {
        bool inside = false;
        char online = '.';
        for (int j = 0; j < X[0].Length; j++)
        {
            char cx = X[i][j];
            if ("|JLF7".Contains(cx))
            {
                switch (cx)
                {
                    case '|': inside = !inside; break;
                    case 'F': online = 'F'; break;
                    case 'L': online = 'L'; break;
                    case '7': if (online == 'L') inside = !inside; break;
                    case 'J': if (online == 'F') inside = !inside;break;
                    default: break;
                }
            }
            else if (cx == '.')
            {
                if (inside) answer2++;
            }
        }
    }


    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

}
