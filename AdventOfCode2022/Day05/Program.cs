Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day05.txt", false);

void Run(string inputfile, bool isTest)
{ 
    string supposedanswer1 = "CMZ";
    string supposedanswer2 = "MCD";

    var S = File.ReadAllLines(inputfile).ToList();
    string answer1 = "";
    string answer2 = "";

    int i = 0;
    var stacks = new Dictionary<int, Stack<char>>();

    while (S[i][1] != '1')
    {
        int z = 1;
        for (int j = 1; j < S[i].Length; j += 4)
        {
            if (S[i][j] <= 'Z' && S[i][j] >= 'A')
            {
                Stack<char> s;
                if (!stacks.TryGetValue(z, out s)) s = new Stack<char>();
                s.Push(S[i][j]);
                stacks[z] = s;
            }
            z++;
        }
        i++;
    }
    
    var stc2 = new Dictionary<int,Stack<char>>();
    foreach(var ss in stacks)
    {
        var s1 = new Stack<char>();
        var s2 = new Stack<char>();
        while (ss.Value.Count > 0)
        {
            var c = ss.Value.Pop();
            s1.Push(c);
            s2.Push(c);
        }
        stacks[ss.Key] = s1;
        stc2[ss.Key] = s2;
    }

    i++; i++;
    while (i < S.Count)
    {
        var s1 = S[i].Split(' ');
        var count = int.Parse(s1[1]);
        var frm = int.Parse(s1[3]);
        var untl = int.Parse(s1[5]);
        var s2 = new Stack<char>();
        for (int z = 0; z < count; z++)
        {
            var c = stacks[frm].Pop();
            stacks[untl].Push(c);
        
            var c2 = stc2[frm].Pop();
            s2.Push(c2);
        }
        for (int z = 0; z < count; z++)
        {
            var c = s2.Pop();
            stc2[untl].Push(c);
        }

        i++;
    }
    foreach (var p in stacks.Keys.OrderBy(k => k))
    {
        answer1 += stacks[p].Peek();
        answer2 += stc2[p].Peek();
    }

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
