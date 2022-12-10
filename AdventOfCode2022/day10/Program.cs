namespace day10;

internal class Program
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
        
        int nextValue = 1;
        var values = new List<int>(){ };
        
        foreach (var line in lines)
        {
            var splitted = line.Split(" ");
            var instruction = splitted[0];
            
            switch (instruction)
            {
                case "noop":
                    values.Add(nextValue);
                    break;
                case "addx":
                    var value = int.Parse(splitted[1]);
                    values.Add(nextValue);
                    values.Add(nextValue);
                    nextValue = nextValue += value;
                    break;
            }
        }

        int sum = 0;
        for (int i = 19; i < 241; i += 40)
        {
            var add = (i+1) * values[i];
            sum += add;
        }
        Console.WriteLine($"Sum of signal strenghts: {sum}");

        int drawCount = 0;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 40; j++)
            {
                var spritePosition = values[drawCount];
                if(spritePosition + 1 >= j && spritePosition - 1 <= j)
                    Console.Write('#');
                else
                    Console.Write(".");
                
                drawCount += 1;
            }
            Console.Write('\n');
        }
        
    }
}