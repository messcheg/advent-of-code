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

Run(@"..\..\..\example.txt", true, 32000000, 0000);
Run(@"..\..\..\example1.txt", true, 11687500, 0000);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day20.txt", false, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    //var S = File.ReadAllText(inputfile).Split("\r\n\r\n").Select(s => s.Split("\r\n").ToList()).ToList();
    //var S = File.ReadAllLines(inputfile).Select(a => a.Select(b => b - '0').ToList()).ToList();
    var S = File.ReadAllLines(inputfile).ToList();

    long answer1 = 0;
    ulong answer2 = 0;

    // initialize modules
    var modules = new Dictionary<string, (char type, int state, string[] destinations, Dictionary<string, int > inputs, string name )>();
    foreach (var s in S)
    {
        var s1 = s.Split(" -> ");
        var type = s1[0][0];
        var name = type == 'b' ? s1[0] : s1[0].Substring(1);
        modules[name] = (s1[0][0], -1, s1[1].Split(", ").ToArray(), new Dictionary<string,int>(), name);
    }
    foreach (var mod in modules.Values)
    {
        foreach (var mod2 in modules.Values)
        {
            if (mod2.destinations.Contains(mod.name)) mod.inputs[mod2.name] = -1;
        }
    }


    //find relevant modules for performance
    var reversemodules = new Dictionary<string, List<string>>();
    var relevant = new HashSet<string>();
    foreach (var t in modules.SelectMany(m => m.Value.destinations.Select(a => (m.Value.name, a))))
    {
        List<string> sources;
        if (!reversemodules.TryGetValue(t.a, out sources))
        {
            sources = new List<string>();
            reversemodules[t.a] = sources;
        }
        sources.Add(t.name);
    }
    var work1 = new Queue<string>();
    work1.Enqueue("rx");
    work1.Enqueue("output");
    while (work1.Count > 0)
    {
        var wrk = work1.Dequeue();
        if (reversemodules.ContainsKey(wrk)) {
            var rev = reversemodules[wrk];
            relevant.Add(wrk);
            foreach (var r in rev) if (!relevant.Contains(r))
                {
                    relevant.Add(r);
                    work1.Enqueue(r);
                }
        }
    }
    var relevantConjunctions = new Dictionary<string, ulong>();
    foreach (var r in relevant)
    {
        if (modules.ContainsKey(r) && modules[r].type == '&') relevantConjunctions.Add(r, 0);
    }

    var work = new Queue<(string target, string origin, int pulse)>();
    long i = 0;
    long top = 1000;
    long highpulses = 0;
    long lowpulses = 0;
    bool skipped = false;
    while (answer1 == 0 || answer2 == 0)
    {
        work.Enqueue(("broadcaster", "button", -1));
        i++;
        while (work.Count > 0)
        {
            var cur = work.Dequeue();

            if (cur.pulse == -1)
            {
                lowpulses++;
                if (relevantConjunctions.Count > 0)
                {
                    if (relevantConjunctions.ContainsKey(cur.target))
                    {
                        relevantConjunctions[cur.target] = (ulong) i;
                    }
                    ulong LCM(ulong a, ulong b)
                    {
                        var (g1, g2) = a > b ? (a, b) : (b, a);
                        while (g2 != 0) (g1, g2) = (g2, g1 % g2);

                        return a * (b / g1);
                    }

                    if (relevantConjunctions.Values.All(v => v > 0)) answer2 = relevantConjunctions.Values.Aggregate((a, b) => LCM(a,b ));
                }
                else answer2 = 1;
            }
            if (cur.pulse == 1) highpulses++;
            if (modules.ContainsKey(cur.target))
            {
                var mod = modules[cur.target];
                var snd = 0;
                switch (mod.type)
                {
                    case 'b': snd = cur.pulse; break;
                    case '%':
                        if (cur.pulse == -1)
                        {
                            if (mod.state == -1) 
                            {
                                mod.state = 1;
                                snd = 1;
                            }
                            else
                            {
                                mod.state = -1;
                                snd = -1;
                            }
                        }
                        break;
                    case '&':
                        mod.inputs[cur.origin] = cur.pulse;
                        if (mod.inputs.Values.Sum() == mod.inputs.Count) snd = -1;
                        else snd = 1;
                        if (mod.inputs.Values.Sum() == - mod.inputs.Count) mod.state = -1;
                        else mod.state = 1;
                        break;
                }
                modules[cur.target] = mod;
                if (snd != 0)
                {
                    foreach (var t in mod.destinations)
                    {
                        if (relevant.Count == 0 || relevant.Contains(t)) work.Enqueue((t, mod.name, snd));
                    }
                }
            }
        }
        if (!skipped)
        {
            var cnt = modules.Select(a => a.Value).Where(c => c.state == -1).ToList().Count;
            if (cnt == modules.Count)
            {
                var gap = (top / i);
                highpulses *= gap;
                lowpulses *= gap;
                i *= gap;
            }
        }
        if (i==top) answer1 = lowpulses * highpulses;

    }


    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, (ulong)supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());
}
