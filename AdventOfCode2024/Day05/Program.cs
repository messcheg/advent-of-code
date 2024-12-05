using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\example1.txt", true);
//Run(@"E:\develop\advent-of-code-input\2024\day05.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 143;
    long supposedanswer2 = 123;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    int i = 0;
    var before = new Dictionary<string, HashSet<string>>();
    var after = new Dictionary<string, HashSet<string>>();

    while (i < S.Count && S[i].Length > 0)
    {
        var ord = S[i].Split('|').ToArray();
        var a = before.GetValueOrDefault(ord[0]);
        if (a == null) { a = new HashSet<string>(); }
        a.Add(ord[1]);
        before[ord[0]] = a;
        i++;
    }
    i++;
    var incorrect = new List<string[]>();
    while (i < S.Count && S[i].Length > 0)
    {
        bool rightorder = true;
        var a = S[i].Split(',').ToArray();
        for (int j = 0; j < a.Length - 1; j++)
        {
            for (int k = j + 1; k < a.Length; k++)
            {
                var look = before.GetValueOrDefault(a[j]);
                if (look != null && look.Contains(a[k])) continue;
                rightorder = false;
                break;
            }
            if (rightorder == false) { break; }
        }
        if (rightorder)
        {
            var middle = (a.Length - 1) / 2;
            answer1 += long.Parse(a[middle]);
        }
        else
        {
            incorrect.Add(a);
        }

        i++;
    }

    foreach (var a in incorrect)
    {
        for (int p = 0; p < a.Length - 1; p++)
        {
            for (int q = p + 1; q < a.Length; q++)
            {
                var lookup = before.GetValueOrDefault(a[p]);
                if (lookup != null && lookup.Contains(a[q])) continue;
                (a[p], a[q]) = (a[q], a[p]);
            }
        }
        var middle = (a.Length - 1) / 2;
        answer2 += long.Parse(a[middle]);
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

