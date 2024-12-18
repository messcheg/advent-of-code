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
    var useless = new bool[size, size];
    for (int i = 0; i < down; i++)
    {
        var c = coordinates[i];
        useless[c.x, c.y] = true;
    }
    for (int j = 0; j < size; j++)
    {
        for (int i = 0; i < size; i++)
        {
            if (useless[i, j]) Console.Write('#');
            else
                Console.Write('.');
        }
        Console.WriteLine();
    }

    for (int k = down - 1; k < coordinates.Length; k++)
    {
        var c = coordinates[k];

        if (!useless[c.x, c.y] || k < down)
        {
            useless[c.x, c.y] = true;

            var work = new PriorityQueue<(int x, int y), int>();
            work.Enqueue((0, 0), 0);
            var state = new (bool visited, bool seen, int costs)[size, size];
            long answer = 0;
            while (work.Count > 0)
            {
                var cur = work.Dequeue();
                state[cur.x, cur.y].visited = true;
                if (cur == (size - 1, size - 1) && answer == 0)
                {
                    answer = state[cur.x, cur.y].costs;
                }
                else
                {
                    void addWOrk(int x, int y)
                    {
                        if (x < 0 || y < 0 || x >= size || y >= size) return;
                        if (useless[x, y]) return;

                        if (!state[x, y].seen)
                        {
                            state[x, y].seen = true;
                            var cost = state[x, y].costs = state[cur.x, cur.y].costs + 1;
                            work.Enqueue((x, y), cost);
                        }
                    }
                    addWOrk(cur.x - 1, cur.y);
                    addWOrk(cur.x + 1, cur.y);
                    addWOrk(cur.x, cur.y - 1);
                    addWOrk(cur.x, cur.y + 1);
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
                    if (!state[i, j].seen) useless[i, j] = true;
                }
        }
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    //Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Anstwer2: " + answer2.x + ',' + answer2.y);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

