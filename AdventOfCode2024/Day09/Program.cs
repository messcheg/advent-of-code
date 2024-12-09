using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example.txt", true);
Run(@"..\..\..\example1.txt", false);
//Run(@"E:\develop\advent-of-code-input\2024\day09.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 1928;
    long supposedanswer2 = 2858;

    var S = File.ReadAllText(inputfile).Select(a => a - '0').ToArray();
    long answer1 = 0;
    long answer2 = 0;
    int id2 = S.Length % 2 == 1 ? S.Length - 1 : S.Length - 2;
    int offset2 = 0;
    bool ready = false;
    long disklocation = 0;
    int id1 = 0;

    while (!ready)
    {
        for (int i = 0; i < S[id1]; i++) // rij enen
        {
            if (id1 == id2 && (int)(S[id1]) - i <= offset2) { ready = true; break; }
            answer1 += disklocation * (id1 / 2);
            disklocation++;
        }
        id1++;
        if (id1 >= id2) { ready = true; break; } // rij nullen
        for (int i = 0; i < S[id1]; i++)
        {
            int current = S[id2];
            if (offset2 >= current)
            {
                id2 -= 2;
                offset2 = 0;
            }
            if (id2 <= id1)
            {
                ready = true; break;
            }
            answer1 += disklocation * (id2 / 2);
            disklocation++;
            offset2++;
        }
        id1++;
    }

    var holes = new SortedList<int, LinkedListNode<(int size, int content)>>[10];
    for (int i = 0; i < holes.Length; i++) holes[i] = new SortedList<int, LinkedListNode<(int size, int content)>>(10);
    var disk = new LinkedList<(int size, int content)>();
    bool space = false;
    id1 = 0;

    foreach (var size in S)
    {
        if (space)
        {
            holes[size].Add(id1, disk.AddLast((size, -1)));
        }
        else
        {
            disk.AddLast((size, id1));
            id1++;
        }
        space = !space;
    }

    var candidate = disk.Last;
    int minId = 0;
    while (candidate != null && candidate.Value.content != minId)
    {
        while (candidate != null && candidate.Value.content < 0) candidate = candidate.Previous;
        if (candidate != null)
        {
            var ah = holes[candidate.Value.size..].Where((h) => h.Count > 0).Select(can => can.First());
            var hole = ah.OrderBy(h => h.Key).FirstOrDefault();

            if (hole.Value != null && candidate.Value.content > hole.Key)
            {
                var block = hole.Value;
                var originalcandidate = candidate.Value;
                var orgblocksize = block.Value.size;

                block.Value = (orgblocksize - candidate.Value.size, -1);
                holes[orgblocksize].Remove(hole.Key);
                holes[block.Value.size].Add(hole.Key, hole.Value);

                candidate.Value = (candidate.Value.size, -1);
                disk.AddBefore(block, originalcandidate);
            }
            candidate = candidate?.Previous;
        }
    }

    Console.WriteLine();
    long location2 = 0;
    answer2 = 0;
    foreach (var blck in disk)
    {
        if (blck.size == 0) continue;
        if (blck.content < 0)
        {
            location2 += blck.size;
            continue;
        }
        long con = blck.content;
        long sz = blck.size;
        answer2 += con * sz * location2 + con * (sz * (sz - 1) / 2);
        location2 += blck.size;

    }

    Console.WriteLine();
    Console.WriteLine("----------------");


    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

