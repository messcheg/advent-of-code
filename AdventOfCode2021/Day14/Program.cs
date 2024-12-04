//Run();
RunShortAndFast();

void Run()
{
    //string inputfile = @"..\..\..\example_input.txt";
    string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 1588;
    long supposedanswer2 = 2188189693529;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    string poly= S[0];
    
    var rules = new Dictionary<(char left, char right), string>();
    for (int i = 2; i < S.Count; i++)
    {
        var s1 = S[i].Split(" -> ");
        rules.Add((s1[0][0],s1[0][1]), s1[1]);
    }

    for (int i = 0; i < 10; i++)
    {
        string poly1 = "" + poly[0];
        char left = poly[0];
        for (int j = 1; j < poly.Length; j++)
        {
            char right = poly[j];
            poly1 += rules[(left, right)] + right;
            left = right;
        }
        poly = poly1;
    }

    var summary = new Dictionary<char, long>();
    foreach (char c in poly)
    {
        long a = 0;
        if (!summary.TryGetValue(c, out a))
        {
            summary.Add(c, 1);
        }
        else
            summary[c] = a + 1;
    }

    long most1 = summary.Values.Max();
    long least1 = summary.Values.Min();
    answer1 = most1 - least1;

    var cachedCalcs = new Dictionary<(char left, char right, int depth), Dictionary<char,long>>();   

    poly = S[0];
    var counters2 = new Dictionary<char, long>();
    for (int j = 0; j < poly.Length-1; j++)
    {
        long a;
        if (counters2.TryGetValue(poly[j], out a))
            counters2[poly[j]] = a + 1;
        else
            counters2.Add(poly[j], 1);
        CountBetween(poly[j], poly[j+1], rules, counters2, 40, cachedCalcs);
    }
    long b;
    if (counters2.TryGetValue(poly[^1], out b))
        counters2[poly[^1]] = b + 1;
    else
        counters2.Add(poly[^1], 1);

    long most2 = counters2.Values.Max();
    long least2 = counters2.Values.Min();
    answer2 = most2 - least2;

    w(1, answer1, supposedanswer1);
    w(2, answer2, supposedanswer2);
}

void RunShortAndFast()
{
    string inputfile = @"..\..\..\example_input.txt";
    //string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 1588;
    long supposedanswer2 = 2188189693529;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    string poly = S[0];
    var rulesandcount = new Dictionary<(char left, char right),( char insert, long[] count)>();
    for (int i = 2; i < S.Count; i++)
    {
        var s1 = S[i].Split(" -> ");
        rulesandcount.Add((s1[0][0], s1[0][1]), (s1[1][0], new long[] { 0, 0 }));
    }
    for (int i = 1;i < poly.Length; i++) rulesandcount[(poly[i-1], poly[i])].count[0]++;

    int c0 = 0;
    
    for (int i = 0; i < 40; i++)
    {
        if (i == 10)
        {
            var values = rulesandcount.Select(C => new { n = C.Key.left, c = C.Value.count[c0] }).Append(new { n = poly[^1], c = 1L }).GroupBy(B => B.n, B => B.c, (n, c) => new { name = n, count = c.Sum() }).ToList();
            answer1 = values.Max(a => a.count) - values.Min(a => a.count);
        }
        int c1 = 1 - c0;
        foreach(var rule in rulesandcount)
        {
            if (rule.Value.count[c0] > 0)
            {
                rulesandcount[(rule.Key.left, rule.Value.insert)].count[c1] += rule.Value.count[c0];
                rulesandcount[(rule.Value.insert, rule.Key.right)].count[c1] += rule.Value.count[c0];
                rule.Value.count[c0] = 0;
            }
        }
        c0 = c1;
    }
    var values2 = rulesandcount.Select(C => new { n = C.Key.left, c = C.Value.count[c0] }).Append(new { n = poly[^1], c = 1L }).GroupBy(B => B.n, B => B.c, (n, c) => new { name = n, count = c.Sum() }).ToList();
    answer2 = values2.Max(a => a.count) - values2.Min(a => a.count);

    w(1, answer1, supposedanswer1);
    w(2, answer2, supposedanswer2);
}

void CountBetween(char left, char right, Dictionary<(char left, char right), string> rules, Dictionary<char, long> counters2, int depth, 
    Dictionary<(char left, char right, int depth), Dictionary<char, long>> cached )
{
    if (depth == 0) return;
    if (!cached.TryGetValue((left, right, depth), out Dictionary<char, long> cachedCalc))
    {
        cachedCalc = new Dictionary<char, long>();
        cached.Add((left, right, depth), cachedCalc);

        char insert = rules[(left, right)][0];
        CountBetween(left, insert, rules, cachedCalc, depth - 1, cached);
        long a;
        if (cachedCalc.TryGetValue(insert, out a))
            cachedCalc[insert] = a + 1;
        else
            cachedCalc.Add(insert, 1);
        CountBetween(insert, right, rules, cachedCalc, depth - 1, cached);
    }
    foreach (var cachedCounter in cachedCalc)
    {
        if(counters2.TryGetValue(cachedCounter.Key, out long count))
        {
            counters2[cachedCounter.Key] = count + cachedCounter.Value;
        }
        else
        {
            counters2.Add(cachedCounter.Key, cachedCounter.Value);
        }
    }
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
