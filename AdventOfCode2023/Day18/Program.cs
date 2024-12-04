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

Run(@"..\..\..\example.txt", true, 62, 952408144115);
//Run(@"..\..\..\example1.txt", true, 61, 0000);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day18.txt", false, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    //var S = File.ReadAllText(inputfile).Split("\r\n\r\n").Select(s => s.Split("\r\n").ToList()).ToList();
    //var S = File.ReadAllLines(inputfile).Select(a => a.Select(b => b - '0').ToList()).ToList();
    var S = File.ReadAllLines(inputfile).ToList();
 
    long answer1 = 0;
    long answer2 = 0;

    var directions = "RDLU";
    bool part1 = true;

    for (int parts = 0; parts < 2; parts++)
    {
        var field = new List<(long x, long y, char type)>();
        long x = 0;
        long y = 0;
        int dir = 3;
        foreach (var s in S)
        {
            var s1 = s.Split(" ").ToArray();
            var d = part1 ? s1[0][0] : directions[s1[2][7] - '0'];
            var cnt = part1 ? long.Parse(s1[1]) : Convert.ToInt64(s1[2].Substring(2, 5), 16);

            switch (d)
            {
                case 'U':
                    {
                        var y1 = y - cnt;
                        if (dir == 0) field.Add((x, y, 'J'));
                        else if (dir == 2) field.Add((x, y, 'L'));
                        y = y1;
                        dir = 3;
                        break;
                    }
                case 'D':
                    {
                        var y1 = y + cnt;
                        if (dir == 0) field.Add((x, y, ')'));
                        else if (dir == 2) field.Add((x, y, 'F'));
                        y = y1;
                        dir = 1;
                        break;
                    }
                case 'L':
                    {
                        var x1 = x - cnt;
                        if (dir == 1) field.Add((x, y, 'J'));
                        else if (dir == 3) field.Add((x, y, ')'));
                        x = x1;
                        dir = 2;
                        break;
                    }
                case 'R':
                    {
                        var x1 = x + cnt;
                        if (dir == 1) field.Add((x, y, 'L'));
                        else if (dir == 3) field.Add((x, y, 'F'));
                        x = x1;
                        dir = 0;
                        break;
                    }
            }
        }

        var ordererd = field.OrderBy(a => a.y).ThenBy(a => a.x).ToList();

        bool inside = false;
        char enter = ' ';
        inside = false;
        var curline = new List<long>();
        long prevY = 0;
        long answer = 0;
        int cornerNr = 0;

        while (cornerNr < ordererd.Count)
        {
            var corner = ordererd[cornerNr];
            if (corner.y != prevY)
            {
                var ysize = corner.y - prevY;
                inside = false;
                long prevX = 0;
                foreach (var Xcorner in curline)
                {
                    if (inside)
                    {
                        answer += (ysize - 1) * (Xcorner + 1 - prevX);
                    }
                    inside = !inside;
                    prevX = Xcorner;
                }
            }
            prevY = corner.y;

            var newline = new List<long>();
            int xCnt = 0;
            inside = false;
            long prevX1 = 0;
            while (cornerNr < ordererd.Count && ordererd[cornerNr].y == prevY && xCnt < curline.Count)
            {
                corner = ordererd[cornerNr];

                if (corner.x <= curline[xCnt])
                {
                    HandleCorner(ref inside, ref enter, ref answer, cornerNr, corner, newline, ref prevX1);

                    if (curline[xCnt] == corner.x) xCnt++;
                    cornerNr++;
                }
                else
                {
                    HandleLine(ref inside, curline, ref answer, newline, xCnt, ref prevX1);
                    xCnt++;
                }
            }
            while (cornerNr < ordererd.Count && ordererd[cornerNr].y == prevY)
            {
                corner = ordererd[cornerNr];
                HandleCorner(ref inside, ref enter, ref answer, cornerNr, corner, newline, ref prevX1);
                cornerNr++;
            }
            while (xCnt < curline.Count)
            {
                HandleLine(ref inside, curline, ref answer, newline, xCnt, ref prevX1);
                xCnt++;
            }

            curline = newline;
        }
        if (part1) answer1 = answer; else answer2 = answer;
        part1 = !part1;
    }
    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

    static void HandleLine(ref bool inside, List<long> curline, ref long answer, List<long> newline, int xCnt, ref long prevX1)
    {
        var curX = curline[xCnt];
        if (inside)
        {
            answer += curX - prevX1 + 1;
            prevX1 = curX + 1;
            inside = false;
        }
        else
        {
            prevX1 = curX;
            inside = true;
        }
        newline.Add(curX);
    }
}

static void HandleCorner(ref bool inside, ref char enter, ref long answer, int cornerNr, (long x, long y, char type) corner, List<long> newline, ref long prevX1)
{
    if (corner.type == 'F' || corner.type == ')') newline.Add(corner.x);
    if (corner.type == 'F' || corner.type == 'L')
    {
        if (inside) answer += corner.x - prevX1;
        enter = corner.type;
        prevX1 = corner.x;
    }
    else if (corner.type == ')' && enter == 'F' ||
        corner.type == 'J' && enter == 'L')
    {
        answer += corner.x - prevX1 + 1;
        prevX1 = corner.x + 1;
    }
    else if (corner.type == ')' && enter == 'L' ||
        corner.type == 'J' && enter == 'F')
    {
        answer += corner.x - prevX1 + 1;
        prevX1 = corner.x + 1;
        inside = !inside;
    }
}