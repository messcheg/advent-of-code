//string inputfile = @"..\..\..\example_input.txt";
//string inputfile = @"..\..\..\real_input.txt";
string inputfile = @"..\..\..\niels_input.txt";

var instructions = File.ReadAllLines(inputfile).ToList();
int horizontal = 0;
int vertical_two = 0;
int aim = 0;

foreach(var  instruction in instructions)
{
    var split = instruction.Split(' ');
    string command = split[0];
    var amount = int.Parse(split[1]);
    if (command == "forward")
    {
        horizontal += amount;
        vertical_two += aim * amount;
    }
    else if (command == "down")
    {
        aim += amount;
    }
    else if (command == "up")
    {
        aim -= amount;
    }    
}

Console.WriteLine("Horizontal " + horizontal);
Console.WriteLine("Depth_one " + aim);
Console.WriteLine("Multiply_one " + (horizontal * aim));
Console.WriteLine("Depth_two " + vertical_two);
Console.WriteLine("Multiply_two " + (horizontal * vertical_two));
