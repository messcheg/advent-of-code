using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\example1.txt", false);
//Run(@"E:\develop\advent-of-code-input\2024\day21.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 126384;
    long supposedanswer2 = 154115708116294;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var numKeypad = new string[] { "789", "456", "123", " 0A" };
    var dirKepad = new string[] { " ^A", "<v>" };
    var numKeys = "0123456789A";
    var dirKeys = "<^>vA";
    var numKeypadmoves = GetMoves(numKeys, numKeypad);
    var dirKeypadmoves = GetMoves(dirKeys, dirKepad);
    var allmoves = numKeypadmoves.Where((k) => !dirKeypadmoves.ContainsKey(k.Key)).Union(dirKeypadmoves).ToDictionary();

    //Console.WriteLine(numKeypadmoves.Count);
    var resultcache1 = new Dictionary<(int level, string key), long>();
    var resultcache2 = new Dictionary<(int level, string key), long>();

    foreach (var s in S)
    {
        long ShortestMoves(string current, int level, int stopatlevel, Dictionary<(int level, string key), long> resultcache)
        {
            if (current == "") return 0;
            if (resultcache.TryGetValue((level, current), out var cacheresult)) return cacheresult;
            if (level == stopatlevel)
            {
                var result = GetSequences(current, allmoves).Select(a => a.Length).Min();
                resultcache.Add((level, current), result);
                return result;
            }

            var firstA = current.IndexOf('A');
            var firstpart = current.Substring(0, firstA + 1);
            var secondpart = current.Substring(firstA + 1);
            long shortest = -1;
            var possibilities = GetSequences(firstpart, allmoves);
            foreach (var seq in possibilities)
            {
                long count = ShortestMoves(seq, level + 1, stopatlevel, resultcache);
                if (shortest > count || shortest == -1) shortest = count;
            }
            if (secondpart != "") shortest += ShortestMoves(secondpart, level, stopatlevel, resultcache);
            resultcache.Add((level, current), shortest);
            return shortest;
        }
        var numpart = int.Parse(s.Substring(0, 3));
        var la1 = ShortestMoves(s, 0, 2, resultcache1);
        answer1 += la1 * numpart;

        var la2 = ShortestMoves(s, 0, 25, resultcache2);
        answer2 += numpart * la2;
    }

    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

(int x, int y) findkey(char key, string[] pad)
{
    for (int i = 0; i < pad[0].Length; i++)
        for (int j = 0; j < pad.Length; j++)
            if (pad[j][i] == key) return (i, j);
    return (-1, -1);
}
Dictionary<(char from, char to), string[]> GetMoves(string keys, string[] keypad)
{
    var keypadmoves = new Dictionary<(char from, char to), string[]>();
    for (int i = 0; i < keys.Length; i++)
    {
        char c1 = keys[i];
        var f1 = findkey(c1, keypad);
        for (int j = i; j < keys.Length; j++)
        {
            char c2 = keys[j];
            var state = new Dictionary<(int x, int y), (int cost, HashSet<string> options)>();
            var work = new PriorityQueue<(int x, int y), int>();
            work.Enqueue((f1.x, f1.y), 0);
            state.Add(f1, (0, new HashSet<string> { "" }));
            while (work.Count > 0)
            {
                var (wx, wy) = work.Dequeue();
                var st = state[(wx, wy)];
                if (keypad[wy][wx] == c2)
                {
                    keypadmoves.Add((c1, c2), st.options.ToArray());
                    if (c1 != c2)
                    {
                        var reverseOptions = new HashSet<string>();
                        foreach (string s1 in st.options)
                        {
                            string rev = "";
                            foreach (char c in s1.Reverse())
                            {
                                switch (c)
                                {
                                    case '>': rev += '<'; break;
                                    case '<': rev += ">"; break;
                                    case '^': rev += 'v'; break;
                                    case 'v': rev += '^'; break;
                                    default: rev += c; break;
                                }
                            }
                            reverseOptions.Add(rev);
                        }
                        keypadmoves.Add((c2, c1), reverseOptions.ToArray());
                    }
                    break;
                }
                void dowork(int x, int y, char direction)
                {
                    if (x < 0 || y < 0 || x == keypad[0].Length || y == keypad.Length) return;
                    if (keypad[wy][wx] == ' ') return;
                    int newcost = st.cost + 1;
                    bool seenbefore = state.TryGetValue((x, y), out var st1);
                    if (!seenbefore) state[(x, y)] = st1 = (newcost, new HashSet<string>());
                    if (newcost == st1.cost)
                    {
                        foreach (string s in st.options)
                        {
                            st1.options.Add(s + direction);
                        }
                        if (!seenbefore) work.Enqueue((x, y), newcost);
                    }
                }
                dowork(wx + 1, wy, '>');
                dowork(wx - 1, wy, '<');
                dowork(wx, wy + 1, 'v');
                dowork(wx, wy - 1, '^');
            }
        }
    }
    return keypadmoves;
}

List<string> GetSequences(string entrycode, Dictionary<(char from, char to), string[]> keypadmoves)
{
    var sequence = new List<string>() { "" };
    char prevKey = 'A';

    foreach (var key in entrycode)
    {
        var newsequence = new List<string>();
        var moves = keypadmoves[(prevKey, key)];
        foreach (var prevStrokes in sequence)
        {
            foreach (var nextStroke in moves)
            {
                newsequence.Add(prevStrokes + nextStroke + 'A');
            }
        }
        prevKey = key;
        sequence = newsequence;
    }
    return sequence;
}