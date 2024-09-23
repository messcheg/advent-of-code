using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day17.txt", false);

void Run(string inputfile, bool isTest)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    long supposedanswer1 = 3068;
    long supposedanswer2 = 1514285714288;
    
    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var recurring = new Dictionary<(int wind, int block, (int top0, int top1, int top2, int top3, int top4, int top5, int top6) profile), (long towerHeight, long blockCnt)>();

    var s = S[0];
    var towerbottom = 0L;
    var tower = new List<string>();
    int currentItem = 0;
    int flowcnt = 0;

    var items = new string[][]
        { new string[]
            {
                "..@@@@."
            },
            new string[] 
            {
                "...@...",
                "..@@@..",
                "...@..."
            },
            new string[]
            {
                "....@..",
                "....@..",
                "..@@@.."
            },
            new string[] 
            {
                "..@....",
                "..@....",
                "..@....",
                "..@...."
            }, 
            new string[]  
            {
                "..@@...",
                "..@@..."
            }
        };

    long i = 0;
    bool aleadyskipped = false;
    long deSkip2022 = 0;
    while ( i < 1000000000000)
    {
        int top = tower.Count;
        while (top > 0 && !tower[top-1].Contains('#')) top--;

        if (i > 2022 && !aleadyskipped)
        {
            // get profile
            var prof = new int[7];
            var profCnt = 0;
            var row = top - 1;
            while (row > 0 && profCnt < 7)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (tower[row][j] == '#' && prof[j] == 0)
                    {
                        prof[j] = top - row;
                        profCnt++;
                    }
                }
                row--;
            }
            if (profCnt == 7)
            {
                var key = (flowcnt, currentItem, (prof[0], prof[1], prof[2], prof[3], prof[4], prof[5], prof[6]));
                if (recurring.ContainsKey(key))
                {
                    var old = recurring[key];
                    long diffTow = towerbottom + top - old.towerHeight;
                    long togo = 1000000000000 - i;
                    long difBLock = i - old.blockCnt;
                    long cycleToSkip = togo / difBLock;
                    long itemsToskipUntil2022 = (2022 - i) / difBLock;
                    towerbottom += diffTow * cycleToSkip;
                    i += difBLock * cycleToSkip;
                    aleadyskipped = true;

                    deSkip2022 = difBLock * cycleToSkip - itemsToskipUntil2022 * difBLock;
                }
                else
                {
                    recurring[key] = (towerbottom + top, i);
                }
            }
        }
        
        for (var j = tower.Count; j < top + 3; j++)
            tower.Add(".......");

        var nextItem = items[currentItem];
        int itemRow = nextItem.Length - 1;
        for (var j = top + 3; j < top + 3 + nextItem.Length; j++)
        {
            if (j < tower.Count) tower[j] = nextItem[itemRow];
            else tower.Add(nextItem[itemRow]);
            itemRow--;
        }

        bool finished = false;
        int bottom = top + 3;
        while (!finished)
        {
            char nextMove = s[flowcnt];
            flowcnt++;
            if (flowcnt == s.Length) flowcnt = 0;
            bool canMove = CanMove(tower, bottom, nextMove);
            if (canMove)
            {
                Move(tower, bottom, nextMove);
            }

            finished = !CanFall(tower, bottom);
            if (!finished)
            {
                bottom = Fall(tower, bottom);
            }
            else
            {
                for (int j = bottom; j < tower.Count; j++)
                {
                    tower[j] = tower[j].Replace('@', '#');
                }
            }
        }

        if ( i > 2022 && (i+1)%(items.Length * s.Length) == 0 )
        {
            var filled = new bool[7];
            int cleanlevel = tower.Count -1;
            int detected = 0;
            while(cleanlevel>=0 && detected < 7 )
            {
                for(int j = 0;j < 7; j++)
                {
                    if (!filled[j] && tower[cleanlevel][j] == '#')
                    {
                        detected++;
                        filled[j] = true;
                    }
                }
                cleanlevel--;
            }
            towerbottom += cleanlevel;
            tower.RemoveRange(0, cleanlevel);
        }
        else if (i == 2021) answer1 = tower.Where(s => s.Contains('#')).Count();

        currentItem++;
        if (currentItem == items.Length) currentItem = 0;

        i++;
    }

    answer2 = towerbottom + tower.Where(s => s.Contains('#')).Count();

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

static bool CanFall(List<string> tower, int bottom)
{
    bool canFall = bottom > 0;
    if (canFall)
    {
        for (int k = bottom; k < tower.Count; k++)
        {
            for (int l = 0; l < 7; l++)
            {
                if (tower[k][l] == '@' && tower[k - 1][l] == '#')
                {
                    canFall = false;
                    break;
                }
            }
        }
    }

    return canFall;
}

static int Fall(List<string> tower, int bottom)
{
    for (int k = bottom; k < tower.Count; k++)
    {
        char[] line1 = new char[7];
        char[] line2 = new char[7];

        for (int l = 0; l < 7; l++)
        {
            if (tower[k][l] == '@')
            {
                line1[l] = '@';
                line2[l] = '.';
            }
            else
            {
                line1[l] = tower[k - 1][l];
                line2[l] = tower[k][l];
            }
        }
        tower[k - 1] = new string(line1);
        tower[k] = new string(line2);
    }
    bottom--;
    return bottom;
}

static bool CanMove(List<string> tower, int bottom, char nextMove)
{
    bool canMove = true;
    for (int k = bottom; k < tower.Count; k++)
    {
        if (nextMove == '>')
        {
            if (tower[k][6] == '@')
                canMove = false;
            else
            {
                for (int l = 0; l < 6; l++)
                {
                    if (tower[k][l] == '@' && tower[k][l + 1] == '#')
                    {
                        canMove = false;
                        break;
                    }
                }
            }
        }
        else
        {
            if (tower[k][0] == '@')
                canMove = false;
            else
            {
                for (int l = 1; l < 7; l++)
                {
                    if (tower[k][l] == '@' && tower[k][l - 1] == '#')
                    {
                        canMove = false;
                        break;
                    }
                }
            }
        }
    }

    return canMove;
}

static void Move(List<string> tower, int bottom, char nextMove)
{
    for (int k = bottom; k < tower.Count; k++)
    {
        if (nextMove == '>')
        {
            var line1 = tower[k].ToArray();
            for (int l = 6; l >= 0; l--)
            {
                if (tower[k][l] == '@')
                {
                    line1[l + 1] = '@';
                    line1[l] = '.';
                }
                else line1[l] = tower[k][l];
            }
            tower[k] = new string(line1);

        }
        else
        {
            var line1 = tower[k].ToArray();
            for (int l = 0; l < 6; l++)
            {
                if (tower[k][l + 1] == '@')
                {
                    line1[l] = '@';
                    line1[l + 1] = '.';
                }
                else line1[l+1] = tower[k][l+1];
            }
            tower[k] = new string(line1);
        }
    }
}