const string filePath = @"./input.txt";

if (!File.Exists(filePath))
{
    Console.WriteLine("no file found");
    return;
}

var linesEnumerable = File.ReadLines(filePath);
int prioritySum = 0;
int badgeSum = 0;

List<string> tempStringList = new List<string>();

foreach (var line in linesEnumerable)
{
    tempStringList.Add(line);
    if (tempStringList.Count == 3)
    {
        var temp = tempStringList[0].Intersect(tempStringList[1]);
        var a = string.Join("", temp);
        var res = a.Intersect(tempStringList[2]).First();
        
        var isUpper = char.IsUpper(res);
        var asciiCode = (int)res;
        var priorityNumber = isUpper ? asciiCode - 38: asciiCode - 96;
        badgeSum += priorityNumber;
        
        tempStringList.Clear();
    }
    
    var boxSize = line.Length / 2;
    var firstBox = line.Substring(0, boxSize);
    var secondBox = line.Substring(boxSize, boxSize);
    var sharedItems = firstBox.Intersect(secondBox);

    foreach (var item in sharedItems)
    {
        var isUpper = char.IsUpper(item);
        var asciiCode = (int)item;
        var priorityNumber = isUpper ? asciiCode - 38: asciiCode - 96;
        prioritySum += priorityNumber;
    }
}
Console.WriteLine($"Priority sum: {prioritySum}");
Console.WriteLine($"Badge sum: {badgeSum}");
