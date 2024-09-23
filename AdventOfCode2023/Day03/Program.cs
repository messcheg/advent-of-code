using System.Diagnostics;
using System.Globalization;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using AocHelper;

Run(@"..\..\..\example.txt", true);
Run(@"E:\develop\advent-of-code-input\2023\day03.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 4361;
    long supposedanswer2 = 467835;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    var gears = new Dictionary<(int, int), List<long>>();

    for(int i = 0; i<S.Count; i++)
    {
        int j = 0;
        while (j < S[i].Length) 
        {
            if (S[i][j] >= '0' && S[i][j] <= '9' )
            {
                var j1 = j;
                while (j1 < S[i].Length && S[i][j1] >= '0' && S[i][j1] <= '9')
                {
                    j1++;
                }
                var number = long.Parse(S[i].Substring(j, j1 - j));
                bool isAdjacent = false;
                if (i>0)
                {
                    for (int k = Math.Max(j-1,0); k < Math.Min(j1 + 1, S[i].Length);k++)
                    {
                        isAdjacent = CheckAdjecentCells(S, gears, i-1, k, number, isAdjacent);
                    }
                }
                if (i < S.Count-1)
                {
                    for (int k = Math.Max(j - 1, 0); k < Math.Min(j1 + 1, S[i].Length); k++)
                    {
                        isAdjacent = CheckAdjecentCells(S, gears, i+1, k, number, isAdjacent);
                    }
                }
                if (j > 0)
                {
                    isAdjacent = CheckAdjecentCells(S, gears, i, j-1, number, isAdjacent);
                }
                if (j1 < S[i].Length)
                {
                    isAdjacent = CheckAdjecentCells(S, gears, i, j1, number, isAdjacent);
                }
                if (isAdjacent)
                {
                    answer1 += number;
                }
                j = j1;
            }
            else
            j++;
        }
  
    }
    foreach (var l in gears.Values.Where(g => g.Count > 1))
    {
        long number = 1;
        foreach (var n in l) number = number * n;
        answer2 += number;
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
}

static bool CheckAdjecentCells(List<string> S, Dictionary<(int, int), List<long>> gears, int i, int j1, long number, bool isAdjacent)
{
    var a = S[i][j1];
    if (a != '.' & !(a >= '0' && a <= '9'))
    {
        isAdjacent = true;
        if (a == '*')
        {
            if (!gears.TryGetValue((i, j1), out List<long>? gear))
            {
                gear = new List<long>();
                gears[(i, j1)] = gear;
            }
            gear.Add(number);
        }
    }

    return isAdjacent;
}