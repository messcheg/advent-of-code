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
    long supposedanswer2 = 30;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    var scores = new Dictionary<int, long>();
    var finalscores = new Dictionary<int, long>();
    var cardstoplay = new List<int>();
    for (int i = 0; i < S.Count; i++) cardstoplay.Add(i);


    foreach (var s in S)
    {
        long score = getscore(s);
        answer1 += score;
    }

    for (int crd = 0; crd < S.Count; crd++)
    {
        answer2 += Getfinalscore(S, scores, finalscores, crd);
    }
    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);

    static long Getfinalscore(List<string> S, Dictionary<int, long> scores, Dictionary<int, long> finalscores, int crd1)
    {
        long subAnswer = 0;
        int crd = crd1;
        if (crd >= S.Count) return 0;

        if (!finalscores.TryGetValue(crd, out subAnswer))
        {
            subAnswer = 1;
            long score;
            if (!scores.TryGetValue(crd, out score))
            {
                score = getscore1(S[crd]);
                scores[crd] = score;
            } 
            while (crd < S.Count && crd < crd + score)
            {
                crd++;
                subAnswer += Getfinalscore(S, scores, finalscores, crd);
            }
            finalscores[crd1] = subAnswer;
        }
        return subAnswer;
    }
}

static long getscore(string? s)
{
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