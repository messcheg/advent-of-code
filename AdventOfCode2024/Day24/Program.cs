using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true, 4);
Run(@"..\..\..\example1.txt", true, 2024);
Run(@"..\..\..\input.txt", false, 0, true);
//Run(@"E:\develop\advent-of-code-input\2024\day24.txt", false);

void Run(string inputfile, bool isTest, long supposedanswer1 = 0, bool doPartII = false)
{
    Stopwatch stopwatch = Stopwatch.StartNew();


    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    int i = 0;
    var wires = new Dictionary<string, char>();
    var allwires = new HashSet<string>();
    while (S[i] != "")
    {
        var s1 = S[i].Split(": ");
        wires.Add(s1[0], s1[1][0]);
        allwires.Add(s1[0]);
        i++;
    }

    var gates = new Dictionary<string, (string operation, string input1, string input2)>();
    i++;
    while (i < S.Count)
    {
        var s1 = S[i].Split(' ');
        gates.Add(s1[4], (s1[1], s1[0], s1[2]));
        if (!wires.ContainsKey(s1[0])) wires.Add(s1[0], ' ');
        if (!wires.ContainsKey(s1[2])) wires.Add(s1[2], ' ');
        if (!wires.ContainsKey(s1[4])) wires.Add(s1[4], ' ');
        allwires.Add(s1[0]);
        allwires.Add(s1[2]);
        allwires.Add(s1[4]);
        i++;
    }

    var undef = wires.Where(a => a.Value == ' ').Select(a => a.Key).ToList();
    bool allZs = false;
    while (!allZs)
    {
        var newUndef = new List<string>();
        allZs = true;
        foreach (var a in undef)
        {
            var gate = gates[a];
            var v1 = wires[gate.input1];
            var v2 = wires[gate.input2];
            if (v1 == ' ' || v2 == ' ')
            {
                newUndef.Add(a);
                if (a[0] == 'z') allZs = false;
            }
            else
            {
                switch (gate.operation)
                {
                    case "AND":
                        if (v1 == '1' && v2 == '1') wires[a] = '1'; else wires[a] = '0';
                        break;
                    case "OR":
                        if (v1 == '1' || v2 == '1') wires[a] = '1'; else wires[a] = '0';
                        break;
                    case "XOR":
                        if (v1 == '1' ^ v2 == '1') wires[a] = '1'; else wires[a] = '0';
                        break;
                }
            }
        }
    }

    foreach (var z in wires.Where(a => a.Key[0] == 'z').OrderByDescending(a => a.Key))
    {
        //Console.WriteLine(z.Key + ':' + z.Value);
        answer1 <<= 1;
        if (z.Value == '1') answer1 |= 1;
    }

    /*
    foreach (var z in wires.Where(a => a.Key[0] == 'z').OrderByDescending(a => a.Key))
    {
        string Determine(string wire)
        {
            if (gates.TryGetValue(wire, out var inputs))
            {
                var v1 = inputs.input1[0] == 'z' ? inputs.input1 : Determine(inputs.input1);
                var v2 = inputs.input2[0] == 'z' ? inputs.input2 : Determine(inputs.input2);
                return "[" + wire + ":( " + v1 + ' ' + inputs.operation + ' ' + v2 + ")]";
            }
            return wire;
        }
        Console.WriteLine(Determine(z.Key));
    }
    */

    string answer2 = "";
    string supposedanswer2 = string.Empty;

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

