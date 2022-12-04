string filePath = @"./input.txt";

if (!File.Exists(filePath))
{
    Console.WriteLine("no file found");
    return;
}

var lines = File.ReadLines(filePath);

int containCounter = 0;
int overlapCounter = 0;

foreach(var line in lines)
{
    var splittedPairs = line.Split(',');
    var splittedValues = splittedPairs.Select(x =>
    {
        var temp = x.Split('-');
        return (int.Parse(temp[0]), int.Parse(temp[1]));
    }).ToList();

    if (DoesContain(splittedValues[0], splittedValues[1])) 
    {
        containCounter += 1;
    }

    if (DoesOverlap(splittedValues[0], splittedValues[1]))
    {
        overlapCounter += 1;
    }
}

Console.WriteLine($"Contain counter: {containCounter}");
Console.WriteLine($"Overlap counter: {overlapCounter}");

static bool DoesContain((int, int) firstPair, (int, int) secondPair)
{
    return (firstPair.Item1 >= secondPair.Item1 && firstPair.Item2 <= secondPair.Item2)
        || (secondPair.Item1 >= firstPair.Item1 && secondPair.Item2 <= firstPair.Item2);
}

static bool DoesOverlap((int, int) firstPair, (int, int) secondPair)
{
    return !((firstPair.Item2 < secondPair.Item1)
        || (secondPair.Item2 < firstPair.Item1));
}