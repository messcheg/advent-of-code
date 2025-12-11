using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true,1);
Run(@"..\..\..\example1.txt", true, 2);
Run(@"..\..\..\input.txt", false);

void Run(string inputfile, bool isTest, int part = 3)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 5;
    long supposedanswer2 = 2;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    var tree = new Dictionary<string, (int number, string[] leaves)>();
    int ctr = 0;
    foreach (var s in S)
    {
        var s1 = s.Split(": ");
        var key = s1[0];
        var value = s1[1].Split(" ");
        tree.Add(key, (ctr, value));
        ctr++;
    }
    
    if (!isTest || part == 1 )
    { 
        var work = new Queue<(string key, long count, HashSet<string> visited)>();
        work.Enqueue(("you", 0, new HashSet<string>()));
        while (work.Count > 0)
        {
            var cur = work.Dequeue();
            if (cur.key == "out")
            {
                answer1++;
            }
            else
            {
                var newvisited = cur.visited.ToHashSet();
                newvisited.Add(cur.key);
                var newcount = cur.count + 1;
                var nextnodes = tree[cur.key].leaves;
                foreach (var n in nextnodes)
                {
                    if (!cur.visited.Contains(n)) work.Enqueue((n, newcount, newvisited));
                }
            }
        }
    }

    if (!isTest || part == 2)
    {
        var work = new Queue<(string key, long count, HashSet<string> visited)>();
        work.Enqueue(("svr", 0, new HashSet<string>()));
        while (work.Count > 0)
        {
            var cur = work.Dequeue();
            if (cur.key == "out")
            {
                if (cur.visited.Contains("fft") && cur.visited.Contains("dac")) answer2++;
            }
            else
            {
                var newvisited = cur.visited.ToHashSet();
                newvisited.Add(cur.key);
                var newcount = cur.count + 1;
                var nextnodes = tree[cur.key].leaves;
                foreach (var n in nextnodes)
                {
                    if (!cur.visited.Contains(n))
                        work.Enqueue((n, newcount, newvisited));
                }                
            }
        }
    }
    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

