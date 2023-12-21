using System.Collections;
using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Net.Security;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security;
using System.Security.Cryptography;
using AocHelper;
using Microsoft.Win32.SafeHandles;

Run(@"..\..\..\example.txt", true, 16, 0000, 6);
//Run(@"..\..\..\example1.txt", true, 11687500, 0000);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day21.txt", false, 0, 0, 64);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2, int targetdist)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    //var S = File.ReadAllText(inputfile).Split("\r\n\r\n").Select(s => s.Split("\r\n").ToList()).ToList();
    //var S = File.ReadAllLines(inputfile).Select(a => a.Select(b => b - '0').ToList()).ToList();
    var S = File.ReadAllLines(inputfile).ToList();

    long answer1 = 0;
    long answer2 = 0;

    var stepsset = new HashSet<(long x, long y)>();
    int X = S[0].Length;
    int Y = S.Count;


    var sx = 0; var sy = 0;
    for (int i = 0; i < X; i++)
        for (int j = 0; j < Y; j++)
            if (S[j][i] == 'S') (sx, sy) = (i, j);
    stepsset.Add((sx, sy));

    for (int i = 0; i < 65; i++)
    {
        if (i == targetdist) answer1 = stepsset.Count;
        var newsteps = new HashSet<(long x, long y)>();
        foreach (var (x, y) in stepsset)
        {
            var ix = (int)(x % X + X) % X;
            var iy = (int)(y % Y + Y) % Y;

            var up = (iy + Y + Y - 1) % Y;
            var down = (iy + Y + 1) % Y;
            var left = (ix + X + X - 1) % X;
            var right = (ix + X + 1) % X;

            if (S[iy][right] != '#') newsteps.Add((x + 1, y));
            if (S[down][ix] != '#') newsteps.Add((x, y + 1));
            if (S[iy][left] != '#') newsteps.Add((x - 1, y));
            if (S[up][ix] != '#') newsteps.Add((x, y - 1));
        }

        if (isTest && i < 35)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Step: " + i.ToString());
            for (int j = -30; j < 30; j++)
            {
                for (int k = -50; k < 50; k++)
                {
                    var ix = (int)(k % X + X) % X;
                    var iy = (int)(j % Y + Y) % Y;

                    if (newsteps.Contains((k, j)))
                    {
                        var clr = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("O");
                        Console.ForegroundColor = clr;
                    }
                    else Console.Write(S[iy][ix]);

                }
                Console.WriteLine();
            }
        }
        stepsset = newsteps;
    }

    // Try to flod unil a certain level
    var visited = new HashSet<(long x, long y)>();
    bool ready = false;
    var work = new HashSet<(long x, long y)>();
    work.Add((sx, sy));
    long steps = 0;
    long steps1 = 0;
    long cnt1 = 0;
    long cnt2 = 0;
    long steps2 = 0;
    while (steps2 == 0)
    {
        steps++;
        var newwork = new HashSet<(long x, long y)>();
        foreach (var pos in work)
        {
            visited.Add(pos);
            var (x, y) = pos;

            var ix = (int)(x % X + X) % X;
            var iy = (int)(y % Y + Y) % Y;

            var up = (iy + Y + Y - 1) % Y;
            var down = (iy + Y + 1) % Y;
            var left = (ix + X + X - 1) % X;
            var right = (ix + X + 1) % X;

            if (S[iy][right] != '#' && !visited.Contains((x + 1, y))) newwork.Add((x + 1, y));
            if (S[down][ix] != '#' && !visited.Contains((x, y + 1))) newwork.Add((x, y + 1));
            if (S[iy][left] != '#' && !visited.Contains((x - 1, y))) newwork.Add((x - 1, y));
            if (S[up][ix] != '#' && !visited.Contains((x, y - 1))) newwork.Add((x, y - 1));

            if (steps1 == 0 && x == 5 * X - 1) steps1 = steps;
            if (x == -4 * X) steps2 = steps;
        }
        if (steps1 == steps) cnt1 = visited.Count(a=> (a.x + a.y) % 2 == 0);
        if (steps2 == steps) cnt2 = visited.Count(a => (a.x + a.y) % 2 == 0);
        work = newwork; 
    }

    var rest  = 26501365 % steps2;
    var div = 26501365 / steps2;



    // 26501365
    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());
}
