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

    var S = File.ReadAllText(inputfile).Split(' ').Select(a => long.Parse(a)).ToList();
    long answer1 = 0;

    var cache = new Dictionary<(long number, int numberoftimes), long>();
    long Getstones(long number, int times)
    {
        if (times == 0) return 1;
        else
        {
            if (cache.TryGetValue((number, times), out long result)) return result;
            if (number == 0)
            {
                result = Getstones(1, times - 1);
            }
            else
            {
                var scur = number.ToString();
                if (scur.Length % 2 == 0)
                {
                    result += Getstones(long.Parse(scur.Substring(0, scur.Length / 2)), times - 1);
                    result += Getstones(long.Parse(scur.Substring(scur.Length / 2)), times - 1);
                }
                else
                {
                    result = Getstones(number * 2024, times - 1);
                }
            }
            cache[(number, times)] = result;
            return result;
        }
    }

    foreach (var number in S)
    {
        answer1 += Getstones(number, numberoftimes);
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

