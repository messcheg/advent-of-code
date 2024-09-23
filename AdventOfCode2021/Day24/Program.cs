Run();

void Run()
{
    //string inputfile = @"..\..\..\example_input.txt";
    string inputfile = @"..\..\..\real_input.txt";
 
    var S = File.ReadAllLines(inputfile).ToList();
    var best = new HashSet<string>[] { new HashSet<string>(), new HashSet<string>()};
    var evaluate = new List<(string key, long value)>[] { new List<(string key, long value)>() };
    evaluate[0].Add(("99999999999999", RunProgram(S.ToArray(), (0, 0, 0, 0), "99999999999999").c.z));
    evaluate[1].Add(("11111111111111", RunProgram(S.ToArray(), (0, 0, 0, 0), "11111111111111").c.z));

    for (int k = 0; k < 2; k++)
    {
        bool newAdded = false;
        while (best[k].Count == 0 || newAdded)
        {
            newAdded = false;
            var neigbours = new Dictionary<string, long>();
            foreach (var e in evaluate[k])
            {
                for (int j = 13; j >= 0; j--)
                {
                    for (int i = 1; i < 10; i++)
                    {
                        var test = ChangeParameter(e.key, j, i);
                        if (!neigbours.ContainsKey(test))
                        {
                            var x = RunProgram(S.ToArray(), (0, 0, 0, 0), test);
                            neigbours.Add(test, Math.Abs(x.c.z));
                            if (x.c.z == 0 && !best[k].Contains(test))
                            {
                                newAdded = true;
                                best[k].Add(test);
                            }
                        }
                    }
                }
            }
            evaluate[k] = neigbours.OrderBy(v => v.Value)
                .Select(v => (v.Key, v.Value)).Take(500).ToList();
        }
    }
   
    string answer1 = best[0].ToList().OrderByDescending(v => v).First();
    string answer2 = best[1].ToList().OrderBy(v => v).First();

    w(1, answer1, "");
    w(2, answer2, "");
}

string ChangeParameter(string input, int location, int value)
{
    var c = input.ToArray();
    c[location] = (char)(value + '0');
    return new string(c);
}

(int w, int x, int y, int z) SetValue(string variable, int value, (int w, int x, int y, int z) variables )
{
    if (variable == "w") variables.w = value;
    else if (variable == "x") variables.x = value;
    else if (variable == "y") variables.y = value;
    else if (variable == "z") variables.z = value;
    return variables;
}

int GetValue(string variable, (int w, int x, int y, int z) variables)
{
    if (variable == "w") return variables.w;
    if (variable == "x") return variables.x;
    if (variable == "y") return variables.y;
    if (variable == "z") return variables.z;
    return 0;
}

bool IsVariable(string v) { return (v == "w" || v == "x" || v == "y" || v == "z"); }
int[] intArray(string s) { return s.Select(c => (int)(c - '0')).ToArray(); }

((int w, int x, int y, int z) c, int error) RunProgram (string[] program, (int w, int x, int y, int z) variables, string modelnumber)
{
    var input = intArray(modelnumber);
    int i = 0;
    foreach (string s in program)
    {
        var p1 = s.Split(' ');
        if (p1[0] == "inp")
        {
            variables = SetValue(p1[1], input[i], variables);
            i++;
        }
        else
        {
            int a = GetValue(p1[1], variables);
            int b = IsVariable(p1[2]) ? GetValue(p1[2], variables) : int.Parse(p1[2]);

            if (p1[0] == "add")
            {
                variables = SetValue(p1[1], a + b, variables);
            }
            else if (p1[0] == "mul")
            {
                variables = SetValue(p1[1], a * b, variables);
            }
            else if (p1[0] == "div")
            {
                if (b > 0) variables = SetValue(p1[1], a / b, variables);
            }
            else if (p1[0] == "mod")
            {
                if (a > 0 && b >= 0) variables = SetValue(p1[1], a % b, variables);
            }
            else if (p1[0] == "eql")
            {
                variables = SetValue(p1[1], a == b ? 1 : 0, variables);
            }
        }
    }
    return (variables, 0);
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
