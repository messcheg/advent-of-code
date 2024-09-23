Run(@"..\..\..\example_input.txt");
Run(@"E:\develop\advent-of-code-input\2022\day04.txt");
Run(@"X:\Thijs\Fun projects\AOC4numbers.txt");


void Run(string inputfile)
{ 
    long supposedanswer1 = 2;
    long supposedanswer2 = 4;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;


    for (int i = 0; i < S.Count; i++)
    {
        var s = S[i].Split(",");
        var a = s[0].Split("-").Select(a => int.Parse(a)).ToArray();
        var b = s[1].Split("-").Select(a => int.Parse(a)).ToArray();
        bool full = a[0] <= b[0] && a[1] >= b[1] || b[0] <= a[0] && b[1] >= a[1];
        if (full) answer1++;
        if (! ( a[1] < b[0] || b[1] < a[0])) answer2++;
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
