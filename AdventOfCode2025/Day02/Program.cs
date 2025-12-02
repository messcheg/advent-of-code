using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\input.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 1227775554;
    long supposedanswer2 = 4174379265;

    var S = File.ReadAllText(inputfile).Split(',').ToList();
    var A = new List<long>();
    var B = new List<long>();
    long answer1 = 0;
    long answer2 = 0;

    answer1 = 0;
    answer2 = 0;

    foreach (var s in S)
    {
        var ft = s.Split("-");
        long f = long.Parse(ft[0]);
        long t = long.Parse(ft[1]);
        for (long l = f; l <= t; l++)
        {
            var num = l.ToString();
            var m = num.Length / 2;

            for (int d = 2; d < num.Length + 1; d++)
            {
                if (num.Length % d == 0)
                {
                    var sz = num.Length / d;
                    var s1 = num[0..sz].ToString();
                    var c = sz;
                    bool ok = true;
                    while (c < num.Length && ok)
                    {
                        var s2 = num[c..(c + sz)].ToString();
                        ok = s1 == s2;
                        c += sz;
                    }
                    if (ok)
                    {
                        if (d == 2) answer1 += l;
                        answer2 += l;
                        break;
                    }
                }

            }
        }

    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}