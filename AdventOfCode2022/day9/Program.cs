namespace day9
{
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
    
            var movesList = new List<Move> { };

            int count = 0;
            foreach (var line in lines)
            {
                var strings = line.Split(' ');
                var direction = strings[0];
                var step = int.Parse(strings[1]);
        
                movesList.Add(new Move()
                {
                    Direction = Helpers.GetMoveDirection(direction),
                    Step = step,
                    Number = count
                });
                count += 1;
            }

            Grid grid = new Grid(1000, 1000, 10, 0, 0);
            
            foreach (var move in movesList)
            {
                grid.ProcessMove(move);
                //Console.WriteLine($"Move {move.Number}:");
                //grid.DrawCurrentGrid();
            }
            
            Console.Write($"Tail visited {grid.KnotList.OrderByDescending(x=>x.Number).First().LocationHistory.Count} locations!");
        }
    }
}

