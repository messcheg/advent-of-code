Run();

void Run()
{
    // target area: x = 20..30, y = -10..-5
    // target area: x = 235..259, y = -118..-62
    
    int x1 = 20;
    int x2 = 30;
    int y1 = -10;
    int y2 = -5;
    
    /*
    int x1 = 235;
    int x2 = 259;
    int y1 = -118;
    int y2 = -62;
    */
    long supposedanswer1 = 45;
    long supposedanswer2 = 112;

    long answer1 = 0;
    long answer2 = 0;

    int location(int v0, int s) { return (2 * v0 * s - s * s + s) / 2; }
    int top(int v0) { return Math.Max(location(v0,v0), location(v0, v0 + 1)); }  // s_top = v0 + 1/2 , see derived formulas. 
    
    var bullseyes = new HashSet<(int vx, int vy)>();
    int vx0 = (int)Math.Ceiling(Math.Sqrt((double)x1 * 2 + 0.25) - 0.5); // start velocity
    while ( vx0 <= x2)
    {
        int s = (int) (vx0 + 0.25 - Math.Sqrt(4*vx0*vx0 + vx0 + 0.25 - 4 * x1)/2 );
        while (location(vx0,s) < x1 && s < vx0) s++;
        while ((location(vx0,s) <= x2) || (s >= vx0 && s <= x2 ))
        {
            int vy0 = (int) (( 2*y1 + s * s + s) / (2 * s)) - 1; // location just below y1
            int y = location(vy0, s);
            while (y <= y2) // else overshot the area
            {
                if (y >= y1) bullseyes.Add((vx0, vy0)); // hit
                vy0++;
                y = location(vy0,s); 
            }
            s++;
        }
        vx0++;
    }
    answer1 = top(bullseyes.Max(p => p.vy));
    answer2 = bullseyes.Count;

    w(1, answer1, supposedanswer1);
    w(2, answer2, supposedanswer2);
   
}

static void w<T>(int number, T val, T supposedval)
{
    string? v = (val == null) ? "(null)" : val.ToString();
    string? sv = (supposedval == null) ? "(null)" : supposedval.ToString();

    var previouscolour = Console.ForegroundColor;
    Console.Write("Answer Part " + number + ": ");
    Console.ForegroundColor = (v == sv) ? ConsoleColor.Green : ConsoleColor.White;
    Console.Write(v);
    Console.ForegroundColor = previouscolour;
    Console.Write(" ... supposed (example) answer: ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(sv);
    Console.ForegroundColor = previouscolour;
}
