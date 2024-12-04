using System.Diagnostics;
using System.Globalization;
using System.Net.Security;
using System.Runtime.InteropServices;
using AocHelper;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\example1.txt", true);
Run(@"E:\develop\advent-of-code-input\2023\day01.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 142;
    long supposedanswer2 = 281;

    string[] nums = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    foreach(var cw in S)
    {
        int a1 = 0 - 1;
        int b1 = -1;
        int a2 = 0 - 1;
        int b2 = -1;
        for (int k = 0; k < cw.Length; k++)
        {
            char c = cw[k];
            if (c >= '0' && c <= '9')
            {
                if (a1 == -1)
                {
                    a1 = c - '0';
                    if (a2 == -1) a2 = a1;

                }

                b2 = b1 = c - '0';
            }
            else
            {
                for (int j = 0; j < 10; j++)
                {
                    if (cw.Substring(k).StartsWith(nums[j]))
                    {
                        if (a2 == -1)
                        {
                            a2 = j;
                        }
                        b2 = j;

                    }
                }

            }
        }
        answer1 += a1 * 10 + b1;
        answer2 += a2 * 10 + b2;
    }


    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
}

