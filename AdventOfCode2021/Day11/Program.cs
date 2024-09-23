Run();

void Run()
{
    string inputfile = @"..\..\..\example_input.txt";
    //string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 1656;
    long supposedanswer2 = 195;
    
    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var grid = new int[10, 10];
    for (int i=0; i<10; i++)
    {
        for (int j = 0; j < 10; j++)
            grid[i, j] = S[i][j] - '0';
    }

    bool allflash = false;
    long counter = 0;
    while (!allflash)
    {
        for (int j = 0; j < 10; j++)
            for (int k = 0; k < 10; k++)
            {
                counter += IncreaseGridpoint(j,k,grid);
                if (answer2 == 99) answer1 = counter;
            }
        allflash = true;
        for (int j = 0; j < 10; j++)
            for (int k = 0; k < 10; k++)
                if (grid[j, k] == -1) grid[j, k] = 0;
                else allflash = false;

        answer2++;

        printgrid(grid, answer2, counter);
    }
    w(1, answer1, supposedanswer1);
    w(2, answer2, supposedanswer2);
}

long IncreaseGridpoint(int j, int k, int[,] grid)
{
    if (grid[j,k] != -1) grid[j, k]++;
    if (grid[j, k] > 9)
    {
        grid[j, k] = -1;
       return 1 + IncreaseNeighbours(j, k, grid);
    }
    return 0;
}

long IncreaseNeighbours(int j, int k, int[,] grid)
{
    long count = 0;
    for (int p = Math.Max(j-1,0); p < Math.Min(j+2,10); p++)
        for(int q = Math.Max(k - 1, 0); q < Math.Min(k + 2, 10); q++)
        {
            count += IncreaseGridpoint(p, q, grid);
        }
    return count;
}

void printgrid(int[,] grid, long step, long flashes)
{
    var previouscolour = Console.ForegroundColor;
    Console.CursorLeft = 0;
    Console.CursorTop = 0;

    Console.WriteLine("Step: " + step);
    Console.WriteLine("Energy: " + flashes + " flashes");

    for (int i = 0; i < 10; i++)
    {
        for (int j = 0; j < 10; j++)
        {
            var a = grid[i, j];
            switch (a)
            {
                case 0: Console.ForegroundColor = ConsoleColor.White; break;
                case 9: Console.ForegroundColor = ConsoleColor.Yellow; break;
                case 8: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                case 7: Console.ForegroundColor = ConsoleColor.Gray; break;
                case 6: Console.ForegroundColor = ConsoleColor.DarkRed; break;
                case 5: Console.ForegroundColor = ConsoleColor.DarkBlue; break;
                case 4: Console.ForegroundColor = ConsoleColor.DarkGreen; break;
                default: Console.ForegroundColor = ConsoleColor.DarkGray; break;
            }       
            Console.Write(grid[i, j]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
    Console.ForegroundColor = previouscolour;
    if (step == 100) Thread.Sleep(1000); else Thread.Sleep(20);
}

static void w<T>(int number, T val, T supposedval)
{
    string? v = (val == null) ? "(null)" : val.ToString();
    string? sv = (supposedval == null) ? "(null)" : supposedval.ToString();

    var previouscolour = Console.ForegroundColor;
    Console.Write("Answer Part " + number + ": ");
    Console.ForegroundColor = (v == sv) ? ConsoleColor.Green :   ConsoleColor.White;
    Console.Write(v);
    Console.ForegroundColor = previouscolour;
    Console.Write(" ... supposed (example) answer: ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(sv);
    Console.ForegroundColor = previouscolour;
}
