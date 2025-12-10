using AocHelper;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\input.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 7;
    long supposedanswer2 = 33;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    foreach (var s in S)
    {
        var s1 = s[1..(s.Length - 1)].Split("] (");
        var target = s1[0].Select(a => a == '#').ToArray();
        var s2 = s1[1].Split(") {");
        var buttons = s2[0].Split(") (").Select(a => a.Split(',').Select(int.Parse).ToArray()).ToArray();
        var jolatge = s2[1].Split(',').Select(long.Parse).ToArray();

        var but_switch = new List<long>(buttons.Length);

        foreach (var but in buttons)
        {
            long wires = 0;
            foreach (int sw in but) wires = wires | (1L << sw);
            but_switch.Add(wires);
        }

        long t = 0;
        foreach (var b in target.Reverse())
        {
            t = (t << 1) + (b ? 1 : 0);
        }
        var work = new PriorityQueue<(long state, long steps), long>();
        var visited = new HashSet<long>();
        work.Enqueue((0, 0), 0);
        while (work.Count > 0)
        {
            var cur = work.Dequeue();
            if (cur.state == t)
            {
                answer1 += cur.steps;
                break;
            }
            visited.Add(cur.state);
            var newsteps = cur.steps + 1;
            foreach (var wires in but_switch)
            {
                var newstate = cur.state ^ wires;
                if (!visited.Contains(newstate)) work.Enqueue((newstate, newsteps), newsteps);
            }
        }

        var startj = new long[jolatge.Length];
        var totaljoltage = jolatge.Sum();
        var visited1 = new HashSet<string>();
        var work1 = new PriorityQueue<(long[] state, long steps), long>();
        var maxsteps = totaljoltage;
        work1.Enqueue((startj, 0), 0);
        while (work1.Count > 0)
        {
            var cur = work1.Dequeue();
            if (cur.steps < maxsteps)
            {
                bool targetreached = true;
                for (int i = 0; i < jolatge.Length; i++)
                {
                    if (jolatge[i] != cur.state[i])
                    {
                        targetreached = false;
                        break;
                    }
                }
                if (targetreached)
                {
                    maxsteps = cur.steps;
                }

                var newsteps = cur.steps + 1;
                foreach (var but in buttons)
                {
                    var newstate = cur.state.ToArray();
                    foreach (var b in but) newstate[b] += 1;
                    bool toobig = false;
                    for (int i = 0; i < jolatge.Length; i++)
                    {
                        if (jolatge[i] < newstate[i])
                        {
                            toobig = true;
                            break;
                        }
                    }
                    if (!toobig)
                    {
                        var newkey = String.Join(',', newstate.Select(a => a.ToString()).ToArray());
                        if (!visited1.Contains(newkey))
                        {
                            work1.Enqueue((newstate, newsteps), newsteps + totaljoltage - newstate.Sum());
                            visited1.Add(newkey);
                        }
                    }
                }
            }
        }
        answer2 += maxsteps;
    }
    
    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

