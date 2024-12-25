using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true, 4);
Run(@"..\..\..\example1.txt", true, 2024);
Run(@"..\..\..\input.txt", false, 0, true);
//Run(@"E:\develop\advent-of-code-input\2024\day24.txt", false);

void Run(string inputfile, bool isTest, long supposedanswer1 = 0, bool doPartII = false)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    int i = 0;
    var wires = new Dictionary<string, char>();
    var allwires = new HashSet<string>();
    var alloutputs = new HashSet<string>();
    var allinputs = new HashSet<string>();
    var dependson = new Dictionary<string, HashSet<string>>();
    while (S[i] != "")
    {
        var s1 = S[i].Split(": ");
        wires.Add(s1[0], s1[1][0]);
        allwires.Add(s1[0]);
        i++;
    }

    var gates = new Dictionary<string, (string operation, string input1, string input2)>();
    i++;
    while (i < S.Count)
    {
        var s1 = S[i].Split(' ');
        var wire = s1[4];
        var oper = s1[1];
        var inp1 = s1[0];
        var inp2 = s1[2];
        gates.Add(wire, (oper, inp1, inp2));
        //if (!wires.ContainsKey(s1[0])) wires.Add(s1[0], ' ');
        //if (!wires.ContainsKey(s1[2])) wires.Add(s1[2], ' ');
        //if (!wires.ContainsKey(s1[4])) wires.Add(s1[4], ' ');
        allwires.Add(s1[0]);
        allinputs.Add(s1[0]);
        allwires.Add(s1[2]);
        allinputs.Add(s1[2]);
        allwires.Add(s1[4]);
        alloutputs.Add(s1[4]);
        if (!dependson.TryGetValue(wire, out var depend)) depend = new HashSet<string>();
        depend.Add(inp1);
        depend.Add(inp2);
        foreach (var h1 in dependson.Values)
        {
            if (h1.Contains(wire))
            {
                h1.Add(inp1);
                h1.Add(inp2);
            }
        }

        i++;
    }
    var emptyinput = wires.Select((a) => (a.Key, '0')).ToArray();
    var allundef = allwires.Except(wires.Keys).ToList();
    (answer1, var loop) = GetResult(wires, gates, allundef);

    Aoc.w(1, answer1, supposedanswer1, isTest);


    if (doPartII)
    {
        // var emptyinput = allwires.Select(a => (a, a[0] == 'x' || a[0] == 'y' ? '0' : ' '));
        var xlabels = new string[45];
        var ylabels = new string[45];
        var zlabels = new string[46];
        var masks = new long[46];
        var masks_one = new long[46];
        var masks_two = new long[46];
        long maskone = 1;
        long masktwo = 1;
        var allinouts = alloutputs.ToArray(); //allinputs.Intersect(alloutputs).ToArray();

        long mask = 1;
        for (int x = 0; x < 45; x++)
        {
            xlabels[x] = "x" + x.ToString("00");
            ylabels[x] = "y" + x.ToString("00");
        }
        for (int z = 0; z < 46; z++)
        {
            zlabels[z] = "z" + z.ToString("00");
            masks[z] = mask;
            masks_one[z] = maskone;
            masks_two[z] = masktwo;
            mask = (mask << 1) | 1L;
            masktwo = maskone | (maskone << 1);
            maskone <<= 1;
        }

        var correct = 0;
        var swapped = new HashSet<string>();
        var dontTouchIfItWorks = new HashSet<string>();
        var swaplevelcache = new int[8];
        for (int z = 0; z < 46; z++)
        {

            bool ok = false;
            int swaplevel = 0;
            while (!ok && swaplevel <= 4)
            {
                if (swaplevel == 0)
                {
                    ok = TestBit(gates, emptyinput, xlabels, ylabels, masks, masks_one, z, allundef);
                }
                else
                {
                    string getWirename(int index) { return index == -1 ? zlabels[z] : allinouts[index]; }

                    swaplevelcache[0] = allinouts.Contains(zlabels[z]) ? -1 : 0;
                    var used = new HashSet<int>() { swaplevelcache[0] };
                    InitCache(0, gates, zlabels, allinouts, swaplevelcache, z, swaplevel, used);

                    while (swaplevelcache[0] < allinouts.Length - 1 && !ok)
                    {
                        ok = TestBit(gates, emptyinput, xlabels, ylabels, masks, masks_one, z, allundef);
                        if (ok)
                        {
                            for (int k = 0; k < swaplevel << 1; k++)
                            {
                                swapped.Add(getWirename(swaplevelcache[k]));
                            }
                            break;
                        }
                        else
                        {
                            int idx = (swaplevel << 1) - 1;
                            bool ready = false;
                            while (!ready && idx > 0)
                            {
                                var l1 = getWirename(swaplevelcache[idx]);
                                var l2 = getWirename(swaplevelcache[idx - 1]);
                                used.Remove(swaplevelcache[idx]);
                                used.Remove(swaplevelcache[idx - 1]);
                                (gates[l1], gates[l2]) = (gates[l2], gates[l1]); // undo last swap
                                swaplevelcache[idx]++;
                                if (swaplevelcache[idx] > allinouts.Length - 1)
                                {
                                    swaplevelcache[idx - 1]++;
                                    if (swaplevelcache[idx - 1] > allinouts.Length - 2)
                                    { idx -= 2; break; }
                                    swaplevelcache[idx] = swaplevelcache[idx - 1] + 1;

                                }
                                l1 = getWirename(swaplevelcache[idx]);
                                l2 = getWirename(swaplevelcache[idx - 1]);
                                (gates[l1], gates[l2]) = (gates[l2], gates[l1]); // new swap
                                used.Add(swaplevelcache[idx]);
                                used.Add(swaplevelcache[idx - 1]);
                                ready = true;
                            }
                            if (!ready && idx > 0)
                            {
                                InitCache(0, gates, zlabels, allinouts, swaplevelcache, z, swaplevel, used);
                            }
                        }
                    }
                }
                if (ok)
                {
                    correct++;
                    var work = new Queue<string>();
                    work.Enqueue(zlabels[z]);
                    while (work.Count > 0)
                    {
                        var w = work.Dequeue();
                        dontTouchIfItWorks.Add(w);
                        if (gates.TryGetValue(w, out var gate))
                        {
                            work.Enqueue(gate.input1);
                            work.Enqueue(gate.input2);
                        }
                    }
                    allinouts = allinouts.Where(x => !dontTouchIfItWorks.Contains(x)).ToArray();
                }
                else swaplevel++;
            }
        }
        string answer2 = string.Join(',', swapped.Order().ToArray());
        string supposedanswer2 = string.Empty;

        Aoc.w(2, answer2, supposedanswer2, isTest);
    }
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");

    static bool TestBit(Dictionary<string, (string operation, string input1, string input2)> gates,
        IEnumerable<(string a, char)> emptyinput, string[] xlabels, string[] ylabels, long[] masks, long[] masks_one, int z, List<string> allundef)
    {
        bool PerformTest(int z1, int zv, char xv, char yv, char cv)
        {
            var testwires = emptyinput.ToDictionary();
            if (z1 < 45)
            {
                testwires[xlabels[z1]] = xv;
                testwires[ylabels[z1]] = yv;
            }
            if (cv == '1' && z1 > 0)
            {
                var z0 = z1 - 1;
                testwires[xlabels[z0]] = '1';
                testwires[ylabels[z0]] = '1';
            }
            var (a, l) = GetResult(testwires, gates, allundef);
            var checkval = zv == 1 ? masks_one[z] : 0;
            return ((a & masks[z1]) == checkval && !l);
        }

        // test 1: add 0 + 0
        var testwires = emptyinput.ToDictionary();
        var (answer, loop) = GetResult(testwires, gates, allundef);
        if ((answer & masks[z]) != 0) return false;

        if (z < 45)
        {
            // test 2: add 1 + 1
            if (!PerformTest(z, 0, '1', '1', '0')) return false;

            // test 3: add 1 + 0
            if (!PerformTest(z, 1, '1', '0', '0')) return false;

            // test 4: add 0 + 1
            if (!PerformTest(z, 1, '0', '1', '0')) return false;

        }
        if (z > 0)
        {
            // test 5: add 0 + 0 + 1
            if (!PerformTest(z, 1, '0', '0', '1')) return false;

            if (z < 45)
            {
                // test 6: add 1 + 1 + 1
                if (!PerformTest(z, 1, '1', '1', '1')) return false;

                // test 7: add 1 + 0 + 1
                if (!PerformTest(z, 0, '1', '0', '1')) return false;

                // test 8: add 0 + 1 + 1
                if (!PerformTest(z, 0, '0', '1', '1')) return false;
            }
        }

        return true;
    }

    void InitCache(int start, Dictionary<string, (string operation, string input1, string input2)> gates, string[] zlabels, string[] allinouts, int[] swaplevelcache, int z, int swaplevel, HashSet<int> used)
    {
        for (int k = start + 1; k < swaplevel << 1; k += 2)
        {
            if (k > 2)
            {
                swaplevelcache[k - 1] = swaplevelcache[k - 3] + 1;
                if (swaplevelcache[k - 1] == swaplevelcache[k - 2]) swaplevelcache[k - 1] = swaplevelcache[k - 2] + 1;
                used.Add(swaplevelcache[k - 1]);
            }
            int next = swaplevelcache[k - 1] + 1;
            bool independent = false;
            var l1 = next > 0 ? allinouts[next - 1] : zlabels[z];
            while (next < allinouts.Length && !independent)
            {
                if (used.Contains(next)) next++;
                else
                {
                    var l2 = allinouts[next];
                    independent = NoDependencies(l1, l2, gates);
                    if (!independent) next++;
                    else (gates[l1], gates[l2]) = (gates[l2], gates[l1]);
                }
            }
            swaplevelcache[k] = next;
            used.Add(swaplevelcache[k]);
            used.Add(swaplevelcache[k - 1]);
        }
    }
}

