using System.Diagnostics;
using System.Globalization;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using AocHelper;

//Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\example1.txt", true);
Run(@"E:\develop\advent-of-code-input\2023\day04.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 13;
    ulong supposedanswer2 = 30;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    ulong answer2 = 0;
    var numberofcopies = new List<ulong>();
    for (int i = 0; i < S.Count; i++) numberofcopies.Add(1);


    foreach (var s in S)
    {
        long score = getscore(s);
        answer1 += score;
    }

    for (int crd = 0; crd < S.Count; crd++)
    {
        var score = getscore1(S[crd]);
        for (int i = crd + 1; i < S.Count && i <= crd + score; i++) 
        {
            numberofcopies[i] += numberofcopies[crd];
        }
        answer2 += numberofcopies[crd];
    }



    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);

}

static long getscore(string? s)
{
    if (s == null) return 0;
    var s1 = s.Split(" | ");
    var s2 = s1[0].Split(": ");
    var winning = s2[1].Trim().Split(" ").Where(a => a != "").Select(a => int.Parse(a)).ToHashSet();
    var numbers = s1[1].Trim().Split(" ").Where(a => a != "").Select(a => int.Parse(a)).ToList();
    long score = 0;
    foreach (var n in numbers)
    {
        if (winning.Contains(n))
        {
            if (score == 0) score += 1;
            else score *= 2;
        }
    }

    return score;
}

static long getscore1(string? s)
{
    if (s == null) return 0;
    var s1 = s.Split(" | ");
    var s2 = s1[0].Split(": ");
    var winning = s2[1].Trim().Split(" ").Where(a => a != "").Select(a => int.Parse(a)).ToHashSet();
    var numbers = s1[1].Trim().Split(" ").Where(a => a != "").Select(a => int.Parse(a)).ToList();
    long score = 0;
    foreach (var n in numbers)
    {
        if (winning.Contains(n))
        {
            score += 1;
        }
    }

    return score;
}