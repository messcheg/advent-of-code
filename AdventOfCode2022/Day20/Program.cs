using System.Diagnostics;
using System.Net.Security;
using System.Runtime.ConstrainedExecution;

Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day20.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 3;
    long supposedanswer2 = 1623178306;
    
    var S = File.ReadAllLines(inputfile).Select(a => int.Parse(a)).ToList();
    long answer1 = 0;
    long answer2 = 0;

      
    int i = 0;
    var newlist = new (int val, int prev, int next)[S.Count];
    var newlist1 = new (long val, int prev, int next)[S.Count];
    long key = 811589153;
    long val1 = 9999 * key;
    while (i<S.Count)
    {
        var s = S[i];
        newlist[i] = (S[i], i - 1, i + 1);
        newlist1[i] = (key * S[i], i - 1, i + 1);
        
        i++;
    }
    newlist1[0].prev = newlist[0].prev = S.Count - 1;
    newlist1[S.Count - 1].next = newlist[S.Count - 1].next = 0;

    int start = 0;
    for (int j = 0; j<S.Count; j++)
    {
        var nr = newlist[j];
        if (j == start) start = nr.next;
        if (nr.val > 0)
        {
            var cur = nr.next;
            newlist[nr.prev].next = nr.next;
            newlist[nr.next].prev = nr.prev;
            for (int k = 0; k < nr.val - 1; k++)
            {
                cur = newlist[cur].next;
            }
            var nxt = newlist[cur].next;
            newlist[nxt].prev = j;
            newlist[cur].next = j;
            newlist[j].next = nxt;
            newlist[j].prev = cur;
        }
        else if(nr.val < 0)
        {
            newlist[nr.prev].next = nr.next;
            newlist[nr.next].prev = nr.prev;
            var cur = nr.prev;
            for (int k = 0; k > nr.val + 1; k--)
            {
                cur = newlist[cur].prev;
            }
            var prv = newlist[cur].prev;
            newlist[prv].next = j;
            newlist[cur].prev = j;
            newlist[j].prev = prv;
            newlist[j].next = cur;
        }
    }

    var loop = newlist.Where(v => v.val == 0).First().next;  
    for (int k = 1; k <= 3000; k++)
    {
        if (k == 1000 || k == 2000 || k == 3000) answer1 += newlist[loop].val; 
        loop = newlist[loop].next;
    }

    start = 0;
    for (int m = 0; m <= 9; m++)
    {
        if (isTest)
        {
            int cr = newlist1.Where(v => v.val == 0).First().next;
            cr = newlist1[cr].prev;
            for (int k = 0; k < S.Count; k++)
            {
                Console.Write($"{newlist1[cr].val}, ");
                cr = newlist1[cr].next;
            }
            Console.WriteLine();
        }
        for (int j = 0; j < S.Count; j++)
        {
            var nr = newlist1[j];
            long moves = nr.val % (S.Count - 1);
            if (j == start) start = nr.next;
            if (moves > 0)
            {
                newlist1[nr.prev].next = nr.next;
                newlist1[nr.next].prev = nr.prev;
                var cur = nr.next;
                for (int k = 0; k < moves - 1; k++)
                {
                    cur = newlist1[cur].next;
                }
                var nxt = newlist1[cur].next;
                newlist1[nxt].prev = j;
                newlist1[cur].next = j;
                newlist1[j].next = nxt;
                newlist1[j].prev = cur;
            }
            else if (moves < 0)
            {
        
                newlist1[nr.prev].next = nr.next;
                newlist1[nr.next].prev = nr.prev;
                var cur = nr.prev;
                for (int k = 0; k > moves + 1; k--)
                {
                    cur = newlist1[cur].prev;
                }
                var prv = newlist1[cur].prev;
                newlist1[prv].next = j;
                newlist1[cur].prev = j;
                newlist1[j].prev = prv;
                newlist1[j].next = cur;
            }
        }
    }
    loop = newlist1.Where(v => v.val == 0).First().next;
    for (int k = 1; k <= 3000; k++)
    {
        if (k == 1000 || k == 2000 || k == 3000) answer2 += newlist1[loop].val;
        loop = newlist1[loop].next;
    }

    stopwatch.Stop();
    Console.WriteLine($"Used time (ms): {stopwatch.ElapsedMilliseconds}");
    Console.WriteLine($"Used time (ticks): {stopwatch.ElapsedTicks}");
    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);
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
