using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;

Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day12.txt", false);

void Run(string inputfile, bool isTest)
{ 
    long supposedanswer1 = 31;
    long supposedanswer2 = 0000;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    int X = S.Count;
    int Y = S[0].Length;

    var visited = new HashSet<(int,int)>();
    var distance = new List<Pad>();
    var start = (0, 0);
    var end = (0, 0);
    for (int i = 0; i < S.Count; i++)
    {
        for (int j = 0; j < S[i].Length; j++)
        {
            if (S[i][j] == 'S') start = (i, j);
            else if (S[i][j] == 'E') end = (i, j);
        }
    }

    (int,int)[] p = new (int,int)[1];
    p[0] = start;
    distance.Add(new Pad(0, p));
    bool ready = false;
    while (!ready)
    {
        var best = distance[0];
        var cur = best.path[^1];
        visited.Add(cur);
        if (cur == end)
        {
            ready = true;
            answer1 = best.distance;
            break;
        }
        var x = cur.Item1;
        var y = cur.Item2;
        char curChar = S[x][y];
        if (curChar == 'S') curChar = 'a';
        if (x > 0 && !visited.Contains((x - 1, y)) && isOk(curChar, S[x - 1][y]))
        {

            p = new (int, int)[best.path.Length + 1];
            for (int i = 0; i < best.path.Length; i++) p[i] = best.path[i];
            p[^1] = (x - 1, y);
            distance.Add(new Pad(best.distance + 1, p));
        }
        if (x < X-1 && !visited.Contains((x + 1, y)) && isOk(curChar, S[x + 1][y]))
        {

            p = new (int, int)[best.path.Length + 1];
            for (int i = 0; i < best.path.Length; i++) p[i] = best.path[i];
            p[^1] = (x + 1, y);
            distance.Add(new Pad(best.distance + 1, p));
        }
        if (y > 0 && !visited.Contains((x, y - 1)) && isOk(curChar, S[x][y -1] ))
        {

            p = new (int, int)[best.path.Length + 1];
            for (int i = 0; i < best.path.Length; i++) p[i] = best.path[i];
            p[^1] = (x, y-1);
            distance.Add(new Pad(best.distance + 1,p));
        }
        if (y < Y - 1 && !visited.Contains((x, y+1)) && isOk(curChar, S[x][y+1] ))
        {

            p = new (int, int)[best.path.Length + 1];
            for (int i = 0; i < best.path.Length; i++) p[i] = best.path[i];
            p[^1] = (x, y+1);
            distance.Add(new Pad(best.distance + 1, p));
        }
        distance = distance.Skip(1).OrderBy(n => n.distance).ToList();

    }

    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);
}

int dist(char from, char to)
{
    char a = from == 'S' ? 'a' : from;
    char b = to == 'E' ? 'z' : to;
    int r = b - a;
    return r;
}
bool isOk(char from, char to)
{
    int r = dist(from,to);
    return r >= 0 && r <= 1;
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

class Pad
{
    public Pad(int d, (int,int)[] p)
    {
        distance = d;
        path = p;
    }
    public int distance = 0;
    public (int, int)[] path = new (int,int)[0]; 
}