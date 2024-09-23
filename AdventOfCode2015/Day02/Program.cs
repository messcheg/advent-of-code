

public class Program
{

    public static void Main(string[] args)
    {
        Program.Run();
    }


    /* Let's rock with Santa!!!
         The crowd says Ho ho ho
        Shout the crowd
    
    */

    /* PART I (Not possible with current Rockstar generator implementation)
 
        Rudolf wants love and attention
        The crowd says x
        Cut attention in the boat with the crowd
        Let the wish be roll the boat
        Cast the wish
        Let hope be roll the boat
        Cast hope 
        Let faith be roll the boat
        Cast faith
        Let there be the wish of hope
        Let christmas be the wish of faith
        Put hope of faith into the book
        Let our mind be there with the book, Christmas
        Let your mind be there
        If your mind is stronger than Christmas
        Let your mind be Christmas

        If your mind is greater than the book
        let your mind be the book

        Let love be with our mind with your mind, our mind
        Give it back


        Rock the rhythm with "2x3x4", "1x1x10"
        The message is nothing
        while the rhythm ain't silent
        Roll the rhythm into the mood
        Put Rudolf taking the message, the mood into the message

        Whisper the message

    */

    public static void Run()
    {
        string inputfile = @"..\..\..\PresentSizes.txt";
        //string inputfile = @"..\..\..\real_input.txt";
        long supposedanswer1 = 0000;
        long supposedanswer2 = 0000;

        var Presents = File.ReadAllLines(inputfile).ToList();
        long answer1 = 0;
        long answer2 = 0;


        // Rockstar.FizzBuzz.LetsRock();
        Rockstar.Santa.LetsRock();


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
 }