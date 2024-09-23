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

Run(@"..\..\..\example.txt", true, 5, 0000);
//Run(@"..\..\..\example1.txt", true, 11687500, 0000);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day22.txt", false, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    //var S = File.ReadAllText(inputfile).Split("\r\n\r\n").Select(s => s.Split("\r\n").ToList()).ToList();
    //var S = File.ReadAllLines(inputfile).Select(a => a.Select(b => b - '0').ToList()).ToList();
    var S = File.ReadAllLines(inputfile)
        .Select(s=>s.Split("~")
            .Select(s1=>s1.Split(",")
            .Select(s2=>int.Parse(s2)).ToList()).ToList())
         .ToList();

    long answer1 = 0;
    long answer2 = 0;

    var S1 = S.OrderBy(b => Math.Min(b[0][2], b[1][2])).ThenBy(b => Math.Max(b[0][2], b[1][2])).ToList();
    var S2 = new List<(int x1, int x2, int y1, int y2, int z1, int z2, int name)>();
    for (int i = 0; i < S1.Count; i++)
    {
        int bottom = 0;
        var a = S1[i];
        var ax1 = Math.Min(a[0][0], a[1][0]);
        var ax2 = Math.Max(a[0][0], a[1][0]);
        var ay1 = Math.Min(a[0][1], a[1][1]);
        var ay2 = Math.Max(a[0][1], a[1][1]);

        for (int j = i - 1; j >= 0; j--)
        {
            var b = S1[j];
            var bx1 = Math.Min(b[0][0], b[1][0]);
            var bx2 = Math.Max(b[0][0], b[1][0]);
            var by1 = Math.Min(b[0][1], b[1][1]);
            var by2 = Math.Max(b[0][1], b[1][1]);
            if ((ax1 >= bx1 && ax1 <= bx2 || ax2 >= bx1 && ax2 <= bx2 || bx1 >=ax1 && bx1 <= ax2 || bx2 >=ax1 && bx2 <=ax2) &&
                (ay1 >= by1 && ay1 <= by2 || ay2 >= by1 && ay2 <= by2 || by1 >= ay1 && by1 <= ay2 || by2 >= ay1 && by2 <= ay2))
            {
                bottom = Math.Max(bottom,Math.Max(b[0][2], b[1][2]));
            }
        }
        var azdiff = Math.Max(a[0][2], a[1][2]) - Math.Min(a[0][2], a[1][2]);
        
        var az1 = a[0][2] = bottom + 1;
        var az2 = a[1][2] = azdiff + bottom + 1;

        S2.Add((ax1, ax2, ay1, ay2, az1, az2, i));
    }

    var sololySuports = new Dictionary<int, HashSet<int>>();
    var supportedby = new Dictionary<int, HashSet<int>>();
    for (int i = 0;i<S2.Count;i++)
    {
        bool canbreak = true;
        var leaning = S2.Where(a => (a.z1 == S2[i].z2 + 1 ) && XYOverlap(a, S2[i])).ToList();
        foreach(var l in leaning)
        {
            if (!supportedby.ContainsKey(l.name))
            {
                supportedby[l.name] = new HashSet<int>();
            }
            supportedby[l.name].Add(S2[i].name);

            var supporting = S2.Where(a => (a.z2 == l.z1 - 1) && XYOverlap(l,a)).ToList();
            if (supporting.Count == 1)
            {
                if (!sololySuports.ContainsKey(S2[i].name))
                {
                    sololySuports[S2[i].name] = new HashSet<int>();
                }
                sololySuports[S2[i].name].Add(l.name);
                canbreak = false;
            }
        }
        if (canbreak) answer1++;
    }
    
    foreach (var sup in sololySuports)
    {
        answer2 += GetFallingBlocks(sup.Value, supportedby);
    }
    
    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());
}

long GetFallingBlocks(HashSet<int> sup, Dictionary<int, HashSet<int>> suportedby)
{
    long cnt = 0;
    var sup1 = sup.ToHashSet();
    while (cnt < sup1.Count)
    {
        cnt = sup1.Count;
        foreach (var stone in suportedby)
        {
            if (!sup1.Contains(stone.Key))
            {
                bool fallen = true;
                foreach (var under in stone.Value) if (!sup1.Contains(under)) { fallen = false; break; }
                if (fallen) sup1.Add(stone.Key);
            }
        }
    }
    return cnt;
}

bool XYOverlap((int x1, int x2, int y1, int y2, int z1, int z2, int name) a, (int x1, int x2, int y1, int y2, int z1, int z2, int name) b)
{
    return ((a.x1 >= b.x1 && a.x1 <= b.x2 || a.x2 >= b.x1 && a.x2 <= b.x2 || b.x1 >= a.x1 && b.x1 <= a.x2 || b.x2 >= a.x1 && b.x2 <= a.x2) &&
                 (a.y1 >= b.y1 && a.y1 <= b.y2 || a.y2 >= b.y1 && a.y2 <= b.y2 || b.y1 >= a.y1 && b.y1 <= a.y2 || b.y2 >= a.y1 && b.y2 <= a.y2));
}