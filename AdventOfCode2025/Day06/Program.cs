using AocHelper;
using System.Diagnostics;
using System.Numerics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\input.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    BigInteger supposedanswer1 = 4277556;
    BigInteger supposedanswer2 = 3263827;

    var S = File.ReadAllLines(inputfile).ToList();
    BigInteger answer1 = 0;
    BigInteger answer2 = 0;

    answer1 = 0;
    answer2 = 0;
    var numbers = new List<List<string>>();
    foreach (var s in S)
    {
        string nextnum = "";
        var l = new List<string>();
        foreach (var c in s)
        {
            if (c == ' ' && nextnum != "")
            {
                l.Add(nextnum);
                nextnum = "";
            }
            if (c != ' ') nextnum += c;
        }
        if (nextnum != "") l.Add(nextnum);
        numbers.Add(l);
    }
    for (int i = 0; i < numbers[0].Count; i++)
    {
        var sign = numbers[numbers.Count-1][i];
        BigInteger nextans = sign == "*" ? 1:0; 
        for (int j = 0; j < numbers.Count-1; j++)
        {
            long nextnum = long.Parse(numbers[j][i]);
            if (sign == "*")
            {
                nextans *= nextnum;
            }
            else nextans += nextnum;
        }
        answer1 += nextans;
    }

    var nextrange = new List<long>();
    for (int i = S[0].Length -1; i >= 0; i--)
    {
        long nextnum = 0;
        for (int j = 0; j < S.Count - 1; j++)
        {
            var c = S[j][i];
            if (c != ' ') nextnum = 10 * nextnum + (c - '0');
        }
        nextrange.Add(nextnum);
        char sign = S[S.Count - 1][i];
        if (sign!=' ')
        {
            BigInteger nextans = sign == '*' ? 1 : 0;
            foreach (var num in nextrange) if (sign == '*') nextans *= num; else nextans += num;
            answer2 += nextans;
            nextrange = new List<long>();
            i--;
        }
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

