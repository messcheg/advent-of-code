using System.Diagnostics;
using System.Net.Security;

Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day13.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 13;
    long supposedanswer2 = 140;
    
    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var packages = new List<string>();
    string p1 = "[[2]]";
    string p2 = "[[6]]";
    packages.Add(p1);
    packages.Add(p2);

    int i = 0;
    int idx = 1;
    while (i<S.Count)
    {
        var left = S[i];
        var right = S[i + 1];
        int order = cmp.compare1(new string[] { left }, new string[] { right });
        if (order == -1) answer1 += idx;
        packages.Add(left);
        packages.Add(right);
        i += 3;
        idx++;
    }

    stopwatch.Stop();
    Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
    Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
    w(1, answer1, supposedanswer1, isTest);

    stopwatch.Restart();
    packages.Sort(new cmp());
    int l1 = packages.IndexOf(p1) + 1;
    int l2 = packages.IndexOf(p2) + 1;
    answer2 = l1 * l2;

    stopwatch.Stop();
    Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
    Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
    w(2, answer2, supposedanswer2, isTest);
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

class cmp : IComparer<string>
{
    int IComparer<string>.Compare(string? x, string? y)
    {
        return compare1(new string[] { x }, new string[] { y });
        
    }
    public static int compare1(string[] left, string[] right)
    {
        for (int i = 0; i < left.Length; i++)
        {
            if (right.Length <= i) return 1;

            if (left[i].Length == 0) return right[i].Length == 0 ? 0 : -1;
            else if (right[i].Length == 0) return 1;

            if (left[i][0] == '[')
            {
                var l1 = makelist(left[i][1..^1]);
                if (right[i][0] == '[')
                {
                    var r1 = makelist(right[i][1..^1]);
                    int order = compare1(l1, r1);
                    if (order != 0) return order;
                }
                else
                {
                    var r1 = new string[] { right[i] };
                    int order = compare1(l1, r1);
                    if (order != 0) return order;
                }
            }
            else if (right[i][0] == '[')
            {
                var l1 = new string[] { left[i] };
                var r1 = makelist(right[i][1..^1]);
                int order = compare1(l1, r1);
                if (order != 0) return order;
            }
            else
            {
                int a = int.Parse(left[i]);
                int b = int.Parse(right[i]);
                if (a > b) return 1;
                else if (a < b) return -1;
            }
        }
        if (right.Length > left.Length) return -1;
        return 0;
    }

    static string[] makelist(string s)
    {
        var l = new List<string>();
        var s1 = "";
        int level = 0;
        foreach (var c in s)
        {
            if  (c == ',' && level == 0)
            {
                l.Add(s1);
                s1 = "";
                continue;
            } 
            if (c == '[') level++; 
            else if (c == ']')  level--;
            s1 += c;
        }
        l.Add(s1);
        return l.ToArray();
    }
}