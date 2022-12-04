string filePath = @"./input.txt";

if (!File.Exists(filePath))
{
    Console.WriteLine("no file found");
    return;
}

var linesEnumerable = File.ReadLines(filePath);

int temp = 0;
List<int> caloriesList = new List<int>();

foreach(var line in linesEnumerable)
{
    if(line.Length == 0)
    {
        caloriesList.Add(temp);
        temp = 0;
    }
    else
    {
        temp += int.Parse(line);
    }
}

Console.WriteLine($"Max calories count:{caloriesList.Max()}");

Console.WriteLine($"Sum of calories for top 3 elves: {caloriesList.OrderByDescending(x=>x).Take(3).Sum()}");