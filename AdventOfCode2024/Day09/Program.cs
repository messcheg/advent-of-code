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

    var disk = new List<(int size, int content)>();
    bool space = false;
    id1 = 0;
    foreach (var size in S)
    {
        if (space)
        {
            disk.Add((size, -1));
        }
        else
        {
            disk.Add((size, id1));
            id1++;
        }
        space = !space;
    }

    int candidate = disk.Count - 1;
    ready = false;
    while (!ready && candidate > 0)
    {
        int block = 0;

        while (disk[candidate].content < 0) candidate--;
        while (block < candidate && disk[block].content >= 0) block++;

        while (candidate > block && disk[block].size < disk[candidate].size)
        {
            block++;
            while (block < disk.Count && disk[block].content >= 0) block++;
        }
        if (candidate > block)
        {
            var originalcandidate = disk[candidate];
            var originalblock = disk[block];

            disk[block] = (originalblock.size - disk[candidate].size, -1);
            disk[candidate] = (disk[candidate].size, -1);
            if (candidate < disk.Count - 1 && disk[candidate + 1].content == -1)
            {
                disk[candidate] = (disk[candidate].size + disk[candidate + 1].size, -1);
                disk.RemoveAt(candidate + 1);
            }
            if (candidate >= block && disk[candidate - 1].content == -1)
            {
                disk[candidate] = (disk[candidate].size + disk[candidate - 1].size, -1);
                disk.RemoveAt(candidate - 1);
                candidate--;
            }
            disk.Insert(block, originalcandidate);
            block++; candidate++;
            if (disk[block].size == 0)
            {
                disk.RemoveAt(block);
                candidate--;
            }
        }
        candidate--;
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

