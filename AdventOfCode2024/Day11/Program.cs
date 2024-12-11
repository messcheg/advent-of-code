using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true, 1, 7);
Run(@"..\..\..\example1.txt", true, 25, 55312);
Run(@"..\..\..\input.txt", false, 25);
Run(@"..\..\..\input.txt", false, 75);
//Run(@"E:\develop\advent-of-code-input\2024\day11.txt", false);

void Run(string inputfile, bool isTest, int numberoftimes, long supposedanswer1 = 0)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer2 = 0;

    var S = File.ReadAllText(inputfile).Split(' ').Select(a => long.Parse(a)).ToList();
    long answer1 = 0;
    long answer2 = 0;
    var prework = new long[numberoftimes];

    var oldlist = new List<long>() { 0 };
    for (int i = 0; i < numberoftimes; i++)
    {
        var newlist = new List<long>();
        for (int j = 0; j < oldlist.Count; j++)
        {
            var cur = oldlist[j];
            if (cur == 0) { newlist.Add(1); continue; }
            var scur = cur.ToString();
            if (scur.Length % 2 == 0)
            {
                newlist.Add(long.Parse(scur.Substring(0, scur.Length / 2)));
                newlist.Add(long.Parse(scur.Substring(scur.Length / 2)));
                continue;
            }
            newlist.Add(cur * 2024);
        }
        oldlist = newlist;
        prework[i] = oldlist.Count;
    }




    var work = new Stack<(long number, int numberoftimes)>();
    for (int i = S.Count - 1; i >= 0; i--)
    {
        work.Push((S[i], numberoftimes));
    }

    while (work.Count > 0)
    {
        var w = work.Pop();
        if (w.numberoftimes == 0) answer1++;
        else
        {
            if (w.number == 0) answer1 += prework[w.numberoftimes - 1];
            else
            {
                var scur = w.number.ToString();
                if (scur.Length % 2 == 0)
                {
                    work.Push((long.Parse(scur.Substring(scur.Length / 2)), w.numberoftimes - 1));
                    work.Push((long.Parse(scur.Substring(0, scur.Length / 2)), w.numberoftimes - 1));
                }
                else
                {
                    work.Push((w.number * 2024, w.numberoftimes - 1));
                }
            }
        }
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

