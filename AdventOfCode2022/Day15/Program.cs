using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
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

    ((long from, long to)[][] field, int cur, int[] cnt) f = (new (long from, long to)[][] { new (long from, long to)[S.Count], new (long from, long to)[S.Count] }, 0, new int[] { 0, 0 });
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
        
        long mhd = Math.Abs(xS - xB) + Math.Abs(yS - yB);
        var c = ((xS, yS), mhd);
        signals.Add(c);
        if (yB == checkrow) beacons.Add((xB,yB));
        f = addRange(((xS, yS), mhd), checkrow, f);
        i++;
    }
    answer1 = f.field[f.cur][0..f.cnt[f.cur]].Select(c => 1L + c.to - c.from).Sum() - beacons.Count;

    //var skipY = new List<(long, long)>(S.Count + 1);
    //foreach (var v in signals)
    //{
    //    if (v.c.x - v.mhd <= 0 && v.c.x + mhd >= maxXY)
    //    {
    //        var skip1 = new List<(long, long)>(S.Count + 1);
    //        long ydif = Math.Min(0 - (v.c.x - v.mhd), v.c.x + mhd >= maxXY);
    //        long ymin = Math.Max(v.c.y - ydif, 0);
    //        long ymax = Math.Min(v.c.y + ydiff, maxXY);
    //        int k = 0;
    //        while ( k < skipY.Count && skipY[k].Item2 < ymin -1)
    //        {
    //            skip1.Add(skipY[k]);
    //            k++;
    //        }
    //        // HIer was ik gebleven
    //    }
    //}
        f.cur = 0;
    f.cnt[0] = 0;
    for (long y = 0; y <= maxXY; y++)
    {
        f.cur = 0;
        f.cnt[0] = 0;
        foreach (var v in signals)
        {
            f = addRange(v, y, f, maxXY);
        }
        if (f.cnt[f.cur] > 1) 
        {
            answer2 = 4000000 * (f.field[f.cur][0].to + 1) + y;
            break;
        }
    }

    stopwatch.Stop();
    Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
    Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);
}

static ((long from, long to)[][] field, int cur, int[] cnt) addRange(((long x, long y) c, long mhd) v, long y, ((long from, long to)[][] field, int cur, int[] cnt) f, long? maxXY = null)
{
    if (y > v.c.y + v.mhd || y < v.c.y - v.mhd)
    {
        return f;
    }
    long mhdx = (v.mhd - (Math.Abs(y - v.c.y)));
    long xmin = v.c.x - mhdx;
    long xmax = v.c.x + mhdx;
    if (maxXY.HasValue)
    {
        xmin = Math.Max(xmin, 0);
        xmax = Math.Min(xmax, maxXY.Value);
    }
    int a = f.cur;
    int b = 1 - f.cur;
    if (xmax >= xmin)
    {
        int k = 0;
        f.cnt[b] = 0;
        while (k < f.cnt[a] && f.field[a][k].to < xmin - 1)
        {
            f.field[b][f.cnt[b]] = f.field[a][k];
            f.cnt[b]++;
            k++;
        }
        if (k < f.cnt[a])
        {
            xmin = Math.Min(xmin, f.field[a][k].from);
            long tempTo = 0;
            while (k < f.cnt[a] && f.field[a][k].from <= xmax + 1)
            {
                tempTo = f.field[a][k].to;
                k++;
            }
            xmax = Math.Max(xmax, tempTo);
            f.field[b][f.cnt[b]] = (xmin, xmax);
            f.cnt[b]++;
            while (k < f.cnt[a])
            {
                f.field[b][f.cnt[b]] = f.field[a][k];
                f.cnt[b]++;
                k++;
            }
        }
        else
        {
            f.field[b][f.cnt[b]] = (xmin, xmax);
            f.cnt[b]++;
        }
        f.cur = b;
        return f;
    }
    return f;

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