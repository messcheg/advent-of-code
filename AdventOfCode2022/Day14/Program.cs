using System.Data.SqlTypes;
using System.Diagnostics;
using System.Net.Security;
using System.Runtime.CompilerServices;

Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day14.txt", false);
RunFaster(@"..\..\..\example_input.txt", true);
RunFaster(@"E:\develop\advent-of-code-input\2022\day14.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 24;
    long supposedanswer2 = 93;
    
    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    var field = new Dictionary<(int x, int y), char>();
    var maxy = 0;
    int i = 0;
    while (i<S.Count)
    {
        var s = S[i];
        var points = s.Split(" -> ");

        var pts = points[0].Split(',');
        var x0 = int.Parse(pts[0]);
        var y0 = int.Parse(pts[1]);

        for (int j= 1; j < points.Length; j++)
        {
            pts = points[j].Split(',');
            var x1  = int.Parse(pts[0]);
            var y1 = int.Parse(pts[1]);
            (var xa, var xb) = x0 > x1 ? (x1,x0) : (x0,x1);
            (var ya, var yb) = y0 > y1 ? (y1,y0) : (y0,y1);
            if (yb > maxy) { maxy = yb; }
            for (int x = xa; x <= xb; x++)
                for (int y = ya; y <= yb; y++)
                    field[(x, y)] = '#';
            (x0, y0) = (x1, y1);
        }
        i++;
    }

    bool ready = false;
    bool useOne = true;
    while (!ready)
    {
        var sx = 500;
        var sy = 0;
        bool a = !field.ContainsKey((sx, sy + 1));
        bool b = !field.ContainsKey((sx - 1, sy + 1));
        bool c = !field.ContainsKey((sx + 1, sy + 1));
        while (sy < maxy + 1 && (a || b || c))
        {
            if (!a)
            { 
                if (b) sx--; else sx++;
            }
            sy++;
            a=!field.ContainsKey((sx, sy + 1));
            b=!field.ContainsKey((sx - 1, sy + 1));
            c=!field.ContainsKey((sx + 1, sy + 1));
        }
        ready = field.ContainsKey((sx, sy));
        if (sy > maxy) useOne = false;
        if (!ready)
        {
            field[(sx, sy)] = 'o';
            answer2++;
            if (useOne) answer1++;
        }
    }


    stopwatch.Stop();
    Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
    Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);
}

void RunFaster(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 24;
    long supposedanswer2 = 93;

    var coords = File.ReadAllLines(inputfile).Select(a => a.Split(" -> ").Select(b => b.Split(',').Select(c => int.Parse(c)).ToArray()).ToArray());
    var field = new bool[1000, 1000];
    long answer1 = 0;
    long answer2 = 0;
    var maxy = 0;
    int i = 0;
    foreach(var line in coords)
    {
        var x0 = line[0][0];
        var y0 = line[0][1];

        for (int j = 1; j < line.Length; j++)
        {
            var x1 = line[j][0];
            var y1 = line[j][1];
            (var xa, var xb) = x0 > x1 ? (x1, x0) : (x0, x1);
            (var ya, var yb) = y0 > y1 ? (y1, y0) : (y0, y1);
            if (yb > maxy) { maxy = yb; }
            for (int x = xa; x <= xb; x++)
                for (int y = ya; y <= yb; y++)
                    field[x, y] = true;
            (x0, y0) = (x1, y1);
        }
        i++;
    }

    bool ready = false;
    bool useOne = true;
    while (!ready)
    {
        var sx = 500;
        var sy = 0;
        var ny = 1;
        bool a = !field[sx, ny];
        bool b = !field[sx - 1, ny];
        bool c = !field[sx + 1, ny];
        while (sy < maxy + 1 && (a || b || c))
        {
            if (!a)
            {
                if (b) sx--; else sx++;
            }
            sy++; ny++;
            a = !field[sx, ny];
            b = !field[sx - 1, ny];
            c = !field[sx + 1, ny];
        }
        ready = field[sx, sy];
        if (sy > maxy) useOne = false;
        if (!ready)
        {
            field[sx, sy] = true;
            answer2++;
            if (useOne) answer1++;
        }
    }


    stopwatch.Stop();
    Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
    Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
    w(1, answer1, supposedanswer1, isTest);
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

class FieldComparer : IComparer<string>
{
    int IComparer<string>.Compare(string? x, string? y)
    {
        if (x==null) { if (y == null) return 0; else return -1; }
        if (y == null) return 1;
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