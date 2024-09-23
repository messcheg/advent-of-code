using System.Diagnostics;
using System.Globalization;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using AocHelper;

Run(@"..\..\..\example.txt", true);
Run(@"E:\develop\advent-of-code-input\2023\day02.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 8;
    long supposedanswer2 = 2286;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    foreach (var s in S)
    {
        var s1 = s.Split(": ");
        var game = int.Parse(s1[0].Split(" ")[1]);
        bool possible = true;
        int maxblue = 0;
        int maxred = 0;
        int maxgreen = 0;
        foreach (var s2 in s1[1].Split("; "))
        {
            int blue = 0;
            int green = 0;
            int red = 0;
            
            foreach (var s3 in s2.Split(", "))
            {
            
                var s4 = s3.Split(" ");
                if (s4[1] == "red") red = int.Parse(s4[0]);
                if (s4[1] == "green") green = int.Parse(s4[0]);
                if (s4[1] == "blue") blue = int.Parse(s4[0]);
            }
            if (red > 12 || green > 13 || blue > 14) possible = false;
            if (maxred < red) maxred = red;
            if (maxgreen < green) maxgreen = green;
            if (maxblue < blue) maxblue = blue;

        }
        if (possible) answer1 += game;
        answer2 += maxred * maxgreen * maxblue;

    }



    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
}
