using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true, 7, 12, true);
Run(@"..\..\..\example1.txt", false, 71, 1024, true);
//Run(@"E:\develop\advent-of-code-input\2024\day18.txt", false);

void Run(string inputfile, bool isTest, int size, int down, bool show)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 22;

    var coordinates = File.ReadAllLines(inputfile).Select(s => s.Split(',').Select(int.Parse).ToArray()).Select(a => (x: a[0], y: a[1])).ToArray();
    long answer1 = 0;
    (int x, int y) answer2 = (0, 0);
    var state = new (int seen, bool useless, int costs)[size, size];
    for (int i = 0; i < down; i++)
    {
        var c = coordinates[i];
        state[c.x, c.y].useless = true;
    }
    for (int j = 0; j < size; j++)
    {
        for (int i = 0; i < size; i++)
        {
            if (state[i, j].useless) Console.Write('#');
            else
                Console.Write('.');
        }
        Console.WriteLine();
    }
    var end = (size - 1, size - 1);
    for (int k = down - 1; k < coordinates.Length; k++)
    {
        var c = coordinates[k];

        if (!state[c.x, c.y].useless || k < down)
        {
            state[c.x, c.y].useless = true;
            bool effective = c.x == 0 || c.y == 0 || c.x == size - 1 || c.y == size - 1 ||
                 state[c.x - 1, c.y].useless ||
                 state[c.x + 1, c.y].useless ||
                 state[c.x - 1, c.y - 1].useless ||
                 state[c.x + 1, c.y - 1].useless ||
                 state[c.x - 1, c.y + 1].useless ||
                 state[c.x + 1, c.y + 1].useless ||
                 state[c.x, c.y - 1].useless ||
                 state[c.x, c.y + 1].useless;

            if (effective)
            {
                var work = new PriorityQueue<(int x, int y), int>();
                work.Enqueue((0, 0), 0);
                long answer = 0;
                while (work.Count > 0)
                {
                    var cur = work.Dequeue();
                    if (cur == end && answer == 0)
                    {
                        answer = state[cur.x, cur.y].costs;
                    }
                    else
                    {
                        int addWork(int x, int y)
                        {
                            if (x < 0 || y < 0 || x >= size || y >= size) return 1;
                            var s = state[x, y];
                            if (s.useless) return 1;
                            if (s.seen == k) return 0;
                            s.seen = k;
                            s.costs = state[cur.x, cur.y].costs + 1;
                            work.Enqueue((x, y), s.costs);
                            state[x, y] = s;
                            return 0;
                        }
                        int blocked = addWork(cur.x - 1, cur.y);
                        blocked += addWork(cur.x + 1, cur.y);
                        blocked += addWork(cur.x, cur.y - 1);
                        blocked += addWork(cur.x, cur.y + 1);
                        if (blocked == 3) state[cur.x, cur.y].useless = true;
                    }
                }
                if (k == down - 1)
                {
                    answer1 = answer;
                }
                else if (answer == 0) { answer2 = coordinates[k]; break; }
                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                    {
                        var s = state[i, j];
                        if (!s.useless && s.seen != k) state[i, j].useless = true;
                    }
            }
        }
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    //Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Anstwer2: " + answer2.x + ',' + answer2.y);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

