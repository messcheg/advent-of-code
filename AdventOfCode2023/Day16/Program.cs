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

Run(@"..\..\..\example.txt", true, 46, 51);
//Run(@"..\..\..\example1.txt", true, 4, 1);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day16.txt", false, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    //var S = File.ReadAllText(inputfile).Split("\r\n\r\n").Select(s => s.Split("\r\n").ToList()).ToList();
    var S = File.ReadAllLines(inputfile).ToList();

    long answer1 = 0;
    long answer2 = 0;

    var currentbeams = new Queue<(int tox, int toy, int fromd)>();
    currentbeams.Enqueue((0, 0, 0));
    var map = new Dictionary<(char c, int d), int>();
    map[('.', 0)] = 0;
    map[('.', 1)] = 1;
    map[('.', 2)] = 2;
    map[('.', 3)] = 3;
    map[('/', 0)] = 3;
    map[('/', 1)] = 2;
    map[('/', 2)] = 1;
    map[('/', 3)] = 0;
    map[('\\', 0)] = 1;
    map[('\\', 1)] = 0;
    map[('\\', 2)] = 3;
    map[('\\', 3)] = 2;
    map[('-', 0)] = 0;
    map[('-', 1)] = 5;
    map[('-', 2)] = 2;
    map[('-', 3)] = 5;
    map[('|', 0)] = 6;
    map[('|', 1)] = 1;
    map[('|', 2)] = 6;
    map[('|', 3)] = 3;


    answer1 = GetEnergyV2(S, currentbeams, map);

    for (int i = 0; i < S.Count; i++)
    {
        currentbeams.Enqueue((0, i, 0));
        var a1 = GetEnergyV2(S, currentbeams, map);
        if (a1 > answer2) answer2 = a1;

        currentbeams.Enqueue((S.Count-1, i, 2));
        a1 = GetEnergyV2(S, currentbeams, map);
        if (a1 > answer2) answer2 = a1;
    }

    for (int i = 0; i < S[0].Length; i++)
    {
        currentbeams.Enqueue((i, 0, 1));
        var a1 = GetEnergy(S, currentbeams);
        if (a1 > answer2) answer2 = a1;

        currentbeams.Enqueue((i, S[0].Length - 1, 3));
        a1 = GetEnergy(S, currentbeams);
        if (a1 > answer2) answer2 = a1;
    }

    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());
}

static long GetEnergy(List<string> S, Queue<(int tox, int toy, int fromd)> currentbeams)
{
    var visited = new HashSet<(int x, int y, int fromdirection)>();

    while (currentbeams.Count > 0)
    {
        var beam = currentbeams.Dequeue();
        if (!visited.Contains(beam))
        {
            visited.Add(beam);
            switch (S[beam.toy][beam.tox])
            {
                case '.':
                    if (beam.fromd == 0 && beam.tox < S[0].Length - 1) currentbeams.Enqueue((beam.tox + 1, beam.toy, beam.fromd));
                    else if (beam.fromd == 2 && beam.tox > 0) currentbeams.Enqueue((beam.tox - 1, beam.toy, beam.fromd));
                    else if (beam.fromd == 1 && beam.toy < S.Count - 1) currentbeams.Enqueue((beam.tox, beam.toy + 1, beam.fromd));
                    else if (beam.fromd == 3 && beam.toy > 0) currentbeams.Enqueue((beam.tox, beam.toy - 1, beam.fromd));
                    break;
                case '/':
                    if (beam.fromd == 3 && beam.tox < S[0].Length - 1) currentbeams.Enqueue((beam.tox + 1, beam.toy, 0));
                    else if (beam.fromd == 1 && beam.tox > 0) currentbeams.Enqueue((beam.tox - 1, beam.toy, 2));
                    else if (beam.fromd == 2 && beam.toy < S.Count - 1) currentbeams.Enqueue((beam.tox, beam.toy + 1, 1));
                    else if (beam.fromd == 0 && beam.toy > 0) currentbeams.Enqueue((beam.tox, beam.toy - 1, 3));
                    break;
                case '\\':
                    if (beam.fromd == 1 && beam.tox < S[0].Length - 1) currentbeams.Enqueue((beam.tox + 1, beam.toy, 0));
                    else if (beam.fromd == 3 && beam.tox > 0) currentbeams.Enqueue((beam.tox - 1, beam.toy, 2));
                    else if (beam.fromd == 0 && beam.toy < S.Count - 1) currentbeams.Enqueue((beam.tox, beam.toy + 1, 1));
                    else if (beam.fromd == 2 && beam.toy > 0) currentbeams.Enqueue((beam.tox, beam.toy - 1, 3));
                    break;
                case '-':
                    if (beam.fromd == 0 && beam.tox < S[0].Length - 1) currentbeams.Enqueue((beam.tox + 1, beam.toy, 0));
                    else if (beam.fromd == 2 && beam.tox > 0) currentbeams.Enqueue((beam.tox - 1, beam.toy, 2));
                    else if (beam.fromd == 1 || beam.fromd == 3)
                    {
                        if (beam.tox < S[0].Length - 1) currentbeams.Enqueue((beam.tox + 1, beam.toy, 0));
                        if (beam.tox > 0) currentbeams.Enqueue((beam.tox - 1, beam.toy, 2));
                    }
                    break;
                case '|':
                    if (beam.fromd == 1 && beam.toy < S.Count - 1) currentbeams.Enqueue((beam.tox, beam.toy + 1, 1));
                    else if (beam.fromd == 3 && beam.toy > 0) currentbeams.Enqueue((beam.tox, beam.toy - 1, 3));
                    else if (beam.fromd == 0 || beam.fromd == 2)
                    {
                        if (beam.toy < S.Count - 1) currentbeams.Enqueue((beam.tox, beam.toy + 1, 1));
                        if (beam.toy > 0) currentbeams.Enqueue((beam.tox, beam.toy - 1, 3));

                    }
                    break;
            }
        }
    }

    return visited.Select(v => (v.x, v.y)).ToHashSet().Count;
}
static long GetEnergyV2(List<string> S, Queue<(int tox, int toy, int fromd)> currentbeams, Dictionary<(char c, int d), int> map)
{
    var visited = new HashSet<(int x, int y, int fromdirection)>();
    var move = new (int dx, int dy)[4] { (1, 0), (0, 1), (-1, 0), (0, -1) };

    while (currentbeams.Count > 0)
    {
        var beam = currentbeams.Dequeue();
        if (!visited.Contains(beam))
        {
            visited.Add(beam);
            var newd = map[(S[beam.toy][beam.tox], beam.fromd)];

            if (newd < 5) Addwork(currentbeams, move, beam, newd, S[0].Length, S.Count);
            else
            {
                var nd1 = newd == 5 ? 0 : 1;
                var nd2 = newd == 5 ? 2 : 3;
                Addwork(currentbeams, move, beam, nd1, S[0].Length, S.Count);
                Addwork(currentbeams, move, beam, nd2, S[0].Length, S.Count);
            }
        }
    }

    return visited.Select(v => (v.x, v.y)).ToHashSet().Count;

    static void Addwork(Queue<(int tox, int toy, int fromd)> currentbeams, (int dx, int dy)[] move, (int tox, int toy, int fromd) beam, int newd, int maxx, int maxy)
    {
        int newx = beam.tox + move[newd].dx;
        int newy = beam.toy + move[newd].dy;
        if (0 <= newx && newx < maxx && newy >= 0 && newy < maxy)
            currentbeams.Enqueue((newx, newy, newd));
    }
}