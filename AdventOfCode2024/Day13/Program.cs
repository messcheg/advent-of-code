using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\example1.txt", true);
//Run(@"E:\develop\advent-of-code-input\2024\day13.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 480;
    long supposedanswer2 = 0;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    int i = 0;
    while (i < S.Count)
    {
        var s = S[i].Split(", ").Select(a => long.Parse(a.Split("+").Last())).ToArray();
        var x1 = s[0];
        var y1 = s[1];
        i++;

        s = S[i].Split(", ").Select(a => long.Parse(a.Split("+").Last())).ToArray();
        var x2 = s[0];
        var y2 = s[1];
        i++;

        s = S[i].Split(", ").Select(a => long.Parse(a.Split("=").Last())).ToArray();
        var x = s[0];
        var y = s[1];
        i++;
        i++;

        var B = (y * x1 - x * y1) / (y2 * x1 - x2 * y1);
        var A = (x - B * x2) / x1;

        if (A >= 0 && B >= 0 && B * x2 + A * x1 == x && B * y2 + A * y1 == y) answer1 += A * 3 + B;

        x += 10000000000000;
        y += 10000000000000;
        B = (y * x1 - x * y1) / (y2 * x1 - x2 * y1);
        A = (x - B * x2) / x1;
        if (A >= 0 && B >= 0 && B * x2 + A * x1 == x && B * y2 + A * y1 == y) answer2 += A * 3 + B;
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

