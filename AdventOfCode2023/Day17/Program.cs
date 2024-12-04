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

    var visited = new bool[S[0].Count, S.Count,4, 4];
    var directions = new (int x, int y)[] { (1, 0), (0, 1), (-1, 0), (0, -1) };
    var work = new PriorityQueue<(int x, int y, int streak,int direction),long>();
    work.Enqueue((0, 0, 0, 0),0);
    work.Enqueue((0, 0, 0, 1), 0);
    var mindist = new Dictionary<(int x, int y, int streak, int direction),long>();
    mindist[(0, 0, 0, 0)] = 0;
    mindist[(0, 0, 0, 1)] = 0;
    bool found = false;
    while (work.Count > 0 && !found)
    {
        var cur = work.Dequeue();
        visited[cur.x, cur.y,cur.direction,cur.streak] = true;
        //add new work to the queue
        if (cur.streak < 2)
        {
            var dir = cur.direction;
            var urge = cur.streak + 1;
            TryToAddWork(S, directions, work, mindist, cur, dir, urge, visited);
            
        }
        if (cur.direction == 0 || cur.direction == 2)
        {
            TryToAddWork(S, directions, work, mindist, cur, 1, 0, visited);
            TryToAddWork(S, directions, work, mindist, cur, 3, 0, visited);
        }
        else if (cur.direction == 1 || cur.direction == 3)
        {
            TryToAddWork(S, directions, work, mindist, cur, 0, 0, visited);
            TryToAddWork(S, directions, work, mindist, cur, 2, 0, visited);
        }
        if (cur.x == S[0].Count - 1 && cur.y == S.Count - 1)
        {
            found = true;
            answer1 = mindist[cur];
        }
    }

    work = new PriorityQueue<(int x, int y, int streak, int direction), long>();
    work.Enqueue((0, 0, 0, 0), 0);
    work.Enqueue((0, 0, 0, 1), 0);
    mindist = new Dictionary<(int x, int y, int streak, int direction), long>();
    mindist[(0, 0, 0, 0)] = 0;
    mindist[(0, 0, 0, 1)] = 0;
    found = false;
    visited = new bool[S[0].Count, S.Count, 4, 10];
    while (work.Count > 0 && !found)
    {

        var cur = work.Dequeue();
        visited[cur.x, cur.y,cur.direction,cur.streak] = true;

        //add new work to the queue
        if (cur.streak < 9)
        {
            var dir = cur.direction;
            var urge = cur.streak + 1;
            TryToAddWork(S, directions, work, mindist, cur, dir, urge, visited);

        }
        if (cur.streak >= 3)
        {
            if (cur.direction == 0 || cur.direction == 2)
            {
                TryToAddWork(S, directions, work, mindist, cur, 1, 0, visited);
                TryToAddWork(S, directions, work, mindist, cur, 3, 0, visited);
            }
            else if (cur.direction == 1 || cur.direction == 3)
            {
                TryToAddWork(S, directions, work, mindist, cur, 0, 0, visited);
                TryToAddWork(S, directions, work, mindist, cur, 2, 0, visited);
            }
            if (cur.x == S[0].Count - 1 && cur.y == S.Count - 1)
            {
                found = true;
                answer2 = mindist[cur];
            }
        }
    }

    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());
}

static void TryToAddWork(List<List<int>> S, (int x, int y)[] directions,
    PriorityQueue<(int x, int y, int streak, int direction), long> work, 
    Dictionary<(int x, int y, int streak, int direction), long> mindist, 
    (int x, int y, int streak, int direction) cur, int dir, int urge,
    bool[,,,] visited)
{
    (int x, int y, int streak, int direction) newpos = (cur.x + directions[dir].x, cur.y + directions[dir].y, urge, dir);
    if (newpos.x >= 0 && newpos.y >= 0 && newpos.x < S[0].Count && newpos.y < S.Count && !visited[newpos.x, newpos.y, newpos.direction, newpos.streak])
    {
        var newval = mindist[cur] + S[newpos.y][newpos.x];
        if (!mindist.ContainsKey(newpos))
        {
            mindist[newpos] = newval;
            work.Enqueue(newpos, newval);
        }
        else if (mindist[newpos] > newval) 
        {
            mindist[newpos] = newval;
            work.Enqueue(newpos, newval);
        }
        
    }
}