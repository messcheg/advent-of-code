using System.Diagnostics;
using System.Globalization;
using System.Net.Security;
using System.Runtime.InteropServices;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\example1.txt", true);
Run(@"E:\develop\advent-of-code-input\2023\day01.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 281;
    long supposedanswer2 = 000;

    string[] nums = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    foreach(var cw in S)
    {
        int a = 0 - 1;
        int b = -1;
        for (int k = 0; k < cw.Length; k++)
        {
            char c = cw[k];
            if (c >= '0' && c <= '9')
            {
                if (a == -1)
                {
                    a = c - '0';
                }
                b = c - '0';
            }
            else
            {
                for (int j = 0; j < 10; j++)
                {
                    if (cw.Substring(k).StartsWith(nums[j]))
                    {
                        if (a == -1)
                        {
                            a = j;
                        }
                        b = j;

                    }
                }

            }
        }
        answer1 += a * 10 + b;

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
    Console.ForegroundColor = (v == sv || !isTest) ? ConsoleColor.Green : ConsoleColor.Red;
    Console.Write(v);
    Console.ForegroundColor = previouscolour;
    if (isTest)
    {
        Console.Write(" ... supposed (example) answer: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(sv);
        Console.ForegroundColor = previouscolour;
    }
    Console.WriteLine();
}
