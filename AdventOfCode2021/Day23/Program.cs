using System.Collections.Generic;

Run();

void Run()
{
    long supposedanswer1 = 12521;
    long supposedanswer2 = 44169;
    string start1 = "bacdbcda"; //example
    //string start1 = "ddccabba"; //real
    //string start2 = "bddaccbdbbacdaca"; //example
    string start2 = "ddddccbcababbaca"; //real

    long answer1 = 0;
    long answer2 = 0;

    
    var a1 = CalculateCheapest(start1);
    answer1 = a1.costs;
    //foreach (var s in a1.history) Console.WriteLine(s);
    Console.WriteLine("ready, press any key to continue");
    Console.ReadKey();
    WriteFinal(a1.history);

    var a2 = CalculateCheapest(start2);
    answer2 = a2.costs;
    //foreach (var s in a2.history) Console.WriteLine(s);
    Console.WriteLine("ready, press any key to continue");
    Console.ReadKey();
    WriteFinal(a2.history);


    w(1, answer1, supposedanswer1);
    w(2, answer2, supposedanswer2);

}

void WriteFinal(string[] finalresult)
{
    Console.Clear();
    Console.WriteLine("#############");
    Console.WriteLine("#           #");
    Console.WriteLine("### # # # ###");
    var s1 = finalresult[0].Split("|");
    int podsize = s1[0].Length/4;
    for(int i=1; i<podsize; i++)
        Console.WriteLine("  # # # # #  ");
    Console.WriteLine("  #########  ");
    string previous = finalresult[0].Split('|')[0];
    var oldcolor = Console.ForegroundColor;
    foreach (string s in finalresult)
    {
        Console.CursorLeft = 1;
        Console.CursorTop = 1;
        s1 = s.Split("|");
        Console.Write(s1[1].Split(" ")[0]);
        for (int i= 0; i < 4; i++)
            for (int j=0; j<podsize; j++)
            {
                Console.CursorLeft = 3 + i * 2;
                Console.CursorTop = 2 + j;
                var c = s1[0][i * podsize + j];
                if (c == '.') Console.ForegroundColor = oldcolor;
                else if (c != previous[i * podsize + j]) Console.ForegroundColor = ConsoleColor.Green;
                else Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(c);
                Console.ForegroundColor = oldcolor;
            }
        Thread.Sleep(500);
        previous = s1[0];
    }
    Console.ForegroundColor = oldcolor;
}

(long costs, string stand, string[] history) CalculateCheapest(string input)
{
    var podsize = input.Length / 4;
    string defaulthallway = "|...........";
    var start = input + defaulthallway;
    var end = new string(input.OrderBy(c => c).ToArray()) + defaulthallway;

    var hit = new HashSet<string>();
    var evaluate = DeterminePossibleMoves(start, podsize);
    var nextbest = new List<(long costs, string stand, string[] history)>();
    foreach (var move in evaluate)
    {
        nextbest.Add((move.costs, move.stand, new string[] { start, move.stand + " (" + move.costs + ")" }));
    }
    nextbest = nextbest.OrderByDescending(c => c.costs).ToList();

    while (nextbest[^1].stand != end)
    {
        var next = nextbest[^1];
        nextbest.RemoveAt(nextbest.Count - 1);
        if (!hit.Contains(next.stand))
        {
            evaluate = DeterminePossibleMoves(next.stand, podsize);
            foreach (var move in evaluate)
            {
                if (!hit.Contains(move.stand))
                    nextbest.Add((next.costs + move.costs, move.stand,
                        next.history.Append(move.stand + " (" + move.costs + ")").ToArray()));
            }
            nextbest = nextbest.OrderByDescending(c => c.costs).ToList();
        }
        hit.Add(next.stand);
    }
    return nextbest[^1];
}

