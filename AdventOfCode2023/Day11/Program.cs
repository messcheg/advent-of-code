using System.Collections.Immutable;
using System.ComponentModel.Design.Serialization;
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

Run(@"..\..\..\example.txt", true, 374, 0000);
//Run(@"..\..\..\example1.txt", true, 4, 1);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day11.txt", false, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    ulong answer2 = 0;

    // find horizontal spaces
    var hEmty = S.Select(b => b.Length == b.Where(c => c == '.').Count()).ToList();
    var vEmpty = new List<bool>(S[0].Length);
    for (int i = 0; i < S[0].Length;i++)
    {
        bool empty = true;
        for (int j = 0; j<S.Count; j++) if (S[j][i] == '#') empty = false;
        vEmpty.Add(empty);
    }

    var G = new List<(int x, int y)>();
    int i1 = 0;
    var G2 = new List<(long x, long y)>();
    long i2 = 0;
    for (int i = 0;i <S.Count;i++)
    {
        if (hEmty[i])
        {
            i1 += 2;
            i2 += 1000000;
        }
        else 
        {
            i2++;
            i1++;
        }

        int j1 = 0;
        long j2 = 0;
        j2 = 0;
        for (int j = 0; j < S[0].Length; j++)
        {
            if (vEmpty[j]) {
                j1 +=2;
                j2 += 1000000;
            }
            else
            {
                j1++;
                j2++;
            }

            if (S[i][j] == '#')
            {
                G.Add((j1, i1));
                G2.Add((j2, i2));
            }
        }
    }
    
    for (int i=0;i<G.Count; i++)
    {
        var a = G[i];
        var a2 = G2[i];

        for (int j = i+1; j<G.Count; j++)
        {
            var b = G[j];
            var b2 = G2[j];
            answer1 += Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
            answer2 += (ulong)Math.Abs(a2.x - b2.x) + (ulong)Math.Abs(a2.y - b2.y);
        }
    }


    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w<ulong>(2, answer2, (ulong)supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

}
