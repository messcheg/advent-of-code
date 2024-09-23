using System.Runtime.CompilerServices;

Run(@"..\..\..\example_input.txt", true);
Run(@"E:\develop\advent-of-code-input\2022\day07.txt", false);

void Run(string inputfile, bool isTest)
{ 
    long supposedanswer1 = 95437;
    long supposedanswer2 = 24933642;

    var S = File.ReadAllLines(inputfile).ToList();
    long answer1 = 0;
    long answer2 = 0;
    List<mDir> alldirst = new List<mDir>();

    mDir root = new mDir();
    var current = root;
    foreach(var s in S)
    {
        var line = s.Split(' ');
        if (line[0] == "$")
        {
            if (line[1] == "cd")
            {
                if (line[2] ==@"/")
                {
                    current = root;
                }
                else if (line[2] == @"..")
                {
                    current = current.Parent;
                }
                else
                {
                    bool found = false;
                    foreach(var dir in current.Dirs)
                    {
                        if (dir.Name == line[2])
                        {
                            current = dir;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        var dir = new mDir(current);
                        current.Dirs.Add(dir);
                        current = dir;
                    }
                }
            }
        }
        else
        {
            if (line[0] == "dir")
            {
                bool found = false;
                foreach (var dir in current.Dirs)
                {
                    if (dir.Name == line[1])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    var dir = new mDir(current);
                    dir.Name = line[1];
                    current.Dirs.Add(dir);
                }
            }
            else
            {
                var size = long.Parse(line[0]);
                var name = line[1];
                bool found = false;
                foreach (var file in current.Files)
                {
                    if (file.Name == line[1]) found = true;
                }
                if (!found)
                {
                    var file = new mFile();
                    file.Name = name;
                    file.Size = size;
                    current.Files.Add(file);
                }
            }
        }
    }
    answer1 = FillSizes(root, alldirst);
    long space = root.Size -(70000000 - 30000000);
    answer2 = alldirst.Where(d => d.Size > space).Min(d => d.Size);
    w(1, answer1, supposedanswer1, isTest);
    w(2, answer2, supposedanswer2, isTest);
}

long FillSizes(mDir current, List<mDir> allDirs)
{
    long count = 0;
    foreach (var dir in current.Dirs)
    {
        count += FillSizes(dir, allDirs);
        current.Size += dir.Size;
        allDirs.Add(dir);
    }
    foreach (var file in current.Files)
    {
        current.Size += file.Size;
    }
    if (current.Size <= 100000)
    {
        return count + current.Size;
    }
    return count;
    
}

static void w<T>(int number, T val, T supposedval, bool isTest)
{
    string? v = (val == null) ? "(null)" : val.ToString();
    string? sv = (supposedval == null) ? "(null)" : supposedval.ToString();

    var previouscolour = Console.ForegroundColor;
    Console.Write("Answer Part " + number + ": ");
    Console.ForegroundColor = (v == sv) ? ConsoleColor.Green : ConsoleColor.White;
    Console.Write(v);
    Console.ForegroundColor = previouscolour;
    if (isTest)
    {
        Console.Write(" ... supposed (example) answer: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(sv);
        Console.ForegroundColor = previouscolour;
    }
    else
        Console.WriteLine();
}

class mFile
{
    public string Name { get; set; }
    public long Size { get; set; } 
}

class mDir
{
    public mDir(mDir? parent = null)
    {
        Parent = parent;
    }
    public string Name { get; set; }
    public mDir Parent { get; set; }
    public readonly List<mDir> Dirs = new List<mDir>();
    public readonly List<mFile> Files = new List<mFile>();
    public long Size { get; set; }
}