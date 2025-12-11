using AocHelper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

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
        var joltage = s2[1].Split(',').Select(long.Parse).ToArray();

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

        
        var startj = new long[joltage.Length];
        var startb = new long[buttons.Length];
        var totaljoltage = joltage.Sum();
        var visited1 = new HashSet<long>();
        var work1 = new PriorityQueue<(long[] state, long[] buttonstate, long steps), long>();
        var maxsteps = totaljoltage;
        var maxbutpress = new long[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            maxbutpress[i] = buttons[i].Select(b => joltage[b]).Min();
        }
        work1.Enqueue((startj, startb, 0), 0);
        while (work1.Count > 0)
        {
            var cur = work1.Dequeue();
            if (cur.steps < maxsteps)
            {
                bool targetreached = true;
                for (int i = 0; i < joltage.Length; i++)
                {
                    if (joltage[i] != cur.state[i])
                    {
                        targetreached = false;
                        break;
                    }
                }
                if (targetreached)
                {
                    maxsteps = cur.steps;
                }

                for (int bi = 0; bi < buttons.Length; bi++)
                {
                    if (cur.buttonstate[bi] == maxbutpress[bi]) continue;
                    var but = buttons[bi];
                        var newstate = cur.state.ToArray();
                        foreach (var b in but) newstate[b] += 1;
                        bool toobig = false;
                        for (int i = 0; i < joltage.Length; i++)
                        {
                            if (joltage[i] < newstate[i])
                            {
                                toobig = true;
                                break;
                            }
                        }
                    if (!toobig)
                    {
                        var newsteps = cur.steps + 1;
                        long newkey = newstate[0];
                        long close = 0;
                        for (int i = 1; i < joltage.Length; i++)
                        {
                            newkey = newkey * (joltage[i] + 1) + newstate[i];
                            close += (joltage[i] - newstate[i]) * (joltage[i] - newstate[i]);
                        }

                        if (!visited1.Contains(newkey))
                        {
                            var newbuttonstate = cur.buttonstate.ToArray();
                            newbuttonstate[bi] += 1;
                            work1.Enqueue((newstate, newbuttonstate, newsteps), newsteps + close);
                            visited1.Add(newkey);
                        }
                    }
                }
            }
        }
        answer2 += maxsteps;
        

        /*
        var emptyComby = new HashSet<int>();
        var buttonusedbefore = new bool[buttons.Length];
        var maxbutpress = new long[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            maxbutpress[i] = buttons[i].Select(b => joltage[b]).Min();
        }
        var byButtonAndValue = new Dictionary<(int button, long value), HashSet<int>>();
        var options = new List<long[]>();
        for (int j = 0; j < joltage.Length; j++)
        {
            //var joptions = new Dictionary<string, (long cost, long[] press)>();
            var joptions = new List<long[]>();
            var newByButtonAndValue = new Dictionary<(int button, long value), HashSet<int>>();

            var buts1 = new List<int>();
            var buttonused = buttonusedbefore.ToArray();
            var buttonusednow = new bool[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
                if (buttons[i].Contains(j))
                {
                    buts1.Add(i);
                    buttonusednow[i] = true;
                    buttonused[i] = true;
                }

            void CreateCombies(long limit, long[] prep, int[] buts)
            {
                for (int b1 = 0; b1 < buts.Length; b1++)
                {
                    var b = buts[b1];
                    var press = prep.ToArray();
                    var blimit = press[b] = Math.Min(limit, maxbutpress[b]);
                    // check if option exceeds total
                    bool possible = blimit == limit;
                    if (possible)
                    {
                        var resultjolt = new long[joltage.Length];
                        for (int bc = 0; bc < buttons.Length; bc++)
                        {
                            foreach (var jc in buttons[bc])
                            {
                                resultjolt[jc] += press[bc];
                                if (resultjolt[jc] > joltage[jc])
                                {
                                    possible = false;
                                    break;
                                }
                            }
                            if (!possible) break;
                        }
                        if (possible)
                        {
                            if (j == 0)
                            {
                                int idx = joptions.Count();
                                joptions.Add(press);
                                for (int b3 = 0; b3 < buttons.Length; b3++)
                                {
                                    if (buttonused[b3])
                                    {
                                        HashSet<int> cb;
                                        var key = (b3, press[b3]);
                                        if (!newByButtonAndValue.TryGetValue(key, out cb)) newByButtonAndValue[key] = cb = new HashSet<int>();
                                        cb.Add(idx);
                                    }
                                }
                            }
                            else 
                            {
                                HashSet<int> combis;
                                if (buttonusedbefore[b])
                                {
                                    HashSet<int> cb1;
                                    if (byButtonAndValue.TryGetValue((b, press[b]), out cb1)) combis = cb1.ToHashSet();
                                    else
                                    {
                                        combis = emptyComby;
                                    }
                                }
                                else
                                {
                                    combis = new HashSet<int>();
                                    for (int i3 = 0; i3 < options.Count; i3++) combis.Add(i3);
                                }
                                
                                for (int b2 = 0; b2 < buttons.Length; b2++)
                                {
                                    if (combis.Count == 0) break;
                                    if (buttonusedbefore[b2] && buttonusednow[b2])
                                    {
                                        HashSet<int> cb1;
                                        if (byButtonAndValue.TryGetValue((b2, press[b2]), out cb1)) combis.IntersectWith(cb1);
                                        else combis = emptyComby;                
                                    }
                                }
                                
                                foreach (var idx in combis)
                                {
                                    var combibuts = options[idx];
                                    var idx2 = joptions.Count();
                                    var combinedoption = new long[buttons.Length];
                                    bool usable = true;
                                    for (int c1 = 0; c1 < buttons.Length; c1++)
                                    {
                                        if (combibuts[c1] == press[c1]) combinedoption[c1] = combibuts[c1];
                                        else if (!buttonusedbefore[c1]) combinedoption[c1] = press[c1];
                                        else if (!buttonusednow[c1]) combinedoption[c1] = combibuts[c1];
                                        else usable = false;
                                    }
                                    if (usable)
                                    {
                                        joptions.Add(combinedoption);
                                        for (int c1 = 0; c1 < buttons.Length; c1++)
                                        {
                                            if (buttonused[c1])
                                            {
                                                HashSet<int> cb;
                                                var key = (c1, combinedoption[c1]);
                                                if (!newByButtonAndValue.TryGetValue(key, out cb)) newByButtonAndValue[key] = cb = new HashSet<int>();
                                                cb.Add(idx2);
                                            }
                                        }
                                    }
                                }   
                            }
                        }
                    }

                    for (long newlimit = Math.Min(limit - 1, blimit); newlimit > 0; newlimit--)
                    {
                        var newprep = prep.ToArray();
                        newprep[b] = newlimit;
                        CreateCombies(limit - newlimit, newprep, buts[(b1 + 1)..]);
                    }
                }
            }

            CreateCombies(joltage[j], new long[buttons.Length], buts1.ToArray());
            options = joptions;
            buttonusedbefore = buttonused;
            byButtonAndValue = newByButtonAndValue;
        }
        answer2 += options.Min(a => a.Sum());
        */
    }

    
    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

