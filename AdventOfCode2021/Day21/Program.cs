Run();

void Run()
{
//    const int pos1 = 4;
//    const int pos2 = 8;
    
    const int pos1 = 3;
    const int pos2 = 10;

    int position1 = pos1;
    int position2 = pos2;

    int throwdice(int d) { return d < 99 ? d + 1 : 0; };
    int newposition(int previous, int addition) {return (previous + addition - 1) % 10 + 1; };

    int dice = 1;
    int throws = 0;
    bool firstplayer = true;
    long score1 = 0;
    long score2 = 0;
    
    while (score1 < 1000 && score2 < 1000)
    {
        throws += 3;
        var value = dice;
        dice = throwdice(dice);
        value += dice;
        dice = throwdice(dice);
        value += dice;
        dice = throwdice(dice);
        if (firstplayer)
        {
            position1 = newposition(position1, value);
            score1 += position1;
        }
        else
        {
            position2 = newposition(position2, value);
            score2 += position2;
        }
        firstplayer = !firstplayer;
    }
    Console.WriteLine("throws: " + throws);
    Console.WriteLine("score1: " + score1);
    Console.WriteLine("score2: " + score2);
    Console.WriteLine("points: " + Math.Min(score1, score2) * throws);

    var multiply = new (int value, ulong count)[] { (3, 1), (4, 3), (5, 6), (6, 7), (7, 6), (8, 3), (9, 1) };
    var games = new ulong[10, 10, 21, 21];
    games[pos1 - 1, pos2 - 1, 0, 0] = 1;
    ulong winsp1 = 0;
    ulong winsp2 = 0;
    firstplayer = true;
    bool gamesleft = true;
    while (gamesleft)
    {
        var newgames = new ulong[10, 10, 21, 21];
        gamesleft = false;
        foreach (var dresult in multiply)
        {
            for (int p1 = 1; p1 <= 10; p1++)
            {
                int new_p1 = firstplayer ? newposition(p1, dresult.value) : p1;
                for (int p2 = 1; p2 <= 10; p2++)
                {
                    int new_p2 = !firstplayer ? newposition(p2, dresult.value) : p2;
                    for (int s1 = 0; s1 < 21; s1++)
                    {
                        int new_s1 = firstplayer ? s1 + new_p1 : s1;
                        for (int s2 = 0; s2 < 21; s2++)
                        {
                            int new_s2 = !firstplayer ? s2 + new_p2 : s2;
                            var universes = games[p1 - 1, p2 - 1, s1, s2] * dresult.count;
                            if (universes > 0)
                            {
                                if (new_s1 >= 21) winsp1 += universes;
                                else if (new_s2 >= 21) winsp2 += universes;
                                else
                                {
                                    gamesleft = true;
                                    newgames[new_p1 - 1, new_p2 - 1, new_s1, new_s2] += universes;
                                }
                            }
                        }
                    }
                }
            }
        }
        games = newgames;
        firstplayer = !firstplayer;
    }
    Console.WriteLine("winscore1: " + winsp1);
    Console.WriteLine("winscore2: " + winsp2);
}
