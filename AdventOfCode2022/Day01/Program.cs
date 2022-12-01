using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

Run();

void Run()
{
    string inputfile = @"..\..\..\example_input.txt";
    //string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 24000;
    long supposedanswer2 = 45000;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var total = new List<long>();
    long sum = 0;
    for (int i = 0; i < S.Count; i++)
    {
        if (S[i] == "")
        {
            total.Add(sum);
            if (answer1 < sum) answer1 = sum;
            sum = 0;
        }
        else
        {
            sum += long.Parse(S[i]);
        }
    }
    total.Add(sum);
    if (answer1 < sum) answer1 = sum;
    
    var x = total.OrderByDescending(l => l).ToList();
    for (int i = 0; i<3;i++)
    {
        answer2 += x[i];
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
