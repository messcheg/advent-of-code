using System.Diagnostics;

Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day11.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 10605;
    long supposedanswer2 = 2713310158;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = PlayGame(S, 3, 20); ;
    long answer2 = PlayGame(S, 1, 10000);

    stopwatch.Stop();
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

static long PlayGame(List<string> S, int div, int turns)
{ List<Monkey> monkeys = new List<Monkey>();
    int i = 0;
    long checkDiv = 1;
    while (i < S.Count)
    {
        var currentMonkey = new Monkey();
        i++; //monkeynumber
        var items = S[i].Substring(18).Split(',').Select(x => long.Parse(x));
        foreach (var item in items) { currentMonkey.items.Enqueue(item); }
        i++;
        var operation = S[i].Substring(13).Split(" ");
        currentMonkey.opleft = operation[2] == "old" ? -1 : int.Parse(operation[2]);
        currentMonkey.operation = operation[3][0];
        currentMonkey.opright = operation[4] == "old" ? -1 : int.Parse(operation[4]);
        i++;
        currentMonkey.testValue = int.Parse(S[i].Substring(21));
        checkDiv *= currentMonkey.testValue;
        i++;
        currentMonkey.throwTrue = int.Parse(S[i].Substring(29));
        i++;
        currentMonkey.throwFalse = int.Parse(S[i].Substring(30));
        i += 2;
        monkeys.Add(currentMonkey);
    }

    for (int j = 0; j < turns; j++)
    {
        foreach (Monkey m in monkeys)
        {
            int cnt = m.items.Count;
            for (int k = 0; k < cnt; k++)
            {
                var item = m.items.Dequeue();
                long valLeft = m.opleft == -1 ? item : m.opleft;
                long valRight = m.opright == -1 ? item : m.opright;
                long newVal = m.operation == '*' ? valLeft * valRight : valLeft + valRight;
                newVal = (newVal / div) % checkDiv;
                if (newVal % m.testValue == 0)
                {
                    monkeys[m.throwTrue].items.Enqueue(newVal);
                }
                else
                {
                    monkeys[m.throwFalse].items.Enqueue(newVal);
                }
                m.numInspections++;
            }
        }
    }

    var tops = monkeys.Select(m => m.numInspections).OrderByDescending(x => x).Take(2).ToArray();
    long answer = tops[0] * tops[1];
    return answer;
}

class Monkey
{
    public Queue<long> items = new Queue<long>();
    public long numInspections = 0;
    public int testValue = 0;
    public int throwTrue = 0;
    public int throwFalse = 0;
    public char operation = '*';
    public int opleft = -1;
    public int opright = -1;
}
