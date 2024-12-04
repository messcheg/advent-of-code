using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Security;

Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day23.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 110;
    long supposedanswer2 = 000;
    
    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    var elves = new HashSet<(long x, long y)>();
  
    int i = 0;
    while (i<S.Count)
    {
        var s = S[i];
        for (int j=0; j< s.Length; j++)
        {
            if (s[j] == '#') elves.Add((j, i));  
        }
        i++;
    }

    bool AddProposed(Dictionary<(long x, long y), List<(long x, long y)>> proposed, (long x, long y) elf, (long x, long y) target, (long x, long y) check1, (long x, long y) check2, (long x, long y) check3 )
    {
        if (!elves.Contains(check1) && !elves.Contains(check2) && !elves.Contains(check3))
        {
            List<(long x, long y)> l;
            if (!proposed.TryGetValue(target, out l))
            {
                l = new List<(long x, long y)>();
            }
            l.Add(elf);
            proposed[target] = l; 
            return true;
        }
        return false;
    }

    var proposed= new Dictionary<(long x, long y), List<(long x, long y)>>();
    int start = 0;
    bool moves = true;
    int k = 0;
    while (moves)
    {
        if (k == 10)
        {
            long minX = elves.Select(a => a.x).Min();
            long maxX = elves.Select(a => a.x).Max();
            long minY = elves.Select(a => a.y).Min();
            long maxY = elves.Select(a => a.y).Max();

            answer1 = (maxX - minX + 1) * (maxY - minY + 1) - elves.Count();
        }

        foreach (var elf in elves)
        {
           
            bool done = NoElvesArround(elves, elf);
            for (int p = start; p < start + 4; p++)
            {
                if (!done && p % 4 == 0) done = AddProposed(proposed, elf, (elf.x, elf.y - 1), (elf.x - 1, elf.y - 1), (elf.x, elf.y - 1), (elf.x + 1, elf.y - 1));
                if (!done && p % 4 == 1) done = AddProposed(proposed, elf, (elf.x, elf.y + 1), (elf.x - 1, elf.y + 1), (elf.x, elf.y + 1), (elf.x + 1, elf.y + 1));
                if (!done && p % 4 == 2) done = AddProposed(proposed, elf, (elf.x - 1, elf.y), (elf.x - 1, elf.y + 1), (elf.x - 1, elf.y), (elf.x - 1, elf.y - 1));
                if (!done && p % 4 == 3) done = AddProposed(proposed, elf, (elf.x + 1, elf.y), (elf.x + 1, elf.y + 1), (elf.x + 1, elf.y), (elf.x + 1, elf.y - 1));
            }
        }

        moves = false;
        foreach(var target in proposed.Keys)
        {
            var l = proposed[target];
            if (l.Count == 1) 
            {
                moves = true;
                var elf = l[0];
                elves.Remove(elf);
                elves.Add(target);
            }
        }
        proposed = new Dictionary<(long x, long y), List<(long x, long y)>>();
        start++; if (start > 3) start = 0;
        k++;
    }
    answer2 = k;

    stopwatch.Stop();
    Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
    Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);
}

bool NoElvesArround(HashSet<(long x, long y)> elves, (long x, long y) elf)
{
    bool result = true;
    for (int x = -1; x <= 1; x++)
        for (int y = -1; y <= 1; y++)
            result = result && (x == 0 && y == 0 || !elves.Contains((elf.x + x, elf.y + y)));
    return result;
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
