
Run();

void Run()
{
    //string inputfile = @"..\..\..\example_input.txt";
    string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 79;
    long supposedanswer2 = 0000;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    var scanresults = new List<List<(int x, int y, int z)>>();
    var beacons = new HashSet<(int x, int y, int z)>();
    int currentscanner = 0;
    foreach(string s in S)
    {
        if (s == "") continue;
        if (s.Substring(0,3) == "---")
        {
            currentscanner = int.Parse(s.Split(' ')[2]);
            scanresults.Add(new List<(int x,int y,int z)>());
        }
        else
        {
            var s1 = s.Split(',');
            if (s1.Length == 3) scanresults[currentscanner].Add((int.Parse(s1[0]), int.Parse(s1[1]), int.Parse(s1[2])));
        }
    }

    var projectedscan = new List<(int x, int y, int z)>[scanresults.Count];
    projectedscan[0] = scanresults[0];
    var scanners = new (int x, int y, int z)[scanresults.Count];
    scanners[0] = (0, 0, 0);
    var newfound = new int[1];
    newfound[0] = 0;
    int found = 1;
    foreach (var bc in scanresults[0]) beacons.Add(bc);
    while (found < scanresults.Count && newfound.Length > 0)
    {
        var recentlyfound = new List<int>();
        foreach (int sc in newfound)
            for (int sc2 = 0; sc2 < scanresults.Count; sc2++)
            {
                if (projectedscan[sc2] == null)
                {
                    //var pr = GetOverlaps(beacons.ToList(), scanresults[sc2]);
                    var pr = GetOverlaps(projectedscan[sc], scanresults[sc2]);
                    if (pr.match)
                    {
                        found++;
                        projectedscan[sc2] = scanresults[sc2].Select(s1 => Convert(s1, pr.rotation, pr.distance, pr.face)).ToList();
                        scanners[sc2] = Convert((0,0,0), pr.rotation, pr.distance, pr.face);
                        recentlyfound.Add(sc2);
                        foreach (var bc in projectedscan[sc2]) beacons.Add(bc);
                    }
                }
            }
        newfound = recentlyfound.ToArray();
    }

    answer1 = beacons.Count;

    int maxDistance = 0;
    for (int i = 0; i < scanners.Length; i++)
        for (int j = i + 1; j < scanners.Length; j++)
            maxDistance = Math.Max(maxDistance,
                Math.Abs(scanners[i].x - scanners[j].x) +
                Math.Abs(scanners[i].y - scanners[j].y) +
                Math.Abs(scanners[i].z - scanners[j].z)
                );

    answer2 = maxDistance;

    w(1, answer1, supposedanswer1);
    w(2, answer2, supposedanswer2);
}

(bool match, rotation rotation, (int x, int y, int z) distance, (bool x, bool y, bool z) face ) GetOverlaps(List<(int x, int y, int z)> scan1, List<(int x, int y, int z)> scan2)
{
    var overlapx = GetOverlapInLine(scan1.Select(s => s.x), scan2.Select(s => s.x));
    var overlapy = GetOverlapInLine(scan1.Select(s => s.y), scan2.Select(s => s.y));
    var overlapz = GetOverlapInLine(scan1.Select(s => s.z), scan2.Select(s => s.z));
    var r = rotation.xyz;
    var result = Check(scan1, scan2, r , overlapx, overlapy, overlapz);
    
    if (!result.match && overlapx.Length > 0)
    {
        overlapy = GetOverlapInLine(scan1.Select(s => s.y), scan2.Select(s => s.z));
        overlapz = GetOverlapInLine(scan1.Select(s => s.z), scan2.Select(s => s.y));
        r = rotation.xzy;
        result = Check(scan1, scan2, r, overlapx, overlapy, overlapz);
    }
    if (!result.match && overlapy.Length > 0)
    {
        overlapx = GetOverlapInLine(scan1.Select(s => s.x), scan2.Select(s => s.z));
        overlapz = GetOverlapInLine(scan1.Select(s => s.z), scan2.Select(s => s.x));
        r = rotation.zyx;
        result = Check(scan1, scan2, r, overlapx, overlapy, overlapz);
    }
    if (!result.match && overlapz.Length > 0)
    {
        overlapx = GetOverlapInLine(scan1.Select(s => s.x), scan2.Select(s => s.y));
        overlapy = GetOverlapInLine(scan1.Select(s => s.y), scan2.Select(s => s.x));
        r = rotation.yxz;
        result = Check(scan1, scan2, r, overlapx, overlapy, overlapz);
    }
    if (!result.match)
    {
        overlapx = GetOverlapInLine(scan1.Select(s => s.x), scan2.Select(s => s.y));
        overlapy = GetOverlapInLine(scan1.Select(s => s.y), scan2.Select(s => s.z));
        overlapz = GetOverlapInLine(scan1.Select(s => s.z), scan2.Select(s => s.x));
        r = rotation.yzx;
        result = Check(scan1, scan2, r, overlapx, overlapy, overlapz);
    }
    if (!result.match)
    {
        overlapx = GetOverlapInLine(scan1.Select(s => s.x), scan2.Select(s => s.z));
        overlapy = GetOverlapInLine(scan1.Select(s => s.y), scan2.Select(s => s.x));
        overlapz = GetOverlapInLine(scan1.Select(s => s.z), scan2.Select(s => s.y));
        r = rotation.zxy;
        result = Check(scan1, scan2, r, overlapx, overlapy, overlapz);
    }
    return (result.match, r, result.distance, result.face);
}

