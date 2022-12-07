string filePath = @"./input.txt";

if (!File.Exists(filePath))
{
    Console.WriteLine("no file found");
    return;
}

var line = File.ReadAllText(filePath);

var markerIndex = -1;
var tempMarkerIndex = 0;
while (markerIndex == -1)
{
    var temp = line.Substring(tempMarkerIndex, 4);
    var isUnique = temp.Distinct().Count() == temp.Length;

    if (isUnique)
        markerIndex = tempMarkerIndex;

    tempMarkerIndex += 1;
}

var messageIndex = -1;
var tempMessageIndex = 0;
while (messageIndex == -1)
{
    var temp = line.Substring(tempMessageIndex, 14);
    var isUnique = temp.Distinct().Count() == temp.Length;

    if (isUnique)
        messageIndex = tempMessageIndex;

    tempMessageIndex += 1;
}


Console.WriteLine($"Market index: {markerIndex+4}");
Console.WriteLine($"Message index: {messageIndex+14}");