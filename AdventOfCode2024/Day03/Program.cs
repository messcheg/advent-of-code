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
    long supposedanswer2 = 48;

    // solution 1
    var S = File.ReadAllText(inputfile).Split("mul(").Select(a => a.Split(')').Select(b => b.Split(',').ToList()).ToList()).ToList();
    long answer1 = 0;
    long answer1a = 0;
    long answer1b = 0;

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
                pair[0].Length >= 1 && pair[1].Length >= 1) answer1a += long.Parse(pair[0]) * long.Parse(pair[1]);
        }
    }

    // solution 2
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
                        answer1b += long.Parse(val1) * long.Parse(val2);
                    }
                }
            }
        }
        index = S1.IndexOf("mul(", index);
    }

    // solution 3
    S1 = File.ReadAllText(inputfile);

    var pattern = @"(mul\((\d{1,3}),(\d{1,3})\))|(do\(\))|(don\'t\(\))";

    var reg = new Regex(pattern);
    var matches = reg.Matches(S1);
    bool do_do = true;
    foreach (var match in matches)
    {
        var a1 = match.ToString();

        if (a1 == "don't()") do_do = false;
        if (a1 == "do()") do_do = true;


        if (a1[0] == 'm')
        {
            var a = a1[4..^1].ToString().Split(',').Select(x => long.Parse(x)).ToList();

            answer1 += a[0] * a[1];
            if (do_do) { answer2 += a[0] * a[1]; }
        }
    }

    Aoc.w(1, answer1a, (long)supposedanswer1, isTest);
    Aoc.w(1, answer1b, (long)supposedanswer1, isTest);
    Aoc.w(1, answer1, (long)supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

