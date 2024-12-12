using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example0.txt", true, 140, 80);
Run(@"..\..\..\example.txt", true, 1930, 1206);
Run(@"..\..\..\example3.txt", true, 772, 436);
Run(@"..\..\..\example1.txt", false);
//Run(@"E:\develop\advent-of-code-input\2024\day12.txt", false);

void Run(string inputfile, bool isTest, long supposedanswer1 = 0, long supposedanswer2 = 0)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    var visited = new bool[S[0].Length, S.Count];

    var work = new Queue<(int x, int y, int region)>();
    int currentRegion = 0;
    var regions = new List<(long area, long perimeter, long sides)>();
    for (int y = 0; y < S.Count; y++)
    {
        for (int x = 0; x < S[y].Length; x++)
        {
            if (!visited[x, y])
            {
                work.Enqueue((x, y, currentRegion));
                var fence = new HashSet<(int x, int y, int direction)>();
                char planttype = S[y][x];
                regions.Add((0, 0, 0));
                while (work.Count > 0)
                {
                    var cur = work.Dequeue();
                    if (!visited[cur.x, cur.y])
                    {
                        visited[cur.x, cur.y] = true;

                        int dowork(int x1, int y1, int direction)
                        {
                            if (y1 >= 0 && x1 >= 0 && y1 < S.Count && x1 < S[y1].Length)
                            {
                                if (S[y1][x1] == planttype)
                                {
                                    if (!visited[x1, y1]) work.Enqueue((x1, y1, cur.region));
                                    return 0;
                                }

                            }
                            fence.Add((x1, y1, direction));
                            return 1;
                        }

                        int perometer = dowork(cur.x, cur.y - 1, 0);
                        perometer += dowork(cur.x, cur.y + 1, 1);
                        perometer += dowork(cur.x - 1, cur.y, 2);
                        perometer += dowork(cur.x + 1, cur.y, 3);
                        regions[cur.region] = (regions[cur.region].area + 1, regions[cur.region].perimeter + perometer, 0);
                    }

                }
                int sides = 0;
                while (fence.Count > 0)
                {
                    sides++;
                    var cur = fence.First();
                    fence.Remove(cur);
                    var (x1, y1, d1) = cur;
                    while (fence.Contains((x1 + 1, y1, d1))) { fence.Remove((x1 + 1, y1, d1)); x1++; }
                    while (fence.Contains((x1 - 1, y1, d1))) { fence.Remove((x1 - 1, y1, d1)); x1--; }
                    while (fence.Contains((x1, y1 + 1, d1))) { fence.Remove((x1, y1 + 1, d1)); y1++; }
                    while (fence.Contains((x1, y1 - 1, d1))) { fence.Remove((x1, y1 - 1, d1)); y1--; }

                }
                regions[currentRegion] = (regions[currentRegion].area, regions[currentRegion].perimeter, sides);
                currentRegion++;
            }
        }
    }
    foreach (var region in regions)
    {
        answer1 += region.area * region.perimeter;
        answer2 += region.area * region.sides;
    }


    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

