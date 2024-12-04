using System.Collections;
using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Security;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security;
using System.Security.Cryptography;
using AocHelper;
using Microsoft.Win32.SafeHandles;

Run(@"..\..\..\example.txt", true, 94, 154);
//Run(@"..\..\..\example1.txt", true, 11687500, 0000);
//Run(@"..\..\..\example2.txt", true, 22, 4);
Run(@"E:\develop\advent-of-code-input\2023\day23.txt", false, 0, 0);

void Run(string inputfile, bool isTest, long supposedanswer1, long supposedanswer2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    //var S = File.ReadAllText(inputfile).Split("\r\n\r\n").Select(s => s.Split("\r\n").ToList()).ToList();
    //var S = File.ReadAllLines(inputfile).Select(a => a.Select(b => b - '0').ToList()).ToList();
    var S = File.ReadAllLines(inputfile);

    long answer1 = 0;
    long answer2 = 0;

    var work = new Queue<(int x, int y, HashSet<(int x, int y)> visited)>();
    var bestresult = 0;
    var X = S[0].Length;
    var Y = S.Count();
    work.Enqueue((1, 0, new HashSet<(int x, int y)>()));

    while (work.Count > 0)
    {
        var cur = work.Dequeue();
        cur.visited.Add((cur.x, cur.y));

        void Add(int x, int y, int direction)
        {
            if (x >= 0 && x < X && y >= 0 && y < Y && S[y][x] != '#' && !cur.visited.Contains((x, y)))
            {
                if (y == Y - 1)
                {
                    bestresult = Math.Max(bestresult, cur.visited.Count);
                }
                else
                {
                    var c = S[y][x];
                    if (c == '.'
                        || direction == 0 && c == '>'
                        || direction == 1 && c == 'v'
                        || direction == 2 && c == '<'
                        || direction == 3 && c == '^'
                        )
                    {
                        var vis1 = cur.visited.ToHashSet();
                        work.Enqueue((x, y, vis1));
                    }
                }
            }
        }
        Add(cur.x + 1, cur.y, 0);
        Add(cur.x, cur.y + 1, 1);
        Add(cur.x - 1, cur.y, 2);
        Add(cur.x, cur.y - 1, 3);
    }
    answer1 = bestresult;

    var work2 = new Queue<(int x, int y, int direction)>();

    // find crosspoints
    work2.Enqueue((1, 0, 1));
    var distances = new Dictionary<(int x, int y), Dictionary<(int x, int y), int>>();
    var crossings = new HashSet<(int x, int y)>();
    while (work2.Count > 0)
    {
        var cur = work2.Dequeue();
        crossings.Add((cur.x, cur.y));

        var x1 = cur.x;
        var y1 = cur.y;

        bool ready = false;
        int distance = 0;
        var direction = cur.direction;
        while (!ready)
        {
            x1 = x1 + ((direction == 0) ? 1 : (direction == 2 ? -1 : 0));
            y1 = y1 + ((direction == 1) ? 1 : (direction == 3 ? -1 : 0));

            distance++;
            var ports = new List<int>(4);
            if (direction != 2 && x1 < X - 1 && S[y1][x1 + 1] != '#') ports.Add(0);
            if (direction != 3 && y1 < Y - 1 && S[y1 + 1][x1] != '#') ports.Add(1);
            if (direction != 0 && x1 > 0 && S[y1][x1 - 1] != '#') ports.Add(2);
            if (direction != 1 && y1 > 0 && S[y1 - 1][x1] != '#') ports.Add(3);
            if (ports.Count == 1) direction = ports[0];
            else
            {
                ready = true;
                var a = (cur.x, cur.y);
                var b = (x1, y1);
                if (!distances.ContainsKey(a)) distances[a] = new Dictionary<(int x, int y), int>();
                if (!distances.ContainsKey(b)) distances[b] = new Dictionary<(int x, int y), int>();
                if (!distances[a].ContainsKey(b)) distances[a][b] = 0;
                if (!distances[b].ContainsKey(a)) distances[b][a] = 0;
                int prevdist = distances[a][b];
                if (prevdist > 0 && prevdist > distance)
                    distance = prevdist;

                distances[a][b] = distances[b][a] = distance;

                if (!crossings.Contains((x1, y1)))
                {
                    foreach (var port in ports)
                    {
                        work2.Enqueue((x1, y1, port));
                    }
                }
            }
        }

    }

    //var extraports = new Dictionary<(int x, int y),List<((int x, int y) key, int value, HashSet<(int x, int y)> notSuitableFor)>>();
    //foreach (var k in distances)
    //{
    //    var v = k.Value.Select(p => (p.Key, p.Value, new HashSet<(int x, int y)>())).ToList();
    //    extraports[k.Key] = v;
    //}

    //var startnode = extraports[(1, 0)];
    //while (extraports.Count > 2)
    //{

    //    // find a node to merge
    //    var mergenodekey = startnode.First().key;

    //    var nodeToMerge = extraports[mergenodekey];
    //    var handeled = new HashSet<((int, int), (int, int))>();
    //    for (int i = 0; i < nodeToMerge.Count - 1; i++)
    //    {
    //        var node1 = nodeToMerge[i];
    //        var dnode1 = extraports[node1.key];
    //        var distArr1 = extraports[node1.key].Where(ka => ka.key == mergenodekey).ToArray();


    //        for (int j = i + 1; j < nodeToMerge.Count; j++)
    //        {
    //            var node2 = nodeToMerge[j];
    //            if (node1.key != node2.key && !handeled.Contains((node1.key,node2.key)))
    //            {
    //                var dnode2 = extraports[node2.key];
    //                var distArr2 = extraports[node2.key].Where(ka => ka.key == mergenodekey).ToArray();

    //                var newdist = new List<(int value,HashSet<(int,int)> notSuitablefor)>();

    //                foreach (var d in distArr1)
    //                {
    //                    var suitable = !d.notSuitableFor.Contains(node2.key);
    //                    if (suitable)
    //                    {
    //                        foreach (var d2 in distArr2)
    //                        {
    //                            if (!d2.notSuitableFor.Contains(node1.key))
    //                            {
    //                                newdist.Add((d.value + d2.value, d.notSuitableFor.Union(d2.notSuitableFor).Where(ka => ka != mergenodekey).ToHashSet()));
    //                            }
    //                        }
    //                    }
    //                }

    //                var wasthere = false;
    //                foreach (var n1 in dnode1)
    //                {
    //                    if (n1.key == node2.key) { wasthere = true; break; }
    //                }
    //                if (wasthere)
    //                {
    //                    // find overlaps between node1 and nodetoreplace
    //                    var overlap1 = new List<(int x, int y)>();
    //                    foreach (var n1 in dnode1)
    //                        foreach (var nm in nodeToMerge)
    //                            if (n1.key == nm.key) overlap1.Add(nm.key);

    //                    var overlap2 = new List<(int x, int y)>();
    //                    foreach (var n2 in dnode2)
    //                        foreach (var nm in nodeToMerge)
    //                            if (n2.key == nm.key) overlap2.Add(nm.key);
    //                    foreach (var nd in newdist)
    //                    {
    //                        dnode2.Add((node1.key, nd.value, nd.notSuitablefor.Union(overlap1).ToHashSet()));
    //                        dnode1.Add((node2.key, nd.value, nd.notSuitablefor.Union(overlap2).ToHashSet()));
    //                    }
    //                }
    //                else
    //                {
    //                    foreach (var nd in newdist)
    //                    {
    //                        dnode2.Add((node1.key, nd.value, nd.notSuitablefor));
    //                        dnode1.Add((node2.key, nd.value, nd.notSuitablefor));
    //                    }
    //                }
    //            }
    //            handeled.Add((node1.key, node2.key));
    //            handeled.Add((node2.key, node1.key));
    //        }
    //    }
    //    foreach (var nd in nodeToMerge)
    //    {
    //        var other = extraports[nd.key];
    //        foreach(var ot in other) { if (ot.notSuitableFor.Contains(mergenodekey)) ot.notSuitableFor.Remove(mergenodekey); }
    //        for (int i = other.Count - 1; i >= 0; i--)
    //        {
    //            if (other[i].key == mergenodekey)
    //                other.RemoveAt(i);
    //            else
    //            {
    //                for (int j = 0; j < i - 1; j++)
    //                {
    //                    if (other[j].key == other[i].key && other[j].notSuitableFor.Count == other[i].notSuitableFor.Count)
    //                    {
    //                        if (other[i].value < other[i].value && other[i].notSuitableFor.All(ka => other[j].notSuitableFor.Contains(ka)))
    //                        {
    //                            other.RemoveAt(i);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    extraports.Remove(mergenodekey);
    //}
    //answer2 = extraports[(1, 0)].First().value;

    var work1 = new PriorityQueue<(int x, int y, HashSet<(int x, int y)> visited, int distance), int>();
    work1.Enqueue((1, 0, new HashSet<(int x, int y)>(), 0), X * Y);
    bestresult = 0;
    var exploredpaths = new Dictionary<(int x, int y), List<(int distance, HashSet<(int x, int y)> visited)>>();

    while (work1.Count > 0)
    {
        var collect = new List<(int x, int y, HashSet<(int x, int y)> visited, int distance)>();


        var cur = work1.Peek();
        var curdist = cur.distance;

        while (work1.Count > 0 && work1.Peek().distance == curdist)
        {
            cur = work1.Dequeue();
            cur.visited.Add((cur.x, cur.y));

            if (cur.y == Y - 1)
            {
                bestresult = Math.Max(bestresult, cur.distance);

            }

            var ports = distances[(cur.x, cur.y)];
            foreach (var port in ports)
            {
                if (!cur.visited.Contains(port.Key))
                {
                    var vis1 = cur.visited.ToHashSet();
                    vis1.Add(port.Key);
                    var newdist = cur.distance + port.Value;
                    if (!exploredpaths.ContainsKey(port.Key)) exploredpaths[port.Key] = new List<(int, HashSet<(int x, int y)>)>();
                    bool found = false;
                    int replace = -1;
                    for (int i = 0; i < exploredpaths[port.Key].Count; i++)
                    {
                        var set = exploredpaths[port.Key][i];
                        if (set.visited.Count == vis1.Count)
                        {
                            bool dif = false;
                            foreach (var pos in set.visited) if (!vis1.Contains(pos)) { dif = true; break; }
                            if (!dif)
                            {
                                if (set.distance < newdist) replace = i;
                                found = true;
                                break;
                            }
                        }

                    }

                    if (!found)
                    {
                        exploredpaths[port.Key].Add((newdist, vis1));
                        collect.Add((port.Key.x, port.Key.y, vis1, newdist));
                    }
                    else if (replace >= 0)
                    {
                        collect.Add((port.Key.x, port.Key.y, vis1, newdist));

                        var set = exploredpaths[port.Key][replace];
                        exploredpaths[port.Key][replace] = (newdist, set.visited);
                        var work3 = new PriorityQueue<(int x, int y, HashSet<(int x, int y)> visited, int distance), int>();

                        while (work1.Count > 0)
                        {
                            var c1 = work1.Dequeue();
                            bool needsupdate = false;
                            foreach (var pos in set.visited) { if (!c1.visited.Contains(pos)) { needsupdate = false; break; } }
                            if (needsupdate)
                                work3.Enqueue((c1.x, c1.y, c1.visited, c1.distance - set.distance + newdist), c1.visited.Count + (X + 1) * (Y + 1) - (c1.distance - set.distance + newdist));
                            else
                                work3.Enqueue(c1,  (X + 1) * (Y + 1) - c1.distance + c1.visited.Count);
                        }
                        work1 = work3;
                    }
                }
            }
        }
        foreach (var item in collect)
        {
            work1.Enqueue(item, item.visited.Count + (X + 1) * (Y + 1) - item.distance);
        }
    }
    answer2 = bestresult;




    stopwatch.Stop();
    if (supposedanswer1 > -1) Aoc.w(1, answer1, supposedanswer1, isTest);
    if (supposedanswer2 > -1) Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());
}
