Run();

void Run()
{
    //string inputfile = @"..\..\..\example_input.txt";
    string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 590784;
    long supposedanswer2 = 2758514936282235;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var instructions = new (bool turnOn, (long from, long to) x, (long from, long to) y, (long from, long to) z)[S.Count];
    int i = 0;
    foreach (string s in S)
    {
        var s1 = s.Split(' ');
        instructions[i].turnOn = s1[0] == "on";
        s1 = s1[1].Split(',');
        var s2 = s1[0].Split('=')[1].Split("..");
        instructions[i].x.from = long.Parse(s2[0]);
        instructions[i].x.to = long.Parse(s2[1]);
        s2 = s1[1].Split('=')[1].Split("..");
        instructions[i].y.from = long.Parse(s2[0]);
        instructions[i].y.to = long.Parse(s2[1]);
        s2 = s1[2].Split('=')[1].Split("..");
        instructions[i].z.from = long.Parse(s2[0]);
        instructions[i].z.to = long.Parse(s2[1]);
        i++;
    }

    var blocks = new List<(bool sign, long[] from, long[] to)>();
    foreach (var instr in instructions)
    {
        int cnt = blocks.Count;
        var from = new long[] { instr.x.from, instr.y.from, instr.z.from };
        var to = new long[] { instr.x.to, instr.y.to, instr.z.to };
        if (instr.turnOn) blocks.Add((instr.turnOn, from ,to));

        for (int j = 0; j< cnt; j++)
        {
            var b = blocks[j];
            var res = Overlap((from, to), (b.from, b.to));
            if (res.to != null) blocks.Add((!b.sign, res.from, res.to));
        }
    }

    answer2 = 0;
    foreach(var b in blocks)
    {
        long x = b.to[0] - b.from[0] + 1;
        long y = b.to[1] - b.from[1] + 1;
        long z = b.to[2] - b.from[2] + 1;
        long count = x * y * z;
        answer2 = b.sign ? answer2 + count : answer2 - count;

        //Console.WriteLine((b.sign ? "Plus:  (" : "Minus: (" )
        //    + b.from[0] + ", " + b.from[1] + ", " + b.from[2] + "), ("
        //    + b.to[0] + ", " + b.to[1] + ", " + b.to[2] + ")");
    }

    w(1, answer1, supposedanswer1);
    w(2, answer2, supposedanswer2);
}

(long[] from, long[] to) Overlap((long[] from, long[] to) a, (long[] from, long[] to) b)
{
    var from = a.from.ToArray();
    var to = a.to.ToArray(); ;
    for (int i = 0; i < 3; i++)
        {
            if (from[i] > b.to[i]) return (null, null); //no overlap
            else if (to[i] < b.from[i]) return (null, null); //no overlap
            if (from[i] < b.from[i]) from[i] = b.from[i];
            if (to[i] > b.to[i]) to[i] = b.to[i] ;
        }
    return (from, to);  
}

static void w<T>(int number, T val, T supposedval)
{
    string? v = (val == null) ? "(null)" : val.ToString();
    string? sv = (supposedval == null) ? "(null)" : supposedval.ToString();

    var previouscolour = Console.ForegroundColor;
    Console.Write("Answer Part " + number + ": ");
    Console.ForegroundColor = (v == sv) ? ConsoleColor.Green : ConsoleColor.White;
    Console.Write(v);
    Console.ForegroundColor = previouscolour;
    Console.Write(" ... supposed (example) answer: ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(sv);
    Console.ForegroundColor = previouscolour;
}
