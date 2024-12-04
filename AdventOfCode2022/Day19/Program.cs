using System.Data.SqlTypes;
using System.Diagnostics;
using System.Net.Security;

Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day19.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 33;
    long supposedanswer2 = 56;
    
    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    int i = 0;
    while (i<S.Count)
    {
        var s = S[i].Split(": ");
        var nr = int.Parse(s[0].Split(' ')[1]);
        var bp = s[1].Split(' ');
        var blueprint = new Robot[4];
        blueprint[0] = new Robot(0, (int.Parse(bp[4]), 0, 0, 0));
        blueprint[1] = new Robot(1, (int.Parse(bp[10]), 0, 0, 0));
        blueprint[2] = new Robot(2, (int.Parse(bp[16]), int.Parse(bp[19]), 0, 0));
        blueprint[3] = new Robot(3, (int.Parse(bp[25]), 0, int.Parse(bp[28]), 0));

        var work = new Queue<(int time, (int, int, int, int) harvest, (int, int, int, int) robots)>();
        var visited = new HashSet< (int time, (int, int, int, int) harvest, (int, int, int, int) robots) > ();

        AddWork(blueprint, visited, work, (24, (0, 0, 0, 0), (1, 0, 0, 0)));
        var best = work.First();
        var bastGeo = 0;
        while (work.Count > 0)
        {
            var c = work.Dequeue();
            visited.Add(c);
            //collect
            var oldc = c;
            c.harvest = Plus(c.harvest, c.robots);
            c.time--;
            if (c.time == 0)
            {
                if (c.harvest.Item4 > best.harvest.Item4) { best = c; }
            }
            else if(c.robots.Item4 >= bastGeo)
            {
                bastGeo = c.robots.Item4; 
                Build(blueprint, visited, work, c, oldc, best);
            }
        }
        Console.WriteLine($"number {nr}: max: {best.harvest.Item4} elapsedtime: {stopwatch.Elapsed.TotalSeconds} seconds.");
        answer1 += best.harvest.Item4 * nr;
        i++;
    }

    i = 0;
    answer2 = 1;
    while (i < Math.Min(S.Count, 3))
    {
        var s = S[i].Split(": ");
        var nr = int.Parse(s[0].Split(' ')[1]);
        var bp = s[1].Split(' ');
        var blueprint = new Robot[4];
        blueprint[0] = new Robot(0, (int.Parse(bp[4]), 0, 0, 0));
        blueprint[1] = new Robot(1, (int.Parse(bp[10]), 0, 0, 0));
        blueprint[2] = new Robot(2, (int.Parse(bp[16]), int.Parse(bp[19]), 0, 0));
        blueprint[3] = new Robot(3, (int.Parse(bp[25]), 0, int.Parse(bp[28]), 0));

        var work = new Queue<(int time, (int, int, int, int) harvest, (int, int, int, int) robots)>();
        var visited = new HashSet<(int time, (int, int, int, int) harvest, (int, int, int, int) robots)>();

        AddWork(blueprint, visited, work, (32, (0, 0, 0, 0), (1, 0, 0, 0)));
        var best = work.First();
        var bastGeo = 0;
        while (work.Count > 0)
        {
            var c = work.Dequeue();
            visited.Add(c);
            //collect
            var oldc = c;
            c.harvest = Plus(c.harvest, c.robots);
            c.time--;
            if (c.time == 0)
            {
                if (c.harvest.Item4 > best.harvest.Item4) { best = c; }
            }
            else if (c.robots.Item4 >= bastGeo)
            {
                bastGeo = c.robots.Item4;
                Build(blueprint, visited, work, c, oldc, best);
            }
        }
        Console.WriteLine($"number {nr}: max: {best.harvest.Item4} elapsedtime: {stopwatch.Elapsed.TotalSeconds} seconds.");
        answer2 *= best.harvest.Item4;
        i++;
    }


    stopwatch.Stop();
    Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
    Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);

    
}

