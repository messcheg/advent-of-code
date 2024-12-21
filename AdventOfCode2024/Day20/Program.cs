using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true, 64, 1, 86);
Run(@"..\..\..\example1.txt", false);
//Run(@"E:\develop\advent-of-code-input\2024\day20.txt", false);

void Run(string inputfile, bool isTest, int cheat = 100, long supposedanswer1 = 0, long supposedanswer2 = 0)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    int sx = 0;
    int sy = 0;
    for (int i = 0; i < S.Count; i++)
    {
        for (int j = 0; j < S[i].Length; j++)
        {
            if (S[j][i] == 'S')
            {
                (sx, sy) = (i, j);
                break;
            }
        }
        if (sx > 0) break;
    }

    var path = new List<(int x, int y)>();
    var position = new Dictionary<(int x, int y), int>();
    var (cx, cy) = (sx, sy);

    var (px, py) = (0, 0);
    path.Add((cx, cy));
    position.Add((cx, cy), 0);
    int step = 1;

    while (S[cy][cx] != 'E')
    {
        void addstep(int x, int y)
        {
            if ((x, y) == (px, py)) return;
            if (S[y][x] == '#') return;
            (px, py) = (cx, cy);
            (cx, cy) = (x, y);
            path.Add((x, y));
            position.Add((x, y), step);
            step++;
        }
        addstep(cx + 1, cy);
        addstep(cx - 1, cy);
        addstep(cx, cy + 1);
        addstep(cx, cy - 1);
    }

    var savings = new Dictionary<int, int>();
    var savings2 = new Dictionary<int, int>();
    for (int i = 0; i < path.Count; i++)
    {
        int TimeSaved(int p, int steps)
        {
            return p - i - steps;
        }
        void CheatOnce(int x, int y, int dx, int dy)
        {
            var (kx, ky) = (x + dx, y + dy);
            if (position.ContainsKey((kx, ky))) return; // not a wall
            (kx, ky) = (kx + dx, ky + dy);
            if (position.TryGetValue((kx, ky), out var p1))
            {
                int save = TimeSaved(p1, 2);
                if (save >= 2)
                {
                    if (!savings.TryGetValue(save, out int prev)) prev = 0;
                    savings[save] = prev + 1;
                }
            }

        }

        var cur = path[i];
        CheatOnce(cur.x, cur.y, 1, 0);
        CheatOnce(cur.x, cur.y, -1, 0);
        CheatOnce(cur.x, cur.y, 0, 1);
        CheatOnce(cur.x, cur.y, 0, -1);

        int minX = Math.Max(1, cur.x - 20);
        int maxX = Math.Min(S[0].Length - 1, cur.x + 21);
        for (int x = minX; x < maxX; x++)
        {
            int relX = Math.Abs(x - cur.x);
            int minY = Math.Max(1, cur.y - 20 + relX);
            int maxY = Math.Min(S.Count - 1, cur.y + 21 - relX);
            for (int y = minY; y < maxY; y++)
            {
                if (position.TryGetValue((x, y), out int steps))
                {
                    int manhattan = relX + Math.Abs(cur.y - y);
                    int gain = steps - i - manhattan;
                    if (gain >= cheat) answer2++;
                }
            }
        }
    }
    answer1 = savings.Where(a => a.Key >= cheat).Sum(a => a.Value);

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

