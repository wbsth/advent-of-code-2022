string filePath = @"./input.txt";

if (!File.Exists(filePath))
{
    Console.WriteLine("no file found");
    return;
}

var lines = File.ReadAllLines(filePath);

int boxWidth = lines[0][1..].Where((x, i) => i % 4 == 0).Count();
int boxHeight = 0;

for (int i = 0; i < lines.Length; i++)
{
    if (lines[i].Length == 0)
    {
        boxHeight = i - 1;
    }
}

var itemArray = new List<char>[boxWidth];
for (int i = 0; i < boxWidth; i++)
{
    itemArray[i] = new List<char>();
}

for (var i = 0; i < boxHeight; i++)
{
    var line = lines[i];
    var temp = line[1..].Where((x, i) => i % 4 == 0).ToList();
    for (int j = 0; j < temp.Count; j++)
    {
        if(temp[j] != ' ')
            itemArray[j].Add(temp[j]);
    }

}

for (var i = boxHeight + 2; i < lines.Length; i++)
{
    var move = lines[i];
    var splitted = move.Split(' ');
    var qty = int.Parse(splitted[1]);
    var from = int.Parse(splitted[3]);
    var to = int.Parse(splitted[5]);
    
    // get items to move
    var tmp = itemArray[from - 1].Take(qty).ToList();
    
    
    // add to new stack
    
    // step1
    foreach (var item in tmp)
    {
        itemArray[to - 1].Insert(0, item);
    }
    
    // step2
    //itemArray[to - 1].InsertRange(0,tmp);
    
    // remove from src
    itemArray[from - 1].RemoveRange(0,qty);
    
}

var tempList = new List<char>();
foreach (var list in itemArray)
{
    tempList.Add(list[0]);
}

var joinedString = new string(tempList.ToArray());
Console.WriteLine($"Crates on top: {joinedString}");
