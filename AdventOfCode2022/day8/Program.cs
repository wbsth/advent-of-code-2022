// so ugly, ugh

string filePath = @"./input.txt";

if (!File.Exists(filePath))
{
    Console.WriteLine("no file found");
    return;
}

var line = File.ReadAllLines(filePath);

var gridHeight = line.Length;
var gridWidth = line[0].Length;
var treeArray = new Tree[gridHeight, gridWidth];

for (var i = 0; i < gridHeight; i++)
{
    var maxLeft = 0;
    var maxRight = 0;
    
    for (var j = 0; j < gridWidth; j++)
    {
        var height = int.Parse(line[i][j].ToString());
        var onBorder = i == 0 || j == 0 || i == gridHeight - 1 || j == gridWidth - 1;
        
        treeArray[i, j] = new Tree()
        {
            Height = height,
            FromLeft = maxLeft,
            OnBorder = onBorder,
            IndexWidth = j,
            IndexHeight = i
        };
        
        if (height > maxLeft)
            maxLeft = height;
    }

    for (var k = gridWidth - 1; k >= 0; k--)
    {
        var height = int.Parse(line[i][k].ToString());
        treeArray[i, k].FromRight = maxRight;
        if (height > maxRight)
            maxRight = height;
    }
}

for (var i = 0; i < gridWidth; i++)
{
    var maxTop = 0;
    var maxBottom = 0;
    for (var j = 0; j < gridHeight; j++)
    {
        var height = int.Parse(line[j][i].ToString());
        treeArray[j, i].FromTop = maxTop;
        if (height > maxTop)
            maxTop = height;
    }
    
    for (var k = gridHeight - 1; k >= 0; k--)
    {
        var height = int.Parse(line[k][i].ToString());
        treeArray[k, i].FromBottom = maxBottom;
        if (height > maxBottom)
            maxBottom = height;
    }
}


for (var i = 1; i < gridHeight - 1; i++)
{

    for (var j = 1; j < gridWidth - 1; j++)
    {
        var tree = treeArray[i, j];

        var iBack = 1;
        var iForw = 1;
        var jBack = 1;
        var jForw = 1;
        
        // left
        while (true)
        {
            var height = treeArray[i, j - jBack].Height;
            if(height >= tree.Height || j - jBack == 0)
                break;
            jBack += 1;
        }
        
        // right
        while (true)
        {
            var height = treeArray[i, j + jForw].Height;
            if(height >= tree.Height || j + jForw == gridWidth - 1)
                break;
            jForw += 1;
        }
        
        // up
        while (true)
        {
            var height = treeArray[i - iBack, j].Height;
            if(height >= tree.Height || i - iBack == 0)
                break;
            iBack += 1;
        }
        
        // bottom
        while (true)
        {
            var height = treeArray[i + iForw, j].Height;
            if(height >= tree.Height || i + iForw == gridHeight - 1)
                break;
            iForw += 1;
        }

        tree.ScenicScore = iBack * iForw * jBack * jForw;
    }

}

var trees = treeArray.Cast<Tree>().ToArray();
var visibleTrees = trees.Count(x => x.IsVisible);
var maxScenicScore = trees.Max(x => x.ScenicScore);

Console.WriteLine($"Visible trees: {visibleTrees}");
Console.WriteLine($"Max scenic score: {maxScenicScore}");

class Tree
{
    public int IndexHeight { get; set; }
    public int IndexWidth { get; set; }
    public int Height { get; set; }
    public int FromLeft { get; set; }
    public int FromRight { get; set; }
    public int FromTop { get; set; }
    public int FromBottom { get; set; }
    
    public bool OnBorder { get; set; }
    public bool IsVisible => OnBorder ||
                             Height > FromLeft ||
                             Height > FromRight ||
                             Height > FromTop ||
                             Height > FromBottom;

    public int ScenicScore;


}