bool NoDependencies(string l1, string l2, Dictionary<string, (string operation, string input1, string input2)> gates)
{
    if (!gates.TryGetValue(l1, out var h1)) return true;
    if (!gates.TryGetValue(l2, out var h2)) return true;
    if (h1.input1 == l2 || h1.input2 == l2) return false;
    if (h2.input1 == l1 || h2.input2 == l1) return false;
    return
        NoDependencies(h1.input1, l2, gates) &&
        NoDependencies(h1.input2, l2, gates) &&
        NoDependencies(h2.input1, l1, gates) &&
        NoDependencies(h2.input2, l1, gates);

}

static (long answer, bool loop) GetResult(Dictionary<string, char> wires, Dictionary<string, (string operation, string input1, string input2)> gates, List<string> allundef)
{
    var undef = allundef;
    bool allZs = false;
    bool changed = true;

    while (!allZs && changed)
    {
        var newUndef = new List<string>();
        allZs = true;
        changed = false;
        foreach (var a in undef)
        {
            var gate = gates[a];
            var b1 = wires.TryGetValue(gate.input1, out var v1);
            var b2 = wires.TryGetValue(gate.input2, out var v2);
            if (!(b1 && b2))
            {
                newUndef.Add(a);
                if (a[0] == 'z') allZs = false;
            }
            else
            {
                switch (gate.operation[0])
                {
                    case 'A':
                        if (v1 == '1' && v2 == '1') wires[a] = '1'; else wires[a] = '0';
                        break;
                    case 'O':
                        if (v1 == '1' || v2 == '1') wires[a] = '1'; else wires[a] = '0';
                        break;
                    case 'X':
                        if (v1 == '1' ^ v2 == '1') wires[a] = '1'; else wires[a] = '0';
                        break;
                }
                changed = true;
            }
        }
        if (!allZs && !changed) return (-1, true);
        undef = newUndef;
    }
    long answer = 0;
    foreach (var z in wires.Where(a => a.Key[0] == 'z').OrderByDescending(a => a.Key))
    {
        //Console.WriteLine(z.Key + ':' + z.Value);
        answer <<= 1;
        if (z.Value == '1') answer |= 1;
    }
    return (answer, false);
}