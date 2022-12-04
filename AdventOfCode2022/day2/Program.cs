internal class Program
{
    private static void Main(string[] args)
    {
        string filePath = @"./input.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("no file found");
            return;
        }

        var linesEnumerable = File.ReadLines(filePath);

        // A - rock - X
        // B - paper - Y
        // C - scissors - Z
        
        // X - lose
        // Y - draw
        // Z - lose

        var scoringDictionary = new int[] { 1, 2, 3 };

        var moveDictionary = new Dictionary<char, char>
        {
            { 'X', 'A' },
            { 'Y', 'B' },
            { 'Z', 'C' }
        };
        int shift = 65;
        int[,] pointArray = new int[,] { { 3, 0, 6 }, { 6, 3, 0 }, { 0, 6, 3 } };
        int scoreSumSimple = 0;
        int scoreSumAdv = 0;

        foreach (var lin in linesEnumerable)
        {
            var opponent = (int)lin[0] - shift;
            var player = (int)moveDictionary[lin[2]] - shift;

            var moveScore = scoringDictionary[player];
            var outcomeScore = pointArray[player, opponent];

            scoreSumSimple += moveScore += outcomeScore;
        }

        Console.WriteLine($"Simple:{scoreSumSimple}");
        Console.WriteLine($"Advanced:{scoreSumAdv}");

    }
}


