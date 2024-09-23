using System.Diagnostics;
using System.Net.Security;

Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day16.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 1651;
    long supposedanswer2 = 1707;
    
    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var valves = new Dictionary<string, Valve>();
    
    int i = 0;
    while (i<S.Count)
    {
        var s = S[i];
        var t = s.Split(' ');
        var r = int.Parse(t[4].Split('=')[1][0..^1]);
        var v = new Valve(t[1], r, t[9..].Select(a => a[0..2]).ToArray());
        valves[v.Name] = v;
        i++;
    }

    calculateshortestpath(valves);

    var cur = valves["AA"];

    // answer1 = CalculateGain(cur, 30, new string[] { }, valves);
    var useFullValves = valves.Values.Where(v => v.Release > 0).Select(v => (v.Name, v.Release)).ToArray();
    answer1 = GetReleasedPressure(30, useFullValves, "AA", valves);
    answer2 = GetReleasedPressureTogether(new int[] { 26, 26 }, useFullValves, new string[] { "AA", "AA"}, valves);

    stopwatch.Stop();
    Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
    Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);
}
void calculateshortestpath (Dictionary<string, Valve> valves)
{
    foreach (var v in valves.Values )
    {
        string target = v.Name;
        Valve cur = valves[target];
        cur.shortestpath[target] = 0;
        SpToTarget(valves, cur, target);
    }
}

void SpToTarget (Dictionary<string, Valve> valves, Valve current, string target)
{
    var visited = new HashSet<string>();

    while (current != null && visited.Count < valves.Count)
    {
        visited.Add(current.Name);
        int distance = current.shortestpath[target] + 1;
        foreach (var t in current.Tunnels)
        {
            if (!visited.Contains(t))
            {
                var c = valves[t];
                if (c.shortestpath.ContainsKey(target))
                {
                    if (distance < c.shortestpath[target]) c.shortestpath[target] = distance;
                }
                else c.shortestpath[target] = distance;
            }
        }
        current = valves.Values.Where(c => !visited.Contains(c.Name) && c.shortestpath.ContainsKey(target)).OrderBy(c => c.shortestpath[target]).FirstOrDefault();
    }
}

long GetReleasedPressure(int timeToGo, (string n, int r)[] usefull, string curV, Dictionary<string, Valve> valves)
{
    long best = 0;
    var cur = valves[curV];
    foreach (var t in usefull)
    {
        int newTimeToGo = timeToGo - cur.shortestpath[t.n] - 1;
        if (newTimeToGo > 0)
        {
            long gain = newTimeToGo * t.r + GetReleasedPressure(newTimeToGo, usefull.Where(c => c.n != t.n).ToArray(), t.n, valves);
            if (best < gain) best = gain;
        }
    }
    return best;
}

long GetReleasedPressureTogether(int[] timeToGo, (string n, int r)[] usefull, string[] curV, Dictionary<string, Valve> valves)
{
    long best = 0;
    int actor = timeToGo[0] > timeToGo[1] ? 0 : 1;

    var cur = valves[curV[actor]];
    foreach (var t in usefull)
    {
        int newTimeToGo = timeToGo[actor] - cur.shortestpath[t.n] - 1;
        if (newTimeToGo > 0)
        {
            var newTimes = new int[] { newTimeToGo, timeToGo[1 - actor] };
            var newLocs = new string[] { t.n, curV[1 - actor] };
            long gain = newTimeToGo * t.r + GetReleasedPressureTogether(newTimes, usefull.Where(c => c.n != t.n).ToArray(), newLocs, valves);
            if (best < gain) best = gain;
        }
    }
    return best;
}


static void w<T>(int number, T val, T supposedval, bool isTest)
{
    string? v = (val == null) ? "(null)" : val.ToString();
    string? sv = (supposedval == null) ? "(null)" : supposedval.ToString();

    var previouscolour = Console.ForegroundColor;
    Console.Write("Answer Part " + number + ": ");
    Console.ForegroundColor = (v == sv) ? ConsoleColor.Green : ConsoleColor.White;
    Console.Write(v);
    Console.ForegroundColor = previouscolour;
    if (isTest)
    {
        Console.Write(" ... supposed (example) answer: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(sv);
        Console.ForegroundColor = previouscolour;
    }
    else
        Console.WriteLine();
}

class Valve
{
    public Valve(string n, int r, string[] t )
    {
        Name = n;
        Release = r;
        Tunnels = t;
    }
    public string Name = "";
    public int Release = 0;
    public string[] Tunnels = new string[] { }; 

    public Dictionary<string, int> shortestpath = new Dictionary<string, int>();
}

