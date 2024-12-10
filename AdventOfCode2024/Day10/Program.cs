using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true, 1, 12);
Run(@"..\..\..\example1.txt", true, 36);
Run(@"..\..\..\example2.txt", false);
//Run(@"E:\develop\advent-of-code-input\2024\day10.txt", false);

void Run(string inputfile, bool isTest, long supposedanswer1 = 0, long supposedanswer2 = 0)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    for (
        int y = 0; y < S.Count; y++)
    {
        var line = S[y];
        for (int x = 0; x < line.Length; x++)
        {
            if (S[y][x] == '0')
            {
                var visited = new HashSet<(int x, int y)>();
                var addedwork = new Dictionary<(int x, int y), long>();
                var ninesVisited = new HashSet<(int x, int y)>();
                var work = new Queue<(int x, int y)>();
                work.Enqueue((x, y));
                addedwork.Add((x, y), 1);
                while (work.Count > 0)
                {
                    var w = work.Dequeue();
                    if (!visited.Contains(w))
                    {
                        visited.Add(w);
                        var c = S[w.y][w.x];
                        if (c == '9') ninesVisited.Add(w);

                        void tryAddwork(int x1, int y1, bool condition)
                        {
                            if (condition && S[y1][x1] - c == 1)
                            {
                                if (addedwork.TryGetValue((x1, y1), out var cnt1))
                                {
                                    addedwork[(x1, y1)] = cnt1 + addedwork[w];
                                }
                                else
                                {
                                    work.Enqueue((x1, y1));
                                    addedwork.Add((x1, y1), addedwork[w]);
                                }
                            }
                        }

                        tryAddwork(w.x - 1, w.y, w.x > 0);
                        tryAddwork(w.x + 1, w.y, w.x < S[0].Length - 1);
                        tryAddwork(w.x, w.y - 1, w.y > 0);
                        tryAddwork(w.x, w.y + 1, w.y < S.Count - 1);
                    }
                }
                answer1 += ninesVisited.Count;
                foreach (var w in ninesVisited) answer2 += addedwork[w];
            }
        }
    }


    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

