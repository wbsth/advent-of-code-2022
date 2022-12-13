class Program
{
    public static void Main(string[] args)
    {
        const string filePath = @"./input3.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("no file found");
            return;
        }

        var rows = File.ReadLines(filePath).ToList();

        var gridHeight = rows.Count;
        var gridWidth = rows[0].Length;

        var terrainGrid = new TerrainGrid(gridHeight, gridWidth);
        terrainGrid.BuildTerrain(rows);
        // terrainGrid.PrintTerrain();
        terrainGrid.GetStepCountForStartingPoint();

    }

    public class TerrainGrid
    {
        private readonly int _gridHeight;
        private readonly int _gridWidth;

        public TerrainGrid(int height, int width)
        {
            _gridHeight = height;
            _gridWidth = width;
            _grid = new Terrain[_gridWidth, _gridHeight];
        }

        private readonly Terrain[,] _grid;

        public void BuildTerrain(List<string> data)
        {
            for (var i = 0; i < _gridHeight; i++)
            {
                var row = data[i];

                for (int j = 0; j < _gridWidth; j++)
                {
                    var character = row[j];
                    int height = 0;

                    if (character == 'S')
                        height = 0;
                    else if (character == 'E')
                        height = 25;
                    else
                    {
                        height = (int)character - 97;
                    }

                    _grid[j, _gridHeight - i - 1] = new Terrain()
                    {
                        PositionX = j,
                        PositionY = _gridHeight - i - 1,
                        Elevation = height,
                        Character = character
                    };
                }
            }
        }

        public void PrintTerrain()
        {
            for (int i = 0; i < _gridHeight; i++)
            {
                for (int j = 0; j < _gridWidth; j++)
                {
                    var element = _grid[j, _gridHeight - i - 1];
                    Console.Write($"|{element.Elevation}|");
                }

                Console.Write('\n');
            }
        }

        public void GetStepCountForStartingPoint()
        {
            var startingPoint = _grid.Cast<Terrain>().First(x => x.Character == 'S');
            if(startingPoint != null)
            {
                List<Terrain[]> a;
                a = GetStepCount(new Terrain[]{startingPoint});
                var test = a.OrderBy(x => x.Length).ToList();
                PrintPath(test[0]);
            }
        }

        private void PrintPath(Terrain[] array)
        {
            for (int i = 0; i < _gridHeight; i++)
            {
                for (int j = 0; j < _gridWidth; j++)
                {
                    var tmp = array.FirstOrDefault(x => x.PositionX == j && x.PositionY == _gridHeight - i - 1);
                    if (tmp != null)
                    {
                        Console.Write('x');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.Write('\n');
            }
        }
        
        private List<TerrainStep[]> GetStepCount(TerrainStep[] cells)
        {
            int stepCount = cells.Length - 1;
            var visitedTerrain = cells.Select(x => x.terrain).ToList();
            var newestStep = cells.Last();

            if (newestStep.terrain.Character == 'E')
            {
                return new List<TerrainStep[]>{ cells } ;
            }
            else
            {
                List<Terrain[]> pathList = new List<Terrain[]>();
                // check cell above
                var cellAbove = Step(EStepDirection.Up, newestStep);
                if(cellAbove != null && !visitedTerrain.Contains(cellAbove.terrain))
                {
                    var pathAbove = GetStepCount(cells.Append(cellAbove).ToArray());
                    pathList.AddRange(pathAbove);
                }
                
                // check cell on left
                var cellLeft = Step(EStepDirection.Left, newestStep);
                if (cellLeft != null && !visitedTerrain.Contains(cellLeft.terrain))
                {
                    var pathLeft = GetStepCount(cells.Append(cellLeft).ToArray());
                    pathList.AddRange(pathLeft);
                }
                    
                // check cell on right
                var cellRight = Step(EStepDirection.Right, newestStep);
                if (cellRight != null && !visitedTerrain.Contains(cellRight.terrain))
                {
                    var pathRight = GetStepCount(cells.Append(cellRight).ToArray());
                    pathList.AddRange(pathRight);
                }
                    
                // check cell on bottom
                var cellBottom = Step(EStepDirection.Bottom, newestStep);
                if (cellBottom != null && !visitedTerrain.Contains(cellBottom.terrain))
                {
                    var pathBottom = GetStepCount(cells.Append(cellBottom).ToArray());
                    pathList.AddRange(pathBottom);
                }

                return pathList;
            }

            return new List<Terrain[]>{};
        }

        private TerrainStep? Step(EStepDirection direction, TerrainStep step)
        {
            var stepDirection = _stepPosition[direction];
            Terrain? newTerrainCell = null;
            try
            {
                newTerrainCell = _grid[step.terrain.PositionX + stepDirection.x, step.terrain.PositionY + stepDirection.y];
            }
            catch (IndexOutOfRangeException exception)
            {
                return null;
            }

            // check height condition
            return step.terrain.Elevation + 1 >= newTerrainCell.Elevation ? new TerrainStep(){terrain = newTerrainCell} : null;
        }
        
        private enum EStepDirection
        {
            Left,
            Up,
            Right,
            Bottom
        }

        private Dictionary<EStepDirection, (int x, int y)> _stepPosition = new()
            {
                { EStepDirection.Left, (-1, 0) },
                { EStepDirection.Up, (0, 1) },
                { EStepDirection.Right, (1, 0) },
                { EStepDirection.Bottom, (0, -1) },
            };

        private class Terrain
        {
            public int PositionX;
            public int PositionY;
            public int Elevation;
            public char Character;
        }

        private class TerrainStep
        {
            private EStepDirection? direction;
            public Terrain terrain;
        }
    }
}