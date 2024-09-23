using System.Collections.Immutable;
using System.ComponentModel.Design.Serialization;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using AocHelper;

Run(@"..\..\..\example.txt", true, 405, 400);
//Run(@"..\..\..\example1.txt", true, 4, 1);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day13.txt", false, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var S = File.ReadAllText(inputfile).Split("\r\n\r\n").Select(s => s.Split("\r\n").ToList()).ToList();
    long answer1 = 0;
    long answer2 = 0;

    foreach (var s in S)
    {
        int v1, h1, v2, h2, v, h; 
        GetReflection(s, out v, out h, out v1, out h1);
        if (h == 0) h = h1;
        if (v == 0) v = v1;
        
        GetReflectionWithFIxedSmudge(s, v, h, out v2, out h2);

        answer1 += 100 * h + v;
        answer2 += 100 * h2 + v2;
    }

    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

   
}

static void GetReflectionWithFIxedSmudge(List<string>? s, int v, int h, out int v3, out int h3)
{
    var s1 = s.ToList();
    v3 = 0;
    h3 = 0;
    for (int y = 0; y < s.Count; y++)
    {
        for (int x = 0; x < s[y].Length; x++)
        {
            int h1, v1, h2, v2;
            var s2 = s1[y];
            var a1 = s1[y].ToArray();
            a1[x] = (a1[x] == '.') ? '#' : '.';
            var s3 = new string(a1);
            s1[y] = s3;

            GetReflection(s1, out v1, out h1, out v2, out h2);
            s1[y] = s2;

            if (v1 > 0 && v != v1) v3 = v1;
            if (h1 > 0 && h != h1) h3 = h1;
            if (v2 > 0 && v != v2) v3 = v2;
            if (h2 > 0 && h != h2) h3 = h2;


            if (h3 > 0 && v3 > 0) 
                return;
            if (h3 > 0 || v3 > 0) 
                return;
        }
    }
    return;
}
static void GetReflection(List<string>? s, out int v1, out int h1, out int v2, out int h2)
{
    if (s == null) s = new List<string>();
    //find vertical line from left
    v1 = 0;
    v2 = 0;
    h1 = 0;
    h2 = 0;
    int r = (s[0].Length / 2) * 2 - 1;
    //int r = s[0].Length-1;
    int l = 0;
    int w = 0;
    int h = 0;
    while (l + w < r - w)
    {
        bool equal = true;
        for (int y = 0; y < s.Count; y++)
        {
            if (s[y][l + w] != s[y][r - w]) equal = false;
        }
        if (equal) w++;
        else
        {
            w = 0;
            equal = false;
            r -= 2;
            //r--;
        }
    }
    if (w > 0) v1 = w;// - (l + w == r - w ? 1 : 0);

    r = s[0].Length - 1;
    l = s[0].Length % 2;
    //l = 0;
    w = 0;
    while (l + w < r - w)
    {
        bool equal = true;
        for (int y = 0; y < s.Count; y++)
        {
            if (s[y][l + w] != s[y][r - w]) equal = false;
        }
        if (equal) w++;
        else
        {
            w = 0;
            equal = false;
            l += 2;
            //l++;
        }
    }
    if (w > 0) v2 = s[0].Length - w;

    int t = 0;
    int b = (s.Count / 2) * 2 - 1;
    //int b = s.Count - 1;
    h = 0;
    while (t + h < b - h)
    {
        if (s[t + h] == s[b - h]) h++;
        else
        {
            h = 0;
            b -= 2;
            //b--;
        }
    }
    if (h > 0) h1 = h;// - (t + h == b - h ? 1 : 0 ) ;

    t = s.Count % 2;
    //t = 0;
    b = s.Count - 1;
    h = 0;
    while (t + h < b - h)
    {
        if (s[t + h] == s[b - h]) h++;
        else
        {
            h = 0;
            t += 2;
            //t++;
        }
    }
    if (h > 0) h2 = s.Count - h;

    //Console.WriteLine();
    //Console.WriteLine("=============================");
    //Console.WriteLine("h1:{0}, h2:{1}, v1:{2}, v2:{3}", h1,h2,v1,v2);
    //for(int i = 0; i < s.Count; i++)
    //{
    //    if (i>0 && (i==h1 || i==h2)) Console.WriteLine("------------------"); 
    //    for (int j = 0; j < s[i].Length; j++)
    //    {
    //        if (j > 0 && (j == v1 || j == v2)) Console.Write("|");
    //        Console.Write(s[i][j]);
    //    }
    //    Console.WriteLine();
    //}

}