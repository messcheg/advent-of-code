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

    var collectedSequences = new Dictionary<long, long>();
    for (int i = 0; i < S.Count; i++)
    {
        var firstSequence = new HashSet<long>();
        var secret = S[i];
        int previousprize = (int)(secret % 10);
        long prevkey = 0;
        for (int j = 0; j < 2000; j++)
        {
            secret = NextSecret(secret);
            int currenprize = (int)(secret % 10);
            int d4 = currenprize - previousprize;
            long key = (prevkey & 0x7FFF) << 5 + d4 + 10;

            if (j >= 3)
            {
                if (!firstSequence.Contains(key))
                {
                    firstSequence.Add(key);
                    if (!collectedSequences.TryGetValue(key, out var totalprize)) totalprize = 0;
                    collectedSequences[key] = totalprize + currenprize;
                }
            }
            prevkey = key;
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
    long result = (previousSecret << 6) ^ previousSecret;
    result = result % prunevalue;
    result = (result >> 5) ^ result;
    result = result % prunevalue;
    result = (result << 11) ^ result;
    result = result % prunevalue;
    return result;
}