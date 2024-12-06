using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\example1.txt", false);
//Run(@"E:\develop\advent-of-code-input\2024\day06.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 41;
    long supposedanswer2 = 6;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    var visited = new HashSet<(int x, int y)>();

    var start = FindStartPos(S);

    var (x, y, d) = start;
    bool ready = false;
    while (!ready)
    {
        visited.Add((x, y));
        if (d == 1 && y == 0) break;
        if (d == 2 && x == S[0].Length - 1) break;
        if (d == 3 && y == S.Count - 1) break;
        if (d == 4 && x == 0) break;

        switch (d)
        {
            case 1: if (S[y - 1][x] == '#') d = 2; else y--; break;
            case 2: if (S[y][x + 1] == '#') d = 3; else x++; break;
            case 3: if (S[y + 1][x] == '#') d = 4; else y++; break;
            case 4: if (S[y][x - 1] == '#') d = 1; else x--; break;
        }
    }
    answer1 = visited.Count;

    var starto = (start.x, start.y);
    foreach (var obstacle in visited.ToArray())
    {
        if (obstacle == starto) continue;
        if (Isloop(S, obstacle, start)) answer2++;
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

bool isGuard(char c) { return c == 'v' || c == '^' || c == '>' || c == '<'; }

(int x, int y, int d) FindStartPos(List<string> S)
{
    (int x, int y, int d) pos = (0, 0, 0);

    for (int y = 0; y < S.Count; y++)
    {
        string line = S[y];
        for (int x = 0; x < line.Length; x++)
        {
            if (isGuard(line[x]))
            {
                switch (line[x])
                {
                    case '^': pos = (x, y, 1); break;
                    case '>': pos = (x, y, 2); break;
                    case 'v': pos = (x, y, 3); break;
                    case '<': pos = (x, y, 4); break;
                }
            }
        }
    }
    return pos;
}

bool Isloop(List<string> S, (int x, int y) obstacle, (int x, int y, int d) start)
{
    var visited1 = new HashSet<(int x, int y, int d)>();
    var pos = start;
    bool loop = false;
    while (!loop)
    {
        if (visited1.Contains(pos)) { loop = true; break; }
        visited1.Add(pos);

        if (pos.d == 1 && pos.y == 0) break;
        if (pos.d == 2 && pos.x == S[0].Length - 1) break;
        if (pos.d == 3 && pos.y == S.Count - 1) break;
        if (pos.d == 4 && pos.x == 0) break;

        bool isObst(int y, int x) { return (S[y][x] == '#' || (x, y) == obstacle); }

        switch (pos.d)
        {
            case 1: if (isObst(pos.y - 1, pos.x)) pos.d = 2; else pos.y--; break;
            case 2: if (isObst(pos.y, pos.x + 1)) pos.d = 3; else pos.x++; break;
            case 3: if (isObst(pos.y + 1, pos.x)) pos.d = 4; else pos.y++; break;
            case 4: if (isObst(pos.y, pos.x - 1)) pos.d = 1; else pos.x--; break;
        }
    }
    return loop;

}