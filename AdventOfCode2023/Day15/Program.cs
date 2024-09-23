using System.Collections;
using System.Collections.Immutable;
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
using System.Security.Cryptography;
using AocHelper;

Run(@"..\..\..\example.txt", true, 1320, 145);
//Run(@"..\..\..\example1.txt", true, 4, 1);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day15.txt", false, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    //var S = File.ReadAllText(inputfile).Split("\r\n\r\n").Select(s => s.Split("\r\n").ToList()).ToList();
    var S = File.ReadAllText(inputfile).Split(",").Select(s => MyHash(s)).ToList();

    long answer1 = S.Sum();

    var S1 = File.ReadAllText(inputfile).Split(",");
    var boxes = new List<(string lens,int focus)>[256];

    

    foreach (string s in S1)
    {
        var command = s.Split('=');
        var lens = command[0];
        if (lens.EndsWith('-')) lens = lens.Substring(0, lens.Length- 1);

        int boxnr = MyHash(lens);
        List<(string lens, int focus)> box = boxes[boxnr];
        if (box == null) box = boxes[boxnr] = new List<(string lens, int focus)>();
        int curslot = -1;
        for (int i = 0; i < box.Count(); i++) if (box[i].lens == lens) curslot = i; 
        if (command.Length == 2)
        {
            if (curslot == -1) box.Add((lens, command[1][0] - '0'));
            else box[curslot] = (lens, command[1][0] - '0');
        }
        else
        {
            if (curslot >= 0) box.RemoveAt(curslot);
        }
    }

    long answer2 = 0;

    for (int i = 0;i< boxes.Length;i++)
    {
        if (boxes[i] != null)
        {
            for (int j = 0; j < boxes[i].Count; j++)
            {
                answer2 += (i + 1) * (j + 1) * boxes[i][j].focus;
            }
        }
    }
    
    

    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());
}

 static int MyHash(string s)
{
    int curval = 0;
    foreach(var c in s)
    {
        curval += c;
        curval *= 17;
        curval %= 256;
    }
    return curval;
}