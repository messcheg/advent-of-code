using System.Collections;

Run();

void Run()
{
    //string inputfile = @"..\..\..\example_input.txt";
    string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 40;
    long supposedanswer2 = 315;

    var S = File.ReadAllLines(inputfile).ToArray();

    long answer1 = CalculatePath(S);
    long answer2 = CalculatePath(EnlargeBy(5, S)); 

    w(1, answer1, supposedanswer1);
    w(2, answer2, supposedanswer2);
}

long CalculatePath(string[] S)
{
    var paths = new List<(int weight, List<(int x, int y)> path)>();
    paths.Add((0, new List<(int x, int y)>(new (int x, int y)[] { (0, 0) })));
    int maxX = S[0].Length;
    int maxY = S.Length;
    var visited = new bool[maxX, maxY];
    bool finished = false;
    while (!finished)
    {
        var minweight = paths.Min(B => B.weight);
        int indexOfShortest = 0;
        while (indexOfShortest < paths.Count && paths[indexOfShortest].weight != minweight) indexOfShortest++;

        var currentnode = paths[indexOfShortest];
        int x = currentnode.path[^1].x;
        int y = currentnode.path[^1].y;
        visited[x, y] = true;

        var newpaths = new List<(int weight, List<(int x, int y)> path)>();

        if (x > 0 && !visited[x - 1, y]) AddNode(x - 1, y, S, newpaths, currentnode);
        if (x < maxX - 1 && !visited[x + 1, y]) AddNode(x + 1, y, S, newpaths, currentnode);
        if (y > 0 && !visited[x, y - 1]) AddNode(x, y - 1, S, newpaths, currentnode);
        if (y < maxY - 1 && !visited[x, y + 1]) AddNode(x, y + 1, S, newpaths, currentnode);
        bool removatReplaced = false;
        for (int i = 0; i < newpaths.Count; i++)
        {
            finished |= newpaths[i].path[^1] == (maxX - 1, maxY - 1);

            bool better = true;
            bool replaced = false;
            for (int j = 0; j < paths.Count; j++)
            {
                if (paths[j].path[^1] == newpaths[i].path[^1])
                {
                    if (paths[j].weight <= newpaths[i].weight)
                    {
                        better = false;
                    }
                    else
                    {
                        paths[j] = newpaths[i];
                        replaced = true;
                    }
                }
            }
            if (better && !replaced)
            {
                if (!removatReplaced)
                {
                    paths[indexOfShortest] = newpaths[i];
                    removatReplaced = true;
                }
                else
                    paths.Add(newpaths[i]);
            }
        }
        if (!removatReplaced) paths.RemoveAt(indexOfShortest);

    }
    return paths.Where(p => p.path[^1] == (maxX - 1, maxY - 1)).Min(p => p.weight);

}

void AddNode(int x, int y, string[] S, List<(int weight, List<(int x, int y)> path)> newpaths, (int weight, List<(int x, int y)> path) currentnode)
{
    int newweight = currentnode.weight + (S[x][y] - '0');
    var newlist = new List<(int x, int y)>(currentnode.path.ToArray());
    newlist.Add((x, y));
    newpaths.Add((newweight, newlist));

}

string[] EnlargeBy(int factor, string[] input)
{
    var output = new string[input.Length * factor];
    for (int i = 0; i<input.Length; i++)
    {
        for (int m = 0; m < factor; m++)
        {
            char[] newline = new char[input[i].Length * factor];
            for (int j = 0; j < factor; j++)
                for (int k = 0; k < input[i].Length; k++)
                    newline[j * input[i].Length + k] = (char)((input[i][k] - '1' + j + m) % 9 + '1');
            output[i + m * input.Length] = new string(newline);
        }
    }
    

    return output;
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