(int costs, string stand)[] DeterminePossibleMoves(string stand, int podSize)
{
    var ports = new int[] { podSize * 4 + 3, podSize * 4 + 5, podSize * 4 + 7, podSize * 4 + 9 };
    var result = new List<(int costs, string stand)>();
    // search for candidates to get out of the pods
    List<int> candidates = new List<int>();
    for (int i = 0; i < podSize * 4;i += podSize)
    {
        int k = i; 
        bool clear = true;
        while (k < i+podSize && clear)
        {
            if (stand[k] != '.')
            {
                bool change = false;
                for (int l = k; l < i + podSize; l++)
                    if (Home(stand[l],podSize) != i) change = true;
                if (change) candidates.Add(k);
                clear = false;
            }
            k++;
        }
    }
    //Determine posibilities to go to
    foreach (var c in candidates)
    {
        int mountingfrom = 2 * (c / podSize) + 3 + 4 * podSize;
        //check if the homepod is free to go to.
        int home = Home(stand[c],podSize);
        bool isfree = true;
        int k = home;
        int c1 = k;
        while (k < home + podSize && isfree)
        {
            if (stand[k] == '.') c1 = k;
            else
                for (int l = k; l < home + podSize; l++)
                    if (Home(stand[l], podSize) != home) isfree = false;
            k++;
        }
        if(isfree)
        {
            int mountinto = 2 * (c1/podSize) + 3 + 4 * podSize;
            if (PathIsClear(mountinto, mountingfrom, stand))
            {
                int pathlength = Math.Abs(mountingfrom - mountinto) + 2 + c % podSize + c1 % podSize;
                int costs = pathlength * CostsOf(stand[c]);
                result.Add((costs, ApplyMove(stand, c, c1)));
            }
        }

        for (int i = podSize * 4 + 1; i < stand.Length;i++)
        {
            if (stand[i] == '.' && !ports.Contains(i))
            {
                if (PathIsClear(i,mountingfrom, stand))
                {
                    int pathlength = Math.Abs(mountingfrom - i) + 1 + c % podSize ;
                    int costs = pathlength * CostsOf(stand[c]);
                    result.Add((costs, ApplyMove(stand,c,i)));
                }
            }
        }
    }
    // Get candidates in hallway to move into pods
    for (int i = podSize * 4 + 1; i < stand.Length; i++)
    {
        if (stand[i] != '.')
        {
            int home = Home(stand[i], podSize);
            bool isfree = true;
            int k = home;
            int c = home;
            while (k < home + podSize && isfree)
            {
                if (stand[k] == '.') c = k;
                else
                    for (int l = k; l < home + podSize; l++)
                        if (Home(stand[l],podSize) != home) isfree = false;
                k++;
            }
            if (isfree)
            {

                int mountingpoint = Mountingpoint(stand[i], podSize);
                if (PathIsClear(i, mountingpoint, stand))
                {
                    int pathlength = Math.Abs(mountingpoint - i) + 1 + c % podSize;
                    int costs = pathlength * CostsOf(stand[i]);
                    result.Add((costs, ApplyMove(stand, c, i)));
                }
            }
        };
    }
    return result.ToArray();
}

string ApplyMove(string stand, int c, int c1)
{
    var r = stand.ToCharArray();
    (r[c], r[c1]) = (r[c1], r[c]);
    return new string(r);
}

bool PathIsClear(int i, int mountingpoint, string hall)
{
    bool clear = true;
    if (mountingpoint > i)
    {
        for (int j = i+1; j < mountingpoint; j++)
        {
            if (hall[j] != '.') clear = false;
        }
    }
    else
    {
        for (int j = mountingpoint + 1; j < i; j++)
        {
            if (hall[j] != '.') clear = false;
        }
    }
    return clear;
}

int CostsOf(char v)
{
    switch (v)
    {
        case 'a': return 1;
        case 'b': return 10;
        case 'c': return 100;
        case 'd': return 1000;
        default: return 0;
    }
}

int Home(char candidate, int podsize) { return (candidate - 'a') * podsize; }
int Mountingpoint(char candidate, int podsize) { return (candidate - 'a') * 2 + 4 * podsize + 3; }

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
