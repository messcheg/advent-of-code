using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\input.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 3;
    long supposedanswer2 = 6;

    var S = File.ReadAllLines(inputfile).Select(s => new { d = s[0], num = long.Parse(s[1..]) }).ToList();
    long answer1 = 0;
    long answer2 = 0;

    answer1 = 0;
    answer2 = 0;
    long p = 50;

    foreach (var s in S)
    {
        bool at0 = p == 0;
        if (s.d == 'L') p -= s.num; else p += s.num;
        long click = p / 100;
        if (p < 0) click = (at0 ? 0 : 1) - click;
        if (p == 0 && s.d == 'L' && click == 0) click = 1;
        p = p % 100;
        if (p < 0) p += 100;
        if (p == 0) answer1++;
        answer2 += click;
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

