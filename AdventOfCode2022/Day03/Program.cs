using System.Runtime.Serialization;

Run(@"..\..\..\example_input.txt");
Run(@"E:\develop\advent-of-code-input\2022\day03.txt");

void Run(string inputfile)
{
    long supposedanswer1 = 0000;
    long supposedanswer2 = 0000;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;


    for (int i = 0; i < S.Count; i++)
    {
        var l = S[i].Length /2;
        var L = S[i].ToArray();
        var p1 = L.Skip(l).ToArray();
        foreach (var c in L.Take(l))
        {
            if (p1.Contains(c))
            {
                if (c >= 'A' && c <= 'Z')
                {
                    answer1 += 27 + (c - 'A');
                }
                else
                {
                    answer1 += 1 + (c - 'a');
                }
                break;
            }
        }
    }

    for (int i = 0; i < S.Count; i += 3)
    {
        foreach (var c in S[i])
        {
            if (S[i+1].Contains(c) && S[i+2].Contains(c))
            {
                if (c >= 'A' && c <= 'Z')
                {
                    answer2 += 27 + (c - 'A');
                }
                else
                {
                    answer2 += 1 + (c - 'a');
                }
                break;
            }
         }

    }

    w(1, answer1, supposedanswer1);
    w(2, answer2, supposedanswer2);
}

static void w<T>(int number, T val, T supposedval)
{
    string? v = (val == null) ? "(null)" : val.ToString();
    string? sv = (supposedval == null) ? "(null)" : supposedval.ToString();

    var previouscolour = Console.ForegroundColor;
    Console.Write("Answer Part " + number + ": ");
    Console.ForegroundColor = (v == sv) ? ConsoleColor.Green : ConsoleColor.White;
    Console.Write(v);
    Console.ForegroundColor = previouscolour;
    Console.Write(" ... supposed (example) answer: ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(sv);
    Console.ForegroundColor = previouscolour;
}
