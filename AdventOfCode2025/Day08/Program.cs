using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true, 10);
Run(@"..\..\..\input.txt", false, 1000);

void Run(string inputfile, bool isTest, int limit = 10)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 40;
    long supposedanswer2 = 25272;

    var S = File.ReadAllLines(inputfile).Select(s => s.Split(',').Select(long.Parse).ToArray()).Select(a => (X: a[0], Y: a[1], Z: a[2])).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var distances = new Dictionary<(int box1, int box2), double>();

    double CalcMinDistance(int a, int b)
    {
        return Math.Sqrt(Math.Pow(S[a].X - S[b].X, 2) + Math.Pow(S[a].Y - S[b].Y,2) + Math.Pow(S[a].Z - S[b].Z,2));
    }

    for (int i = 0; i < S.Count - 1; i++)
    {
        for (int j = i + 1; j < S.Count; j++)
        {
            distances[(i, j)] = CalcMinDistance(i, j);
        }
    }

    var circuits = new List<HashSet<int>>();

    int times = 0;
    foreach (var combi in distances.OrderBy(vp => vp.Value))
    {
        // find circuits
        int c1 = -1;
        int c2 = -1;
        var b1 = combi.Key.box1;
        var b2 = combi.Key.box2;
        for (int c = 0; c < circuits.Count; c++)
        {
            var circ = circuits[c];
            if (circ.Contains(b1)) c1 = c;
            if (circ.Contains(b2)) c2 = c;
        }
        if (c1 == -1)
        {
            if (c2 == -1)
            {
                var hs = new HashSet<int>();
                hs.Add(b1);
                hs.Add(b2);
                circuits.Add(hs);
            }
            else
            {
                circuits[c2].Add(b1);
            }
        }
        else
        {
            if (c2 == -1)
            {
                circuits[c1].Add(b2);
            }
            else if (c1 != c2)
            {
                foreach(var b in circuits[c2])
                { circuits[c1].Add(b); }
                circuits.RemoveAt(c2);
            } 
        }
        times++;
        if (times == limit)
        {
            var l1 = circuits.Select(hs => hs.Count).OrderByDescending(cnt=>cnt).ToArray();
            answer1 = l1[0] * l1[1] * l1[2];
        }
        if (circuits[0].Count == S.Count)
        {
            answer2 = S[b1].X * S[b2].X;
            break;
        }
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

