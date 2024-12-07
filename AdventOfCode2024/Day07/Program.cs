using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true, true);
Run(@"..\..\..\example1.txt", false, true);
Run(@"..\..\..\example.txt", true, false);
Run(@"..\..\..\example1.txt", false, false);
//Run(@"E:\develop\advent-of-code-input\2024\day07.txt", false);

void Run(string inputfile, bool isTest, bool bruteforce)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 3749;
    long supposedanswer2 = 11387;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    foreach (var s in S)
    {
        var s1 = s.Split(": ");
        var a = long.Parse(s1[0]);
        var s2 = s1[1].Split(' ').Select(x => long.Parse(x)).ToArray();

        if (bruteforce)
        {
            var res = AllAnswers1(s2, a);
            if (res.l1.Contains(a)) answer1 += a;
            if (res.l2.Contains(a)) answer2 += a;
        }
        else
        {
            if (IsPossible(s2, a, false)) answer1 += a;
            if (IsPossible(s2, a, true)) answer2 += a;
        }
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

bool IsPossible(long[] terms, long target, bool useconcat)
{
    if (terms.Length == 1) return terms[0] == target;
    if (terms.Length >= 2)
    {
        var last = terms[terms.Length - 1];
        var rest = terms[0..(terms.Length - 1)];
        if (target % last == 0 && IsPossible(rest, target / last, useconcat)) return true;
        if (target - last > 0 && IsPossible(rest, target - last, useconcat)) return true;
        if (useconcat)
        {
            var laststr = last.ToString();
            var targstr = target.ToString();
            if (targstr.Length > laststr.Length
                && targstr.Substring(targstr.Length - laststr.Length) == laststr
                && IsPossible(rest, long.Parse(targstr.Substring(0, targstr.Length - laststr.Length)), useconcat)) return true;
        }
    }

    return false;
}

(List<long> l1, List<long> l2) AllAnswers1(long[] terms, long max)
{
    if (terms.Length == 0) return (new List<long>(), new List<long>());
    if (terms.Length == 1) return (terms.ToList(), terms.ToList());
    var last = terms[terms.Length - 1];
    var rest = terms[0..(terms.Length - 1)];
    var prevAnswers = AllAnswers1(rest, max);
    var answer1 = new List<long>();
    void AddAndCheck(List<long> l, long a) { if (a <= max) l.Add(a); }
    foreach (var ans in prevAnswers.l1)
    {
        AddAndCheck(answer1, last * ans);
        AddAndCheck(answer1, last + ans);
    }
    var answer2 = new List<long>();
    foreach (var ans in prevAnswers.l2)
    {
        AddAndCheck(answer2, last * ans);
        AddAndCheck(answer2, last + ans);
        AddAndCheck(answer2, long.Parse(ans.ToString() + last.ToString()));
    }
    return (answer1, answer2);
}