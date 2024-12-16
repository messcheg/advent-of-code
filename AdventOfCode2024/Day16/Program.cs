using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true, 7036, 45);
Run(@"..\..\..\example1.txt", true, 11048, 64);
Run(@"..\..\..\input.txt", false);
//Run(@"E:\develop\advent-of-code-input\2024\day16.txt", false);

void Run(string inputfile, bool isTest, long supposedanswer1 = 0, long supposedanswer2 = 0)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    int sx = 0;
    int sy = 0;
    int ex = 0;
    int ey = 0;
    var state = new (bool visitedOrWall, long cost, HashSet<(int x, int y)> seats)[S[0].Length, S.Count, 4];
    for (int j = 0; j < S.Count; j++)
    {
        var line = S[j];
        for (int i = 0; i < line.Length; i++)
        {
            state[i, j, 0].visitedOrWall =
                state[i, j, 1].visitedOrWall =
                state[i, j, 2].visitedOrWall =
                state[i, j, 3].visitedOrWall =
                (line[i] == '#');
            if (line[i] == 'E') (ex, ey) = (i, j);
            if (line[i] == 'S') (sx, sy) = (i, j);
        }
    }

    // direction 0 = up, 1 = right, 2 = down, 3 = left
    var work = new PriorityQueue<(int x, int y, int direction), long>();
    bool ready = false;
    work.Enqueue((sx, sy, 1), 0);
    state[sx, sy, 1].seats = new HashSet<(int x, int y)> { (sx, sy), (ex, ey) };

    while (work.Count > 0 && !ready)
    {
        var cur = work.Dequeue();
        var w = state[cur.x, cur.y, cur.direction];
        if ((cur.x, cur.y) == (ex, ey))
        {
            ready = true;
            answer1 = w.cost;
            answer2 = w.seats.Count;
        }
        else
        {
            state[cur.x, cur.y, cur.direction].visitedOrWall = true;

            void addwork(int x, int y, int direction)
            {
                if (state[x, y, direction].visitedOrWall) return;
                long cost = w.cost + 1;
                if (direction != cur.direction)
                {
                    var d1LR = (direction == 1 || direction == 3);
                    var d2LR = (cur.direction == 1 || cur.direction == 3);

                    if (d1LR == d2LR) return;
                    else cost += 1000;
                }
                var prev = state[x, y, direction];
                if (prev.seats == null || cost <= prev.cost)
                {
                    var h = (cost == prev.cost) ? w.seats.Union(prev.seats).ToHashSet() : w.seats.ToHashSet();
                    h.Add((x, y));
                    state[x, y, direction] = (false, cost, h);
                    if (cost != prev.cost) work.Enqueue((x, y, direction), cost);
                }
            }
            addwork(cur.x, cur.y - 1, 0);
            addwork(cur.x + 1, cur.y, 1);
            addwork(cur.x, cur.y + 1, 2);
            addwork(cur.x - 1, cur.y, 3);
        }
    }
    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

