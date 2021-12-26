Run();

void Run()
{
    long supposedanswer1 = 12521;
    long supposedanswer2 = 0000;
    string start = "bacdbcda"; //example
    //string start = "ddccabba"; //real
    long answer1 = 0;
    long answer2 = 0;

    string defaulthall = "...........";
    var stand = (defaulthall, start);
    var endstand = defaulthall + "aabbccdd";
    var solutioncosts = new Dictionary<string, (long costs, string[] path) >();
    solutioncosts.Add(stand.defaulthall + stand.start, (0, new string[] { defaulthall + "|" + stand.start }));
    var evaluate = new List<((string hall, string rooms) stand, long costs, (string hall, string rooms) previous)>();
    evaluate.Add((stand , 0, stand));
    var next = stand;
    bool deadend = true;
    while (evaluate.Count > 0 && !solutioncosts.ContainsKey(endstand))
    {
        if (deadend)
        {
            var n = evaluate.OrderBy(c => c.costs).First();
            next = n.stand;
            if (!solutioncosts.TryGetValue(next.defaulthall+next.start, out var costs))
            {
                var key = next.defaulthall + next.start;
                var nextkey = n.previous.hall + n.previous.rooms;
                solutioncosts.Add(key, (solutioncosts[nextkey].costs + n.costs,
                                      solutioncosts[nextkey].path.Append(next.defaulthall + "|" + next.start).ToArray()));
            }
            evaluate.Remove(n);
        }
        var moves = DeterminePossibleMoves(next).OrderBy(c => c.costs);
        deadend = true;

        foreach (var move in moves)
        {
            string key = move.stand.hall + move.stand.rooms;
            string nextkey = next.defaulthall + next.start;
            bool found = solutioncosts.TryGetValue(key, out var costs);
            bool cheaper = found ? costs.costs > solutioncosts[nextkey].costs + move.costs : false;
            if (!found || cheaper)
            {
                evaluate.Add((move.stand, solutioncosts[nextkey].costs + move.costs, next));
                
                if (deadend)
                {
                    if (cheaper)
                     solutioncosts[key] = (solutioncosts[nextkey].costs + move.costs,
                      solutioncosts[nextkey].path.Append(move.stand.hall + "|" + move.stand.rooms).ToArray());
                    else
                        solutioncosts.Add(key, (solutioncosts[nextkey].costs + move.costs,
                         solutioncosts[nextkey].path.Append(move.stand.hall + "|" + move.stand.rooms).ToArray()));

                    next = move.stand;
                    deadend = false;  
                }        
            }
            
        }
    }
    foreach (var s in solutioncosts[endstand].path)
    {
        Console.WriteLine(s);
    }

    answer1 = solutioncosts[endstand].costs;
    w(1, answer1, supposedanswer1);
    w(2, answer2, supposedanswer2);
}

(int from, int to, int costs, (string hall, string rooms) stand)[] DeterminePossibleMoves((string hall, string rooms) stand)
{
    // search for candidates to get out of the pods
    var result = new List<(int from, int to, int costs, (string hall, string rooms) stand)>(); 
    List<int> candidates = new List<int>();
    for (int i = 0; i<stand.rooms.Length;i += 2)
    {
        if (stand.rooms[i] == '.')
        {
            if (stand.rooms[i+1] != '.')
            {
                if (i != Home(stand.rooms[i + 1]))
                    candidates.Add(i + 1);
            }
        }
        else 
        {
            if (i != Home(stand.rooms[i]) || i != Home(stand.rooms[i+1]))
                candidates.Add(i);
        }
    }
    //Determine posibilities to go to
    foreach (var c in candidates)
    {
        int mountingpoint = (c / 2) * 2 + 2;

        //check if a pod is free
        int home = Home(stand.rooms[c]);
        if (stand.rooms[home] == '.' &&
            (stand.rooms[home + 1] == '.' ||
                stand.rooms[home + 1] == stand.rooms[c]))
        {
            int mountinto = home + 2;
            if (PathIsClear(mountinto, mountingpoint, stand.hall))
            {
                int c1 = stand.rooms[home + 1] == '.' ? home + 1 : home;
                int pathlength = Math.Abs(mountingpoint - mountinto) + 2 + c % 2 + c1 % 2;
                int costs = pathlength * CostsOf(stand.rooms[c]);
                result.Add((c, c1, costs, ApplyMovePod(stand, c, c1)));
            }
        }

        for (int i = 0; i < stand.hall.Length;i++)
        {
            if (stand.hall[i] == '.' && !new int[] { 2, 4, 6, 8 }.Contains(i))
            {
                if (PathIsClear(i,mountingpoint, stand.hall))
                {
                    int pathlength = Math.Abs(mountingpoint - i) + 1 + c % 2 ;
                    int costs = pathlength * CostsOf(stand.rooms[c]);
                    result.Add((c, i + 8, costs, ApplyMove(stand,c,i)));
                }
            }
        }
    }

    // Get candidates to move into pods
    for (int i = 0; i < stand.hall.Length; i++)
    {
        if (stand.hall[i] != '.')
        {
            int home = Home(stand.hall[i]);
            if (stand.rooms[home] == '.' &&
                (stand.rooms[home + 1] == '.' ||
                    stand.rooms[home + 1] == stand.hall[i]))
            {
                int mountingpoint = home + 2;
                if (PathIsClear(i, mountingpoint, stand.hall))
                {
                    int c = stand.rooms[home + 1] == '.' ? home + 1 : home;
                    int pathlength = Math.Abs(mountingpoint - i) + 1 + c % 2;
                    int costs = pathlength * CostsOf(stand.hall[i]);
                    result.Add((c, i + 8, costs, ApplyMove(stand, c, i)));
                }
            }
        };
    }
    return result.ToArray();
}

(string hall, string rooms) ApplyMovePod((string hall, string rooms) stand, int c, int c1)
{
    var h = stand.hall.ToCharArray();
    var r = stand.rooms.ToCharArray();
    (r[c], r[c1]) = (r[c1], r[c]);

    return (new string(h), new string(r));
}

(string hall, string rooms) ApplyMove((string hall, string rooms) stand, int roompos, int hallpos)
{
    var h = stand.hall.ToCharArray();
    var r = stand.rooms.ToCharArray();
    (h[hallpos], r[roompos]) = (r[roompos], h[hallpos]);

    return (new string(h), new string (r));
}
bool PathIsClear(int i, int mountingpoint, string hall)
{
    bool clear = true;
    if (mountingpoint > i)
    {
        for (int j = i+1; j < mountingpoint; j++)
        {
            if (hall[j] != '.') clear = false;
        }
    }
    else
    {
        for (int j = mountingpoint + 1; j < i; j++)
        {
            if (hall[j] != '.') clear = false;
        }
    }
    return clear;
}

int CostsOf(char v)
{
    switch (v)
    {
        case 'a': return 1;
        case 'b': return 10;
        case 'c': return 100;
        case 'd': return 1000;
        default: return 0;
    }
}

int Home(char candidate)
{
    return (candidate - 'a') * 2;
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
