Run();

void Run()
{
    //string inputfile = @"..\..\..\example_input.txt";
    string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 35;
    long supposedanswer2 = 3351;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    var algorithm = new List<byte>();

    int i = 0;
    foreach (var s in S)
    {
        if (s == "") break;
        foreach (var c in s) algorithm.Add((byte) (c == '.' ? 0x0 : 0x1));
        i++;
    }

    var image = new List<byte[]>();
    foreach (string s in S.Skip(i + 1))
    {
        image.Add (s.Select(c => (byte)(c == '.' ? 0 : 1)).ToArray()); 
    }
    int bordersize = 1;
    byte invisibledotColor = 0;
    for (int k= 0; k< 50; k++)
    {
        
        int rows = image.Count + 2 * bordersize; 
        int rowsize = image[0].Length + 2 * bordersize;

        var newImage = new List<byte[]>();
        for (int p = 0; p < rows; p++)
            newImage.Add(new byte[rowsize]);

        var newImage1 = new List<byte[]>();
        for (int q = 0; q < bordersize; q++)
        {
            var b1 = new byte[rowsize];
            for (int p = 0; p < rowsize; p++) b1[p] = invisibledotColor;
            newImage1.Add(b1);
        }
        foreach (var s in image)
        {
            var bl = new byte[rowsize];
            for (int j =0; j < bordersize; j++) bl[j] = bl[^(j+1)] = invisibledotColor;
            for (int j = 0; j < s.Length; j++) bl[j + 1] = s[j];
            newImage1.Add(bl);
        }
        for (int q = 0; q < bordersize; q++)
        {
            var b1 = new byte[rowsize];
            for (int p = 0; p < rowsize; p++) b1[p] = invisibledotColor;
            newImage1.Add(b1);
        }

        image = newImage1;

        //Console.WindowWidth = Math.Max(Console.WindowWidth, rowsize);
        //foreach (var a in image)
        //{
        //    foreach (var b in a) Console.Write(b == 0 ? '.' : '#');
        //    Console.WriteLine();
        //}
        //Console.WriteLine("------------------------");


        for (int p = 0; p < image.Count ; p++)
            for (int q = 0; q < image[0].Length ; q++)
            {
                byte[] colors = new byte[9];
                colors[0] = p == 0 || q == 0 ? invisibledotColor : image[p - 1][q - 1];
                colors[1] = p == 0 ? invisibledotColor : image[p - 1][q];
                colors[2] = p == 0 || q == rowsize -1 ? invisibledotColor : image[p - 1][q + 1];
                colors[3] = q == 0 ? invisibledotColor : image[p][q - 1];
                colors[4] = image[p][q];
                colors[5] = q == rowsize - 1 ? invisibledotColor : image[p][q + 1];
                colors[6] = p == rows -1 || q == 0 ? invisibledotColor : image[p + 1][q - 1];
                colors[7] = p == rows -1 ? invisibledotColor : image[p + 1][q];
                colors[8] = p == rows -1 || q == rowsize -1 ? invisibledotColor : image[p + 1][q + 1];
                uint result = 0;
                foreach (var b in colors) result = (result << 1) | b;

                newImage[p][q] = algorithm[(int)result];
            }
        image = newImage;
        invisibledotColor = (invisibledotColor == 0) ? algorithm[0] : algorithm[511];
        if (k==1) answer1 = image.Sum(s => s.Sum(t => (long)t));
    }

   // foreach (var a in image)
   // {
   //     foreach (var b in a) Console.Write(b == 0 ? '.' : '#');
   //     Console.WriteLine();
   // }
   // Console.WriteLine("------------------------");
    
    answer2 = image.Sum(s => s.Sum(t => (long) t));

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
