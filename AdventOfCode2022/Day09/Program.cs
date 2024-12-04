using System.Globalization;
using System.Security.Cryptography.X509Certificates;

Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day09.txt", false);

void Run(string inputfile, bool isTest)
{ 
    long supposedanswer1 = 13;
    long supposedanswer2 = 0000;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    (int x, int y)[] rope1 = { (0, 0), (0, 0) };
    var visited = new HashSet<(int x, int y)>();
    visited.Add(rope1[0]);

    var rope2 = new (int x, int y)[10];
    var visited2 = new HashSet<(int x, int y)>();
    visited.Add(rope2[0]);

    for (int i = 0; i < S.Count; i++)
    {
        var cmd = S[i].Split(' ');
        var dir = cmd[0];
        int step = int.Parse(cmd[1]);
        for (int j = 0; j < step; j++)
        {
            Move(rope1, dir);
            visited.Add(rope1[1]);
            Move(rope2, dir);
            visited2.Add(rope2[9]);
        }
    }

    answer1 = visited.Count();
    answer2 = visited2.Count();
    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);
}

static void Move ((int x, int y)[] rope, string dir)
{
    (int x, int y) move = (0,0);

    if (dir == "U") move = (0, 1);
    else if (dir == "D") move = (0, -1);
    else if (dir == "R") move = (1, 0);
    else if (dir == "L") move = (-1, 0);
    rope[0].x += move.x;
    rope[0].y += move.y;

    for (int i = 0; i < rope.Length - 1;  i++)
    {
        (int x, int y) dif; 
        dif.x = rope[i].x - rope[i+1].x;
        dif.y = rope[i].y - rope[i+1].y;
        
        (int x, int y) newmove = (0,0);

        if (dif == (-2, 1) || dif == (-2, 2) || dif == (-1, 2)) newmove = (-1, 1);
        else if (dif == (-2, 0)) newmove = (-1, 0);
        else if (dif == (-2, -1) || dif == (-2, -2) || dif == (-1, -2)) newmove = (-1, -1);
        else if (dif == (0, -2)) newmove = (0, -1);
        else if (dif == (2, -1) || dif == (2, -2) || dif == (1, -2)) newmove = (1, -1);
        else if (dif == (2, 0)) newmove = (1, 0);
        else if (dif == (2, 1) || dif == (2, 2) || dif == (1, 2)) newmove = (1, 1);
        else if (dif == (0, 2)) newmove = (0, 1);

        rope[i+1].x += newmove.x;
        rope[i+1].y += newmove.y;

    }
    return;
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
