using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example1.txt", true);
//Run(@"..\..\..\example1.txt", true);
Run(@"..\..\..\input.txt", false);
//Run(@"E:\develop\advent-of-code-input\2024\day23.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 7;
    string supposedanswer2 = "co,de,ka,ta";

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    string answer2 = "";
    var connections = new HashSet<(string, string)>();
    var nodes = new HashSet<string>();
    foreach (var s in S)
    {
        var s1 = s.Split('-');
        if (string.Compare(s1[0], s1[1], true) < 0)
        {
            connections.Add((s1[0], s1[1]));
        }
        else
        {
            connections.Add((s1[1], s1[0]));
        }
        nodes.Add(s1[0]);
        nodes.Add(s1[1]);
    }
    var n1 = nodes.Order().ToList();
    var connectiolist = new List<string[]>();
    for (int i = 0; i < n1.Count - 2; i++)
    {
        var a = n1[i];
        for (int j = i + 1; j < n1.Count - 1; j++)
        {
            var b = n1[j];
            for (int k = j + 1; k < n1.Count; k++)
            {
                {
                    var c = n1[k];
                    if (connections.Contains((a, b)) && connections.Contains((a, c)) && connections.Contains((b, c)))
                    {
                        connectiolist.Add(new string[] { a, b, c });
                        if (a[0] == 't' || b[0] == 't' || c[0] == 't')
                        {
                            answer1++;
                        }
                    }
                }
            }
        }
    }

    bool ready = false;
    while (!ready)
    {
        var newlist = new List<string[]>();
        foreach (var list1 in connectiolist)
        {
            foreach (var d in n1)
            {
                if (string.Compare(list1[^1], d, true) < 0)
                {
                    bool found = true;
                    foreach (var l in list1)
                    {
                        if (!connections.Contains((l, d)))
                        {
                            found = false; break;
                        }
                    }
                    if (found)
                    {
                        newlist.Add(list1.Append<string>(d).ToArray());
                    }
                }
            }
        }
        if (newlist.Count > 0) { connectiolist = newlist; }
        else ready = true;
    }

    answer2 = connectiolist[0][0];
    foreach (var s3 in connectiolist[0].Skip(1)) answer2 += ',' + s3;

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

