static void w<T>(T val) { Console.WriteLine(val); }

//string inputfile = @"..\..\..\example_input.txt";
string inputfile = @"..\..\..\real_input.txt";

var S = File.ReadAllLines(inputfile).ToList();
long answer1 = 0;
long answer2 = 0;

int i = 0;
var scores = new List<long>();
foreach (var s in S)
{
    var c = ' ';
    var stack = new Stack<char>();
    foreach (var c1 in s)
    {
        if ("[{<(".Contains(c1)) stack.Push(c1);
        else if ("]}>)".Contains(c1))
        {
            var d = stack.Pop();
            if (conflict(d, c1))
            {
                c = c1;
                break;
            }
        }
    }
    if (c == ')') answer1 += 3;
    else if (c == ']') answer1 += 57;
    else if (c == '}') answer1 += 1197;
    else if (c == '>') answer1 += 25137;
    else if (stack.Count > 0)
    {
        long score = 0;
        foreach (char c1 in stack.ToArray())
        {
            if (c1 == '(') score = (score * 5) + 1;
            else if (c1 == '[') score = (score * 5) + 2;
            else if (c1 == '{') score = (score * 5) + 3;
            else if (c1 == '<') score = (score * 5) + 4;
        }
        scores.Add(score);
    }
    i++;
}
scores.Sort();
answer2 = scores[scores.Count/2];

w(answer1);
w(answer2);

bool conflict(char a, char b)
{
    return (a == '(' && b != ')') || (a == '[' && b != ']') || (a == '<' && b != '>') || (a == '{' && b != '}');
}