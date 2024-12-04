Run();

void Run()
{
    //string inputfile = @"..\..\..\example_input.txt";
    string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 12;
    long supposedanswer2 = 0000;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = -1;
    var S1 = S[0].Split(", ");

    int direction = 0;
    int blockN = 0;
    int blockE = 0;
    HashSet<(int N, int E)> visited = new HashSet<(int N, int E)> ();
   
    for (int i = 0; i < S1.Length; i++)
    {
        var prevN = blockN;
        var prevE = blockE;

        char turn = S1[i][0];
        int steps = int.Parse(S1[i][1..]);
        int turneffect = (turn == 'L') ? -1 : 1;
        steps = turneffect * steps;
        
        if (direction == 0 || direction == 3) blockE += steps;
        else if (direction == 1) blockN -= steps;
        else if (direction == 2) blockE -= steps;
        else if (direction == 3) blockN += steps;
    
        direction += turneffect;
        
        if (direction > 3) direction = 0;
        else if (direction < 0) direction = 3;
         

        var ndir = prevN <= blockN ? 1 : -1;
        var edir = prevE <= blockE ? 1 : -1;

        bool first = true;
        for (int n = prevN; n != blockN + ndir; n += ndir)
            for (int e = prevE; e != blockE + edir; e += edir)
            {
                if (!first)
                {
                    if (answer2 == -1 && visited.Contains((n, e)))
                    {
                        answer2 = Math.Abs(n) + Math.Abs(e);
                    }
                    visited.Add((n, e));
                }
                first = false;
            }
    }

    answer1 = Math.Abs(blockN) + Math.Abs(blockE);
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
