using System.Collections;
using System.Collections.Immutable;
using System.ComponentModel.Design.Serialization;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using AocHelper;

Run(@"..\..\..\example.txt", true, 136, 64);
//Run(@"..\..\..\example1.txt", true, 4, 1);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day14.txt", false, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    //var S = File.ReadAllText(inputfile).Split("\r\n\r\n").Select(s => s.Split("\r\n").ToList()).ToList();
    var S = File.ReadAllLines(inputfile).Select(s => s.ToArray()).ToList();

    long answer1 = 0;
    long answer2 = 0;

    long cycle = 0;
    var cyclestore = new Dictionary<long, List<(long cycle, BitArray pattern)>>();
    bool finishing = false;
    long gap = 0;

    while (cycle < 1000000000)
    {
        // roll the rocks notrth
        for (int y = 0; y < S.Count; y++)
        {
            for (int x = 0; x < S[0].Length; x++)
            {
                if (S[y][x] == 'O')
                {
                    int i = y - 1;
                    while (i >= 0 && S[i][x] == '.')
                    {
                        S[i + 1][x] = '.';
                        S[i][x] = 'O';
                        i--;
                    }
                }
            }
        }

        //count weight
        if (answer1 == 0) for (int y = 0; y < S.Count; y++)
            {
                answer1 += (S.Count - y) * S[y].Select(s => (s == 'O') ? 1 : 0).Sum();
            }


        // roll the rocks west
        for (int x = 0; x < S[0].Length; x++)
        {
            for (int y = 0; y < S.Count; y++)
            {
                if (S[y][x] == 'O')
                {
                    int i = x - 1;
                    while (i >= 0 && S[y][i] == '.')
                    {
                        S[y][i + 1] = '.';
                        S[y][i] = 'O';
                        i--;
                    }
                }
            }
        }
        // roll the rocks south
        for (int y = S.Count - 1; y >= 0; y--)
        {
            for (int x = 0; x < S[0].Length; x++)
            {
                if (S[y][x] == 'O')
                {
                    int i = y + 1;
                    while (i < S.Count && S[i][x] == '.')
                    {
                        S[i - 1][x] = '.';
                        S[i][x] = 'O';
                        i++;
                    }
                }
            }
        }
        // roll the rocks east
        for (int x = S[0].Length - 1; x >= 0; x--)
        {
            for (int y = 0; y < S.Count; y++)
            {
                if (S[y][x] == 'O')
                {
                    int i = x + 1;
                    while (i < S[0].Length && S[y][i] == '.')
                    {
                        S[y][i - 1] = '.';
                        S[y][i] = 'O';
                        i++;
                    }
                }
            }
        }

        // print it
        //Console.WriteLine();
        //Console.WriteLine("=============================");
        //Console.WriteLine((cycle+1).ToString());
        //for (int i = 0; i < S.Count; i++)
        //{
        //    for (int j = 0; j < S[i].Length; j++)
        //    {
        //        Console.Write(S[i][j]);
        //    }
        //    Console.WriteLine();
        //}

        if (!finishing)
        {

            long current = 0;
            BitArray bar = new BitArray(S.Count * S[0].Length);
            for (int y = 0; y < S.Count; y++)
            {
                current += (S.Count - y) * S[y].Select(s => (s == 'O') ? 1 : 0).Sum();
                for (int x = 0; x < S[0].Length; x++)
                {
                    bar[y * S[0].Length + x] = S[y][x] == 'O';
                }
            }
            if (cyclestore.ContainsKey(current))
            {
                var store = cyclestore[current];
                foreach (var p in store)
                {
                    bool eq = true;
                    for (int i = 0; i < bar.Length; i++) if (bar[i] != p.pattern[i]) { eq = false; break; }
                    if (eq)
                    {
                        finishing = true;
                        gap = cycle - p.cycle;
                        long jump = ((1000000000 - cycle) / gap) * gap;
                        cycle += jump;
                        break;
                    }
                }
                if (!finishing) store.Add((cycle, bar));
            }
            else
            {
                var store = new List<(long cycle, BitArray pattern)>();
                store.Add((cycle, bar));
                cyclestore[current] = store;
            }
        
        }
        cycle++;

    }

    for (int y = 0; y < S.Count; y++)
    {
        answer2 += (S.Count - y) * S[y].Select(s => (s == 'O') ? 1 : 0).Sum();
    }


    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());


}
