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
    var rnd = new Random();

    int checknodes = 500;
    var fromnodes = new string[checknodes];
    var tonodes = new string[checknodes];
    var nodelist = nodes.ToArray();
    for (int i=0; i<fromnodes.Length; i++)
    {
        int j = rnd.Next() % nodes.Count;
        fromnodes[i] = nodelist[j];
        while (fromnodes[i] == nodelist[j])
        {
            j = rnd.Next() % nodes.Count;
        }
        tonodes[i] = nodelist[j];
    }
    var hitcount = new Dictionary<(string,string), int>();
    foreach (var sc1 in sc) hitcount[sc1] = 0;
    for (int i=0; i< checknodes; i++)
    {
        
        var dist = new Dictionary<string, (bool visited, int distance, List<(string,string)> path)>();
        dist.Add(fromnodes[i], (false,0, new List<(string, string)>()));
        bool found = false;
        while (!found)
        {
            var w = dist.Where(a=> a.Value.visited == false).OrderBy(a=>a.Value.distance).First();
            dist[w.Key] = (true, w.Value.distance, w.Value.path);
            found = w.Key == tonodes[i];
            if (!found)
            {
                var con1 = sc.Where(e => e.a == w.Key).Select(e => e.b);
                var con2 = sc.Where(e => e.b == w.Key).Select(e => e.a);
                foreach (var nd in con1.Union(con2).ToHashSet())
                {
                    if (!dist.ContainsKey(nd) || dist[nd].visited == false && dist[nd].distance > w.Value.distance + 1)
                    {
                        var path1 = w.Value.path.ToList();
                        path1.Add((w.Key, nd));
                        dist[nd] = (false, w.Value.distance + 1, path1);
                    }
                }    
            }
            else
            {
                foreach (var p in w.Value.path)
                {
                    var key = p;
                    if (!hitcount.ContainsKey(p)) key = (p.Item2,p.Item1);
                    hitcount[key] = hitcount[key] + w.Value.path.Count; // volgende optie: af laten  middelsten harder tellen
                }
            }
        }
       
    }
    // possible nodes to cut have the highest hitcount
    var mostpopular = hitcount.OrderByDescending(a => a.Value).ToList();
    var possiblecuts = mostpopular.Select(a=>a.Key).Take(20).ToList();
    var leaveout = new int[] { 0, 1, 2 };
  
    bool ready = false;
    while (!ready)
    {

        var subsets = new Dictionary<string, HashSet<string>>();
        var consleft = sc.ToList();
        foreach (var n in nodes)
        {
            subsets[n] = new HashSet<string>() { n };
        }

        for (int i = 0; i<3;i++)
        {
            consleft.Remove(possiblecuts[leaveout[i]]);
        }
        int concnt = 0;
        while (subsets.Count >= 2  && concnt < consleft.Count)
        {
            var con = consleft[concnt];
            concnt++;
            var ss1 = subsets.Where(s => s.Value.Contains(con.a)).First();
            var ss2 = subsets.Where(s => s.Value.Contains(con.b)).First();
            {
                if (ss1.Key == ss2.Key) continue;

                subsets.Remove(ss2.Key);
                foreach (var n in ss2.Value) { ss1.Value.Add(n); }
            }
        }

        var a = subsets.Values.First();
        var b = subsets.Values.Last();
        answer1 = a.Count * b.Count;
        ready = subsets.Count == 2 && a.Count > 1 && b.Count>1;

        leaveout[2]++;
        if (leaveout[2] >= possiblecuts.Count) { leaveout[2] = 2; leaveout[1]++; }
        if (leaveout[1] >= possiblecuts.Count-1) { leaveout[1] = 1; leaveout[0]++; }
        if (leaveout[0] >= possiblecuts.Count - 2) { leaveout[0] = 0; }

    }


    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

}

