using System.Runtime.InteropServices;

Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day08.txt", false);

void Run(string inputfile, bool isTest)
{ 
    long supposedanswer1 = 21;
    long supposedanswer2 = 8;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    for (int i = 0; i < S.Count; i++)
    {
        for (int j = 0; j < S[i].Length; j++)
        {
            int sc1 = 0;
            bool v1 = true;
            for (int k = i - 1; k >= 0; k--)
            {
                if (v1) sc1 = i - k;
                if (S[k][j] >= S[i][j]) v1 = false;
            }
            bool v2 = true;
            int sc2 = 0;
            for (int k = i + 1; k < S.Count(); k++)
            {
                if (v2) sc2 = k - i;
                if (S[k][j] >= S[i][j]) v2 = false;
            }
            bool v3 = true;
            int sc3 = 0;
            for (int k = j - 1; k >= 0; k--)
            {
                if (v3) sc3 = j - k;
                if (S[i][k] >= S[i][j]) v3 = false;
            }
            bool v4 = true;
            int sc4 = 0;
            for (int k = j + 1; k < S[i].Length; k++)
            {
                if (v4) sc4 = k - j;
                if (S[i][k] >= S[i][j]) v4 = false;
            }
            if (v1 || v2 || v3 || v4) answer1++;

            var sc = sc1 * sc2 * sc3 * sc4;
            if (sc > answer2) answer2 = sc;
        }
    }

    w(1, answer1, supposedanswer1, isTest);
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
