Run();

void Run()
{
    //string inputfile = @"..\..\..\example_input.txt";
    string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 58;
    long supposedanswer2 = 0000;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var Seabottom = new char[2, S.Count, S[0].Length];
    for (int i = 0; i < S.Count; i++)
        for (int j = 0; j < S[0].Length; j++)
        {
            Seabottom[0, i, j] = S[i][j];
        }

    //WriteSeaBottom(0, S.Count, S[0].Length, Seabottom);

    long moves = 0;
    bool hasMoved = true;
    while (hasMoved)
    {
        hasMoved = false;
        // move east
        for (int i = 0; i < S.Count; i++)
            for (int j = 0; j < S[0].Length; j++)
            {
                if (Seabottom[0, i, j] == '>')
                {
                    int k = (j == S[0].Length - 1) ? 0 : j + 1;
                    if (Seabottom[0, i, k] == '.')
                    {
                        Seabottom[1, i, j] = '.';
                        hasMoved = true;
                    }
                    else Seabottom[1, i, j] = '>';
                }
                else if (Seabottom[0, i, j] == '.')
                {
                    int k = (j == 0) ? S[0].Length - 1 : j - 1;
                    if (Seabottom[0, i, k] == '>')
                    {
                        Seabottom[1, i, j] = '>';
                    }
                    else Seabottom[1, i, j] = '.';
                }
                else Seabottom[1, i, j] = Seabottom[0, i, j];
            }
       // WriteSeaBottom(1, S.Count, S[0].Length, Seabottom);
        
        for (int i = 0; i < S.Count; i++)
            for (int j = 0; j < S[0].Length; j++)
            {
                if (Seabottom[1, i, j] == 'v')
                {
                    int k = (i == S.Count - 1) ? 0 : i + 1;
                    if (Seabottom[1, k, j] == '.')
                    {
                        Seabottom[0, i, j] = '.';
                        hasMoved = true;
                    }
                    else Seabottom[0, i, j] = 'v';
                }
                else if (Seabottom[1, i, j] == '.')
                {
                    int k = (i == 0) ? S.Count - 1 : i - 1;
                    if (Seabottom[1, k, j] == 'v')
                    {
                        Seabottom[0, i, j] = 'v';
                        hasMoved = true;
                    }
                    else Seabottom[0, i, j] = '.';
                }
                else
                    Seabottom[0, i, j] = Seabottom[1, i, j];
            }
        // WriteSeaBottom(0, S.Count, S[0].Length, Seabottom);

        moves++;
        Console.WriteLine(moves);
    }
    answer1 = moves;
    w(1, answer1, supposedanswer1);
    w(2, answer2, supposedanswer2);
}

void WriteSeaBottom(int idx,int cnt1 ,int cnt2, char[,,] Seabottom)
{
    Console.Clear();
    for (int i = 0; i < cnt1; i++)
    {
        for (int j = 0; j < cnt2; j++)
        {
            Console.Write(Seabottom[idx, i, j]);
        }
        Console.WriteLine();
    }
    Console.WriteLine("----------------------");

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
