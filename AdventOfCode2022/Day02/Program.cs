Run();

void Run()
{
    //string inputfile = @"..\..\..\example_input.txt";
    string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 15;
    long supposedanswer2 = 0000;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;


    for (int i = 0; i < S.Count; i++)
    {
        char a = S[i][0];
        char b = S[i][2];
        int point1 = (int)(b- 'W');
        int point2 = 0;
        int point1a = 0;
        int point2a = 0;
        if (a=='A' )
        {
            if (b == 'X')
            {
                point2 = 3;
                point2a = 0;
                point1a = 3;
            }
            else if (b == 'Y')
            {
                point2 = 6;
                point2a = 3;
                point1a = 1;
            }
            else
            {
                point2 = 0;
                point2a = 6;
                point1a = 2;
            }
        }
        else if (a=='B')
        {

            if (b == 'X')
            {
                point2 = 0;
                point2a = 0;
                point1a = 1;
            }
            else if (b == 'Y')
            {
                point2 = 3;
                point2a = 3;
                point1a = 2;
            }
            else
            {
                point2 = 6;
                point2a = 6;
                point1a = 3;
            }

        }
        else
        {
            if (b == 'X')
            {
                point2 = 6;
                point2a = 0;
                point1a = 2;
            }
            else if (b == 'Y')
            {
                point2 = 0;
                point2a = 3;
                point1a = 3;
            }
            else
            {
                point2 = 3;
                point2a = 6;
                point1a = 1;
            }

            if (b == 'X') point2 = 6;
            else if (b == 'Y') point2 = 0;
            else point2 = 3;

        }
        answer1 += point1 + point2;
        answer2 += point1a + point2a;
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
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
