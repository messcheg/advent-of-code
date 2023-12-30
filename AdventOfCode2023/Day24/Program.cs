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

Run(@"..\..\..\example.txt", true, 2, 47, 7, 27);
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

    var minx = S.Select(l => l[0][0]).Min();
    var miny = S.Select(l => l[0][1]).Min();
    var minz = S.Select(l => l[0][2]).Min();

    minx = miny = minz = 0;

    var M = new List<List<double>>();
    for (int i= 0; i < 3; i++)
    {
        AddHail(M, S, i, minx, miny, minz);
    }

    int k = 3;
    bool ready = Optimize(M);
    while (!ready && k < S.Length-1)
    {
        AddHail(M, S, k, minx, miny, minz);
        ready = Optimize(M);
        k++;
    }

    var (px, py, pz, vx, vy, vz) = (M[0][0], M[1][0], M[2][0], M[3][0], M[4][0], M[5][0]);
    // var (px, py, vx, vy) = (M[0][0], M[1][0], M[2][0], M[3][0]);

    answer2 = (long)Math.Round(px + py + pz + minx + miny + minz);

    stopwatch.Stop(); // ti = (X(ti) - px) /vx  - dat nog uitrekenen. 

    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

}

void AddHail(List<List<double>> M, double[][][] S, int i, double minx, double miny, double minz)
{
    var (ax, ay, az, avx, avy, avz) = (S[i][0][0]-minx, S[i][0][1]-miny, S[i][0][2]-minz, S[i][1][0], S[i][1][1], S[i][1][2]);
    var (bx, by, bz, bvx, bvy, bvz) = (S[i + 1][0][0]-minx, S[i + 1][0][1]-miny, S[i + 1][0][2]-minz, S[i + 1][1][0], S[i + 1][1][1], S[i + 1][1][2]);

    //Add(M, new double[] { ax * avy - bx * bvy + by * bvx - ay * avx, avy - bvy, bvx - avx, by - ay, ax - bx });
       Add(M, new double[] { ax * avy - bx * bvy + by * bvx - ay * avx, avy - bvy, bvx - avx, 0, by - ay, ax - bx, 0 });
       Add(M, new double[] { ay * avz - by * bvz + bz * bvy - az * avy, 0, avz - bvz, bvy - avy, 0, bz - az, ay - by });
       Add(M, new double[] { az * avx - bz * bvx + bx * bvz - ax * avz, bvz - avz, 0, avx - bvx, az - bz,  0, bx - ax });
    
}


bool Optimize(List<List<double>> m)
{
    bool ready = true;
    var maxlen = m.Max(l => l.Count);
    foreach (var l in m) if (l.Count < maxlen) l.AddRange(new double[maxlen - l.Count]);

    for (int i = 0; i< m[m.Count-1].Count-1; i++)
    {
        int line = i;
        
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
                var factor = m[k][i + 1];
                for (int j = 0; j < m[i].Count; j++)
                {
                    m[k][j] -= m[i][j] * factor;
                }
            }
        }
    }
    return ready;
}


static void Add(List<List<double>> M, double[] first)
{
    var line = new List<double>(first);
    M.Add(line);
}



static void Add1(List<List<double>> M, double[] first, double[] second,  int count)
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