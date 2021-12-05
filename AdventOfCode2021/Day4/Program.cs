var numbers = new int[] { 7, 4, 9, 5, 11, 17, 23, 2, 0, 14, 21, 24, 10, 16, 13, 6, 15, 25, 12, 22, 18, 20, 8, 19, 3, 26, 1 };
var scorecards = new int[][] {new int[] {22, 13, 17, 11, 0
                            , 8, 2, 23, 4, 24
                            , 21, 9, 14, 16, 7
                            , 6, 10, 3, 18, 5
                            , 1, 12, 20, 15, 19},
                new int[] { 3, 15, 0, 2, 22
                           , 9, 18, 13, 17, 5
                           ,19, 8, 7, 25, 23
                           ,20, 11, 10, 24, 4
                           ,14, 21, 16, 12, 6},
                new int[] {14, 21, 17, 24, 4
                           ,10, 16, 15, 9, 19
                           ,18, 8, 23, 26, 20
                           ,22, 11, 13, 6, 5
                           , 2, 0, 12, 3, 7}};
var marked = new System.Collections.BitArray[] { new System.Collections.BitArray(25)
                                                , new System.Collections.BitArray(25)
                                                , new System.Collections.BitArray(25)};

foreach (var number in numbers)
{
    bool finished = false;
    for (int player = 0; player < 3; player++)
    {
        var scorecard = scorecards[player];
        var marks = marked[player];

        bool found = false;
        // search number in the scorecard.
        for (int i = 0; i < 25; i++)
        {
            if (scorecard[i] == number)
            {
                marks[i] = found = true;
                break;
            }
        }

        //check for bingo
        bool bingo = false;
        if (found) for (int i = 0; i < 5; i++)
            {
                var bingoHorizontal = true;
                var bingoVertical = true;
                for (int j = 0; j < 5; j++)
                {
                    bingoHorizontal &= marks[i * 5 + j];
                    bingoVertical &= marks[i + j * 5];
                }
                if (bingoHorizontal || bingoVertical)
                {
                    bingo = true;
                    break;
                }
            }

        //calculate the score
        if (bingo)
        {
            int totalUnmarked = 0;
            for (int i = 0; i < 25; i++) if (!marks[i]) totalUnmarked += scorecard[i];

            int totalScore = totalUnmarked * number;

            Console.WriteLine("The winner is player " + (player + 1));
            Console.WriteLine("He/she has " + totalScore + " points.");
            Console.WriteLine();
            Console.WriteLine("With the following card");

            var previouscolor = Console.BackgroundColor;
            for (int k = 0; k < 3; k++)
            {
                Console.WriteLine("Player " + (k + 1));
                var crd = scorecards[k];
                var mrks = marked[k];
                var color = player == k ? ConsoleColor.DarkRed : ConsoleColor.DarkGray;
                for (int i = 0; i < 25; i += 5)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Console.BackgroundColor = mrks[i + j] ? color : previouscolor;
                        Console.Write((" " + crd[i + j].ToString("00")));
                    }
                    Console.WriteLine();
                }
            }
            Console.BackgroundColor = previouscolor;
            finished = true;
            break;
        }
    }
    if (finished) break;
}