(bool match, (int x, int y, int z) distance, (bool x, bool y, bool z) face) 
    Check(List<(int x, int y, int z)> scan1, List<(int x, int y, int z)> scan2, rotation r, (int d, bool f)[] ox, (int d, bool f)[] oy, (int d, bool f)[] oz)
{
    var org = new HashSet<(int x, int y, int z)>(scan1);
    foreach (var px in ox)
        foreach (var py in oy)
            foreach (var pz in oz)
            {
                int count = 0;

                foreach (var sc in scan2.OrderBy(c => c.x)) 
                    if (org.Contains(Convert(sc, r, (px.d, py.d, pz.d), (px.f, py.f, pz.f))))
                        count++;
               
                if (count >= 12) return (true, (px.d, py.d, pz.d), (px.f, py.f, pz.f)); 
            }
    return (false,(0,0,0), (false,false, false));
}

(int distance, bool sign)[] GetOverlapInLine(IEnumerable<int> sx1, IEnumerable<int> sx2)
{
    var counters = new Dictionary<int, int>();
    var counters1 = new Dictionary<int, int>();
    foreach (var x1 in sx1)
        foreach (var x2 in sx2)
        {
        //if (Math.Abs(x1 - x2) <= 2000)
            if ( counters.TryGetValue(x1 - x2, out int counter))
                counters[x1 - x2] = counter + 1;
            else
                counters.Add(x1 - x2, 1);
        //if (Math.Abs(x1 + x2) <= 2000)
                if (counters1.TryGetValue(x1 + x2, out int counter1))
                counters1[x1 + x2] = counter1 + 1;
            else
                counters1.Add(x1 + x2, 1);
        }
    var matching = counters.Where(v => v.Value >= 12).ToArray();
    var matching1 = counters1.Where(v => v.Value >= 12).ToArray();
    int distance = 0;
    bool sign = false;
    var result = new (int distance, bool sign)[matching.Length + matching1.Length];
    for (int i = 0; i < matching.Length; i++) result[i] = (matching[i].Key, true);
    for (int i = 0; i < matching1.Length; i++) result[matching.Length + i] = (matching1[i].Key, false);

    return result;
}

(int x, int y, int z) Convert((int x, int y, int z) coordinate, rotation rotation, (int x, int y, int z) distance, (bool x, bool y, bool z) face)
{
    var c = coordinate;

    switch (rotation)
    {
        case rotation.yzx: c = (c.y, c.z, c.x); break;
        case rotation.xzy: c = (c.x, c.z, c.y); break;
        case rotation.yxz: c = (c.y, c.x, c.z); break;
        case rotation.zxy: c = (c.z, c.x, c.y); break;
        case rotation.zyx: c = (c.z, c.y, c.x); break;
        default: break;
    }

    c.x = face.x ? c.x + distance.x : distance.x - c.x;
    c.y = face.y ? c.y + distance.y : distance.y - c.y;
    c.z = face.z ? c.z + distance.z : distance.z - c.z;

    return c;
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
enum rotation { xyz, xzy, yzx, yxz, zyx, zxy }
