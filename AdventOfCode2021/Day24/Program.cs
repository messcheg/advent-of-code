Run();

void Run()
{
    //string inputfile = @"..\..\..\example_input.txt";
    string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 0000;
    long supposedanswer2 = 0000;

    var S = File.ReadAllLines(inputfile).ToList();
 
    //var formula = SimplifyProgram(S.ToArray());
    //Console.WriteLine(formula.ToString());

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
   
    string answer = best[0].ToList().OrderByDescending(v => v).First();
    

    w(1, answer1, "");
    w(2, answer2, "");
}

string ChangeParameter(string input, int location, int value)
{
    var c = input.ToArray();
    c[location] = (char)(value + '0');
    return new string(c);
}

string GetMiddle(string a, string b)
{
    var a1 = a.Select(c => (int)(c - '1')).ToArray();
    var b1 = b.Select(c => (int)(c - '1')).ToArray();
    ulong a2 = 0;
    ulong b2 = 0;
    ulong d = 1;
    for (int i = 0; i < 14; i++)
    { 
        a2 = a2 * 9 + (ulong)a1[i];
        b2 = b2 * 9 + (ulong)b1[i];
        d = d * 9;
    }
    ulong c2 = (a2 + b2) / 2;
    var c1 = new char[14];
    for (int i = 0; i<14;i++)
    {
        d = d / 9;
        int m = (int) (c2 / d);
        c1[i] = (char)(m + '1');
        c2 = c2 % d;
    }
    string c = new string(c1);
    return c;
}
string GetAdd(string a, long b)
{
    var a1 = a.Select(c => (int)(c - '1')).ToArray();
    ulong a2 = 0;
    ulong d = 1;
    for (int i = 0; i < 14; i++)
    {
        a2 = a2 * 9 + (ulong)a1[i];
        d = d * 9;
    }
    ulong c2 = b >= 0 ? a2 + (ulong) b: a2 - (ulong) -b ;
    var c1 = new char[14];
    for (int i = 0; i < 14; i++)
    {
        d = d / 9;
        int m = (int)(c2 / d);
        c1[i] = (char)(m + '1');
        c2 = c2 % d;
    }
    string c = new string(c1);
    return c;
}

string LowerNumber (int place, string input)
{
    bool ready = false;
    var x = input.ToArray();
    x[place]--;
    if (x[place] < '1') x[place] = '1';

    return new string(x) ;
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

bool IsVariable(string v)
{
    return (v == "w" || v == "x" || v == "y" || v == "z");
}

int[] intArray(string s)
{
    return s.Select(c => (int)(c - '0')).ToArray();
}

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
                if (b <= 0) ;
                //return (variables, 100 + i);
                else
                    variables = SetValue(p1[1], a / b, variables);
            }
            else if (p1[0] == "mod")
            {
                if (a < 0 || b <= 0) ;
                // return (variables, 100 + i);
                else
                    variables = SetValue(p1[1], a % b, variables);
            }
            else if (p1[0] == "eql")
            {
                variables = SetValue(p1[1], a == b ? 1 : 0, variables);
            }
        }
    }
    return (variables, 0);
}


(string w, string x, string y, string z) SimplifyProgram(string[] program)
{
    var formulas = new Dictionary<string, string>();
    string nul = "0";
    formulas.Add("w", nul);
    formulas.Add("x", nul);
    formulas.Add("y", nul);
    formulas.Add("z", nul);

    int i = 0;
    foreach (string s in program)
    {
        var p1 = s.Split(' ');
        if (p1[0] == "inp")
        {
            formulas[p1[1]] = "input[" + i + "]";
            i++;
        }
        else
        {
            string a = p1[1];
            string b = IsVariable(p1[2]) ? formulas[p1[2]]  : p1[2];
            bool digitb, digita;
            int valb, vala;
            digitb = int.TryParse(b, out valb);
            digita = int.TryParse(formulas[a], out vala);
            digita = digita ? formulas[a] == vala.ToString() : false;
            digitb = digitb ? b == valb.ToString() : false;
            var opena = digita ? "" : "(";
            var closea = digita ? "" : ")";
            var openb = digitb ? "" : "(";
            var closeb = digitb ? "" : ")";


            if (p1[0] == "add")
            {
                if (digitb && digita) formulas[a] = (vala + valb).ToString();
                else if (b != nul) formulas[a] = formulas[a] == nul ? b : formulas[a] + "+" + b;               
            }
            else if (p1[0] == "mul")
            {
                if (b == nul || formulas[a] == nul) formulas[a] = nul;
                else if (formulas[a] == "1") formulas[a] = b;
                else if (b == "1");
                else if (digitb && digita) formulas[a] = (vala * valb).ToString();
                else formulas[a] = opena + formulas[a] + closea+ "*" + openb + b + closeb; 
            }
            else if (p1[0] == "div")
            {
                if (formulas[a] == nul) formulas[a] = nul;
                if (digitb && digita) formulas[a] = (vala / valb).ToString();
                else if (formulas[a] == b) formulas[a] = "1";
                else if (b == "1") ;
                else formulas[a] = opena + formulas[a] + closea + "/" + openb + b + closeb; ;
            }
            else if (p1[0] == "mod")
            {
                if ((formulas[a] == nul) || (b == "1")) formulas[a] = nul;
                else if (digitb && digita) formulas[a] = (vala % valb).ToString();
                else if (formulas[a] == b) formulas[a] = nul;
                else formulas[a] = opena + formulas[a] + closea + "%" + openb + b + closeb; ;
            }
            else if (p1[0] == "eql")
            {
                if (formulas[a] == b) formulas[a] = "1";
                if (digitb && digita) formulas[a] = (vala == valb)? "1": nul;
                else if (b == nul  && formulas[a][^1] == ']') formulas[a] = nul;
                else if (digitb && (valb > 9 || valb < 1) && formulas[a][^1] == ']') formulas[a] = nul;
                else if (digita && (vala > 9 || vala < 1) && b[^1] == ']') formulas[a] = nul;
                else formulas[a] = opena + formulas[a] + closea + "==" + openb + b + closeb; ;
            }
        }
    }
    return (formulas["w"], formulas["x"], formulas["y"], formulas["z"]);
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
