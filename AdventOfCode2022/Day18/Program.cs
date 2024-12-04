using System.Diagnostics;
using System.Net.Security;

Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day18.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 64;
    long supposedanswer2 = 58;
    
    var S = File.ReadAllLines(inputfile).Select(c => c.Split(',').Select(n => int.Parse(n)).ToArray()).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var area = new Dictionary<(int x, int y, int z), byte>();
    int i = 0;
    while (i<S.Count)
    {
        var s = S[i];
        area[(s[0], s[1], s[2])] = 1;
        i++;
    }

    foreach (var cube in area.Keys)
    {
        if (!area.ContainsKey((cube.x - 1, cube.y, cube.z))) answer1++;
        if (!area.ContainsKey((cube.x + 1, cube.y, cube.z))) answer1++;
        if (!area.ContainsKey((cube.x, cube.y + 1, cube.z))) answer1++;
        if (!area.ContainsKey((cube.x, cube.y - 1, cube.z))) answer1++;
        if (!area.ContainsKey((cube.x, cube.y, cube.z - 1))) answer1++;
        if (!area.ContainsKey((cube.x, cube.y , cube.z + 1))) answer1++;
    }

    (int minx, int maxx, int miny, int maxy, int minz, int maxz) mx;
    mx.minx = area.Keys.Min(c => c.x);
    mx.miny = area.Keys.Min(c => c.y);
    mx.minz = area.Keys.Min(c => c.z);
    mx.maxx = area.Keys.Max(c => c.x);
    mx.maxy = area.Keys.Max(c => c.y);
    mx.maxz = area.Keys.Max(c => c.z);

    FLoodFromSurface(area, mx.minx, mx.minx, mx.miny, mx.maxy, mx.minz, mx.maxz, mx);
    FLoodFromSurface(area, mx.maxx, mx.maxx, mx.miny, mx.maxy, mx.minz, mx.maxz, mx);
    FLoodFromSurface(area, mx.minx, mx.maxx, mx.miny, mx.miny, mx.minz, mx.maxz, mx);
    FLoodFromSurface(area, mx.minx, mx.maxx, mx.maxy, mx.maxy, mx.minz, mx.maxz, mx);
    FLoodFromSurface(area, mx.minx, mx.maxx, mx.miny, mx.maxy, mx.minz, mx.minz, mx);
    FLoodFromSurface(area, mx.minx, mx.maxx, mx.miny, mx.maxy, mx.maxz, mx.maxz, mx);

    foreach (var cube in area.Keys)
    {
        if (area[(cube.x, cube.y, cube.z)] == 1)
        {
            if (IsWater(area, cube.x + 1, cube.y, cube.z, mx)) answer2++;
            if (IsWater(area, cube.x - 1, cube.y, cube.z, mx)) answer2++;
            if (IsWater(area, cube.x, cube.y + 1, cube.z, mx)) answer2++;
            if (IsWater(area, cube.x, cube.y - 1, cube.z, mx)) answer2++;
            if (IsWater(area, cube.x, cube.y, cube.z + 1, mx)) answer2++;
            if (IsWater(area, cube.x, cube.y, cube.z - 1, mx)) answer2++;
        }
    }

    stopwatch.Stop();
    Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
    Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);
}

void FLoodFromSurface(Dictionary<(int x, int y, int z), byte> area, int x1, int x2, int y1, int y2, int z1, int z2, (int minx, int maxx, int miny, int maxy, int minz, int maxz) mx)
{
    for (int x = x1; x <= x2; x++)
        for (int y = y1; y <= y2; y++)
            for (int z = z1; z <= z2; z++)
                Flood(area, x, y, z, mx);
}
void Flood(Dictionary<(int x, int y, int z), byte> area, int x, int y, int z, (int minx, int maxx, int miny, int maxy, int minz, int maxz) mx)
{
    if (area.ContainsKey((x, y, z))) return;

    Queue<(int x, int y, int z)> q = new Queue<(int x, int y, int z)>();
    q.Enqueue((x, y, z));
    while (q.Count > 0)
    {
        var c = q.Dequeue();
        if (!(
            c.x < mx.minx || c.x > mx.maxx || 
            c.y < mx.miny || c.y > mx.maxy || 
            c.z < mx.minz || c.z > mx.maxz || 
            area.ContainsKey((c.x, c.y, c.z))))
        {
            area[(c.x, c.y, c.z)] = 2;
            if (c.x > mx.minx) q.Enqueue(( c.x - 1, c.y, c.z));
            if (c.x < mx.maxx) q.Enqueue(( c.x + 1, c.y, c.z));
            if (c.y > mx.miny) q.Enqueue(( c.x, c.y - 1, c.z));
            if (c.y < mx.maxy) q.Enqueue(( c.x, c.y + 1, c.z));
            if (c.z > mx.minz) q.Enqueue(( c.x, c.y, c.z - 1));
            if (c.z < mx.maxz) q.Enqueue(( c.x, c.y, c.z + 1));
        }
    }
}

bool IsWater(Dictionary<(int x, int y, int z), byte> area, int x, int y, int z, (int minx, int maxx, int miny, int maxy, int minz, int maxz) mx)
{
    if (x < mx.minx || x > mx.maxx || y < mx.miny || y > mx.maxy || z < mx.minz || z > mx.maxz) return true;
    return area.ContainsKey((x, y, z)) && area[(x, y, z)] == 2;
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
