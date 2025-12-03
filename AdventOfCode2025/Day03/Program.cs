using AocHelper;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\input.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    BigInteger supposedanswer1 = 357;
    BigInteger supposedanswer2 = 3121910778619;

    var S = File.ReadAllLines(inputfile).ToList();
    var A = new List<long>();
    var B = new List<long>();
    BigInteger answer1 = 0;
    BigInteger answer2 = 0;

    answer1 = 0;
    answer2 = 0;

    foreach (var s in S)
    {
        BigInteger GetResult(int size)
        {
            var result = "";
            int next = 0;
            for (int i = size - 1; i >= 0; i--)
            {
                int y = next + 1;
                int found = next;
                while (y < S[0].Length - i)
                {
                    if (s[y] > s[found]) { found = y; }
                    y++;
                }
                next = found + 1;
                result += s[found];
            }
            return BigInteger.Parse(result);
        }
        answer1 += GetResult(2);
        answer2 += GetResult(12);
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}