

public class Program
{

    public static void Main(string[] args)
    {
        Program.Run();
    }


    /* Let's rock with Santa!!!

      Midnight takes your heart and your soul
      While your heart is as high as your soul
      Put your heart without your soul into your heart

      Give back your heart


      Desire is a lovestruck ladykiller
      My world is nothing 
      Fire is ice
      Hate is water
      Until my world is Desire,
      Build my world up
      If Midnight taking my world, Fire is nothing and Midnight taking my world, Hate is nothing
      Shout "FizzBuzz!"
      Take it to the top

      If Midnight taking my world, Fire is nothing
      Shout "Fizz!"
      Take it to the top

      If Midnight taking my world, Hate is nothing
      Say "Buzz!"
      Take it to the top

      Whisper my world
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