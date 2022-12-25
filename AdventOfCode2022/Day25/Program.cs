using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Security;

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
        sum += SnafuToDec(s);

        i++;
    }
    if (isTest) Debug.Assert(sum == checkdec1);
    answer1 = DecToSnafu(sum);

    stopwatch.Stop();
    Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
    Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);
}

long SnafuToDec(string snafu)
{
    long dec = 0;
    for (int i = 0; i < snafu.Length; i++)
    {
        int a = 0;
        switch(snafu[i])
        {
            case '0': a =  0; break;
            case '1': a = 1; break;
            case '2': a = 2; break;
            case '-': a = -1; break;
            case '=': a = -2; break;
        }
        dec = 5 * dec + a;
    }
    return dec;
}

string DecToSnafu(long dec)
{
    string snafu = "";
    long snfp = 0;
    long snf0 = 0;
    long snf1 = 1;
    while (dec > snf1 + snfp) 
    {
        snfp += 2 * snf0;
        snf0 = snf1;
        snf1 *= 5;
    }

    while (snf0 > 0)
    {
        snf1 = snf0;
        snf0 /= 5;
        
        var d1 = dec >= 0 ? (dec + snfp) / snf1 : (dec - snfp) / snf1;
        snfp -= 2 * snf0;

        char nxt = '0';
        switch(d1)
        {
            case -2: nxt = '='; break;
            case -1: nxt = '-'; break;
            case 0: nxt = '0'; break;
            case 1: nxt = '1'; break;
            case 2: nxt = '2'; break;
        }
        snafu += nxt;
        dec -= d1 * snf1;        
    }
    return snafu;
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
