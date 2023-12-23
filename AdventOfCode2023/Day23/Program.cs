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

Run(@"..\..\..\example.txt", true, 94, 0000);
//Run(@"..\..\..\example1.txt", true, 11687500, 0000);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day23.txt", false, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    //var S = File.ReadAllText(inputfile).Split("\r\n\r\n").Select(s => s.Split("\r\n").ToList()).ToList();
    //var S = File.ReadAllLines(inputfile).Select(a => a.Select(b => b - '0').ToList()).ToList();
    var S = File.ReadAllLines(inputfile);
    
    long answer1 = 0;
    long answer2 = 0;

    var work = new Queue<(int x, int y, HashSet<(int x, int y)> visited)>();
    var bestresult = 0;
    var X = S[0].Length;
    var Y = S.Count();
    work.Enqueue((1, 0, new HashSet<(int x, int y)>()));

    while (work.Count > 0)
    {
        var cur = work.Dequeue();
        cur.visited.Add((cur.x, cur.y));

        void Add(int x, int y, int direction)
        {
            if ( x >= 0  && x  < X && y >= 0 && y < Y && S[y][x] != '#' && !cur.visited.Contains((x,y)))
            {
                if (y == Y - 1)
                {
                    bestresult = Math.Max(bestresult, cur.visited.Count);
                }
                else
                {
                    var c = S[y][x];
                    if (c == '.' 
                        || direction == 0 && c == '>'
                        || direction == 1 && c == 'v'
                        || direction == 2 && c == '<'
                        || direction == 3 && c == '^'
                        )
                    {
                        var vis1 = cur.visited.ToHashSet();
                        work.Enqueue((x, y, vis1));
                    }
                }
            }
        }
        Add(cur.x + 1, cur.y, 0);
        Add(cur.x, cur.y + 1, 1);
        Add(cur.x - 1, cur.y, 2);
        Add(cur.x, cur.y - 1, 3);

    }

    answer1 = bestresult;
    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());
}
