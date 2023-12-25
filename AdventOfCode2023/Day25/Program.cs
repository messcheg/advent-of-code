using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Security;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security;
using System.Security.Cryptography;
using System.Transactions;
using AocHelper;
using Microsoft.Win32.SafeHandles;

Run(@"..\..\..\example.txt", true, 54, 0000);
//Run(@"..\..\..\example1.txt", true, 11687500, 0000);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day25.txt", false, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var S = File.ReadAllLines(inputfile).Select(s => s.Split(": ").Select(s1 => s1.Split(" ").ToArray()).ToArray()).ToArray();

    long answer1 = 0;
    long answer2 = 0;

    var nodes = new HashSet<string>();

    var sc = new List<(string a, string b)>();
    foreach (var connection in S)
    {
        foreach (var c in connection[1])
        {
            if (!sc.Contains((connection[0][0], c)) && !sc.Contains((c, connection[0][0]))) sc.Add((connection[0][0], c));
        }
    }

    foreach (var si in sc)
    {
        nodes.Add(si.a);
        nodes.Add(si.b);
    }

    bool ready = false;
    while (!ready)
    {
        var subsets = new Dictionary<string, HashSet<string>>();
        var consleft = sc.ToList();
        foreach (var n in nodes)
        {
            subsets[n] = new HashSet<string>() { n };
        }

        while (subsets.Count > 2 && consleft.Count >= 3)
        {
            int i = new Random().Next() % consleft.Count;
            var con = sc[i];
            consleft.RemoveAt(i);
            var subset1 = subsets.Where(s => s.Value.Contains(con.a)).First();
            var subset2 = subsets.Where(s => s.Value.Contains(con.a)).First();

            if (subset1.Key == subset2.Key) continue;

            subsets.Remove(subset2.Key);
            foreach (var n in subset2.Value) { subset1.Value.Add(n); }
        }

        var notused = 0;
        foreach (var si in consleft)
        {
            var subset = subsets.Where(s => s.Value.Contains(si.a)).First();
            if (!subset.Value.Contains(si.b)) notused++;

        }
        answer1 = subsets.Values.First().Count * subsets.Values.Skip(1).First().Count;
        ready = notused == 3 && subsets.Count == 2;
    } 


    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

}

