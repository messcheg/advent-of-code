Run();

void Run()
{
    //string inputfile = @"..\..\..\example_input.txt";
    string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 590784;
    long supposedanswer2 = 2758514936282235;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var instructions = new (bool turnOn, (long from, long to) x, (long from, long to) y, (long from, long to) z)[S.Count];
    int i = 0;
    foreach (string s in S)
    {
        var s1 = s.Split(' ');
        instructions[i].turnOn = s1[0] == "on";
        s1 = s1[1].Split(',');
        var s2 = s1[0].Split('=')[1].Split("..");
        instructions[i].x.from = long.Parse(s2[0]);
        instructions[i].x.to = long.Parse(s2[1]);
        s2 = s1[1].Split('=')[1].Split("..");
        instructions[i].y.from = long.Parse(s2[0]);
        instructions[i].y.to = long.Parse(s2[1]);
        s2 = s1[2].Split('=')[1].Split("..");
        instructions[i].z.from = long.Parse(s2[0]);
        instructions[i].z.to = long.Parse(s2[1]);
        i++;
    }

    /* 
    i = 0;
    CubeArea cube = new CubeArea() { size = 1 , restriction = (true, -50,50)};
    foreach (var instr in instructions)
    {
        if (instr.turnOn)
        {
            cube.Fit(instr.x.from, instr.y.from, instr.z.from, instr.x.to, instr.y.to, instr.z.to);
            cube.Add(instr.x.from, instr.y.from, instr.z.from, instr.x.to, instr.y.to, instr.z.to);
        }
        else
        {
            cube.Delete(instr.x.from, instr.y.from, instr.z.from, instr.x.to, instr.y.to, instr.z.to);
        }

//      Console.WriteLine(S[i]);
//      Console.WriteLine("Counter: " + cube.Count());

        i++;
    }

    answer1 = cube.Count();

    */

    var blocks = new List<(bool sign, long[] from, long[] to)>();
    foreach (var instr in instructions)
    {
        int cnt = blocks.Count;
        var from = new long[] { instr.x.from, instr.y.from, instr.z.from };
        var to = new long[] { instr.x.to, instr.y.to, instr.z.to };
        if (instr.turnOn) blocks.Add((instr.turnOn, from ,to));

        for (int j = 0; j< cnt; j++)
        {
            var b = blocks[j];
            var res = Overlap((from, to), (b.from, b.to));
            if (res.to != null) blocks.Add((!b.sign, res.from, res.to));
        }
    }

    answer2 = 0;
    foreach(var b in blocks)
    {
        long x = b.to[0] - b.from[0] + 1;
        long y = b.to[1] - b.from[1] + 1;
        long z = b.to[2] - b.from[2] + 1;
        long count = x * y * z;
        answer2 = b.sign ? answer2 + count : answer2 - count;

        //Console.WriteLine((b.sign ? "Plus:  (" : "Minus: (" )
        //    + b.from[0] + ", " + b.from[1] + ", " + b.from[2] + "), ("
        //    + b.to[0] + ", " + b.to[1] + ", " + b.to[2] + ")");
    }

    w(1, answer1, supposedanswer1);
    w(2, answer2, supposedanswer2);
}

