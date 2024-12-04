using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;

Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day12.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    long supposedanswer1 = 31;
    long supposedanswer2 = 29;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    int X = S.Count;
    int Y = S[0].Length;

    var nodes = new Node[X,Y];
    var start = (0, 0);
    var end = (0, 0);
    for (int i = 0; i < X; i++)
    {
        for (int j = 0; j < Y; j++)
        {
            char c = S[i][j];
            if (c == 'S')
            {
                start = (i, j);
                c = 'a';
            }
            else if (c == 'E')
            {
                end = (i, j);
                c = 'z';
            }
            nodes[i, j] = new Node(c, i ,j);
        }
    }
    nodes[end.Item1, end.Item2].canuse = true;
    nodes[end.Item1, end.Item2].distance = 0;
    bool ready = false;
    while (!ready)
    {
        Node best = null;
        foreach (var n in nodes)
        {
            if (n.canuse && !n.visited)
            {
                if (best == null || best.distance > n.distance) best = n;
            }
        }
        if (best == null) break;
        best.visited = true;
        
        (int x, int y) = best.location;
       
        if (x > 0) checkNode(best, nodes[x - 1, y]);
        if (x < X-1) checkNode(best, nodes[x + 1, y]);
        if (y > 0) checkNode(best, nodes[x , y - 1]);
        if (y < Y - 1) checkNode(best, nodes[x, y + 1]);
    }
    answer1 = nodes[start.Item1, start.Item2].distance;

    var lst = new List<Node>(X * Y);
    foreach (var n in nodes) if (n.val == 'a') lst.Add(n);
    answer2 = lst.Min(n => n.distance);

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

static void checkNode(Node best, Node test)
{
    if (!test.visited)
    {
        int d =  best.val - test.val;
        if ( d <= 1)
        {
            if (best.distance + 1 < test.distance)
            {
                test.distance = best.distance + 1;
                test.canuse = true;
                test.prev = best;
            }
        }
    }
}
class Node
{
    public Node(char c, int x, int y)
    {
        val = c;
        location = (x, y);
    }
    public char val;
    public int distance = int.MaxValue;
    public Node prev;
    public bool visited;
    public bool canuse;
    public (int, int) location;
}
