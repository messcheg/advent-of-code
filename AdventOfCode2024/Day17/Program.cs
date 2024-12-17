using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example2.txt", true, false, true, false);

Run(@"..\..\..\example1.txt", true, false, true, false);
Run(@"..\..\..\example.txt", false, true, true, true);

void Run(string inputfile, bool isTest, bool caclulate, bool emulate, bool part2)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var S = File.ReadAllLines(inputfile).ToList();
    var prog = new Computer();
    var A = prog.A = int.Parse(S[0].Split(": ")[1]);
    prog.B = int.Parse(S[1].Split(": ")[1]);
    prog.C = int.Parse(S[2].Split(": ")[1]);
    prog.program = S[4].Substring(9).Split(',').Select(byte.Parse).ToList();

    Console.WriteLine("Answer1:");
    if (emulate)
    {
        prog.RunProgram();
        Console.Write("Emulated:   ");
        Console.WriteLine(String.Join(',', prog.output.Select(i => i.ToString())));
    }

    long getOUtput(long A1)
    {
        long B1 = A1 % 8;
        B1 = B1 ^ 4;
        long C = (A1 / (long)Math.Pow(2, B1)) % 8;
        B1 = B1 ^ C;
        B1 = B1 ^ 4;
        B1 = B1 % 8;
        return B1;
    }

    if (caclulate)
    {
        Console.Write("Calculated: ");
        while (A > 0)
        {
            long B = getOUtput((long)A);
            Console.Write(B.ToString() + ",");
            A = A >> 3;
        }
        Console.WriteLine();
    }

    if (part2)
    {
        Console.WriteLine("Answer 2 determination:");
        long answer2 = 0;
        var results = new HashSet<long>() { 0 };
        for (int i = prog.program.Count - 1; i >= 0; i--)
        {
            var nextHash = new HashSet<long>();
            foreach (var Ar in results)
            {
                var Ar1 = Ar << 3;
                for (int j = 0; j < 8; j++)
                {
                    var Ar2 = Ar1 ^ j;
                    bool ok = true;
                    for (int k = i; k < prog.program.Count; k++)
                    {

                        if (Ar2 != 0 && getOUtput(Ar2) == prog.program[k]) Ar2 = Ar2 >> 3;
                        else { ok = false; break; }
                    }
                    if (ok) nextHash.Add(Ar1 ^ j);
                }
            }
            results = nextHash;
        }
        answer2 = results.FirstOrDefault();

        foreach (long A3 in results)
        {
            Console.WriteLine(A3.ToString());
            var A4 = A3;
            Console.Write("Calculated: ");
            while (A4 > 0)
            {
                long B = getOUtput((long)A4);
                Console.Write(B.ToString() + ",");
                A4 = A4 / 8;
            }
            Console.WriteLine();

            Console.Write("Emulated:   ");
            prog.HardReset();
            prog.A = A3;
            prog.RunProgram();
            Console.WriteLine(String.Join(',', prog.output.Select(i => i.ToString())));
        }

        Aoc.w(2, answer2, 0, isTest);
    }
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

class Computer
{
    public long A { get; set; } = 0;
    public long B { get; set; } = 0;
    public long C { get; set; } = 0;

    public int IP = 0;
    public List<byte> program = new List<byte>();
    public List<byte> output = new List<byte>();
    public enum Instructions
    {
        _adv = 0,
        _bxl = 1,
        _bst = 2,
        _jnz = 3,
        _bxc = 4,
        _out = 5,
        _bdv = 6,
        _cdv = 7
    }

    public void RunProgram()
    {
        IP = 0;
        while (IP < program.Count)
        {
            byte opcode = program[IP];
            byte operand = program[IP + 1];
            switch ((Instructions)opcode)
            {
                case Instructions._adv:
                    {
                        long nom = A;
                        long denom = (int)Math.Pow(2, ComboValue(operand));
                        A = nom / denom;
                        IP += 2;
                        break;
                    }
                case Instructions._bxl:
                    {
                        B = B ^ operand;
                        IP += 2;
                        break;
                    }
                case Instructions._bst:
                    B = ComboValue(operand) % 8;
                    IP += 2;
                    break;
                case Instructions._jnz:
                    {
                        if (A > 0)
                        {
                            IP = operand;
                        }
                        else { IP += 2; }
                        break;
                    }
                case Instructions._bxc:
                    {
                        B = B ^ C;
                        IP += 2;
                        break;
                    }
                case Instructions._out:
                    {
                        output.Add((byte)(ComboValue(operand) & 7));
                        IP += 2;
                        break;
                    }
                case Instructions._bdv:
                    {
                        long nom = A;
                        long denom = (long)Math.Pow(2, ComboValue(operand));
                        B = nom / denom;
                        IP += 2;
                        break;
                    }
                case Instructions._cdv:
                    {
                        long nom = A;
                        long denom = (long)Math.Pow(2, ComboValue(operand));
                        C = nom / denom;
                        IP += 2;
                        break;
                    }
            }
        }
    }

    public long ComboValue(byte operand)
    {
        if (operand < 4) return operand;
        switch (operand)
        {
            case 4: return A;
            case 5: return B;
            case 6: return C;
        }
        return 0;
    }

    public void HardReset()
    {
        (A, B, C) = (0, 0, 0);
        output.Clear();
        IP = 0;
    }
}

