using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\example1.txt", true);
//Run(@"E:\develop\advent-of-code-input\2024\day02.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 2;
    long supposedanswer2 = 4;

    var S = File.ReadAllLines(inputfile).Select(a => a.Split(' ').Select(b => long.Parse(b)).ToList()).ToList();
    long answer1 = 0;
    long answer2 = 0;
    foreach (var s in S)
    {
        (bool, int) test(List<long> s)
        {
            bool up = s[0] <= s[1];
            for (int i = 1; i < s.Count; i++)
            {
                long dif = up ? dif = s[i] - s[i - 1] : dif = s[i - 1] - s[i];
                if (dif < 1 || dif > 3) return (false, i);
            }
            return (true, 0);
        }

        (bool safe, int location) = test(s);
        if (safe)
        {
            answer1++;
            answer2++;
        }
        else
        {
            var s1 = s.ToList();
            s1.RemoveAt(location - 1);
            (safe, int l1) = test(s1);
            if (safe) answer2++;
            else
            {
                s1 = s.ToList();
                s1.RemoveAt(location);
                (safe, l1) = test(s1);
                if (safe) answer2++;
                else
                {
                    if (location > 1)
                    {
                        s1 = s.ToList();
                        s1.RemoveAt(location - 2);
                        (safe, l1) = test(s1);
                    }
                    if (safe) answer2++;
                }
            }
        }

    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

