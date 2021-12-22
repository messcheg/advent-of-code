Run();

void Run()
{
    string inputfile = @"..\..\..\example_input.txt";
    //string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 590784;
    long supposedanswer2 = 0000;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var instructions = new (bool turnOn, (int from, int to) x, (int from, int to) y, (int from, int to) z)[S.Count];
    int i = 0;
    foreach(string s in S)
    {
        var s1 = s.Split(' ');
        instructions[i].turnOn = s1[0] == "on";
        s1 = s1[1].Split(',');
        var s2 = s1[0].Split('=')[1].Split("..");
        instructions[i].x.from = int.Parse(s2[0]);
        instructions[i].x.to = int.Parse(s2[1]);
        s2 = s1[1].Split('=')[1].Split("..");
        instructions[i].y.from = int.Parse(s2[0]);
        instructions[i].y.to = int.Parse(s2[1]); 
        s2 = s1[2].Split('=')[1].Split("..");
        instructions[i].z.from = int.Parse(s2[0]);
        instructions[i].z.to = int.Parse(s2[1]);
        i++;
    }

    var minicubes = new List<Cube>();
    for(int j=0; j<instructions.Length; j++)
    {
        var c = instructions[j];
        if (c.turnOn)
        {
            var cubestoadd = new Queue<Cube>();
            cubestoadd.Enqueue(new Cube() { from = (c.x.from, c.y.from, c.z.from), to = (c.x.to, c.y.to, c.z.to)});
            while (cubestoadd.Count > 0)
            {
                var currentcube = cubestoadd.Dequeue();
                bool no_overlaps = true;
                foreach (var mini in minicubes)
                {
                    var res = currentcube.CutResult(mini);
                    if (res.Length > 1)
                    {
                        no_overlaps = false;
                        foreach (var cube in res) cubestoadd.Enqueue(cube);
                    }
                }
                if (no_overlaps) minicubes.Add(currentcube);
            }
        }
        else
        {
            var newMinicubes = new List<Cube>();
            var clearArea = new Cube() { from = (c.x.from, c.y.from, c.z.from), to = (c.x.to, c.y.to, c.z.to) };
            foreach(var currentcube in minicubes)
            {
               var res = currentcube.CutResult(clearArea);
               foreach( var c1 in res)
                {
                    newMinicubes.Add(c1);
                }
            }
            minicubes = newMinicubes;
        }
        }

    answer1 = minicubes.Sum(c => c.Count());

    w(1, answer1, supposedanswer1);
    w(2, answer2, supposedanswer2);
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

class Cube
{
    public (int x, int y, int z) from;
    public (int x, int y, int z) to;
    public bool Overlaps(Cube other)
    {
        return Between(this.from, other.from, other.to) || Between(this.to, other.from, other.to);
    }
    public bool EnclosedBy(Cube other)
    {
        return Between(this.from, other.from, other.to) && Between(this.to, other.from, other.to);
    }
    public bool Encloses(Cube other)
    {
        return Between(other.from, this.from, this.to) && Between(other.to, this.from, this.to);
    }

    public long Count()
    {
        return (to.x - from.x + 1) * (to.y - from.y + 1) * (to.z - from.z + 1);
    }
    public Cube[] CutResult (Cube other)
    {
        if (EnclosedBy(other)) return new Cube[0];
        
        List<Cube> result = new List<Cube>();
        result.Add(this);
        result = CutX(result, other);
        result = CutY(result, other);
        result = CutZ(result, other);
        return result.Where(c => !c.EnclosedBy(other)).ToArray();
    }
    private List<Cube> CutX(List<Cube> input, Cube other)
    {
        List<Cube> output = new List<Cube>();
        foreach (var c in input)
        {
            if (Between(other.from.x, c.from.x, c.to.x))
            {
                if (Between(other.to.x, c.from.x, c.to.x))
                {
                    if (c.from.x != other.from.x) output.Add(new Cube() { from = c.from, to = (other.from.x -1, c.to.y, c.to.z) });
                    output.Add(new Cube() { from = (other.from.x, c.from.y, c.from.z) , to = (other.to.x, c.to.y, c.to.z) });
                    if (c.to.x != other.to.x) output.Add(new Cube() { from = (other.to.x + 1, c.from.y, c.from.z), to = c.to});
                }
                else
                {
                    if (c.from.x != other.from.x)  output.Add(new Cube() { from = c.from, to = (other.from.x - 1, c.to.y, c.to.z) });
                    output.Add(new Cube() { from = (other.from.x, c.from.y, c.from.z), to = c.to});
                }
            }
            else if (Between(other.to.x, c.from.x, c.to.x))
            {
                output.Add(new Cube() { from = c.from, to = (other.to.x, c.to.y, c.to.z) });
                if (c.to.x != other.to.x) output.Add(new Cube() { from = (other.to.x + 1, c.from.y, c.from.z), to = c.to });
            }
            else output.Add(c);
        }
        return output;
    }
    private List<Cube> CutY(List<Cube> input, Cube other)
    {
        List<Cube> output = new List<Cube>();
        foreach (var c in input)
        {
            if (Between(other.from.y, c.from.y, c.to.y))
            {
                if (Between(other.to.y, c.from.y, c.to.y))
                {
                    if (c.from.y != other.from.y) output.Add(new Cube() { from = c.from, to = (c.to.x, other.from.y - 1, c.to.z) });
                    output.Add(new Cube() { from = (c.from.x, other.from.y, c.from.z), to = (c.to.x, other.to.y, c.to.z) });
                    if (c.to.y != other.to.y) output.Add(new Cube() { from = (c.from.x, other.to.y + 1, c.from.z), to = c.to });
                }
                else
                {
                    if (c.from.y != other.from.y) output.Add(new Cube() { from = c.from, to = (c.to.x, other.from.y - 1, c.to.z) });
                    output.Add(new Cube() { from = (c.from.x, other.from.y, c.from.z), to = c.to });
                }
            }
            else if (Between(other.to.y, c.from.y, c.to.y))
            {
                output.Add(new Cube() { from = c.from, to = (c.to.x, other.to.y, c.to.z) });
                if (c.to.y != other.to.y) output.Add(new Cube() { from = (c.from.x, other.to.y + 1, c.from.z), to = c.to });
            }
            else output.Add(c);
        }
        return output;
    }

    private List<Cube> CutZ(List<Cube> input, Cube other)
    {
        List<Cube> output = new List<Cube>();
        foreach (var c in input)
        {
            if (Between(other.from.z, c.from.z, c.to.z))
            {
                if (Between(other.to.z, c.from.z, c.to.z))
                {
                    if (c.from.z != other.from.z) output.Add(new Cube() { from = c.from, to = (c.to.x, c.to.y, other.from.z - 1) });
                    output.Add(new Cube() { from = (c.from.x, c.from.y, other.from.z), to = (c.to.x, c.to.y, other.to.z) });
                    if (c.to.z != other.to.z) output.Add(new Cube() { from = (c.from.x, c.from.y, other.to.z + 1), to = c.to });
                }
                else
                {
                    if (c.from.z != other.from.z) output.Add(new Cube() { from = c.from, to = (c.to.x, c.to.y, other.from.z - 1) });
                    output.Add(new Cube() { from = (c.from.x, c.from.y, other.from.z), to = c.to });
                }
            }
            else if (Between(other.to.z, c.from.z, c.to.z))
            {
                output.Add(new Cube() { from = c.from, to = (c.to.x, c.to.y, other.to.z) });
                if (c.to.z != other.to.z) output.Add(new Cube() { from = (c.from.x, c.from.y, other.to.z + 1), to = c.to });
            }
            else output.Add(c);
        }
        return output;
    }
    

    public bool Between((int x, int y, int z) a, (int x, int y, int z) from, (int x, int y, int z) to)
    {
        return Between(a.x, from.x, to.x) &&
           Between(a.y, from.y, to.y) &&
           Between(a.z, from.z, to.z);
    }
    public bool Between(int a, int from, int to)
    {
        return from <= a && to >= a;
    }
}

class CubeArea
{
    public int level;
    public int size;
    public int x;
    public int y;
    public int z;

    public bool isOneColor;
    public CubeArea[2,2,2] subareas;
    public int color;

    public int Add(int xfrom, int yfrom, int zfrom, int xto, int yto, int zto)
    {
        while (! (x <= xfrom && x + size > xto && 
            y <= yfrom && y + size > yto &&
            z <= yfrom && z + size > zto))
        {
            var newsubcubes = new CubeArea[2,2,2];
            for (i)

        }
    }

}