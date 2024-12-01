using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\example1.txt", true);
//Run(@"E:\develop\advent-of-code-input\2024\day24.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 0;
    long supposedanswer2 = 0;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    foreach (var s in S)
    {

    }


    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

