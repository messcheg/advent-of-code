using System.Diagnostics;
using System.Net.Security;

Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day21.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 152;
    long supposedanswer2 = 301;
    
    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var sums = new Dictionary<string, string>();
    int i = 0;
    while (i<S.Count)
    {
        var s = S[i];
        var sum = s.Split(": ");
        sums[sum[0]] = sum[1];

        i++;
    }
    answer1 = Solve(sums, "root");
    var rootsum = sums["root"].Split(' ');
    string left = rootsum[0];
    string right = rootsum[2];

    string[] pathTOMe = FindMe("root", sums);
    if (pathTOMe[^1] == rootsum[0])
    {
        answer2 = MyAnswer(rootsum[0], pathTOMe[0..^1], sums, expect: Solve(sums, rootsum[2]));
    }
    else
    {
        answer2 = MyAnswer(rootsum[2], pathTOMe[0..^1], sums, expect: Solve(sums, rootsum[0]));
    }


    stopwatch.Stop();
    Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
    Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);
}

long MyAnswer(string v, string[] path, Dictionary<string, string> sums, long expect)
{
    var subsum = sums[v];
    var ss = subsum.Split(' ');

    if (ss[0] == "humn")
    {
        var sum = Solve(sums, ss[2]);
        if (ss[1] == "*") return expect / sum; 
        if (ss[1] == "+") return expect - sum; 
        if (ss[1] == "-") return expect + sum; 
        if (ss[1] == "/") return expect * sum; 
    }
    if (ss[2] == "humn")
    {
        var sum = Solve(sums, ss[0]);
        if (ss[1] == "*") return expect / sum; 
        if (ss[1] == "+") return expect - sum; 
        if (ss[1] == "-") return sum - expect;
        if (ss[1] == "/") return sum / expect; 
    }
    if (ss[0] == path[^1])
    {
        var sum = Solve(sums, ss[2]);
        if (ss[1] == "*") return MyAnswer(ss[0], path[0..^1], sums, expect / sum);
        if (ss[1] == "+") return MyAnswer(ss[0], path[0..^1], sums, expect - sum);
        if (ss[1] == "-") return MyAnswer(ss[0], path[0..^1], sums, expect + sum);
        if (ss[1] == "/") return MyAnswer(ss[0], path[0..^1], sums, expect * sum);
    }
    if (ss[2] == path[^1])
    {
        var sum = Solve(sums, ss[0]);
        if (ss[1] == "*") return MyAnswer(ss[2], path[0..^1], sums, expect / sum);
        if (ss[1] == "+") return MyAnswer(ss[2], path[0..^1], sums, expect - sum);
        if (ss[1] == "-") return MyAnswer(ss[2], path[0..^1], sums, sum - expect);
        if (ss[1] == "/") return MyAnswer(ss[2], path[0..^1], sums, sum / expect);
    }

    return 0;
}

string[] FindMe(string v, Dictionary<string, string> sums)
{
    var subsum = sums[v];
    var ss = subsum.Split(' ');

    if (ss.Length == 1) return new string[] { };
    if (ss[0] == "humn") return new string[] { "humn" };
    if (ss[1] == "humn") return new string[] { "humn" };
    var fm1 = FindMe(ss[0], sums);
    var fm2 = FindMe(ss[2], sums);
    if (fm1.Length > 0)
    {
        var fm = new List<string>(fm1);
        fm.Add(ss[0]);
        return fm.ToArray();
    }
    if (fm2.Length > 0)
    {
        var fm = new List<string>(fm2);
        fm.Add(ss[2]);
        return fm.ToArray();
    }
    return new string[] { };
}

long Solve(Dictionary<string, string> sums, string v)
{
    var subsum = sums[v];
    var ss = subsum.Split(' ');
    if (ss.Length == 1) return long.Parse(ss[0]);
        if (ss[1] == "*") return Solve(sums, ss[0]) * Solve(sums, ss[2]);
        else if (ss[1] == "-") return Solve(sums, ss[0]) - Solve(sums, ss[2]);
        else if (ss[1] == "+") return Solve(sums, ss[0]) + Solve(sums, ss[2]);
        else if (ss[1] == "/") return Solve(sums, ss[0]) / Solve(sums, ss[2]);
    return 0;

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
