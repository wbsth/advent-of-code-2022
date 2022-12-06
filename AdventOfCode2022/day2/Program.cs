internal class Program
{
    private static void Main(string[] args)
    {
        const string filePath = @"./input.txt";

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
        const int shift = 65;
        int[,] pointArray = new int[,] { { 3, 0, 6 }, { 6, 3, 0 }, { 0, 6, 3 } };
        int[,] desiredMoveArray = new int[,] { { 2, 0, 1 }, { 0, 1, 2 }, { 1, 2, 0 }};
        int scoreSumSimple = 0;
        int scoreSumAdv = 0;

        foreach (var lin in linesEnumerable)
        {
            var opponent = (int)lin[0] - shift;
            var player = (int)moveDictionary[lin[2]] - shift;

            // simple way
            var moveScore = scoringDictionary[player];
            var outcomeScore = pointArray[player, opponent];
            scoreSumSimple += moveScore + outcomeScore;
            
            // advanced way
            var desiredMove = desiredMoveArray[opponent, player]; // 0/1/2
            var advMoveScore = scoringDictionary[desiredMove];
            var advOutcomeScore = pointArray[desiredMove, opponent];
            scoreSumAdv += advMoveScore + advOutcomeScore;
        }

        Console.WriteLine($"Simple:{scoreSumSimple}");
        Console.WriteLine($"Advanced:{scoreSumAdv}");

    }
}


