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
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
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

        if (isTest && i < 31)
        {
            Drawfield(S, X, Y, i, newsteps);
        }
        stepsset = newsteps;
    }

    // Try to flod unil a certain level
    var visited = new HashSet<(long x, long y)>();
    var visited1 = new HashSet<(long x, long y)>();
    bool ready = false;
    var work = new HashSet<(long x, long y)>();
    work.Add((sx, sy));
    long steps = 0;
    long cnt1 = 0;
    long cnt2 = 0;
   
    long steps1 = 5 * X + 65;
    long steps2 = 6 * X + 65;
    while (steps <= steps2)
    {
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

        }
        if (steps == targetdist)
        {
            answer1 = GetCount(steps, visited);
        }
        if (steps == steps1)
        {
            cnt1 = GetCount(steps, visited);
            visited1 = visited.ToHashSet();
        }
        if ( steps == steps2) cnt2 = GetCount(steps, visited);
        work = newwork;
        steps++;

    }
    Dictionary<(int x, int y), (long odds, long evens)> cards1 = GetCardValues(X, Y, visited1, 7);
    Dictionary<(int x, int y), (long odds, long evens)> cards2 = GetCardValues(X, Y, visited, 8);

    for (int i = -10; i <= 10; i++)
    {
        for (int j = -10; j <= 10; j++)
        {

            if (cards1.ContainsKey((i, j))) Console.Write(cards1[(i, j)].odds.ToString(" 0000 "));
            else Console.Write("      ");
        }
        Console.WriteLine();
    }

    for (int i = -10; i <= 10; i++)
    {
        for (int j = -10; j <= 10; j++)
        {

            if (cards2.ContainsKey((i, j))) Console.Write(cards2[(i, j)].odds.ToString(" 0000 "));
            else Console.Write("      ");
        }
        Console.WriteLine();
    }

    var testvalue = EstimateHugeNumerOfSteps(X, cards2, steps1);
    var testvalue1 = EstimateHugeNumerOfSteps(X, cards2, steps2);
    long target = 26501365;
    answer2 = EstimateHugeNumerOfSteps(X, cards2, target);

    // 26501365
    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());
}

static int GetCount(long stps, HashSet<(long x, long y)> vis)
{
    var odd = stps % 2;
    var a = vis.Count(a => (a.x + a.y) % 2 == 0);
    var b = vis.Count - a;

    return odd == 0 ? a : b;
}

static Dictionary<(int x, int y), (long odds, long evens)> GetCardValues(int X, int Y, HashSet<(long x, long y)> visited1 , int distance)
{
    var cards1 = new Dictionary<(int x, int y), (long odds, long evens)>();
    var work1 = new Queue<(int x, int y)>();
    work1.Enqueue((0, 0));
    while (work1.Count > 0)
    {
        var wrk = work1.Dequeue();
        if (!cards1.ContainsKey(wrk))
        {
            var Y0 = wrk.y * Y;
            var X0 = wrk.x * X;
            var card = visited1.Where(a => X0 <= a.x && a.x < X + X0 && Y0 <= a.y && a.y < Y + Y0);
            var evens = card.Count(a => (a.x + a.y) % 2 == 0);
            var odds = card.Count() - evens;
            cards1[wrk] = (odds, evens);
            if (Math.Abs(wrk.x) + Math.Abs(wrk.y) < distance)
            {
                work1.Enqueue((wrk.x + 1, wrk.y));
                work1.Enqueue((wrk.x - 1, wrk.y));
                work1.Enqueue((wrk.x, wrk.y + 1));
                work1.Enqueue((wrk.x, wrk.y - 1));
            }
        }
    }

    return cards1;
}

static long EstimateHugeNumerOfSteps(int X, Dictionary<(int x, int y), (long odds, long evens)> cards2, long target)
{
    long numberofcards = target / X;
    BigInteger numberOdd = 0;
    BigInteger numberEven = 0;
    if (numberofcards % 2 == 0) { numberOdd = (numberofcards - 1) * (numberofcards - 1); numberEven = (numberofcards ) * (numberofcards ); }
    else { numberOdd = (numberofcards ) * (numberofcards ); numberEven = (numberofcards - 1) * (numberofcards - 1); }
    BigInteger numberSideFirstrow = numberofcards;
    BigInteger numberSideSecondrow = numberofcards - 1;

    BigInteger answer =
            numberOdd * cards2[(0, 0)].odds +
            numberEven * cards2[(0, 0)].evens +
            numberSideFirstrow * (cards2[(-1, 6)].odds + cards2[(1, 6)].odds + cards2[(-1, -6)].odds + cards2[(1, -6)].odds) +
            numberSideSecondrow * (cards2[(-1, 5)].odds + cards2[(1, 5)].odds + cards2[(-1, -5)].odds + cards2[(1, -5)].odds) +
            (cards2[(0, 6)].odds + cards2[(6, 0)].odds + cards2[(0, -6)].odds + cards2[(-6, 0)].odds);
    BigInteger answer2 =
            numberOdd * cards2[(0, 0)].evens +
            numberEven * cards2[(0, 0)].odds +
            numberSideFirstrow * (cards2[(-1, 6)].odds + cards2[(1, 6)].odds + cards2[(-1, -6)].odds + cards2[(1, -6)].odds) +
            numberSideSecondrow * (cards2[(-1, 5)].odds + cards2[(1, 5)].odds + cards2[(-1, -5)].odds + cards2[(1, -5)].odds) +
            (cards2[(0, 6)].odds + cards2[(6, 0)].odds + cards2[(0, -6)].odds + cards2[(-6, 0)].odds);
    
   
    return target % 2 == 1 ? (long)answer : (long)answer2;
}

static void Drawfield(List<string> S, int X, int Y, int i, HashSet<(long x, long y)> newsteps)
{
    Console.SetCursorPosition(0, 0);
    Console.WriteLine("Step: " + i.ToString());
    for (int j = -30; j < 30; j++)
    {
        for (int k = -30; k < 30; k++)
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