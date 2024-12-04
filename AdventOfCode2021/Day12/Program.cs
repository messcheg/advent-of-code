Run();

void Run()
{
    var network = new Dictionary<string, Node>(20);
    //string inputfile = @"..\..\..\example_input.txt";
    string inputfile = @"..\..\..\real_input.txt";
    long supposedanswer1 = 226;
    long supposedanswer2 = 3509;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;

    

    int i = 0;
    foreach (string s in S)
    {
        var s1 = s.Split('-');
        Node n;
        if (!network.TryGetValue(s1[0], out n))
        {
            n = new Node();
            n.Name = s1[0];
            network.Add(s1[0], n);
        }
        n.connections.Add(s1[1]);
        
        if (!network.TryGetValue(s1[1], out n))
        {
            n = new Node();
            n.Name = s1[1];
            network.Add(s1[1], n);
        }
        n.connections.Add(s1[0]);
        i++;
    }

    answer1 = NumberofPath("start", Array.Empty<string>(), network);
    answer2 = NumberofPathTwice("start", Array.Empty<string>(), network);

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

long NumberofPath(string startnode, string[] alreadyvisited, Dictionary<string, Node> nodes)
{
    long count = 0;

    if (startnode == "end") return 1;
    Node n;
    
    var newvisited = new List<string>(alreadyvisited).Append(startnode).ToArray();
    
    if (nodes.TryGetValue(startnode, out n))
    {
        foreach (string s in n.connections)
        {
            if (!alreadyvisited.Contains(s) || nodes[s].isBig()) 
            {
                count += NumberofPath(s, newvisited, nodes);
            }
        }
    
    }

    return count;
}

long NumberofPathTwice(string startnode, string[] alreadyvisited, Dictionary<string, Node> nodes, bool twice = false)
{
    long count = 0;

    if (startnode == "end") return 1;
    Node n;

    var newvisited = new List<string>(alreadyvisited).Append(startnode).ToArray();

    if (nodes.TryGetValue(startnode, out n))
    {
        foreach (string s in n.connections)
        {
            Node next = nodes[s];
            if (!alreadyvisited.Contains(s) || next.isBig())
            {
               count += NumberofPathTwice(s, newvisited, nodes, twice);
            }
            else if (!twice && !next.isBig() && !next.isStart() && !next.isEnd())
            {
                count += NumberofPathTwice(s, newvisited, nodes, true);
            }
        }

    }

    return count;
}


class Node
{
    public String Name;
    public List<string> connections = new List<string>(20);
    public bool isBig()
    {
        return Name == Name.ToUpper();
    }
    public bool isEnd()
    {
        return Name == "end";
    }
    public bool isStart()
    {
        return Name == "start";
    }
}

