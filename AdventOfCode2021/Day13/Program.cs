Run();

void Run()
{
    //string inputfile = @"..\..\..\example_input.txt";
    string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 17;
    long supposedanswer2 = 0000;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    IList<(int x,int y)> dots = new List<(int x, int y)>();
    int i = 0;
    foreach(string s in S)
    {
        if (s.Trim() == "") break;
        var s1 = s.Split(',');
        dots.Add((int.Parse(s1[0]), int.Parse(s1[1])));
        i++;
    }
    bool first = true;
    while (i < S.Count)
    {
        var newdots = new HashSet<(int x, int y)>(dots.Count);
        var s1 = S[i].Split(' ');
        if (s1.Length == 3)
        {
            var s2 = s1[2].Split('=');
            int fold = int.Parse(s2[1]);
            if (s2[0] == "x")
            {
                foreach ((int x, int y) in dots) newdots.Add((Fold(x,fold), y));
            }
            else
            {
                foreach ((int x, int y) in dots) newdots.Add((x, Fold(y, fold)));
            }
            dots = newdots.ToArray();
            if (first) answer1 = dots.Count;
            first = false;
        }
        i++;
    }
    
    Console.Clear();
    foreach((int x, int y) in dots)
    {
        Console.CursorLeft = x;
        Console.CursorTop = y;
        Console.Write("#");
    }
    Console.CursorTop = dots.Max(D => D.y) + 1;
    Console.CursorLeft = 0;

    w(1, answer1, supposedanswer1);
    w(2, answer2, supposedanswer2);
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

static int Fold(int number, int fold) { return number > fold ? fold - (number - fold) : number; };