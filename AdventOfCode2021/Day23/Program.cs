Run();

void Run()
{
    string inputfile = @"..\..\..\example_input.txt";
    //string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 12521;
    long supposedanswer2 = 0000;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    int i = 0;
    foreach(string s in S)
    {

        i++;
    }
    
    var room = new int[][] { new int[] {1,0 }, new int[]{2,3 }, new int[]{1,2 }, new int[]{3,1 } };
    //var room = new int[][] { new int[] {3,3 }, new int[]{2,2 }, new int[]{0,1 }, new int[]{1,0 } };
    int[] energy_usage = new int[] { 1, 10, 100, 1000 };
    int[] hallway = new int[11]; 



    w(1, answer1, supposedanswer1);
    w(2, answer2, supposedanswer2);
}
enum pod { a = 0, b=1, c = 2, d = 3}
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
