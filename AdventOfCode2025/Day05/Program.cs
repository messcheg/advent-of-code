using AocHelper;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\input.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 3;
    long supposedanswer2 = 14;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var F = new List<(long min, long max)>();
    var A = new List<long>();
;
    answer1 = 0;
    answer2 = 0;

    int i = 0;
    while (S[i] != "")
    {
        var f = S[i].Split('-').Select(a => long.Parse(a)).ToArray();
        F.Add((f[0], f[1]));
        i++;
    }
    i++;
    while(i<S.Count)
    {
        A.Add(long.Parse(S[i]));
        i++;
    }
    F = F.OrderBy(f => f.min).ToList();
    A.Sort();

    bool changed = true;
    while (changed)
    {
        var F1 = new List<(long min, long max)>();
        changed = false;
        for (int c = 0; c < F.Count; c++)
        {
            for (int d = c + 1; d < F.Count; d++)
            {
                if (F[c].max >= F[d].min && F[c].max <= F[d].max || F[d].max < F[c].max)
                {
                    F[c] = (Math.Min(F[c].min, F[d].min), Math.Max(F[c].max, F[d].max));
                    F.RemoveAt(d);
                    changed = true;
                }
            }
            F1.Add(F[c]);
        }
        F = F1;
    }

    i = 0;
    
    foreach (var a in A)
    {
        while (i < F.Count && a > F[i].max) i++;
        if (i < F.Count && F[i].min <= a) answer1++;
    }

    foreach (var f in F)
    {
        answer2 += (f.max - f.min + 1);
    }

    
   
    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}