using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\example1.txt", false);
//Run(@"E:\develop\advent-of-code-input\2024\day08.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 14;
    long supposedanswer2 = 34;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var antinodes = new HashSet<(int x, int y)>();
    var antinodes2 = new HashSet<(int x, int y)>();
    var nodes = new Dictionary<char, List<(int x, int y)>>();
    for (int y = 0; y < S.Count; y++)
    {
        string s = S[y];
        for (int x = 0; x < s.Length; x++)
        {
            var c = s[x];
            if (c != '.')
            {
                if (!nodes.TryGetValue(c, out var positions)) nodes[c] = positions = new List<(int x, int y)>();
                positions.Add((x, y));
            }
        }
    }

    foreach (var node in nodes.Values)
    {
        if (node != null)
        {
            for (int i = 0; i < node.Count - 1; i++)
            {
                var loc1 = node[i];
                for (int j = i + 1; j < node.Count; j++)
                {

                    var loc2 = node[j];
                    var dx = loc2.x - loc1.x;
                    var dy = loc2.y - loc1.y;
                    var ax = loc1.x;
                    var ay = loc1.y;
                    int P = 0;
                    while (ax >= 0 && ax < S[0].Length && ay >= 0 && ay < S.Count)
                    {
                        if (P == 1) antinodes.Add((ax, ay));
                        antinodes2.Add((ax, ay));
                        ax -= dx;
                        ay -= dy;
                        P++;
                    }
                    ax = loc2.x;
                    ay = loc2.y;
                    P = 0;
                    while (ax >= 0 && ax < S[0].Length && ay >= 0 && ay < S.Count)
                    {
                        if (P == 1) antinodes.Add((ax, ay));
                        antinodes2.Add((ax, ay));
                        ax += dx;
                        ay += dy;
                        P++;
                    }
                }
            }
        }
    }
    answer1 = antinodes.Count;
    answer2 = antinodes2.Count;

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

