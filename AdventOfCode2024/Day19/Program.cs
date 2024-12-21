using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\example1.txt", false);
//Run(@"E:\develop\advent-of-code-input\2024\day19.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 6;
    ulong supposedanswer2 = 16;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    ulong answer2 = 0;

    var designs = S[0].Split(", ").ToHashSet();
    var designs1 = S[0].Split(", ").ToHashSet();

    var impossible = new HashSet<string>();
    var precalc = new Dictionary<string, ulong>();
    foreach (var s in S[2..])
    {
        if (IsPossible(s, designs, impossible))
        {
            answer1++;
            ulong cnt = CountPossible(s, designs1, designs, impossible, precalc);
            answer2 += cnt;
        }
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

bool IsPossible(string pattern, HashSet<string> designs, HashSet<string> impossible)
{
    if (designs.Contains(pattern)) return true;
    int i = 0;
    if (pattern.Length == 1) return designs.Contains(pattern);
    if (impossible.Contains(pattern)) return false;
    bool possible = false;
    int p = pattern.Length;
    while ((i < p - 1))
    {
        var p1 = pattern.Substring(0, i + 1);
        var p2 = pattern.Substring(i + 1);
        if (IsPossible(p1, designs, impossible) && IsPossible(p2, designs, impossible)) { possible = true; break; }
        i++;
    }
    if (possible) designs.Add(pattern);
    else impossible.Add(pattern);

    return possible;
}

ulong CountPossible(string pattern, HashSet<string> designs, HashSet<string> possible, HashSet<string> impossible, Dictionary<string, ulong> precalc)
{
    if (pattern.Length == 0) return 1;
    if (impossible.Contains(pattern)) return 0;
    if (precalc.TryGetValue(pattern, out ulong pre)) return pre;
    int i = 1;
    ulong count = 0;
    int p = pattern.Length;
    while ((i <= p))
    {
        var p1 = pattern.Substring(0, i);
        if (designs.Contains(p1))
        {
            var p2 = pattern.Substring(i);
            if (p2.Length == 0) count += 1;
            else if (IsPossible(p2, possible, impossible))
            {
                count += CountPossible(p2, designs, possible, impossible, precalc);
            }
        }
        i++;
    }
    precalc[pattern] = count;
    return count;
}
