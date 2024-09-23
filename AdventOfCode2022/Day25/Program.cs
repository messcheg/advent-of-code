using Day25;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Security;

Run1(@"..\..\..\example_input.txt", true);
Run1(@"E:\develop\advent-of-code-input\2022\day25.txt", false);
Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day25.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    string supposedanswer1 = "2=-1=0";
    long checkdec1 = 4890;
    long supposedanswer2 = 000;
    
    var S = File.ReadAllLines(inputfile).ToList();
    string  answer1 = "";
    long answer2 = 0;
    var elves = new HashSet<(long x, long y)>();
    long sum = 0;
    int i = 0;
    while (i<S.Count)
    {
        var s = S[i];
        sum += SnafuMath.SnafuToDec(s);

        i++;
    }
    if (isTest) Debug.Assert(sum == checkdec1);
    answer1 = SnafuMath.DecToSnafu(sum);

    stopwatch.Stop();
    Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
    Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);
}


void Run1(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    string supposedanswer1 = "2=-1=0";
    long supposedanswer2 = 000;

    var S = File.ReadAllLines(inputfile).ToList();
    string answer1 = "";
    long answer2 = 0;
    
    string sum = "";
    int i = 0;
    while (i < S.Count)
    {
        var s = S[i];
        sum = SnafuMath.Sum(sum, s);

        i++;
    }
    answer1 = sum;

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

