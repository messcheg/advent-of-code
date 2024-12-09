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

    var disk = new LinkedList<(int size, int content)>();
    bool space = false;
    id1 = 0;

    foreach (var size in S)
    {
        if (space)
        {
            disk.AddLast((size, -1));
        }
        else
        {
            disk.AddLast((size, id1));
            id1++;
        }
        space = !space;
    }

    var candidate = disk.Last;
    var firstempty = disk.First?.Next;
    ready = false;
    int minId = 0;
    while (!ready && candidate != null && candidate.Value.content != minId)
    {
        var block = firstempty;

        while (candidate != null && candidate.Value.content < 0) candidate = candidate.Previous;
        while (block != null && block != candidate && block.Value.content >= 0)
        {
            if (block.Value.content == minId + 1) minId++;
            block = block.Next;
            firstempty = block;
        }

        while (candidate != null && block != null && candidate != block && block.Value.size < candidate.Value.size)
        {
            block = block.Next;
            while (block != null && block != candidate && block.Value.content >= 0) block = block.Next;
        }
        if (candidate != null && block != null && candidate != block)
        {
            var originalcandidate = candidate.Value;
            var originalblock = block.Value;

            block.Value = (originalblock.size - candidate.Value.size, -1);
            candidate.Value = (candidate.Value.size, -1);
            disk.AddBefore(block, originalcandidate);

            if (block.Value.size == 0)
            {
                if (firstempty == block) firstempty = block.Next;
                disk.Remove(block);
            }
        }
        candidate = candidate?.Previous;
    }

    Console.WriteLine();
    int location2 = 0;
    foreach (var blck in disk)
    {
        if (blck.size == 0) continue;
        if (blck.content < 0)
        {
            location2 += blck.size;
            continue;
        }
        for (int j = 0; j < blck.size; j++)
        {
            answer2 += location2 * blck.content;
            location2++;
        }
    }

    Console.WriteLine();
    Console.WriteLine("----------------");


    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

