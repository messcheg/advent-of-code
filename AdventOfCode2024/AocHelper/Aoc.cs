namespace AocHelper
{
    public static class Aoc
    {
        public static void w<T>(int number, T val, T supposedval, bool isTest)
        {
            string? v = (val == null) ? "(null)" : val.ToString();
            string? sv = (supposedval == null) ? "(null)" : supposedval.ToString();

            var previouscolour = Console.ForegroundColor;
            Console.Write("Answer Part " + number + ": ");
            Console.ForegroundColor = (v == sv || !isTest) ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write(v);
            Console.ForegroundColor = previouscolour;
            if (isTest)
            {
                Console.Write(" ... supposed (example) answer: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(sv);
                Console.ForegroundColor = previouscolour;
            }
            Console.WriteLine();
        }
    }
}