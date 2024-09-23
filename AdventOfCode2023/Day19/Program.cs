using System.Collections;
using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Net.Security;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security;
using System.Security.Cryptography;
using AocHelper;
using Microsoft.Win32.SafeHandles;

Run(@"..\..\..\example.txt", true, 19114, 167409079868000);
//Run(@"..\..\..\example1.txt", true, 61, 0000);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day19.txt", false, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var S = File.ReadAllText(inputfile).Split("\r\n\r\n").Select(s => s.Split("\r\n").ToList()).ToList();
    //var S = File.ReadAllLines(inputfile).Select(a => a.Select(b => b - '0').ToList()).ToList();
    //var S = File.ReadAllLines(inputfile).ToList();

    long answer1 = 0;
    long answer2 = 0;

    var rules = new Dictionary<string, (int v, char op, long val, string target)[]>();
    foreach (var rule in S[0])
    {
        var s1 = rule.Split("{");
        var name = s1[0];
        var s2 = s1[1].Substring(0, s1[1].Length-1).Split(",");
        var ar = new (int v, char op, long val, string target)[s2.Length];
        for (int i=0; i<s2.Length-1;i++)
        {
            var s3 = s2[i].Substring(2).Split(":");
            var v = s2[i][0];
            var j = v == 'x' ? 0 : v == 'm' ? 1 : v == 'a' ? 2 : 3;

            ar[i] = (j, s2[i][1], long.Parse(s3[0]),s3[1]);
        }
        ar[s2.Length - 1] = (' ', ' ', 0, s2[^1]);
        rules[name] = ar;
    }

    foreach (var part in S[1]) 
    {
        var s1 = part.Substring(1, part.Length - 2).Split(",").Select(p => long.Parse(p.Substring(2))).ToArray();
        var workflow = "in";
        if (isTest) Console.Write(workflow);
        while (workflow != "A" && workflow!="R")
        {
            if (isTest) Console.Write(" -> ");
            var wf = rules[workflow];
            for (int i=0; i< wf.Length;i++)
            {
                var w = wf[i];
                if (w.op == '<' && s1[w.v] < w.val || w.op == '>' && s1[w.v] > w.val || w.op == ' ')
                {
                    workflow = w.target;
                    break;
                }
            }
            if (isTest) Console.Write(workflow);
        }
        long total = s1[0] + s1[1] + s1[2] + s1[3];
        if (isTest) Console.WriteLine("  (" + total.ToString() + ")");
        if (workflow == "A") answer1 += total;
    }

    var work = new Queue<(string flow, (long min, long max)[] bounds)>();
    var accepted = new List<List<(long min, long max)>>();
    work.Enqueue(("in", new (long min, long max)[]{ (0,4001),(0,4001),(0, 4001),(0, 4001)}));
    while (work.Count > 0)
    {
        var wrk = work.Dequeue();
        var rl = rules[wrk.flow];
        var rest = wrk.bounds.ToArray();
        foreach (var r in rl)
        {
            var first = rest.ToArray();
            switch (r.op)
            {
                case '>':
                    if (r.val < first[r.v].max)
                    {
                        first[r.v].min = Math.Max(first[r.v].min, r.val);                   
                    }
                    if(r.val > rest[r.v].min)
                    {
                        rest[r.v].max = Math.Min(rest[r.v].max, r.val+1);
                    }
                    
                    break;
                case '<':
                    if (r.val > first[r.v].min)
                    {
                        first[r.v].max = Math.Min(first[r.v].max, r.val);

                    }
                    if (r.val < rest[r.v].max)
                    {
                        rest[r.v].min = Math.Max(rest[r.v].min, r.val-1);
                    }
                    break;
                case ' ': break;
            }
            
            if (r.target != "R")
            {
                if (r.target == "A")
                {
                    accepted.Add(first.ToList());
                }
                else
                {
                    work.Enqueue((r.target, first));
                }
            }

        }
    }
    
    // calculate sum
    foreach (var acc in accepted)
    {
        long prod = 1;
        foreach (var item in acc) 
        {
            var rng = item.max - item.min -1;
            prod *= rng;
        }
        answer2 += prod;
    }

    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

}
