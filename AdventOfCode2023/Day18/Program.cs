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

Run(@"..\..\..\example.txt", true, 62, 0000);
Run(@"..\..\..\example1.txt", true, 61, 0000);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day18.txt", true, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    //var S = File.ReadAllText(inputfile).Split("\r\n\r\n").Select(s => s.Split("\r\n").ToList()).ToList();
    //var S = File.ReadAllLines(inputfile).Select(a => a.Select(b => b - '0').ToList()).ToList();
    var S = File.ReadAllLines(inputfile).ToList();
 
    long answer1 = 0;
    long answer2 = 0;

    var field = new Dictionary<(int x, int y), List<char>>();
    var secondfield = new List<(long x, long y)>();
    int x = 0;
    int y = 0;
    long x2 = 0; long y2 = 0;
    int dir = 3;
    foreach (var s in S)
    {
        var s1 = s.Split(" ").ToArray();
        var d = s1[0];
        var d2 = s1[2][6]-'0';
        var cnt2 = Convert.ToInt64(s1[2].Substring(1, 5), 16);
        var cnt = int.Parse(s1[1]);
        void SetField(int x, int y, char value)
        {
            List<char> l;
            if (field == null) return;
            if (!field.TryGetValue((x,y),out l))
            {
                l = new List<char>();
                field[(x, y)] = l ;
            }
            else
            {
                Console.WriteLine("overlap at: " + x.ToString() + ", " + y.ToString());
            }
            l.Add(value);
        }
        

        switch (d)
        {
            case "U":
                {
                    var y1 = y - cnt;
                    if (dir == 0) SetField(x,y,'J');
                    else if (dir == 2) SetField(x,y, 'L');
                    for (int i = y - 1; i > y1; i--) SetField(x, i, '|');
                    y = y1;
                    dir = 3;
                    break;
                }
            case "D":
                {
                    var y1 = y + cnt;
                    if (dir == 0) SetField(x,y, ')');
                    else if (dir == 2) SetField(x,y, 'F');
                    for (int i = y + 1; i < y1; i++) SetField(x,i, '|');
                    y = y1;
                    dir = 1;
                    break;
                }
            case "L":
                {
                    var x1 = x - cnt;
                    if (dir == 1) SetField(x,y, 'J');
                    else if (dir == 3) SetField(x,y, ')');
                    for (int i = x - 1; i > x1; i--) SetField(i, y, '-');
                    x = x1;
                    dir = 2;
                    break;
                }
            case "R":
                {
                    var x1 = x + cnt;
                    if (dir == 1) SetField(x,y, 'L');
                    else if (dir == 3) SetField(x,y, 'F');
                    for (int i = x + 1; i < x1; i++) SetField(i, y, '-');
                    x = x1;
                    dir = 0;
                    break;
                }
        }
        switch (d2)
        {
            case 0: x2 += cnt2; break;
            case 1: y2 += cnt2; break;
            case 2: x2 -= cnt2; break;
            case 3: y2 -= cnt2; break;
        }
        secondfield.Add((x2, y2));
    }
    int maxy = field.Keys.Select(k => k.y).Max();
    int maxx = field.Keys.Select(k => k.x).Max();
    int miny = field.Keys.Select(k => k.y).Min();
    int minx = field.Keys.Select(k => k.x).Min();

    bool inside = false;
    char enter = ' ';
    for (int j=miny;j<=maxy;j++)
    {
        inside = false;
        for (int i =minx;i<=maxx;i++)
        {
            if (field.ContainsKey((i, j)))
            {
                var c = field[(i, j)];
                answer1++;
                switch (c[0])
                {
                    case '|': inside = !inside;  break;
                    case 'L': enter = 'L'; break;
                    case 'F': enter = 'F'; break;
                    case 'J':
                        if (enter == 'F') inside = !inside;
                        break;
                    case ')':
                        if (enter == 'L') inside = !inside;
                        break;
                }
               if (isTest) Console.Write(c[0]);
            }
            else
            {
                if (inside) 
                { 
                    answer1++;
                    if (isTest) Console.Write('#');
                }
                else
                {
                    if (isTest) Console.Write('.');
                }
            }
        }
        if (isTest) Console.WriteLine();
    }

    var ordererd = secondfield.OrderBy(a => a.y).ThenBy(a=>a.x).ToList();
    int edgeNr = 0;
    var curline = new List<long>();
    var lastline = curline;
    long prevY = 0;
    while (edgeNr < ordererd.Count)
    {
        var curEdge = ordererd[edgeNr];
        var ysize = curEdge.y - prevY;
        inside = false;
        long prevX = 0;
        foreach (var Xedge in curline)
        {
            if(inside) 
            {
                answer2 += (ysize - 1) * (Xedge + 1 - prevX);
            }
            inside = !inside;
            prevX = Xedge;
        }
        prevY = curEdge.y;
        int xCnt = 0;

        var newline = new List<long>();
        inside = false;
        bool curinside = false;
        prevX = 0;
        while (edgeNr < ordererd.Count && ordererd[edgeNr].y == prevY && xCnt < curline.Count) 
        {
            if (ordererd[edgeNr].x < curline[xCnt])
            {
                if (!inside && !curinside)
                {
                    curinside = true;
                    prevX = ordererd[edgeNr].x;
                    newline.Add(ordererd[edgeNr].x);
                }
                else if (!inside && curinside)
                {
                    curinside = false;
                    answer2 += ordererd[edgeNr].x + 1 - prevX;
                    prevX = ordererd[edgeNr].x +1;
                    newline.Add(ordererd[edgeNr].x);
                }
                else if (inside && curinside)
                {
                    prevX = ordererd[edgeNr].x;
                    newline.Add(ordererd[edgeNr].x);
                }
                else if (inside && !curinside)
                {

                }
                edgeNr++;
            }
            else if (ordererd[edgeNr].x > curline[xCnt])
            {
                xCnt++;
            }
            else
            {
            
                edgeNr++;
                xCnt++;
            }
        }
        while (edgeNr < ordererd.Count)
        {
            newline.Add(ordererd[edgeNr].x);
            edgeNr++;
        }
        while (xCnt < curline.Count)
        {
            edgeNr++;
            xCnt++;
        }

        curline = newline;
    }
    if (lastline.Count > 0)
    {
        inside = false;
        long prevX = 0;
        foreach (var Xedge in curline)
        {
            if (inside)
            {
                answer2 += (Xedge + 1 - prevX);
            }
            inside = !inside;
            prevX = Xedge;
        }

    }

    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());
}
