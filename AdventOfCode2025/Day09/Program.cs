using AocHelper;
using System.Diagnostics;
using System.Runtime.CompilerServices;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\input.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 50;
    long supposedanswer2 = 24;

    var S = File.ReadAllLines(inputfile).Select(s => s.Split(',').Select(long.Parse).ToArray()).Select(a => (X: a[0], Y: a[1])).OrderBy(a => a.X).ThenBy(a => a.Y).ToList();
    long answer1 = 0;
    long answer2 = 0;


    var lines = new List<(long x1, long y1, long x2, long y2)>();
    var byX = new SortedList<long, SortedList<long, (long X, long Y)>>();
    var byY = new SortedList<long, SortedList<long, (long X, long Y)>>();
    foreach (var pos in S)
    {
        SortedList<long, (long X, long Y)> sameX;
        if (!byX.TryGetValue(pos.X, out sameX)) byX[pos.X] = sameX = new SortedList<long, (long X, long Y)>();
        sameX.Add(pos.Y, pos);

        SortedList<long, (long X, long Y)> sameY;
        if (!byY.TryGetValue(pos.Y, out sameY)) byY[pos.Y] = sameY = new SortedList<long, (long X, long Y)>();
        sameY.Add(pos.X, pos);
    }

    for (int i = 0; i < S.Count - 1; i++)
    {
        for (int j = S.Count - 1; j > i; j--)
        {
            var c1 = S[i];
            var c2 = S[j];
            var a = (1 + Math.Abs(c1.X - c2.X)) * (1 + Math.Abs(c1.Y - c2.Y));
            if (a > answer1) answer1 = a;

            if (a > answer2)
            {
                //check if points ar in the polygon 
                bool inPoly(long x, long y)
                {
                    var border = new List<(long x1, long x2, int dir)>();

                    var ly = byY[y].Keys;

                    for (int p = 0; p < ly.Count; p += 2)
                    {
                        if (ly[p] <= x)
                        {
                            if (ly[p + 1] >= x) return true; // point on the line
                            border.Add((ly[p], ly[p + 1], 0));
                        }
                    }

                    foreach (var kv in byX.Where(a => a.Key <= x))
                    {
                        var lxy = kv.Value.Keys;
                        for (int p = 0; p < lxy.Count; p += 2)
                        {
                            if (lxy[p] <= y && lxy[p + 1] >= y)
                            {
                                if (kv.Key == x) return true; // point is on the line
                                int d = 0;
                                if (lxy[p] == y) d = 1;
                                if (lxy[p + 1] == y) d = 2;
                                border.Add((kv.Key, kv.Key, d));
                            }
                        }
                    }
                   
                    var sb = border.OrderBy(a => a.x1).ThenBy(a => a.x2).ToArray();
                    bool inside = false;
                    bool wasinside = false;
                    for (int i = 0; i < sb.Length; i++)
                    {
                        if (!inside)
                        {
                            inside = true;
                            wasinside = false;
                        }
                        else
                        {
                            if (sb[i].x1 == sb[i - 1].x2)
                            {
                                if (sb[i].dir != 0)
                                {
                                    if (sb[i].dir != sb[i - 2].dir)
                                    {
                                        inside = !wasinside;
                                    }
                                    else
                                    {
                                        inside = wasinside;
                                    }
                                }
                            }
                            else
                            {
                                if (sb[i].dir == 0)
                                {
                                    inside = false;
                                }
                                else
                                {
                                    wasinside = true;
                                }
                            }
                        }
                    }
                    return inside;
                }

                if (inPoly(c1.X, c2.Y) && inPoly(c2.X, c1.Y))
                {
                    var x1 = Math.Min(c1.X, c2.X);
                    var x2 = Math.Max(c1.X, c2.X);
                    var y1 = Math.Min(c1.Y, c2.Y);
                    var y2 = Math.Max(c1.Y, c2.Y);

                    // select all vertical  lines between x1 an x2
                    var xline = byX.Where(a => a.Key > x1 && a.Key < x2).Select(a => a.Value.Keys).ToArray();

                    bool Crosseslines(long a1, long a2, IList<long>[] vv)
                    {
                        foreach (var v in vv)
                            for (int l = 0; l < v.Count; l += 2)
                            {
                                if (v[l] < a1 && v[l + 1] > a1) return true;
                                if (v[l] < a2 && v[l + 1] > a2) return true;
                            }
                        return false;
                    }

                    if (!Crosseslines(y1, y2, xline)) 
                    {
                        // select all horontal lines between y1 and y2
                        var yline = byY.Where(a => a.Key > y1 && a.Key < y2).Select(a => a.Value.Keys).ToArray();
                        if (!Crosseslines(x1, x2, yline)) answer2 = a;
                    }
                }
            }
        }
    }



    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

