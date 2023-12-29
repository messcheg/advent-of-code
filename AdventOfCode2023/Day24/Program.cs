using System.Collections;
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

Run(@"..\..\..\example.txt", true, 2, 0000, 7, 27);
//Run(@"..\..\..\example1.txt", true, 11687500, 0000);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day24.txt", false, 0, 0 , 200000000000000, 400000000000000);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2, double atleast , double atmost)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    //var S = File.ReadAllText(inputfile).Split("\r\n\r\n").Select(s => s.Split("\r\n").ToList()).ToList();
    //var S = File.ReadAllLines(inputfile).Select(a => a.Select(b => b - '0').ToList()).ToList();
    var S = File.ReadAllLines(inputfile).Select(s => s.Split(" @ ").Select(s1 => s1.Split(", ").Select(l => double.Parse(l)).ToArray()).ToArray()).ToArray(); 

    long answer1 = 0;
    long answer2 = 0;

    for (int i = 0; i< S.Length - 1; i++)
    {
        var a = S[i];
        for (int j= i+1; j< S.Length; j++)
        {
            var b = S[j];

            var noemera = a[1][0] - a[1][1] * (b[1][0] / b[1][1]);
            var noemerb = b[1][0] - b[1][1] * (a[1][0] / a[1][1]);
            if (noemera!= 0 && noemerb!=0)
            {
                var tellera = b[0][0] - a[0][0] - (b[0][1] - a[0][1]) * (b[1][0] / b[1][1]);
                var tellerb = a[0][0] - b[0][0] - (a[0][1] - b[0][1]) * (a[1][0] / a[1][1]);
                var timea = tellera / noemera;
                var timeb = tellerb / noemerb;

                if (timea >= 0 && timeb >= 0)
                {
                    var x = a[0][0] + timea * a[1][0];
                    var y = a[0][1] + timea * a[1][1];

                    if (x >= atleast && x <= atmost && y >= atleast && y <= atmost) answer1++;
                }
            }
        }
    }

    var M = new List<List<double>>();
    for (int i= 0; i < 3; i++)
    {
        AddHail(M, S, i);
    }

    int k = 3;
    bool ready = Optimize(M);
    while (!ready && k < S.Length)
    {
        AddHail(M, S, k);
        ready = Optimize(M);
        k++;
    }

    var (rpx, rpy, rpz, trvx, trvy, trvz, x1, y1, z1) = (M[0][0], M[1][0], M[2][0], M[3][0], M[4][0], M[5][0], M[6][0], M[7][0], M[8][0]);
    var (p1x, p1y, p1z, v1x, v1y, v1z) = (S[0][0][0], S[0][0][1], S[0][0][2], S[0][1][0], S[0][1][1], S[0][1][2]);
    var (t1, t2, t3) = ((x1 - p1x) / v1x, (y1 - p1y) / v1y, (z1 - p1z) / v1z);
    

    stopwatch.Stop(); // ti = (X(ti) - px) /vx  - dat nog uitrekenen. 

    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

}

void AddHail(List<List<double>> M, double[][][] S, int i)
{
    var (px, py, pz, vx, vy, vz) = (S[i][0][0], S[i][0][1], S[i][0][2], S[i][1][0], S[i][1][1], S[i][1][2]);
    Add(M, new double[] { px, 0, 0, 0 }, new double[] { 0, 0, 0, 1, 0, 0 }, i);
    Add(M, new double[] { py, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 1, 0 }, i);
    Add(M, new double[] { pz, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 1 }, i);
    Add(M, new double[] { 0, 1, 0, 0 }, new double[] { 1, 0, 0, px, 0, 0 }, i);
    Add(M, new double[] { 0, 0, 1, 0 }, new double[] { 0, 1, 0, 0, py, 0 }, i);
    Add(M, new double[] { 0, 0, 0, 1 }, new double[] { 0, 0, 1, 0, 0, pz }, i);
    Add(M, new double[] { px / vx - py / vy, 0, 0, 0 }, new double[] { 0, 0, 0, 1 / vx, -1 / vy, 0 }, i);
    Add(M, new double[] { py / vy - pz / vz, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 1 / vy, -1 / vz }, i);
    Add(M, new double[] { pz / vz - px / vx, 0, 0, 0 }, new double[] { 0, 0, 0, -1 / vx, 0, 1 / vz }, i);

    if (i > 0)
    {
        AddCombi(M, 4, new double[] { 1, 0, 0, 1, 0, 0 }, i - 1, new double[] { -1, 0, 0, -1, 0, 0 }, i);
        AddCombi(M, 4, new double[] { 0, 1, 0, 0, 1, 0 }, i - 1, new double[] { 0, -1, 0, 0, -1, 0 }, i);
        AddCombi(M, 4, new double[] { 0, 0, 1, 0, 0, 1 }, i - 1, new double[] { 0, 0, -1, 0, 0, -1 }, i);
    }

}

bool Optimize(List<List<double>> m)
{
    bool ready = true;
    for (int i = 0; i< m[m.Count-1].Count-1; i++)
    {
        int line = i;
        var maxlen = m.Max(l => l.Count);
        foreach(var l in m) if(l.Count < maxlen) l.AddRange(new double[maxlen-l.Count]);    

        while (line < m.Count && m[line][i + 1] == 0) line++;
        if (line == m.Count) { ready = false; break; }
        
        var div = 1 / m[line][i + 1];
        if (line == i)
        {
            for (int j = 0; j < m[i].Count; j++)
            {
                m[i][j] = m[i][j] * div;
            }
        }
        else 
        { 
            for (int j = 0; j < m[line].Count; j++)
            {
                m[i][j] += m[line][j] * div;
            }
        }
        for (int k = 0; k < m.Count; k++)
        {
            if (k != i)
            {
                for (int j = 0; j < m[i].Count; j++)
                {
                    m[k][j] -= m[i][j] * m[k][i + 1];
                }
            }
        }
    }
    return ready;
}

static void Add(List<List<double>> M, double[] first, double[] second,  int count)
{
    var line = new List<double>(first);
    for (int i = 0; i < count; i++) line.AddRange(new double[second.Length]);
    line.AddRange(second);
    M.Add(line);
}


static void AddCombi(List<List<double>> M, int offset, double[] first, int c1, double[] second, int c2)
{
    var line = new List<double>();
    line.AddRange(new double[offset]);
    for (int i = 0; i < c1; i++) line.AddRange(new double[first.Length]);
    line.AddRange(first);
    for (int i = 0; i < c2; i++) line.AddRange(new double[second.Length]);
    line.AddRange(second);
    M.Add(line);
}