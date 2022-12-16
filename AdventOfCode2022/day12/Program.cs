class Program
{
    public static void Main(string[] args)
    {
        // load text from file
        const string filePath = @"./input.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("no file found");
            return;
        }
        
        var rows = File.ReadLines(filePath).ToList();

        var terrainGrid = new TerrainGrid(rows);
        
        terrainGrid.BuildTerrain(rows);

        var explorer = new TerrainExplorer(terrainGrid, terrainGrid.StartPosition);
        while (explorer.IsProcessing)
        {
            Position currentPosition = explorer.Explore();
            if (currentPosition == terrainGrid.EndPosition)
            {
                var distance = explorer.GetDistance(currentPosition);
                Console.WriteLine(distance + 1);
            }
        }

        var allTerrain = terrainGrid.Grid.Cast<TerrainGrid.Terrain>();
        var lowTerrain = allTerrain.Where(x => x.Character == 'a');

        int lowest = Int32.MaxValue - 1;
        int cnt = 0;
        foreach (var terrain in lowTerrain)
        {
            Console.WriteLine(cnt);
            var tempPosition = new Position(terrain.RowIndex, terrain.ColumnIndex);
            var newExplorer = new TerrainExplorer(terrainGrid, tempPosition);
            while (newExplorer.IsProcessing)
            {
                Position currentPosition = newExplorer.Explore();
                if (currentPosition == terrainGrid.EndPosition)
                {
                    var distance = newExplorer.GetDistance(currentPosition);
                    if (distance < lowest)
                        lowest = distance;
                }
            }

            cnt += 1;
        }
        Console.WriteLine($"Part 2: {1 + lowest}");

    }

    public class TerrainGrid
    {
        public readonly int GridHeight;
        public readonly int GridWidth;
        public readonly Terrain[,] Grid;
        public Position StartPosition { get; set; }
        public Position EndPosition { get; set; }
        
        public TerrainGrid(List<string> data) {
            
            GridHeight = data.Count;
            GridWidth = data[0].Length;
            Grid = new Terrain[GridHeight, GridWidth];
            BuildTerrain(data);
        }

        public List<Position> GetPossiblePositions(Position fromPosition)
        {
            List<Position> tempPositionList = new List<Position>();
            foreach (var candidatePosition in fromPosition.NeighbouringPositions())
            {
                if(CheckMove(fromPosition, candidatePosition))
                    tempPositionList.Add(candidatePosition);
            }
            return tempPositionList;
        }

        public bool CheckMove(Position fromPosition, Position toPosition)
        {
            if (toPosition.columnIndex < 0 ||
                toPosition.columnIndex > GridWidth - 1 ||
                toPosition.rowIndex < 0 ||
                toPosition.rowIndex > GridHeight - 1)
                return false;
            
            int fromHeight = Grid[fromPosition.rowIndex, fromPosition.columnIndex].Elevation;
            int toHeight = Grid[toPosition.rowIndex, toPosition.columnIndex].Elevation;
            return fromHeight + 1 >= toHeight;
        }
        
        public void BuildTerrain(List<string> data)
        {
            for (var i = 0; i < GridHeight; i++) 
            {
                var row = data[i];

                for (int j = 0; j < GridWidth; j++)
                {
                    var character = row[j];
                    int height = 0;

                    if (character == 'S')
                    {
                        height = 0;
                        StartPosition = new Position(GridHeight - i - 1, j);
                    }
                        
                    else if (character == 'E')
                    {
                        height = 25;
                        EndPosition = new Position(GridHeight - i - 1, j);
                        
                    }
                    else
                    {
                        height = (int)character - 97;
                    }

                    var terrain = new Terrain()
                    {
                        RowIndex = GridHeight - i - 1,
                        ColumnIndex = j,
                        Elevation = height,
                        Character = character
                    };
                    Grid[GridHeight - i - 1, j] = terrain;
                }
            }
        }

        public void PrintTerrain()
        {
            for (int i = 0; i < GridHeight; i++)
            {
                for (int j = 0; j < GridWidth; j++)
                {
                    var element = Grid[GridHeight - i - 1, j];
                    Console.Write($"|{element.Character}|");
                }

                Console.Write('\n');
            }
        }

        // public void GetStepCountForStartingPoint()
        // {
        //     var startingPoint = _grid.Cast<Terrain>().First(x => x.Character == 'S');
        //     if(startingPoint != null)
        //     {
        //         List<Terrain[]> a;
        //         a = GetStepCount(new Terrain[]{startingPoint});
        //         var test = a.OrderBy(x => x.Length).ToList();
        //         PrintPath(test[0]);
        //     }
        // }

        // private void PrintPath(Terrain[] array)
        // {
        //     for (int i = 0; i < _gridHeight; i++)
        //     {
        //         for (int j = 0; j < _gridWidth; j++)
        //         {
        //             var tmp = array.FirstOrDefault(x => x.PositionX == j && x.PositionY == _gridHeight - i - 1);
        //             if (tmp != null)
        //             {
        //                 Console.Write('x');
        //             }
        //             else
        //             {
        //                 Console.Write('.');
        //             }
        //         }
        //         Console.Write('\n');
        //     }
        // }
        
        // private List<TerrainStep[]> GetStepCount(TerrainStep[] cells)
        // {
        //     int stepCount = cells.Length - 1;
        //     var visitedTerrain = cells.Select(x => x.terrain).ToList();
        //     var newestStep = cells.Last();
        //
        //     if (newestStep.terrain.Character == 'E')
        //     {
        //         return new List<TerrainStep[]>{ cells } ;
        //     }
        //     else
        //     {
        //         List<Terrain[]> pathList = new List<Terrain[]>();
        //         // check cell above
        //         var cellAbove = Step(EStepDirection.Up, newestStep);
        //         if(cellAbove != null && !visitedTerrain.Contains(cellAbove.terrain))
        //         {
        //             var pathAbove = GetStepCount(cells.Append(cellAbove).ToArray());
        //             pathList.AddRange(pathAbove);
        //         }
        //         
        //         // check cell on left
        //         var cellLeft = Step(EStepDirection.Left, newestStep);
        //         if (cellLeft != null && !visitedTerrain.Contains(cellLeft.terrain))
        //         {
        //             var pathLeft = GetStepCount(cells.Append(cellLeft).ToArray());
        //             pathList.AddRange(pathLeft);
        //         }
        //             
        //         // check cell on right
        //         var cellRight = Step(EStepDirection.Right, newestStep);
        //         if (cellRight != null && !visitedTerrain.Contains(cellRight.terrain))
        //         {
        //             var pathRight = GetStepCount(cells.Append(cellRight).ToArray());
        //             pathList.AddRange(pathRight);
        //         }
        //             
        //         // check cell on bottom
        //         var cellBottom = Step(EStepDirection.Bottom, newestStep);
        //         if (cellBottom != null && !visitedTerrain.Contains(cellBottom.terrain))
        //         {
        //             var pathBottom = GetStepCount(cells.Append(cellBottom).ToArray());
        //             pathList.AddRange(pathBottom);
        //         }
        //
        //         return pathList;
        //     }
        //
        //     return new List<Terrain[]>{};
        // }
        //
        // private TerrainStep? Step(EStepDirection direction, TerrainStep step)
        // {
        //     var stepDirection = _stepPosition[direction];
        //     Terrain? newTerrainCell = null;
        //     try
        //     {
        //         newTerrainCell = _grid[step.terrain.PositionX + stepDirection.x, step.terrain.PositionY + stepDirection.y];
        //     }
        //     catch (IndexOutOfRangeException exception)
        //     {
        //         return null;
        //     }
        //
        //     // check height condition
        //     return step.terrain.Elevation + 1 >= newTerrainCell.Elevation ? new TerrainStep(){terrain = newTerrainCell} : null;
        // }
        //
        // private enum EStepDirection
        // {
        //     Left,
        //     Up,
        //     Right,
        //     Bottom
        // }
        //
        // private Dictionary<EStepDirection, (int x, int y)> _stepPosition = new()
        //     {
        //         { EStepDirection.Left, (-1, 0) },
        //         { EStepDirection.Up, (0, 1) },
        //         { EStepDirection.Right, (1, 0) },
        //         { EStepDirection.Bottom, (0, -1) },
        //     };

        public class Terrain
        {
            public int RowIndex;
            public int ColumnIndex;
            public int Elevation;
            public char Character;
        }

        // private class TerrainStep
        // {
        //     private EStepDirection? direction;
        //     public Terrain terrain;
        // }
    }

    public class TerrainExplorer
    {
        public TerrainExplorer(TerrainGrid grid, Position position)
        {
            Grid = grid;
            StartingPosition = position;
            DistanceArray = new int[grid.GridHeight, grid.GridWidth];
            for (int i = 0; i < grid.GridHeight; i++)
            {
                for (int j = 0; j < grid.GridWidth; j++)
                {
                    DistanceArray[i, j] = -1;
                }
            }
            ExploreQueue = new Queue<Position>();
            ExploreQueue.Enqueue(StartingPosition);
            
        }
        
        private TerrainGrid Grid { get; }
        private Position StartingPosition { get; }
        private int[,] DistanceArray;
        
        private Queue<Position> ExploreQueue { get; set; }
        public bool IsProcessing => ExploreQueue.Any();

        public Position Explore()
        {
            if (!ExploreQueue.Any())
            {
                throw new Exception();
            }
            
            Position dequeued = ExploreQueue.Dequeue();
            var possiblePositions =  Grid.GetPossiblePositions(dequeued);
            foreach (var possiblePosition in possiblePositions)
            {
                // was not visited before
                if (DistanceArray[possiblePosition.rowIndex, possiblePosition.columnIndex] == -1)
                {
                    DistanceArray[possiblePosition.rowIndex, possiblePosition.columnIndex] =
                        DistanceArray[dequeued.rowIndex, dequeued.columnIndex] + 1;
                    ExploreQueue.Enqueue(possiblePosition);
                }
            }
            return dequeued;
        }

        public int GetDistance(Position currentPosition)
        {
            return DistanceArray[currentPosition.Row, currentPosition.Column];
        }
    }

    public record Position(int rowIndex, int columnIndex)
    {
        public readonly int Row = rowIndex;
        public readonly int Column = columnIndex;
        public Position North => new Position(Row + 1, Column);
        public Position East => new Position(Row, Column + 1);
        public Position South => new Position(Row - 1, Column);
        public Position West => new Position(Row, Column - 1);

        public List<Position> NeighbouringPositions()
        {
            return new List<Position>() { North, East, South, West };
        }

    }
}