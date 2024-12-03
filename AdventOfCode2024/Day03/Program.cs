using AocHelper;
using System.Diagnostics;
using System.Text.RegularExpressions;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\example1.txt", true);
//Run(@"E:\develop\advent-of-code-input\2024\day03.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 161;
    long supposedanswer2 = 0;

    var S = File.ReadAllText(inputfile).Split("mul(").Select(a => a.Split(')').Select(b => b.Split(',').ToList()).ToList()).ToList();
    ulong answer1 = 0;
    ulong answer1a = 0;
    ulong answer1b = 0;

    long answer2 = 0;


    bool alldigits(string a)
    {
        foreach (var c in a)
        {
            if (c < '0' || c > '9') return false;
        }
        return true;
    }

    foreach (var s in S)
    {
        var pair = s[0];
        if (pair.Count == 2)
        {
            if (alldigits(pair[0]) && alldigits(pair[1]) &&
                pair[0].Length <= 3 && pair[1].Length <= 3 &&
                pair[0].Length >= 1 && pair[1].Length >= 1) answer1a += ulong.Parse(pair[0]) * ulong.Parse(pair[1]);
        }
    }


    var S1 = File.ReadAllText(inputfile);

    int index = S1.IndexOf("mul(");
    while (index > -1)
    {
        index += 4;
        var index1 = S1.IndexOf(",", index);
        if (index1 > -1)
        {
            var val1 = S1.Substring(index, index1 - index);
            if (val1.Length > 0 && val1.Length <= 3 && alldigits(val1))
            {
                index = index1 + 1;
                var index2 = S1.IndexOf(")", index);
                if (index2 > -1)
                {
                    var val2 = S1.Substring(index, index2 - index);
                    if (val2.Length > 0 && val2.Length <= 3 && alldigits(val2))
                    {
                        answer1b += ulong.Parse(val1) * ulong.Parse(val2);
                    }
                }
            }
        }
        index = S1.IndexOf("mul(", index);
    }

    S1 = File.ReadAllText(inputfile);

    var pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
    var reg = new Regex(pattern);
    var matches = reg.Matches(S1);
    foreach (var match in matches)
    {
        var a = match.ToString()[4..^1].ToString().Split(',').Select(x => ulong.Parse(x)).ToList();
        answer1 += a[0] * a[1];

    }

    Aoc.w(1, answer1, (ulong)supposedanswer1, isTest);
    Aoc.w(1, answer1a, (ulong)supposedanswer1, isTest);
    Aoc.w(1, answer1b, (ulong)supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

