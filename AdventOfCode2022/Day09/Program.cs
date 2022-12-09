using System.Globalization;
using System.Security.Cryptography.X509Certificates;

Run(@"..\..\..\example_input.txt", true);
Run(@"..\..\..\real_input.txt", false);

void Run(string inputfile, bool isTest)
{ 
    long supposedanswer1 = 13;
    long supposedanswer2 = 0000;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    (int x, int y) H = (0, 0);
    (int x, int y) T = (0, 0);
    var visited = new HashSet<(int x, int y)>();
    visited.Add(H);

    var visited2 = new HashSet<(int x, int y)>();
    visited2.Add(H);
    var rope = new (int x, int y)[10];
    for (int i = 0; i < S.Count; i++)
    {
        var cmd = S[i].Split(' ');
        var dir = cmd[0];
        int step = int.Parse(cmd[1]);
        for (int j = 0; j < step; j++)
        {
            (H, T) = Move(H, T, dir);
            visited.Add(T);

            for (int k= 0; k < 9; k++)
            {
                // <HIER VANMIDDAG VERDER >
            }
        }
    }

    answer1 = visited.Count();
    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);
}

static ((int x, int y), (int x, int y)) Move ((int x, int y)H, (int x, int y)T, string dir)
{
    int difx = H.x - T.x;
    int dify = H.y - T.y;
    bool U = dir == "U" ;
    bool D = dir == "D";
    bool R = dir == "R";
    bool L = dir == "L";


    if ((difx, dify) == (-1, 1))
    {
        if (L || U)
        {
            T.x--;
            T.y++;
        }
    }
    else if ((difx, dify) == (-1, 0))
    {
        if ( L )
        {
            T.x--;
        }
    }
    else if ((difx, dify) == (-1, -1))
    {
        if ( L || D)
        {
            T.x--;
            T.y--;
        }
    }
    else if ((difx, dify) == (0, -1))
    {
        if ( D )
        {
            T.y--;
        }
    }
    else if ((difx, dify) == (1, -1))
    {
        if ( D || R)
        {
            T.x++;
            T.y--;
        }
    }
    else if ((difx, dify) == (1, 0))
    {
        if ( R )
        {
            T.x++;
        }
    }
    else if ((difx, dify) == (1, 1))
    {
        if (R || U)
        {
            T.x++;
            T.y++;
        }
    }
    else if ((difx, dify) == (0, 1))
    {
        if (  U )
        {
            T.y++;
        }
    }

    if (R)
    {
        H.x++;
    }
    else if (L)
    {
        H.x--;
    }
    else if (U)
    {
        H.y++;
    }
    else if (D)
    {
        H.y--;
    }

    return (H, T);
}

static void w<T>(int number, T val, T supposedval, bool isTest)
{
    string? v = (val == null) ? "(null)" : val.ToString();
    string? sv = (supposedval == null) ? "(null)" : supposedval.ToString();

    var previouscolour = Console.ForegroundColor;
    Console.Write("Answer Part " + number + ": ");
    Console.ForegroundColor = (v == sv) ? ConsoleColor.Green : ConsoleColor.White;
    Console.Write(v);
    Console.ForegroundColor = previouscolour;
    if (isTest)
    {
        Console.Write(" ... supposed (example) answer: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(sv);
        Console.ForegroundColor = previouscolour;
    }
    else
        Console.WriteLine();
}
