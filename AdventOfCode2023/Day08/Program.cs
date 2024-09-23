using System.Collections.Immutable;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using AocHelper;

Run(@"..\..\..\example.txt", true, 2, 2);
Run(@"..\..\..\example1.txt", true, -1 , 6);
Run(@"E:\develop\advent-of-code-input\2023\day08.txt", false);

void Run(string inputfile, bool isTest, long supposedanswer1 = 0, long supposedanswer2 = 0 )
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    
    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var instr = S[0];
    var map = new Dictionary<string, (string L, string R)>();
    foreach ( var s in S.Skip(2))
    {
        var a = s.Split(" = ");
        var b = a[1].Substring(1, 3);
        var c = a[1].Substring(6, 3);
        map[a[0]] = (b, c);
    }

    var cur = "AAA";
    var ip = 0;
    if (supposedanswer1 > -1)
    {
        while (cur != "ZZZ")
        {
            cur = instr[ip] == 'L' ? map[cur].L : map[cur].R;
            answer1++;
            ip++;
            if (ip == instr.Length) ip = 0;
        }
    }

    var cur2 = map.Keys.Where(k => k[2] == 'A').ToList();
    ip = 0;
    answer2 = 1;

    foreach (var c2 in cur2)
    {
        var c3 = c2;
        long cnt = 0;
        ip = 0;
        while (c3[2] != 'Z')
        {
            c3 = instr[ip] == 'L' ? map[c3].L : map[c3].R;
            ip++;
            cnt++;
            if (ip == instr.Length) ip = 0;
        }
        // determine smallest common multiplier
        var (g1, g2) = answer2 > cnt ? (answer2, cnt) : (cnt, answer2);
        while (g2 != 0) (g1, g2) = (g2, g1 % g2);
        
        answer2 = answer2 * (cnt / g1);
    }
 
    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

}
