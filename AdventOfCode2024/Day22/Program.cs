using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example1.txt", true);
Run(@"..\..\..\example.txt", true, 37327623);
Run(@"..\..\..\input.txt", false);

//Run(@"E:\develop\advent-of-code-input\2024\day22.txt", false);
const long prunevalue = 16777216;

void Run(string inputfile, bool isTest, long supposedanswer1 = 0, long supposedanswer2 = 0)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var S = File.ReadAllLines(inputfile).Select(long.Parse).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var collectedSequences = new Dictionary<(int d1, int d2, int d3, int d4), long>();
    for (int i = 0; i < S.Count; i++)
    {
        var firstSequence = new HashSet<(int d1, int d2, int d3, int d4)>();
        var secret = S[i];
        int previousprize = (int)(secret % 10);
        var (d1, d2, d3, d4) = (0, 0, 0, 0);
        for (int j = 0; j < 2000; j++)
        {
            secret = NextSecret(secret);
            int currenprize = (int)(secret % 10);
            d4 = currenprize - previousprize;
            if (j >= 3)
            {
                if (!firstSequence.Contains((d1, d2, d3, d4)))
                {
                    firstSequence.Add((d1, d2, d3, d4));
                    if (!collectedSequences.TryGetValue((d1, d2, d3, d4), out var totalprize)) totalprize = 0;
                    collectedSequences[(d1, d2, d3, d4)] = totalprize + currenprize;
                }
            }
            (d1, d2, d3) = (d2, d3, d4);
            previousprize = currenprize;
        }
        answer1 += secret;
    }
    answer2 = collectedSequences.Values.Max();

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

long NextSecret(long previousSecret)
{
    long result = (previousSecret * 64) ^ previousSecret;
    result = result % prunevalue;
    result = (result / 32) ^ result;
    result = result % prunevalue;
    result = (result * 2048) ^ result;
    result = result % prunevalue;
    return result;
}