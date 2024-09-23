using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using AocHelper;

Run(@"..\..\..\example.txt", true);
//Run(@"..\..\..\example1.txt", true);
Run(@"E:\develop\advent-of-code-input\2023\day06.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 288;
    long supposedanswer2 = 71503;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var times = S[0].Split(":")[1].Split(" ").Where(a => a != "").Select(a => long.Parse(a)).ToArray();
    var dist = S[1].Split(":")[1].Split(" ").Where(a => a != "").Select(a => long.Parse(a)).ToArray();
    answer1 = 1;
    for (int i = 0; i<times.Length; i++)
    {
        var t = times[i];
        var d = dist[i];
        long res = Numberoftimes(t, d);
        answer1 = answer1 * res;
    }

    var t1 = long.Parse(S[0].Split(":")[1].Replace(" ",""));
    var d1 = long.Parse(S[1].Split(":")[1].Replace(" ", ""));


    answer2 = Numberoftimes(t1, d1);


    stopwatch.Stop();
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);

}

static long Numberoftimes(long t, long d)
{
    long low = (long)((t - Math.Sqrt(t * t - 4 * d)) / 2);
    var highd = ((t + Math.Sqrt(t * t - 4 * d)) / 2);
    var high = (long)highd;
    if (high == highd) high = high - 1;
    var res = high - low;
    return res;
}