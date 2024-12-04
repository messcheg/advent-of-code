Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day06.txt", false);

void Run(string inputfile, bool isTest)
{ 
    long supposedanswer1 = 7;
    long supposedanswer2 = 19;

    var T = File.ReadAllLines(inputfile).First();
    long answer1 = 0;
    long answer2 = 0;

    int k = -1;
    for (int i = 3; i < T.Length; i++)
    {
        string s = "";
        for (int j = i - 3; j <= i; j++)
        {
            if (s.Contains(T[j])) break;
            s = s + T[j];
        }
        if (s.Length == 4)
        {
            k = i + 1;
            break;
        }
    }
    answer1 = k;
    w(1, answer1, supposedanswer1, isTest);

    k = -1;
    for (int i = 13; i < T.Length; i++)
    {
        string s = "";
        for (int j = i - 13; j <= i; j++)
        {
            if (s.Contains(T[j])) break;
                s = s + T[j];
        }
        if (s.Length == 14)
        {
            k = i + 1;
            break;
        }
    }
    answer2 = k;
    w(2, answer2, supposedanswer2, isTest);
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