(long[] from, long[] to) Overlap((long[] from, long[] to) a, (long[] from, long[] to) b)
{
    var from = a.from.ToArray();
    var to = a.to.ToArray(); ;
    for (int i = 0; i < 3; i++)
        {
            if (from[i] > b.to[i]) return (null, null); //no overlap
            else if (to[i] < b.from[i]) return (null, null); //no overlap
            if (from[i] < b.from[i]) from[i] = b.from[i];
            if (to[i] > b.to[i]) to[i] = b.to[i] ;
        }
    return (from, to);  
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
class CubeArea
{
    public int size = 1;
    public (int x, int y, int z) offset = (0,0,0);  
    public bool isOn;
    public CubeArea[,,] subareas;

    public (bool doit, int min, int max) restriction;
   
    public int Restrict(int k)
    {
        if (!restriction.doit) return k;
        if (k < restriction.min) return restriction.min;
        if (k > restriction.max) return restriction.max;
        return k;
    }
    public void Fit(int xfrom, int yfrom, int zfrom, int xto, int yto, int zto)
    {
        while (Restrict(xfrom) < offset.x || Restrict(xto) >= offset.x + size ||
               Restrict(yfrom) < offset.y || Restrict(yto) >= offset.y + size ||
               Restrict(zfrom) < offset.z || Restrict(zto) >= offset.z + size)
        {
            CubeArea[,,] newsubcubes;
            if (isOn || subareas != null)
            {
                newsubcubes = new CubeArea[3, 3, 3];
                newsubcubes[1, 1, 1] = new CubeArea() { size = size, isOn = isOn, subareas = subareas, offset = offset, restriction = restriction };
                this.subareas = newsubcubes;
            }
            offset = (offset.x - size, offset.y - size, offset.z - size);
            size = size * 3;
        }

    }
    public void Add(int xfrom, int yfrom, int zfrom, int xto, int yto, int zto)
    {
        if (NotInRestrictionZone(xfrom, yfrom, zfrom, xto, yto, zto)) return;
        xfrom = Restrict(xfrom); 
        yfrom = Restrict(yfrom); 
        zfrom = Restrict(zfrom); 
        xto   = Restrict(xto  ); 
        yto   = Restrict(yto  );
        zto   = Restrict(zto  );

        if (isOn) return;
        var parameters = new int[][] { new int[] {offset.x, xfrom , xto }, 
                     new int [] {offset.y, yfrom, yto }, 
                     new int [] {offset.z, zfrom, zto } }; 

        var skipareas = new bool[3, 3]; 
        bool on = true;
        for (int i = 0; i < 3; i++)
        {
            int a = parameters[i][0];
            int from = parameters[i][1];
            int to = parameters[i][2];
            if (a < from || to < a + size - 1) on = false;
            int start = (from - a) * 3 / size;
            int end = (to - a) * 3 / size;
            skipareas[i, 0] = start > 0;
            skipareas[i, 1] = start > 1 || end < 1;
            skipareas[i, 2] = end < 2;
        }

        if (on)
        {
            isOn = true;
            this.subareas = null;
        }
        else
        {
            for (int x = 0; x<3;x++)
                for (int y = 0; y < 3; y++)
                    for (int z = 0; z < 3; z++)
                    {
                        if (!skipareas[0,x] && !skipareas[1,y] && !skipareas[2,z])
                        {
                            if (subareas == null) subareas = new CubeArea[3, 3, 3];
                            CubeArea a = subareas[x, y, z];
                            int sd3 = size / 3;
                            if (a == null) 
                                a = subareas[x, y, z] = new CubeArea()
                                {
                                    size = sd3,
                                    offset = (offset.x + x * sd3, offset.y + y * sd3, offset.z + z * sd3),
                                    isOn = false || sd3==1,
                                    subareas = null
                                };
                            if (sd3 > 1 && !a.isOn) a.Add(xfrom, yfrom, zfrom, xto, yto, zto);
                        }
                    }
            CheckAllOn();
        }
    }

    private bool NotInRestrictionZone(int xfrom, int yfrom, int zfrom, int xto, int yto, int zto)
    {   if (restriction.doit) return 
            (
               xfrom < restriction.min && xto < restriction.min
            || yfrom < restriction.min && yto < restriction.min
            || zfrom < restriction.min && zto < restriction.min
            || xfrom > restriction.max && xto > restriction.max
            || yfrom > restriction.max && yto > restriction.max
            || zfrom > restriction.max && zto > restriction.max
            );
        return false;
    }

    public void CheckAllOn()
    {
        if (subareas == null) return;
        for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
                for (int z = 0; z < 3; z++)
                {
                    if (subareas[x, y, z] == null || !subareas[x, y, z].isOn) return;
                }
        isOn = true;
        subareas = null;
    }

    public void Delete(int xfrom, int yfrom, int zfrom, int xto, int yto, int zto)
    {
        if (subareas == null && !isOn) return;
        var parameters = new int[][] { new int[] {offset.x, xfrom , xto },
                     new int [] {offset.y, yfrom, yto },
                     new int [] {offset.z, zfrom, zto } };

        var skipareas = new bool[3, 3];
        bool off = true;
        for (int i = 0; i < 3; i++)
        {
            int a = parameters[i][0];
            int from = parameters[i][1];
            int to = parameters[i][2];
            if (a < from || to < a + size - 1) off = false;
            int start = (from - a) * 3 / size;
            int end = (to - a) * 3 / size;
            skipareas[i, 0] = start > 0;
            skipareas[i, 1] = start > 1 || end < 1;
            skipareas[i, 2] = end < 2;
        }

        if (off)
        {
            isOn = false;
            this.subareas = null;
        }
        else
        {
            if (subareas == null && isOn && size > 1)
            {
                subareas = new CubeArea[3, 3, 3];
                int sd3 = size / 3;
                for (int x = 0; x < 3; x++)
                    for (int y = 0; y < 3; y++)
                        for (int z = 0; z < 3; z++)
                        {
                            subareas[x, y, z] = new CubeArea()
                            {
                                size = sd3,
                                offset = (offset.x + x * sd3, offset.y + y * sd3, offset.z + z * sd3),
                                isOn = true,
                                subareas = null
                            };
                        }
                isOn = false;
            }
        
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                    for (int z = 0; z < 3; z++)
                    {
                        if (!skipareas[0, x] && !skipareas[1, y] && !skipareas[2, z])
                        {
                            if (subareas != null)
                            {
                                CubeArea a = subareas[x, y, z];
                                if (a != null) a.Delete(xfrom, yfrom, zfrom, xto, yto, zto);
                            }
                        }
                    }
            CheckAllOff();
        }
    }
    public void CheckAllOff()
    {
        if (subareas == null) return;
        for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
                for (int z = 0; z < 3; z++)
                {
                    if (subareas[x, y, z] != null) return;
                }
        isOn = false;
        subareas = null;
    }
    public long Count()
    {
        if (this.subareas == null) return isOn ? (long)size * size * size : 0;
        long cnt = 0;
        foreach (var sub in subareas) if (sub != null) cnt += sub.Count();
        return cnt;
    }
}