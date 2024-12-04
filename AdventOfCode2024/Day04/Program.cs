using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\example1.txt", true);
//Run(@"E:\develop\advent-of-code-input\2024\day04.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 18;
    long supposedanswer2 = 0;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    for (int i = 0; i < S.Count; i++)
    {
        for (int j = 0; j < S[i].Length; j++)
        {
            if (S[i][j] == 'X')
            {
                if (j <= S[i].Length - 4 && S[i][j..(j + 4)] == "XMAS") answer1++;
                if (j >= 3 && S[i][(j - 3)..(j + 1)] == "SAMX") answer1++;
                if (i <= S.Count - 4 && S[i + 1][j] == 'M' && S[i + 2][j] == 'A' && S[i + 3][j] == 'S') answer1++;
                if (i >= 3 && S[i - 1][j] == 'M' && S[i - 2][j] == 'A' && S[i - 3][j] == 'S') answer1++;
                if (i <= S.Count - 4 && j <= S[i].Length - 4 && S[i + 1][j + 1] == 'M' && S[i + 2][j + 2] == 'A' && S[i + 3][j + 3] == 'S') answer1++;
                if (i >= 3 && j >= 3 && S[i - 1][j - 1] == 'M' && S[i - 2][j - 2] == 'A' && S[i - 3][j - 3] == 'S') answer1++;
                if (i <= S.Count - 4 && j >= 3 && S[i + 1][j - 1] == 'M' && S[i + 2][j - 2] == 'A' && S[i + 3][j - +3] == 'S') answer1++;
                if (i >= 3 && j <= S[i].Length - 4 && S[i - 1][j + 1] == 'M' && S[i - 2][j + 2] == 'A' && S[i - 3][j + 3] == 'S') answer1++;
            }
            if (S[i][j] == 'A' && j > 0 && i > 0 && j < S[i].Length - 1 && i < S.Count - 1)
            {
                bool masA = S[i - 1][j - 1] == 'M' && S[i + 1][j + 1] == 'S';
                bool masAR = S[i - 1][j - 1] == 'S' && S[i + 1][j + 1] == 'M';

                bool masB = S[i - 1][j + 1] == 'M' && S[i + 1][j - 1] == 'S';
                bool masBR = S[i - 1][j + 1] == 'S' && S[i + 1][j - 1] == 'M';

                bool xMas = ((masA || masAR) && (masB || masBR));
                if (xMas) answer2++;
            }
        }
    }


    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

