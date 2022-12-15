using System;
using System.Diagnostics;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

Run(@"..\..\..\example_input.txt", true, 10, 20);
Run(@"E:\develop\advent-of-code-input\2022\day15.txt", false, 2000000, 4000000);

void Run(string inputfile, bool isTest, long checkrow, long maxXY)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 26;
    long supposedanswer2 = 56000011;
    
    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var field = new Dictionary<(long, long), char>();
    var field1 = new List<(long from, long to)>();
    var beacons = new HashSet<(long x, long y)>();
    var signals = new List<((long x, long y) c, long mhd)>();
    int i = 0;
    while (i<S.Count)
    {
        var s = S[i];
        var sb = s.Split(" ");
        long xS = long.Parse(sb[2][0..^1].Split("=")[1]);
        long yS = long.Parse(sb[3][0..^1].Split("=")[1]);
        long xB = long.Parse(sb[8][0..^1].Split("=")[1]);
        long yB = long.Parse(sb[9].Split("=")[1]);
        
        if ( yS == checkrow ) field[(xS, yS)] = 'S';
        if ( yB == checkrow) field[(xB, yB)] = 'S';
        
        long mhd = Math.Abs(xS - xB) + Math.Abs(yS - yB);
        var c = ((xS, yS), mhd);
        signals.Add(c);
        if (yB == checkrow) beacons.Add((xB,yB));
        field1 = addRange(((xS, yS), mhd), checkrow, field1);
        i++;
    }
    answer1 = field1.Select(c => 1L + c.to - c.from).Sum() - beacons.Count;

    for (long y = 0; y <= maxXY; y++)
    {
        var field2 = new List<(long from, long to)>();
        foreach (var v in signals)
        {
            field2 = addRange(v, y, field2, maxXY);
        }
        if (field2.Count > 1) 
        {
            answer2 = 4000000 * (field2[0].to + 1) + y;
            break;
        }
    }

    stopwatch.Stop();
    Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
    Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);
}
static List<(long from,long to)> addRange(((long x, long y) c, long mhd) v, long y, List<(long from, long to)> field1, long? maxXY = null)
{
    if (y > v.c.y + v.mhd || y < v.c.y - v.mhd)
    {
        return field1;
    }
    long mhdx = (v.mhd - (Math.Abs(y - v.c.y)));
    long xmin = v.c.x - mhdx;
    long xmax = v.c.x + mhdx;
    if (maxXY.HasValue)
    {
        xmin = Math.Max(xmin, 0);
        xmax = Math.Min(xmax, maxXY.Value);
    }

    int k = 0;
    var field2 = new List<(long from, long to)>();
    if (xmax >= xmin)
    {
        while (k < field1.Count && field1[k].to < xmin - 1)
        {
            field2.Add(field1[k]);
            k++;
        }
        if (k < field1.Count)
        {
            xmin = Math.Min(xmin, field1[k].from);
            long tempTo = 0;
            while (k < field1.Count && field1[k].from <= xmax + 1)
            {
                tempTo = field1[k].to;
                k++;
            }
            xmax = Math.Max(xmax, tempTo);
            field2.Add((xmin, xmax));
            while (k < field1.Count())
            {
                field2.Add(field1[k]);
                k++;
            }

        }
        else
        {
            field2.Add((xmin, xmax));
        }

        return field2;
    }
    return field1;
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



class cmp : IComparer<string>
{
    int IComparer<string>.Compare(string? x, string? y)
    {
        return compare1(new string[] { x }, new string[] { y });
        
    }
    public static int compare1(string[] left, string[] right)
    {
        for (int i = 0; i < left.Length; i++)
        {
            if (right.Length <= i) return 1;

            if (left[i].Length == 0) return right[i].Length == 0 ? 0 : -1;
            else if (right[i].Length == 0) return 1;

            if (left[i][0] == '[')
            {
                var l1 = makelist(left[i][1..^1]);
                if (right[i][0] == '[')
                {
                    var r1 = makelist(right[i][1..^1]);
                    int order = compare1(l1, r1);
                    if (order != 0) return order;
                }
                else
                {
                    var r1 = new string[] { right[i] };
                    int order = compare1(l1, r1);
                    if (order != 0) return order;
                }
            }
            else if (right[i][0] == '[')
            {
                var l1 = new string[] { left[i] };
                var r1 = makelist(right[i][1..^1]);
                int order = compare1(l1, r1);
                if (order != 0) return order;
            }
            else
            {
                int a = int.Parse(left[i]);
                int b = int.Parse(right[i]);
                if (a > b) return 1;
                else if (a < b) return -1;
            }
        }
        if (right.Length > left.Length) return -1;
        return 0;
    }

    static string[] makelist(string s)
    {
        var l = new List<string>();
        var s1 = "";
        int level = 0;
        foreach (var c in s)
        {
            if  (c == ',' && level == 0)
            {
                l.Add(s1);
                s1 = "";
                continue;
            } 
            if (c == '[') level++; 
            else if (c == ']')  level--;
            s1 += c;
        }
        l.Add(s1);
        return l.ToArray();
    }
}