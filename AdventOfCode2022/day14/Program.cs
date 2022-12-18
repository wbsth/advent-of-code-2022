// load text from file
namespace day14;

class Program
{
    public static void Main(string[] args)
    {
        const string filePath = @"./input.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("no file found");
        }

        var lines = File.ReadAllLines(filePath);

        var myCave = new Cave(lines, true);
        myCave.BuildCaveGrid();

        
        // drop sand until it overflows
        // count how many times it was dropped
        var cnt = 0;
        while (!myCave.SandOverflow)
        {
            cnt += 1;
            myCave.DropSand();
        }
        
        //myCave.PrintCaveGrid();
        Console.WriteLine(cnt - 1);
    }

    class Cave
    {
        public Cave(string[] lines, bool floorLevel = false)
        {
            BuildPathList(lines);
            
            _maxY = ParsedPath.Max(x => Math.Max(x.From.y, x.To.y));
            _minY = 0; // ParsedPath.Min(x => Math.Min(x.From.y, x.To.y));
            
            _maxX = ParsedPath.Max(x => Math.Max(x.From.x, x.To.x));
            _minX = ParsedPath.Min(x => Math.Min(x.From.x, x.To.x));

            if (floorLevel)
            {
                ParsedPath.Add(new Path((_minX - 10000, _maxY + 2), (_minX + 10000, _maxY + 2)));
                _maxY = ParsedPath.Max(x => Math.Max(x.From.y, x.To.y));
                // _minY = ParsedPath.Min(x => Math.Min(x.From.y, x.To.y));
            
                _maxX = ParsedPath.Max(x => Math.Max(x.From.x, x.To.x));
                _minX = ParsedPath.Min(x => Math.Min(x.From.x, x.To.x));
            }
            
            _width = _maxX - _minX + 1 + 2; // extra one cell on left and right
            _height = _maxY + 1; // - _minY + 4; // extra three cell above
            
            _widthOffset = _minX - 1;
            _heightOffset = _minY; // - 3;
            
            // create grid containing empty cells
            caveGrid = new CaveCell[_height, _width];
            for (var i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    caveGrid[i, j] = new CaveCell() { CellType = ECaveCellType.Empty };
                }
            }
        }

        private CaveCell[,] caveGrid;
        List<Path> ParsedPath = new List<Path>();
        private readonly int _maxX;
        private readonly int _maxY;
        private readonly int _minX;
        private readonly int _minY;
        private readonly int _height;
        private readonly int _width;
        private readonly int _heightOffset;
        private readonly int _widthOffset;
        public bool SandOverflow { get; set; } = false;
        
        public void DropSand()
        {
            var xPos = 500;
            // drop sand from 500, 0 location
            for (int i = 0; i < _height; i++)
            {
                var cell = caveGrid[0 + i, xPos - _widthOffset];
                
                // no move possible !
                if (i == 0 && cell.CellType == ECaveCellType.Sand)
                {
                    SandOverflow = true;
                    return;
                }
                if (cell.CellType is ECaveCellType.Rock or ECaveCellType.Sand)
                {
                    // check cell on the left
                    var lCell = caveGrid[0 + i, xPos - _widthOffset - 1];
                    if (lCell.CellType == ECaveCellType.Empty)
                    {
                        xPos -= 1;
                        continue;
                    }
                    
                    // check cell on the right
                    var rCell = caveGrid[0 + i, xPos - _widthOffset + 1];
                    if (rCell.CellType == ECaveCellType.Empty)
                    {
                        xPos += 1;
                        continue;
                    }
                    
                    caveGrid[0 + i - 1, xPos - _widthOffset].CellType = ECaveCellType.Sand;
                    return;
                }
            }

            SandOverflow = true;
        }
        
        private void BuildPathList(string[] lines)
        {
            foreach (var line in lines)
            {
                var splitted = line.Split(" -> ");
                for (int i = 0; i < splitted.Length - 1; i++)
                {
                    var from = Path.ParseLocation(splitted[i]);
                    var to = Path.ParseLocation(splitted[i + 1]);
                    var newPath = new Path(from, to);
                    ParsedPath.Add(newPath);
                }
            }
        }

        public void BuildCaveGrid()
        {
            foreach (var path in ParsedPath)
            {
                var locations = path.GetAllPathLocations();
                foreach (var location in locations)
                {
                    caveGrid[location.y - _heightOffset, location.x - _widthOffset].CellType = ECaveCellType.Rock;
                }
            }
        }

        public void PrintCaveGrid()
        {
            for (var i = 0; i < _height; i++)
            {
                Console.Write($"{i}|");
                for (int j = 0; j < _width; j++)
                {
                    switch (caveGrid[i, j].CellType)
                    {
                        case ECaveCellType.Empty:
                            Console.Write('.');
                            break;
                        case ECaveCellType.Rock:
                            Console.Write('#');
                            break;
                        case ECaveCellType.Sand:
                            Console.Write('o');
                            break;
                    }
                }
                Console.WriteLine('\n');
            }
        }
    }

    class CaveCell
    {
        public ECaveCellType CellType { get; set; }
    }
    
    class Path
    {
        public Path((int x, int y) from, (int x, int y) to)
        {
            From = from;
            To = to;
        }

        public (int x, int y) From { get; set; }
        public (int x, int y) To { get; set; }
        
        public static (int x, int y) ParseLocation(string txt)
        {
            var splitted = txt.Split(",");
            var parsed = splitted.Select(int.Parse).ToList();
            return (parsed[0], parsed[1]);
        }

        public List<(int x, int y)> GetAllPathLocations()
        {
            var tempList = new List<(int x, int y)>{(From.x, From.y)};

            var xDirection = Math.Sign(To.x - From.x); // > 1 if path goes right, 0 if no move, < 1 if path goes left
            var yDirection = Math.Sign(To.y - From.y); // > 1 if path goes right, 0 if no move, < 1 if path goes left

            var tempLocation = From;
            while (tempLocation != To)
            {
                tempLocation.x += xDirection;
                tempLocation.y += yDirection;
                tempList.Add((tempLocation.x, tempLocation.y));
            }
            return tempList;
        }
    }

    enum ECaveCellType
    {
        Rock,
        Sand,
        Empty
    }
}