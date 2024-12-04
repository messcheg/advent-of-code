using System.Collections.Immutable;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using AocHelper;

Run(@"..\..\..\example.txt", true, 114, 2);
//Run(@"..\..\..\example1.txt", true, -1, 6);
Run(@"E:\develop\advent-of-code-input\2023\day09.txt", false);

void Run(string inputfile, bool isTest, long supposedanswer1 = 0, long supposedanswer2 = 0)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    foreach (var s in S)
    {
        var line = s.Split(" ").Select(l=>long.Parse(l)).ToList();
        var lines = new List<List<long>>();
        lines.Add(line);
        var allzeros = false;
        while (!allzeros)
        {
            line = new List<long>();
            var curline = lines[lines.Count - 1];
            allzeros = true;
            for (int i = 0; i< curline.Count-1;i++)
            {
                long dif = curline[i + 1] - curline[i];
                if (dif != 0) allzeros = false;
                line.Add(dif);
            }
            lines.Add(line);
        }
        answer1 += lines.Select(l => l[l.Count-1]).Sum();
        
        long tot = 0;
        for (int i = lines.Count-1; i>=0;i--)
        {
            tot = lines[i][0] - tot;
        }
        answer2 += tot;
    }





    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

}