static void Build(Robot[] blueprint, HashSet<(int time, (int, int, int, int) harvest, (int, int, int, int) robots)>  visited, Queue<(int time, (int, int, int, int) harvest, (int, int, int, int) robots)> work, (int time, (int, int, int, int) harvest, (int, int, int, int) robots) c, (int time, (int, int, int, int) harvest, (int, int, int, int) robots) oldc, (int time, (int, int, int, int) harvest, (int, int, int, int) robots)best)
{
    if (UseFullToGoOn(blueprint, c, best))
    {

        if (CanBuild(oldc.harvest, blueprint[3].costs))
        {
            //Buy(blueprint, work, (c.time, Min(c.harvest, blueprint[3].costs), Plus(c.robots, (0, 0, 0, 1))), (c.time, Min(oldc.harvest, blueprint[3].costs), Plus(c.robots, (0, 0, 0, 1))), best);
            AddWork(blueprint, visited, work, (c.time, Min(c.harvest, blueprint[3].costs), Plus(c.robots, (0, 0, 0, 1))));
        }
        else
        {
            if (CanBuild(oldc.harvest, blueprint[2].costs))
            {
                //Buy(blueprint, work, (c.time, Min(c.harvest, blueprint[2].costs), Plus(c.robots, (0, 0, 1, 0))), (c.time, Min(oldc.harvest, blueprint[2].costs), Plus(c.robots, (0, 0, 1, 0))), best);
                AddWork(blueprint, visited, work, (c.time, Min(c.harvest, blueprint[2].costs), Plus(c.robots, (0, 0, 1, 0))));
            }
            if (CanBuild(oldc.harvest, blueprint[1].costs))
            {
                //Buy(blueprint, work, (c.time, Min(c.harvest, blueprint[1].costs), Plus(c.robots, (0, 1, 0, 0))), (c.time, Min(oldc.harvest, blueprint[1].costs), Plus(c.robots, (0, 1, 0, 0))), best);
                AddWork(blueprint, visited, work, (c.time, Min(c.harvest, blueprint[1].costs), Plus(c.robots, (0, 1, 0, 0))));
            }
            if (CanBuild(oldc.harvest, blueprint[0].costs))
            {
                //Buy(blueprint, work, (c.time, Min(c.harvest, blueprint[0].costs), Plus(c.robots, (1, 0, 0, 0))), (c.time, Min(oldc.harvest, blueprint[0].costs), Plus(c.robots, (1, 0, 0, 0))), best); 
                AddWork(blueprint, visited, work, (c.time, Min(c.harvest, blueprint[0].costs), Plus(c.robots, (1, 0, 0, 0))));
            }
            else AddWork(blueprint, visited, work, c);
        }
    }

    
}
static void AddWork(Robot[] blueprint, HashSet<(int time, (int, int, int, int) harvest, (int, int, int, int) robots)> visited, Queue<(int time, (int, int, int, int) harvest, (int, int, int, int) robots)> work, (int time, (int, int, int, int) harvest, (int, int, int, int) robots) c)
{
    if (!visited.Contains(c))
    { 
        work.Enqueue(c);
        visited.Add(c);
    }
}
static bool CanBuild((int,int,int,int) harvest, (int,int,int,int) costs)
{
    return
       (harvest.Item1 >= costs.Item1) &&
       (harvest.Item2 >= costs.Item2) &&
       (harvest.Item3 >= costs.Item3) &&
       (harvest.Item4 >= costs.Item4);
}
 static (int, int, int, int) Plus ((int, int, int, int) a, (int, int, int, int) b)
{
    return
        (
            a.Item1 + b.Item1,
            a.Item2 + b.Item2,
            a.Item3 + b.Item3,
            a.Item4 + b.Item4
        );
}

static (int, int, int, int) Min((int, int, int, int) a, (int, int, int, int) b)
{
    return
        (
            a.Item1 - b.Item1,
            a.Item2 - b.Item2,
            a.Item3 - b.Item3,
            a.Item4 - b.Item4
        );
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


static bool UseFullToGoOn(Robot[] blueprint, (int time, (int, int, int, int) harvest, (int, int, int, int) robots) c, (int time, (int, int, int, int) harvest, (int, int, int, int) robots)  best)
{
   // if (c.time == 1 && c.robots.Item4 == 0) return false; // Geode cracker too late
   // if (c.robots.Item3 == 0 && (c.time * c.time + c.time)/2 < blueprint[3].costs.Item3) return false; // Obsidian Robot too late
   // if (c.robots.Item2 == 0 && (c.time * c.time + c.time) / 2 < blueprint[3].costs.Item3 + blueprint[2].costs.Item2) return false; // Clay robot too late;
   // if (c.harvest.Item4 + c.time * c.robots.Item4 + (c.time * c.time + c.time) / 2 < best.harvest.Item4) return false; // can't make enough crackers to win                                                                                                                          
   // if (c.robots.Item4 == 0 &&
   //     (2 * c.time + c.harvest.Item1 + c.harvest.Item2 + c.harvest.Item3 + 3) <
   //     Math.Sqrt(1 + 8 * (blueprint[1].costs.Item1 + blueprint[2].costs.Item1)) +
   //     Math.Sqrt(1 + 8 * blueprint[2].costs.Item2) +
   //     Math.Sqrt(1 + 8 * blueprint[3].costs.Item3)
   //     ) return false;
    return true;
}
static int Score((int time, (int, int, int, int) harvest, (int, int, int, int) robots) k, Robot[] blueprint)
{
    return k.harvest.Item1 +
                            k.harvest.Item2 * blueprint[1].costs.Item1 +
                            k.harvest.Item3 * (blueprint[2].costs.Item1 + blueprint[1].costs.Item1 * blueprint[2].costs.Item2) +
                            k.harvest.Item4 * (blueprint[3].costs.Item1 + blueprint[3].costs.Item3 * (blueprint[2].costs.Item1 + blueprint[1].costs.Item1 * blueprint[2].costs.Item2));
}
class Robot
{
    public Robot(int t, (int, int, int, int) c) { type = t; costs = c;; }
    public int type;
    public (int, int, int, int) costs;

}

