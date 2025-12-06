using AocHelper;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\input.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    BigInteger supposedanswer1 = 13;
    BigInteger supposedanswer2 = 43;

    var S = File.ReadAllLines(inputfile).Select(a=>a.ToList()).ToList();
    BigInteger answer1 = 0;
    BigInteger answer2 = 0;

    answer1 = 0;
    answer2 = 0;

    int width = S[0].Count;
    int heigth = S.Count;
    bool ready = false;
    bool first = true;
    while (!ready)
    {
        int rolls = 0;
        for (int y = 0; y < heigth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int cnt = 0;
                char cell = S[y][x];

                bool checkcell(int y1, int x1)
                {
                    return y1 >= 0 && x1 >=0 && y1 < heigth && x1 < width && (S[y1][x1] == '@' || S[y1][x1] == 'X');
                }

                if (cell == '@')
                {
                    if (checkcell(y - 1,x - 1)) cnt++;
                    if (checkcell(y - 1,x)) cnt++;
                    if (checkcell(y - 1,x + 1)) cnt++;
                    if (checkcell(y,x - 1)) cnt++;
                    if (checkcell(y,x + 1)) cnt++;
                    if (checkcell(y + 1,x - 1)) cnt++;
                    if (checkcell(y + 1,x)) cnt++;
                    if (checkcell(y + 1,x + 1)) cnt++;

                    if (cnt < 4)
                    {
                        rolls++;
                        S[y][x] = 'X';
                    }
                }
            }
        }
        for (int y = 0; y < heigth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (S[y][x] == 'X') S[y][x] = '.';
            }
        }
        if (first) answer1 = rolls;
        answer2 += rolls;
        first = false;
        ready = rolls == 0;
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}