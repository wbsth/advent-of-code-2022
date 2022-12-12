// See https://aka.ms/new-console-template for more information

namespace day11;

internal static class Program
{
    public static void Main(string[] args)
    {
        const string filePath = @"./input.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("no file found");
            return;
        }

        var lines = File.ReadAllLines(filePath);
        
        var monkeyCircle = new MonkeyCircle();
        
        for (int i = 0; i < lines.Length; i+=7)
        {
            var input = lines[i..(i+6)];
            
            var firstLine = input[0];
            var monkeyNumber = int.Parse(new string(firstLine.Where(char.IsDigit).ToArray()));

            var secondLine = input[1];
            var startingItems = secondLine[18..].Split(", ").Select(ulong.Parse).ToList();
            
            var thirdLine = input[2];
            var thirdLineSplittedValues = thirdLine[23..].Split(' ');
            var operationType = thirdLineSplittedValues[0];
            var operationValue = thirdLineSplittedValues[1];
            
            var fourthLine = input[3];
            var testDivisionNumber = int.Parse(new string(fourthLine.Where(char.IsDigit).ToArray()));
            
            var fifthLine = input[4];
            var throwValueIfTrue = int.Parse(new string(fifthLine.Where(char.IsDigit).ToArray()));
            
            var sixthLine = input[5];
            var throwValueIfFalse = int.Parse(new string(sixthLine.Where(char.IsDigit).ToArray()));
            
            var monkey = new Monkey()
            {
                Id = monkeyNumber,
                Items = startingItems,
                TestDivisionNumber = testDivisionNumber,
                ThrowValueIfTrue = throwValueIfTrue,
                ThrowValueIfFalse = throwValueIfFalse,
                OperationType = operationType,
                OperationValue = operationValue
            };
            
            monkeyCircle.AddMonkey(monkey);

        }
        
        for (int i = 0; i < 10000 ; i++)
        {
            monkeyCircle.PlayRound(false);
        }

        var orderedByCount = monkeyCircle.MonkeyList.Select(x=>x.InspectCount).OrderByDescending(x=>x).ToList();
        var topTwo = orderedByCount.Take(2).ToList();
        var multiplied = (ulong)topTwo[0] * (ulong)topTwo[1];
        Console.WriteLine($"Level of monkey business: {multiplied}");
    }
}