using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true, 7, 12, true);
Run(@"..\..\..\example1.txt", false, 71, 1024, true);
//Run(@"E:\develop\advent-of-code-input\2024\day18.txt", false);

void Run(string inputfile, bool isTest, int size, int down, bool show)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 22;
    long supposedanswer2 = 0;

    var coordinates = File.ReadAllLines(inputfile).Select(s => s.Split(',').Select(int.Parse).ToArray()).Select(a => (x: a[0], y: a[1])).ToArray();
    long answer1 = 0;
    (int x, int y) answer2 = (0, 0);
    var corrupt = coordinates[..down].ToHashSet();
    for (int j = 0; j < size; j++)
    {
        for (int i = 0; i < size; i++)
        {
            if (corrupt.Contains((i, j))) Console.Write('#');
            else
                Console.Write('.');
        }
        Console.WriteLine();
    }

    for (int k = down - 1; k < coordinates.Length; k++)
    {
        corrupt.Add(coordinates[k]);

        var work = new PriorityQueue<(int x, int y), int>();
        work.Enqueue((0, 0), 0);
        var visited = new HashSet<(int x, int y)>();
        var state = new Dictionary<(int x, int y), int>();
        state.Add((0, 0), 0);
        long answer = 0;
        while (work.Count > 0 && answer == 0)
        {
            var cur = work.Dequeue();
            visited.Add(cur);
            if (cur == (size - 1, size - 1))
            {
                answer = state[cur];
                break;
            }
            else
            {
                void addWOrk(int x, int y, int cost)
                {
                    if (x < 0 || y < 0 || x >= size || y >= size) return;
                    if (corrupt.Contains((x, y))) return;
                    if (!state.ContainsKey((x, y)))
                    {
                        state[(x, y)] = cost;
                        work.Enqueue((x, y), cost);
                    }
                }
                addWOrk(cur.x - 1, cur.y, state[cur] + 1);
                addWOrk(cur.x + 1, cur.y, state[cur] + 1);
                addWOrk(cur.x, cur.y - 1, state[cur] + 1);
                addWOrk(cur.x, cur.y + 1, state[cur] + 1);
            }
        }
        if (k == down) { answer1 = answer; }
        else if (answer == 0) { answer2 = coordinates[k]; break; }
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    //Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Anstwer2: " + answer2.x + ',' + answer2.y);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

