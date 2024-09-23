using System.Collections.Immutable;
using System.ComponentModel.Design.Serialization;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using AocHelper;

Run(@"..\..\..\example.txt", true, 21, 525152);
//Run(@"..\..\..\example1.txt", true, 4, 1);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day12.txt", false, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    foreach (var s in S)
    {
        var s1 = s.Split(" ").ToArray();
        var rec = s1[0];
        var desc = s1[1].Split(",").Select(a => int.Parse(a)).ToArray();

        var rec2 = rec + "?" + rec + "?" + rec + "?" + rec + "?" + rec;
        var desc2 = desc.ToList();
        for (int i = 0; i < 4; i++) foreach (var v in desc) desc2.Add(v);
        var desc2a = desc2.ToArray();

        var subresults = new Dictionary<(int p, int c), long>();
        answer1 += CheckPos(rec, desc, 0, 0, subresults);
        subresults = new Dictionary<(int p, int c), long>();
        answer2 += CheckPos(rec2, desc2a, 0, 0, subresults);
    }

    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

}

static long CheckPos(string rec, int[] desc, int offset, int current,Dictionary<(int p, int c), long> subresults )
{
    long pb = 0;
    int i = offset;
    bool ready = false;
    if (subresults.ContainsKey((offset, current))) pb = subresults[(offset, current)];
    else
    {
        while (!ready && i <= rec.Length - desc[current])
        {
            string s = rec.Substring(i, desc[current]);
            var off1 = i + desc[current];
            if (s.Length == desc[current] && !s.Contains('.'))
            {
                if (current == desc.Length - 1)
                {
                    if (off1 == rec.Length || !rec.Substring(off1).Contains("#")) pb++;
                }
                else
                {
                    if (off1 < rec.Length && (rec[off1] == '.' || rec[off1] == '?'))
                    {
                        pb += CheckPos(rec, desc, off1 + 1, current + 1, subresults);
                    }
                }
            }
            if (rec[i] == '.' || rec[i] == '?') i++;
            else ready = true;
        }
        subresults[(offset, current)] = pb;
    }
    return pb;
}