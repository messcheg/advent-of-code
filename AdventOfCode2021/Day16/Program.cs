Run();

void Run()
{
    //string inputfile = @"..\..\..\example_input.txt";
    string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 31;
    
    var S = File.ReadAllLines(inputfile).ToList();

    long answer1 = 0;
    ulong answer2 = 0;

    var read = new BitRead(S[0]);

    var P = read.nextPackage();

    answer1 = P.SumVersions();
    answer2 = P.value();

    //P.DisplayPackege();
    P.DisplaySum();

    w(1, answer1, supposedanswer1);
    //w(2, answer2, supposedanswer2);
    Console.WriteLine("{0:X}", answer2);
    Console.WriteLine( answer2);

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

public class Pack
{
    public int version;
    public int typeid;
    public byte[] litterals;
    public List<Pack> Subpackets;

    public bool isOperator() { return typeid != 4; }

    public int SumVersions()
    {
        int restult = version;
        if (Subpackets != null) foreach (var P in Subpackets) restult += P.SumVersions();
        return restult;
    }

    public ulong value()
    {
        ulong value = 0;
        switch (typeid)
        {
            case 0:
                {
                    foreach (var P in Subpackets) value += P.value();
                    return value;
                }
            case 1:
                {
                    value = 1;
                    foreach (var P in Subpackets) value *= P.value();
                    return value;
                }
            case 2: return Subpackets.Min(P => P.value());
            case 3: return Subpackets.Max(P => P.value());
            case 4:
                {
                    foreach (var b in litterals) value = (value << 4) | b;
                    return value;
                }
            case 5: return (ulong)(Subpackets[0].value() > Subpackets[1].value() ? 1 : 0);
            case 6: return (ulong)(Subpackets[0].value() < Subpackets[1].value() ? 1 : 0);
            case 7: return (ulong)(Subpackets[0].value() == Subpackets[1].value() ? 1 : 0);
            default: return 0;
        };
    }

    public void DisplayPackege(int pad = 10)
    {
        Console.WriteLine("{          ".PadLeft(pad));
        Console.WriteLine("version: ".PadLeft(pad) + version);
        Console.WriteLine("type: ".PadLeft(pad) + typeid);
        Console.WriteLine("value: ".PadLeft(pad) + value());
        Console.WriteLine( new string[] { "sum", "product", "min", "max", "litteral", "greater than", "less than", "equal" }[typeid].PadLeft(pad));
        int pad1 = pad + 3;
        if (!isOperator())
        {
            Console.Write("".PadLeft(pad1));
            foreach (var l in litterals) Console.Write("0123456789ABCDEF"[l]);
            Console.WriteLine();
        }
        else
        {
            foreach (Pack P in Subpackets) P.DisplayPackege(pad1);   
        }
        Console.WriteLine("}          ".PadLeft(pad));
    }
    public void DisplaySum()
    {
        if (!isOperator())
        {
            Console.Write(value());
        }
        else
        {
            Console.Write(new string[] { "sum", "product", "min", "max", "litteral", "gt", "lt", "equal" }[typeid]);
            Console.Write("(");
            Subpackets[0].DisplaySum();
            foreach (Pack P in Subpackets.Skip(1))
            {
                Console.Write(", ");
                P.DisplaySum();
            }
            Console.Write(")");
        }
    }
}

class BitRead
{
    string S;

    public BitRead(string s)
    {
        S = s;
    }
    int nibble = 0;
    int bit = 0;

    public bool nextbit()
    {
        bool val = false;
        switch (bit)
        {
            case 0: val = (currentnibble() & 0x8) == 0x8; break;
            case 1: val = (currentnibble() & 0x4) == 0x4; break;
            case 2: val = (currentnibble() & 0x2) == 0x2; break;
            case 3: val = (currentnibble() & 0x1) == 0x1; break;
            default: val = false; break;
        }
        bit++;
        if (bit == 4)
        {
            bit = 0;
            nibble++;
        }
        return val;
    }

    public int position()
    {
        return nibble * 4 + bit;
    }
    public bool ready()
    {
        return nibble == S.Length;
    }
    public byte nextbyte(int len)
    {
        var val = 0;
        for (int i = 0; i<len; i++)
        {
            val <<= 1;
            if (nextbit()) val |= 0x1;
        }
        return (byte)val;
    }

    public int nextint(int len)
    {
        int val = 0;
        if (len > 8) val = nextbyte(len - 8) << 8;
        return val | nextbyte(Math.Min(len ,8));
    }

    byte currentnibble()
    {
        var c = S[nibble];
        if ('0' <= c && c <= '9') return (byte) (c - '0');
        if ('A' <= c && c <= 'F') return (byte) (c - 'A' + 10);
        return 255;
    }

    public Pack nextPackage(bool padding = true)
    {
        var P = new Pack();
        P.version = nextbyte(3);
        P.typeid = nextbyte(3);
        if (P.isOperator())
        {
            P.Subpackets = new List<Pack>();
            bool readrecords = nextbit();
            if (readrecords)
            {
                int size = nextint(11);
                for (int i=0; i< size;i++)
                {
                    P.Subpackets.Add(nextPackage(false));
                }
            }
            else
            {
                int size = nextint(15);
                int until = position() + size;
                while (position() < until)
                {
                    P.Subpackets.Add(nextPackage(false));
                }
            }

        }
        else
        {
            var ltterals = new List<byte>();
            while(nextbit())
            {
                ltterals.Add(nextbyte(4));
            }
            ltterals.Add(nextbyte(4));
            P.litterals = ltterals.ToArray();
        }
        if (padding) doPadding();
        return P;
    }

    public void doPadding()
    {
        if(bit != 0)
        {
            bit = 0;
            nibble++;
        }
    }

}
    