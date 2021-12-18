Run();

void Run()
{
    string inputfile = @"..\..\..\example_input.txt";
    //string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 4140;
    long supposedanswer2 = 3993;

    var S = File.ReadAllLines(inputfile).Select(s => ToSnailNumber(s)).ToList();
    long answer1 = 0;
    long answer2 = 0;
   
    var sn = S[0];
    for (int i=1; i < S.Count; i++)
    {
        sn = Add(sn, S[i]);
        displaynumber(sn,"Subtotl");
    }

   answer1 = Magnitude(sn);

    answer2 = 0;
    for (int i = 0; i < S.Count; i++)
        for (int j = i + 1; j < S.Count; j++)
        {
            answer2 = Math.Max(answer2, Magnitude(Add(S[i], S[j])));
            answer2 = Math.Max(answer2, Magnitude(Add(S[j], S[i])));
        }
    
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

const int open = -1;
const int close = -2;
const int comma = -3;
const int skip = -4;

void displaynumber(int[] sn, string label = "")
{
    Console.Write(label + ": ");
    foreach (int n in sn)
    {
        if (n == open) Console.Write("[");
        if (n == close) Console.Write("]");
        if (n == comma) Console.Write(",");
        if (n >= 0) Console.Write(n);
    }
    Console.WriteLine();
}

int[] ToSnailNumber (string S1, int[] res = null, int start = 0) 
{
    res = res ?? new int[S1.Length];
    int i = start;
    foreach(var s in S1)
    {
        if (s >= '0' && s <= '9') res[i] = s - '0';
        else if (s == ',') res[i] = comma;
        else if (s == '[') res[i] = open;
        else if (s == ']') res[i] = close;
        i++;
    }
    return res;
} 
int[] Add(int[] N1, int[] N2)
{
    int[] newNumber = new int[3 + N1.Length + N2.Length];
    (newNumber[0], newNumber[N1.Length + 1], newNumber[^1]) = (open, comma, close);
    for (int i = 0; i < N1.Length; i++) newNumber[1 + i] = N1[i];
    for (int i = 0; i < N2.Length; i++) newNumber[N1.Length + 2 + i] = N2[i];
    while (Explode(newNumber)) newNumber = Split(newNumber);
    return newNumber;
}

bool Explode (int[] sn)
{
    bool exploded = false;
    int level = 0;
    int i = 0;
    while (i < sn.Length)
    {
        int s = sn[i];
        if (s == open) level++;
        else if (s == close) level--;
        else if (s >= 0)
        {
            if (level == 5)
            {
                exploded = true;
                int j = i - 1;
                while (j > 0 && sn[j] < 0) j--;
                if (j >= 0 && sn[j] >= 0 ) sn[j] += sn[i];
                j = i + 3;
                while (j < sn.Length && sn[j] < 0) j++;
                if (j < sn.Length && sn[j] >= 0) sn[j] += sn[i + 2];
                sn[i - 1] = skip; // '['
                sn[i] = skip ;    // number
                sn[i + 1] = skip; // ','
                sn[i + 2] = skip; // 2nd number
                sn[i + 3] = 0;    // ']'
                level = 4;
                i += 3;
            }
        };
        i++;
    }
    return exploded;
}

int[] Split(int[] input)
{   
    var sn = input;
    bool splitsleft = true;
    bool newexplode = false;
    while (splitsleft && !newexplode)
    {
        var result = new List<int>(input.Length + 12);
        splitsleft = false;
        int i = 0;
        int level = 0;
        while (i < sn.Length)
        {
            if (sn[i] != skip)
            {
                if (sn[i] <= 9 || newexplode || splitsleft)
                {
                    result.Add(sn[i]);
                    if (sn[i] == open) level++;
                    if (sn[i] == close) level--; 
                }
                else
                {
                    int left = sn[i] / 2;
                    int right = sn[i] - left;
                    result.AddRange(new int[] { open, left, comma, right, close });
                    if (right > 9) splitsleft = true;
                    if (level == 4) newexplode = true;
                }
            }
            i++;
        }
        sn = result.ToArray();
    }
    return sn;
}

long Magnitude(int[] input)
{
    var sl = input.Where(n => n != skip).Select(n => (long)n).ToList();
    while (sl.Count > 1)
    {
        var sn = sl;
        int i = 0;
        sl = new List<long>(sn.Count);
        while (i < sn.Count)
        {
            if (sn[i] == open && sn[i + 2] == comma && sn[i + 4] ==close)
            {
                sl.Add(3 * sn[i + 1] + 2 * sn[i + 3]);
                i += 4;
            }
            else sl.Add(sn[i]);
            i++;
        }
    }
    return sl[0];
}
