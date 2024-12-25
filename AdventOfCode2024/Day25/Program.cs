using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\example1.txt", false);
//Run(@"E:\develop\advent-of-code-input\2024\day25.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 3;
    long supposedanswer2 = 0;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    int i = 0;
    var locks = new List<int[]>();
    var keys = new List<int[]>();
    while (i < S.Count)
    {
        var thing = new int[5];
        var s = S[i];
        bool isKey = (s == ".....");
        while (i < S.Count && S[i] != "")
        {
            for (int j = 0; j < S[i].Length; j++)
            {
                if (S[i][j] == '#') thing[j]++;
            }
            i++;
        }
        if (isKey) { keys.Add(thing); }
        else { locks.Add(thing); }
        i++;
    }

    foreach (var key in keys)
    {
        foreach (var lck in locks)
        {
            bool fit = true;
            for (int j = 0; j < lck.Length; j++)
            {
                if (lck[j] + key[j] > 7) fit = false;
            }
            if (fit) answer1++;
        }
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

