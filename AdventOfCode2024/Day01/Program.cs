using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\example1.txt", true);
//Run(@"E:\develop\advent-of-code-input\2024\day01.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 11;
    long supposedanswer2 = 31;

    var S = File.ReadAllLines(inputfile).ToList();
    var A = new List<long>();
    var B = new List<long>();
    long answer1 = 0;
    long answer2 = 0;
    var A1 = new Dictionary<long, long>();
    var B1 = new Dictionary<long, long>();
    foreach (var s in S)
    {
        var nums = s.Split("   ");
        long a = long.Parse(nums[0]);
        long b = long.Parse(nums[1]);
        A.Add(a);
        B.Add(b);
        long na, nb;
        if (A1.TryGetValue(a, out na)) { A1[a] = na + 1; } else { A1[a] = 1; }
        if (B1.TryGetValue(b, out nb)) { B1[b] = nb + 1; } else { B1[b] = 1; }
    }
    A.Sort();
    B.Sort();
    for (int i = 0; i < A.Count; i++)
    {
        answer1 += Math.Abs(A[i] - B[i]);
    }

    foreach (var kvp in A1)
    {
        long vb;
        if (B1.TryGetValue((long)kvp.Key, out vb)) { answer2 += B1[kvp.Key] * kvp.Value * kvp.Key; }
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

