using AocHelper;
using System.Diagnostics;

Run(@"..\..\..\example0.txt", true, 908, 618);
Run(@"..\..\..\example.txt", true, 2028, 1751);
Run(@"..\..\..\example1.txt", true, 10092, 9021);
Run(@"..\..\..\input.txt", false);
//Run(@"E:\develop\advent-of-code-input\2024\day15.txt", false);

void Run(string inputfile, bool isTest, long supposedanswer1 = 0, long supposedanswer2 = 0)
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var floormap = new List<List<char>>();
    var floormap1 = new List<List<char>>();

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    bool at_instructions = false;
    int j = 0;
    int rx = 0;
    int ry = 0;
    int rx1 = 0;
    int ry1 = 0;
    int w = S[0].Length;
    int h = 0;
    foreach (var s in S)
    {
        if (!at_instructions)
        {
            if (s == "")
            {
                at_instructions = true;
                h = j;
            }
            else
            {
                floormap.Add(s.ToList());
                var s1 = new List<char>();
                for (int i = 0; i < s.Length; i++)
                {
                    switch (s[i])
                    {
                        case '@':
                            rx = i;
                            rx1 = s1.Count;
                            ry1 = ry = j;
                            s1.Add(s[i]);
                            s1.Add('.');
                            break;
                        case 'O':
                            s1.Add('[');
                            s1.Add(']');
                            break;
                        case '.':
                            s1.Add('.');
                            s1.Add('.');
                            break;
                        case '#':
                            s1.Add('#');
                            s1.Add('#');
                            break;
                    }
                }
                floormap1.Add(s1);

            }

            j++;
        }
        else
        {
            foreach (var c in s)
            {
                switch (c)
                {
                    case '<':
                        if (rx > 0)
                        {
                            if (floormap[ry][rx - 1] == '.')
                            {
                                floormap[ry][rx - 1] = '@';
                                floormap[ry][rx] = '.';
                                rx--;
                            }
                            else if (floormap[ry][rx - 1] == 'O')
                            {
                                int i = rx - 1;
                                while (i > 0 && floormap[ry][i] == 'O') i--;
                                if (floormap[ry][i] == '.')
                                {
                                    floormap[ry][rx - 1] = '@';
                                    floormap[ry][rx] = '.';
                                    floormap[ry][i] = 'O';
                                    rx--;
                                }
                            }
                        }
                        break;
                    case '>':
                        if (rx < w - 1)
                        {
                            if (floormap[ry][rx + 1] == '.')
                            {
                                floormap[ry][rx + 1] = '@';
                                floormap[ry][rx] = '.';
                                rx++;
                            }
                            else if (floormap[ry][rx + 1] == 'O')
                            {
                                int i = rx + 1;
                                while (i < w - 1 && floormap[ry][i] == 'O') i++;
                                if (floormap[ry][i] == '.')
                                {
                                    floormap[ry][rx + 1] = '@';
                                    floormap[ry][rx] = '.';
                                    floormap[ry][i] = 'O';
                                    rx++;
                                }
                            }
                        }
                        break;
                    case '^':
                        if (ry > 0)
                        {
                            if (floormap[ry - 1][rx] == '.')
                            {
                                floormap[ry - 1][rx] = '@';
                                floormap[ry][rx] = '.';
                                ry--;
                            }
                            else if (floormap[ry - 1][rx] == 'O')
                            {
                                j = ry - 1;
                                while (j > 0 && floormap[j][rx] == 'O') j--;
                                if (floormap[j][rx] == '.')
                                {
                                    floormap[ry - 1][rx] = '@';
                                    floormap[ry][rx] = '.';
                                    floormap[j][rx] = 'O';
                                    ry--;
                                }
                            }
                        }
                        break;
                    case 'v':
                        if (ry < h - 1)
                        {
                            if (floormap[ry + 1][rx] == '.')
                            {
                                floormap[ry + 1][rx] = '@';
                                floormap[ry][rx] = '.';
                                ry++;
                            }
                            else if (floormap[ry + 1][rx] == 'O')
                            {
                                j = ry + 1;
                                while (j < h - 1 && floormap[j][rx] == 'O') j++;
                                if (floormap[j][rx] == '.')
                                {
                                    floormap[ry + 1][rx] = '@';
                                    floormap[ry][rx] = '.';
                                    floormap[j][rx] = 'O';
                                    ry++;
                                }
                            }
                        }
                        break;

                }
                switch (c)
                {
                    case '<':
                        if (rx1 > 0)
                        {
                            if (floormap1[ry1][rx1 - 1] == '.')
                            {
                                floormap1[ry1][rx1 - 1] = '@';
                                floormap1[ry1][rx1] = '.';
                                rx1--;
                            }
                            else if (floormap1[ry1][rx1 - 1] == ']')
                            {
                                int i = rx1 - 1;
                                while (i > 0 && (floormap1[ry1][i] == '[' || floormap1[ry1][i] == ']')) i--;
                                if (floormap1[ry1][i] == '.')
                                {
                                    while (i < rx1)
                                    {
                                        floormap1[ry1][i] = floormap1[ry1][i + 1];
                                        i++;
                                    }
                                    floormap1[ry1][rx1 - 1] = '@';
                                    floormap1[ry1][rx1] = '.';
                                    rx1--;
                                }
                            }
                        }
                        break;
                    case '>':
                        if (rx1 < w * 2 - 1)
                        {
                            if (floormap1[ry1][rx1 + 1] == '.')
                            {
                                floormap1[ry1][rx1 + 1] = '@';
                                floormap1[ry1][rx1] = '.';
                                rx1++;
                            }
                            else if (floormap1[ry1][rx1 + 1] == '[')
                            {
                                int i = rx1 + 1;
                                while (i < w * 2 - 1 && (floormap1[ry1][i] == '[' || floormap1[ry1][i] == ']')) i++;
                                if (floormap1[ry1][i] == '.')
                                {
                                    while (i > rx1)
                                    {
                                        floormap1[ry1][i] = floormap1[ry1][i - 1];
                                        i--;
                                    }
                                    floormap1[ry1][rx1 + 1] = '@';
                                    floormap1[ry1][rx1] = '.';
                                    rx1++;
                                }
                            }
                        }
                        break;
                    case '^':
                        if (ry1 > 0)
                        {
                            if (floormap1[ry1 - 1][rx1] == '.')
                            {
                                floormap1[ry1 - 1][rx1] = '@';
                                floormap1[ry1][rx1] = '.';
                                ry1--;
                            }
                            else if (floormap1[ry1 - 1][rx1] == '[' || floormap1[ry1 - 1][rx1] == ']')
                            {
                                j = ry1 - 1;

                                var blocktopush = new List<List<int>>();
                                if (floormap1[j][rx1] == '[') blocktopush.Add(new List<int>() { rx1 });
                                if (floormap1[j][rx1] == ']') blocktopush.Add(new List<int> { rx1 - 1 });
                                bool blocked = false;
                                bool ready = false;
                                while (j > 0 && !blocked && !ready)
                                {
                                    j--;
                                    var nextline = new List<int>();
                                    foreach (var b in blocktopush.Last())
                                    {
                                        var p1 = floormap1[j][b];
                                        var p2 = floormap1[j][b + 1];
                                        if (p1 == '[' || p1 == ']' || p2 == '[' || p2 == ']')
                                        {
                                            if (p1 == ']') nextline.Add(b - 1);
                                            if (p2 == '[') nextline.Add(b + 1);
                                            if (p1 == '[') nextline.Add(b);
                                        }
                                        if (p1 == '#' || p2 == '#') blocked = true;
                                    }
                                    if (nextline.Count > 0) blocktopush.Add(nextline);
                                    else ready = true;
                                }
                                if (!blocked)
                                {
                                    for (int k = blocktopush.Count - 1; k >= 0; k--)
                                    {
                                        foreach (var b in blocktopush[k])
                                        {
                                            floormap1[ry1 - 2 - k][b] = '[';
                                            floormap1[ry1 - 2 - k][b + 1] = ']';
                                            floormap1[ry1 - 1 - k][b] = '.';
                                            floormap1[ry1 - 1 - k][b + 1] = '.';
                                        }
                                    }
                                    floormap1[ry1 - 1][rx1] = '@';
                                    floormap1[ry1][rx1] = '.';
                                    ry1--;
                                }
                            }
                        }
                        break;
                    case 'v':
                        if (ry1 < h - 1)
                        {
                            if (floormap1[ry1 + 1][rx1] == '.')
                            {
                                floormap1[ry1 + 1][rx1] = '@';
                                floormap1[ry1][rx1] = '.';
                                ry1++;
                            }
                            else if (floormap1[ry1 + 1][rx1] == '[' || floormap1[ry1 + 1][rx1] == ']')
                            {
                                j = ry1 + 1;

                                var blocktopush = new List<List<int>>();
                                if (floormap1[j][rx1] == '[') blocktopush.Add(new List<int>() { rx1 });
                                if (floormap1[j][rx1] == ']') blocktopush.Add(new List<int> { rx1 - 1 });
                                bool blocked = false;
                                bool ready = false;
                                while (j < h - 1 && !blocked && !ready)
                                {
                                    j++;
                                    var nextline = new List<int>();
                                    foreach (var b in blocktopush.Last())
                                    {
                                        var p1 = floormap1[j][b];
                                        var p2 = floormap1[j][b + 1];
                                        if (p1 == '[' || p1 == ']' || p2 == '[' || p2 == ']')
                                        {
                                            if (p1 == ']') nextline.Add(b - 1);
                                            if (p2 == '[') nextline.Add(b + 1);
                                            if (p1 == '[') nextline.Add(b);
                                        }
                                        if (p1 == '#' || p2 == '#') blocked = true;
                                    }
                                    if (nextline.Count > 0) blocktopush.Add(nextline);
                                    else ready = true;
                                }
                                if (!blocked)
                                {
                                    for (int k = blocktopush.Count - 1; k >= 0; k--)
                                    {
                                        foreach (var b in blocktopush[k])
                                        {
                                            floormap1[ry1 + 2 + k][b] = '[';
                                            floormap1[ry1 + 2 + k][b + 1] = ']';
                                            floormap1[ry1 + 1 + k][b] = '.';
                                            floormap1[ry1 + 1 + k][b + 1] = '.';
                                        }
                                    }
                                    floormap1[ry1 + 1][rx1] = '@';
                                    floormap1[ry1][rx1] = '.';
                                    ry1++;
                                }
                            }
                        }
                        break;

                }
            }
        }
    }
    for (int p = 0; p < h; p++)
    {
        for (int q = 0; q < w; q++)
        {
            if (floormap[p][q] == 'O') answer1 += 100 * p + q;
        }
        for (int q = 0; q < w * 2; q++)
        {
            if (floormap1[p][q] == '[') answer2 += 100 * p + q;
        }

    }
    Aoc.w(1, answer1, supposedanswer1, isTest);
    Aoc.w(2, answer2, supposedanswer2, isTest);
    Console.WriteLine("Duration: " + stopwatch.ElapsedMilliseconds.ToString() + " miliseconds.");
}

