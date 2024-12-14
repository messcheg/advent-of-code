using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example0.txt", false, 11, 7);
Run(@"..\..\..\example.txt", true, 11, 7);
Run(@"..\..\..\example1.txt", false, 101, 103);
//Run(@"E:\develop\advent-of-code-input\2024\day14.txt", false);

void Run(string inputfile, bool isTest, long wide, long tall)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 12;
    long supposedanswer2 = 0;


    var space = new long[wide, tall];
    var S = File.ReadAllLines(inputfile).ToList();
    var robots = new List<((long x, long y) p, (long vx, long vy) v)>();
    long answer1 = 0;
    long answer2 = 0;
    long times = 100;
    foreach (var s in S)
    {
        var robot = s.Split(' ').ToArray();
        var sp = robot[0].Substring(2).Split(',').Select(long.Parse).ToArray();
        var sv = robot[1].Substring(2).Split(',').Select(long.Parse).ToArray();
        robots.Add(((sp[0], sp[1]), (sv[0], sv[1])));
        var x = ((sp[0] + times * sv[0]) % wide + wide) % wide;
        var y = ((sp[1] + times * sv[1]) % tall + tall) % tall;
        space[x, y] += 1;
    }

    long split1 = wide / 2;
    long split2 = tall / 2;
    var (a, b, c, d) = (0L, 0L, 0L, 0L);
    for (var i = 0; i < wide; i++)
        for (var j = 0; j < tall; j++)
        {
            if (i < split1 && j < split2) { a += space[i, j]; }
            if (i < split1 && j > split2) { b += space[i, j]; }
            if (i > split1 && j < split2) { c += space[i, j]; }
            if (i > split1 && j > split2) { d += space[i, j]; }
        }
    answer1 = a * b * c * d;

    long max = robots.Count * wide * tall;
    for (long t = 0; t < max; t++)
    {
        var space1 = new HashSet<(int x, int y)>();
        foreach (var r in robots)
        {
            var x = ((r.p.x + t * r.v.vx) % wide + wide) % wide;
            var y = ((r.p.y + t * r.v.vy) % tall + tall) % tall;
            if (space1.Contains(((int)x, (int)y))) break;
            space1.Add(((int)x, (int)y));
        }
        if (space1.Count == robots.Count)
        {
            for (int i = 0; i < wide; i++)
            {
                for (int j = 0; j < tall; j++)
                {
                    if (space1.Contains((i, j))) Console.Write('*'); else Console.Write('.');
                }
                Console.WriteLine();
            }
            answer2 = t;
            break;
        }
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

