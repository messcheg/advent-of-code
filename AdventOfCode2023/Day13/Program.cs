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

Run(@"..\..\..\example.txt", true, 405, 0000);
//Run(@"..\..\..\example1.txt", true, 4, 1);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day13.txt", false, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var S = File.ReadAllText(inputfile).Split("\r\n\r\n").Select(s=>s.Split("\r\n").ToList()).ToList();
    long answer1 = 0;
    long answer2 = 0;

    foreach(var s in S)
    {
        //find vertical split
        var splitAfterV = 0;
        var splitwidhV = 0;
        for (int x=0;x<s[0].Length-1;x++)
        {
            int width = 0;
            bool allEqual = true;
            while (allEqual && width <= x && width + x + 1 < s[0].Length)
            {
                for (int y = 0; y < s.Count; y++)
                {
                    if (s[y][x-width] != s[y][x + width + 1]) allEqual = false;
                }
                if (allEqual) width++;
            }
            if (width > splitwidhV)
            {
                splitwidhV = width;
                splitAfterV = x;
            }
        }
        var splitAfterH = 0;
        var splitHeightH = 0;
        for (int y = 0; y< s.Count - 1; y++)
        {
            int height = 0;
            bool allEqual = true;
            while (allEqual && height <= y && height + y + 1 < s.Count)
            {
                for (int x = 0; x < s[0].Length; x++)
                {
                    if (s[y-height][x] != s[y + height + 1][x]) allEqual = false;
                }
                if (allEqual) height++;
            }
            if (height > splitHeightH)
            {
                splitHeightH = height;
                splitAfterH = y;
            }
        }
        if (splitAfterH == splitHeightH || splitHeightH + splitAfterH + 1 == s.Count)
        {
            answer1 += 100 * (splitAfterH + 1);
        }
        else
        {
            answer1 += splitAfterV + 1;  
        }

    }

    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

}
