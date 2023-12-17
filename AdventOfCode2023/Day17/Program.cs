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

Run(@"..\..\..\example.txt", true, 102, 94);
Run(@"..\..\..\example1.txt", true, 59, 71);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day17.txt", false, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    //var S = File.ReadAllText(inputfile).Split("\r\n\r\n").Select(s => s.Split("\r\n").ToList()).ToList();
    var S = File.ReadAllLines(inputfile).Select(a=>a.Select(b=>b-'0').ToList()).ToList();

    long answer1 = 0;
    long answer2 = 0;

    var directions = new (int x, int y)[] { (1, 0), (0, 1), (-1, 0), (0, -1) };
    var work = new Dictionary<(int x, int y, int moveurge,int direction),long>();
    work[(0, 0, 0, 0)] = 0;
    work[(0, 0, 0, 1)] = 0;
    var mindist = new Dictionary<(int x, int y, int moveurge, int direction),long>();
    bool found = false;
    while (work.Count > 0 && !found)
    {
        var min = work.Min(x => x.Value);
        var cur = work.Where(x => x.Value == min).First();
        work.Remove(cur.Key);
        if (!mindist.ContainsKey(cur.Key))
        {
            mindist[cur.Key] = min;
        }
        //add new work to the queue
        if (cur.Key.moveurge < 2)
        {
            var dir = cur.Key.direction;
            var urge = cur.Key.moveurge + 1;
            TryToAddWork(S, directions, work, mindist, cur, dir, urge);
            
        }
        if (cur.Key.direction == 0 || cur.Key.direction == 2)
        {
            TryToAddWork(S, directions, work, mindist, cur, 1, 0);
            TryToAddWork(S, directions, work, mindist, cur, 3, 0);
        }
        else if (cur.Key.direction == 1 || cur.Key.direction == 3)
        {
            TryToAddWork(S, directions, work, mindist, cur, 0, 0);
            TryToAddWork(S, directions, work, mindist, cur, 2, 0);
        }
        if (cur.Key.x == S[0].Count - 1 && cur.Key.y == S.Count - 1)
        {
            found = true;
            answer1 = cur.Value;
        }
    }

    work = new Dictionary<(int x, int y, int moveurge, int direction), long>();
    work[(0, 0, 0, 0)] = 0;
    work[(0, 0, 0, 1)] = 0;
    mindist = new Dictionary<(int x, int y, int moveurge, int direction), long>();
    found = false;
    while (work.Count > 0 && !found)
    {
        var min = work.Min(x => x.Value);
        var cur = work.Where(x => x.Value == min).First();
        work.Remove(cur.Key);
        if (!mindist.ContainsKey(cur.Key))
        {
            mindist[cur.Key] = min;
        }
        //add new work to the queue
        if (cur.Key.moveurge < 9)
        {
            var dir = cur.Key.direction;
            var urge = cur.Key.moveurge + 1;
            TryToAddWork(S, directions, work, mindist, cur, dir, urge);

        }
        if (cur.Key.moveurge >= 3)
        {
            if (cur.Key.direction == 0 || cur.Key.direction == 2)
            {
                TryToAddWork(S, directions, work, mindist, cur, 1, 0);
                TryToAddWork(S, directions, work, mindist, cur, 3, 0);
            }
            else if (cur.Key.direction == 1 || cur.Key.direction == 3)
            {
                TryToAddWork(S, directions, work, mindist, cur, 0, 0);
                TryToAddWork(S, directions, work, mindist, cur, 2, 0);
            }
            if (cur.Key.x == S[0].Count - 1 && cur.Key.y == S.Count - 1)
            {
                found = true;
                answer2 = cur.Value;
            }
        }
    }

    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());
}

static void TryToAddWork(List<List<int>> S, (int x, int y)[] directions, 
    Dictionary<(int x, int y, int moveurge, int direction), long> work, 
    Dictionary<(int x, int y, int moveurge, int direction), long> mindist, 
    KeyValuePair<(int x, int y, int moveurge, int direction), long> cur, int dir, int urge)
{
    (int x, int y, int moveurge, int direction) newpos = (cur.Key.x + directions[dir].x, cur.Key.y + directions[dir].y, urge, dir);
    if (newpos.x >= 0 && newpos.y >= 0 && newpos.x < S[0].Count && newpos.y < S.Count && !mindist.ContainsKey(newpos))
    {
        var newval = cur.Value + S[newpos.y][newpos.x];
        if (!work.ContainsKey(newpos)) work[newpos] = newval;
        else if  (work[newpos] > newval) { work[newpos] = newval; }
        
    }
}