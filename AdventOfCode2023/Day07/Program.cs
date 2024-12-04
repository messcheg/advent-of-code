using System.Collections.Immutable;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using AocHelper;

Run(@"..\..\..\example.txt", true);
//Run(@"..\..\..\example1.txt", true);
Run(@"E:\develop\advent-of-code-input\2023\day07.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 6440;
    long supposedanswer2 = 5905;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    var cardset = new List<(string strong, string org, long value)>();
    var cardset2 = new List<(string strong, string org, long value)>();
    var hands = S.Select(s => s.Split(" "));
    var cards = "AKQJT98765432";
    var cards2 = "AKQT98765432J";

    foreach ( var h in hands)
    {
        var c1 = ' ';
        var cnt = 1;
        var handtype = "";
        var js = 0;
        foreach (char c in h[0].Order())
        {
            if (c1 == c) cnt++;
            else
            {
                if (c1 != ' ')
                {
                    handtype = handtype + cnt.ToString();
                    if (c1 == 'J')
                        js = cnt;
                }
                cnt = 1;
                c1 = c;
            }
        }
        handtype += cnt.ToString();
        if (c1 == 'J') js = cnt;
        handtype = new string(handtype.OrderDescending().ToArray());

        int worth = GetStrength(h, handtype);
        int w2 = 0;
        if (js == 0)
            w2 = worth;
        else
        {
            string ht2 = "";
            bool done = false;
            foreach (char c in handtype) 
            {
                if (!done && c - '0' == js)
                    done = true;
                else
                    ht2 += c;
            }
            string ht3 = "";
            if (js == 5)
                ht3 = "5";
            else
            {
                char ch2 = (char)(ht2[0] + js);
                ht3 = ch2.ToString();
                foreach (var c in ht2.Skip(1)) ht3 += c;
            }
            w2 = GetStrength(h, ht3);
        }

        string strong = worth.ToString();
        foreach (char c in h[0])
        {
            char c2 = (char)(cards.IndexOf(c) + 'A');
            strong += c2;
        }
        cardset.Add((strong, h[0], long.Parse(h[1])));

        string strong2 = w2.ToString();
        foreach (char c in h[0])
        {
            char c2 = (char)(cards2.IndexOf(c) + 'A');
            strong2 += c2;
        }

        cardset2.Add((strong2, h[0], long.Parse(h[1])));
    }
    var cs1 = cardset.OrderByDescending(a => a.strong).ToList();
    for (int i = 0; i< cs1.Count; i++)
    {
        answer1 += (i + 1) * cs1[i].value;
    }

    var cs2 = cardset2.OrderByDescending(a => a.strong).ToList();
    for (int i = 0; i < cs2.Count; i++)
    {
        answer2 += (i + 1) * cs2[i].value;
    }


    stopwatch.Stop();
    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Time in miliseconds: " + stopwatch.ElapsedMilliseconds.ToString());

}

static int GetStrength(string[]? h, string handtype)
{
    var worth = 0;
    switch (handtype)
    {
        case "5": worth = 1; break;
        case "41": worth = 2; break;
        case "32": worth = 3; break;
        case "311": worth = 4; break;
        case "221": worth = 5; break;
        case "2111": worth = 6; break;
        default:
         /*   if (h[0].Where(c => '2' <= c && c <= '9').Count() == 0)
                worth = 7;
            else */
                worth = 8;
            break;
    }

    return worth;
}