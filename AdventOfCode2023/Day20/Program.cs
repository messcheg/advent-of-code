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
    long answer2 = 0;

    var modules = new Dictionary<string, (char type, int state, string[] destinations, Dictionary<string, int > inputs, string name )>();
    
    foreach(var s in S)
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
    
    var work = new Queue<(string target, string origin, int pulse)>();
    long i = 0;
    long top = 1000;
    long highpulses = 0;
    long lowpulses = 0;
    bool skipped = false;
    while (i < top)
    {
        work.Enqueue(("broadcaster", "button", -1));
        while (work.Count > 0)
        {
            var cur = work.Dequeue();
            if (cur.pulse == -1) lowpulses++;
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
                        if (t == "vr")
                        {
                            Console.Write("Current: " + mod.name + " Pulse " + cur.pulse + " values ");
                            foreach (var v in mod.inputs) { Console.Write(v.Key + ":" + v.Value + " "); }
                            Console.WriteLine();

                        }
                        work.Enqueue((t, mod.name, snd));
                    }
                }
            }
        }
        i++;
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
    }

    answer1 = lowpulses * highpulses;


    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());
}
