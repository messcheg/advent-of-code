using System.Diagnostics;
using System.Net.Security;
using static System.Net.Mime.MediaTypeNames;

Run(@"..\..\..\example_input.txt", true, 4, false);
Run(@"E:\develop\advent-of-code-input\2022\day22.txt", false, 50, true);

void Run(string inputfile, bool isTest, int block, bool paint)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 6032;
    long supposedanswer2 = 5031;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;


    int i = 0;
    var instruction = S[^1];
    int x = 0;
    int y = 0;
    int dir = 0;
    int xCnt = 0;
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.SetCursorPosition(0, 0);
    Console.SetBufferSize(250, S.Count + 100);
    Console.SetWindowSize(200, 50);
    

    for (int j = 0; j < S.Count - 2; j++)
    {
        xCnt = Math.Max(xCnt, S[j].Length);
        if ( paint) Console.WriteLine(S[j]);
    }
    int virtDir = 0;
    while (S[0][x] != '.') x++;

    if (paint)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write('>');
    }
    var directions = new char[] {'>', 'V', '<', '^' };
    while (i < instruction.Length)
    {
        string a = "";
        while (i < instruction.Length && instruction[i] <= '9' && instruction[i] >= '0')
        {
            a += instruction[i];
            i++;
        }
        char b = ' ';
        if (i < instruction.Length)
        {
            b = instruction[i];
            i++;
        }

        for (int j = 0; j < int.Parse(a); j++)
        {
            int nextX = x;
            int nextY = y;
            int nextdir = dir;

            if (dir == 0)
            {
                bool around = false;
                nextX = x + 1;
                if (nextX == S[y].Length) around = true;
                else if (S[y][nextX] == ' ') around = true;
                if (around && block == 4)
                {
                    if (nextX == block * 3 && 0 <= y && y < block) // side1
                    {
                        nextY = block * 3 - y - 1; nextX = block * 4 - 1;
                        nextdir = 2;
                    }
                    else if (nextX == block * 3 && block <= y && y < block * 2) // side4
                    {
                        nextY = block * 2; nextX = block * 4 - (y - block) - 1;
                        nextdir = 1;
                    }
                    else //side 6
                    {
                        nextY = block - (y - block * 2) - 1; nextX = block * 3 - 1;
                        nextdir = 2;
                    }
                }
                if (around && block == 50)
                {
                    if (nextX == block * 3 && 0 <= y && y < block) // side2
                    {
                        nextY = block * 3 - y - 1; nextX = block * 2 - 1;
                        nextdir = 2;
                    }
                    else if (nextX == block * 2 && block <= y && y < block * 2) // side 3
                    {
                        nextY = block - 1; nextX = block + y;
                        nextdir = 3;
                    }
                    else if (nextX == block * 2 && block * 2 <= y && y < block * 3) // side 5
                    { 
                        nextY = block - (y - 2 * block) - 1; nextX = block * 3 - 1;
                        nextdir = 2;
                    }
                    else if (nextX == block && block * 3 <= y && y < block * 4) // side 6
                    {
                        nextY = block * 3 - 1; nextX = y - 2 * block;
                        nextdir = 3;
                    }
                    else
                    {
                        Console.Write("Error.");
                    }
                }
            }
            else if (dir == 1)
            {
                bool around = false;
                nextY = y + 1;
                if (nextY == S.Count - 2) around = true;
                else if (x > S[nextY].Length || S[nextY][x] == ' ') around = true;
                if (around && block == 4)
                {
                    //                       nextY = 0;
                    //                       while (x >= S[nextY].Length || S[nextY][x] == ' ') nextY++;

                    if (nextY == block * 2 && 0 <= x && x < block) // side2
                    {
                        nextX = block * 3 - x - 1; nextY = block * 3 - 1;
                        nextdir = 3;
                    }
                    else if (nextY == block * 2 && block <= x && x < block * 2) // side3
                    {
                        nextY = block * 3 - (x - block) - 1; nextX = block * 2;
                        nextdir = 0;
                    }
                    else if (nextY == block * 3 && block * 2 <= x && x < block * 3) // side5
                    {
                        nextX = block - (x - block * 2) - 1; nextY = block * 2 - 1;
                        nextdir = 3;
                    }
                    else if (nextY == block * 3 && block * 3 <= x && x < block * 4) // side6
                    {
                        nextY = block * 2 - (x - block * 3) - 1; nextX = 0;
                        nextdir = 0;
                    }

                }
                if (around && block == 50)
                {
                    if (nextY == block * 4 && 0 <= x && x < block) // side6
                    {
                        nextY = 0; nextX = x + 2 * block;
                        nextdir = 1;
                    }
                    else if (nextY == block * 3 && block <= x && x < block * 2) // side5
                    {
                        nextY = x + 2 * block; nextX = block - 1;
                        nextdir = 2;
                    }
                    else if (nextY == block && block * 2 <= x && x < block * 3) // side 2
                    {
                        nextY = x - block; nextX = 2 * block - 1;
                        nextdir = 2;
                    }
                    else
                    {
                        Console.Write("Error.");
                    }
                }
            }
            else if (dir == 2)
            {
                bool around = false;
                nextX = x - 1;
                if (nextX < 0) around = true;
                else if (S[y][nextX] == ' ') around = true;
                if (around && block == 4)
                {
                    //nextX = S[y].Length - 1;
                    //while (S[y][nextX] == ' ') nextX--;
                    if (nextX == block * 2 - 1 && 0 <= y && y < block) // side1
                    {
                        nextY = block; nextX = block + y;
                        nextdir = 1;
                    }
                    else if (nextX == block * 2 && block * 2 <= y && y < block * 3) // side5
                    {
                        nextY = block * 2 - 1; nextX = block * 2 - (y - block * 2) - 1;
                        nextdir = 3;
                    }
                    else //side 2
                    {
                        nextY = block * 3 - 1; nextX = block * 4 - (y - block) - 1;
                        nextdir = 3;
                    }
                }
                if (around && block == 50)
                {
                    if (nextX == block - 1 && 0 <= y && y < block) // side 1
                    {
                        nextY = block * 3 - y - 1; nextX = 0;
                        nextdir = 0;
                    }
                    else if (nextX == block - 1 && block <= y && y < block * 2) // side3
                    {
                        nextY = block * 2; nextX = y - block;
                        nextdir = 1;
                    }
                    else if (nextX == -1 && block * 2 <= y && y < block * 3) // side 4
                    {
                        nextY = block - (y - block * 2 ) - 1; nextX = block;
                        nextdir = 0;
                    }
                    else  if (nextX == -1 && block * 3 <= y && y < block * 4) // side6
                    {
                        nextY = 0; nextX = y - block * 2;
                        nextdir = 1;
                    }
                    else
                    {
                        Console.Write("Error.");
                    }
                }
            }
            else if (dir == 3)
            {
                bool around = false;
                nextY = y - 1;
                if (nextY < 0) around = true;
                else if (x > S[nextY].Length || S[nextY][x] == ' ') around = true;
                if (around && block == 4)
                {
                    // nextY = S.Count - 3;
                    // while (x > S[nextY].Length || S[nextY][x] == ' ') nextY--;
                    if (nextY == block - 1 && 0 <= x && x < block) // side2
                    {
                        nextX = block * 3 - x - 1; nextY = 0;
                        nextdir = 1;
                    }
                    else if (nextY == block - 1 && block <= x && x < block * 2) // side3
                    {
                        nextY = x - block; nextX = block * 2;
                        nextdir = 0;
                    }
                    else if (nextY == -1 && block * 2 <= x && x < block * 3) // side1
                    {
                        nextX = block - (x - block * 2) - 1; nextY = block;
                        nextdir = 1;
                    }
                    else if (nextY == block * 2 - 1 && block * 3 <= x && x < block * 4) // side6
                    {
                        nextY = block * 2 - (x - block * 3) - 1; nextX = block * 3 - 1;
                        nextdir = 2;
                    }
                }
                if (around && block == 50)
                {
                    if (nextY == block * 2 - 1 && 0 <= x && x < block) // side 4
                    {
                        nextY = block + x; nextX = block;
                        nextdir = 0;
                    }
                    else if (nextY == -1 && block <= x && x < block * 2) // side1
                    {
                        nextY = x + block * 2; nextX = 0;
                        nextdir = 0;
                    }
                    else if (nextY == -1 && block * 2 <= x && x < block * 3) // side 2
                    {
                        nextY = block * 4 - 1; nextX = x - block * 2;
                        nextdir = 3;
                    }
                    else
                    {
                        Console.Write("Error.");
                    }
                }
            }
            if (S[nextY][nextX] == '.') { x = nextX; y = nextY; dir = nextdir; }

            if (paint)
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(directions[dir]);
                //Thread.Sleep(10);
            }
        }
        if (b == 'L') dir--;
        else if (b == 'R') dir++;
        if (dir < 0) dir = 3;
        if (dir > 3) dir = 0;
        if (b == 'L') virtDir--;
        else if (b == 'R') virtDir++;
        if (virtDir < 0) virtDir = 3;
        if (virtDir > 3) virtDir = 0;

        if (paint)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(directions[dir]);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }

    answer2 = 1000 * (y+1) + 4 * (x+1) + dir;

    stopwatch.Stop();

    if (paint)
    {
        Console.SetCursorPosition(0, S.Count);
    }

    Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
    Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);
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
