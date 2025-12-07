using AocHelper;
using System.Diagnostics;
using System.Numerics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\input.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 21;
    long supposedanswer2 = 40;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    var timelines = new long[S[0].Length];
    for (int j = 0; j < S.Count; j++)
    {
        var newtimelines = new long[S[0].Length];
        for (int i = 0; i < S[j].Length; i++)
        {
            switch (S[j][i])
            {
                case 'S':
                    newtimelines[i] = 1;
                    break;
                case '^':
                    var tl = timelines[i];
                    if (tl > 0)
                    {
                        answer1++;
                        newtimelines[i - 1] += tl;
                        newtimelines[i + 1] += tl;
                    }
                    break;
                default:
                    newtimelines[i] += timelines[i];
                    break;
            }
        }
        timelines = newtimelines;
    }
    answer2 = timelines.Sum();

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

