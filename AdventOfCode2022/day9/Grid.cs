namespace day9;

public class Grid
{
    public Grid(int width, int height, int knotNumber, int startPositionX, int startPositionY)
    {
        _width = width;
        _height = height;
        _knotNumber = knotNumber;

        for (int i = knotNumber - 1; i >= 0; i--)
        {
            var childKnot = KnotList.Find(x => x.Number == i + 1);
            KnotList.Add(new Knot(startPositionX, startPositionY)
            {
                Number = i,
                ChildKnot = childKnot??null
            });
        }
    }

    private readonly int _width;
    private readonly int _height;
    private readonly int _knotNumber;
    
    public List<Knot> KnotList = new List<Knot>(){};
    
    public void DrawCurrentGrid()
    {
        for (int i = _height - 1; i >= 0; i--)
        {
            for (int j = 0; j < _width; j++)
            {
                string charToWrite = ".";
                foreach (var knot in KnotList.OrderByDescending(x=>x.Number))
                {
                    if (knot.Location == (j, i))
                    {
                        charToWrite = knot.DisplayName;
                    }
                }
                Console.Write(charToWrite);
            }
            Console.Write('\n');
        }
    }

    public void ProcessMove(Move move)
    {
        for (int i = 0; i < move.Step; i++)
        {
            Move(move.Direction);
        }
    }

    private void Move(EMoveDirection direction)
    {
        var headKnot = KnotList.Find(x => x.Number == 0);
        if(headKnot == null) return;
        switch (direction)
        {
            case EMoveDirection.Up:
                headKnot.SetLocation(headKnot.Location.x, headKnot.Location.y += 1);
                break;
            case EMoveDirection.Down:
                headKnot.SetLocation(headKnot.Location.x, headKnot.Location.y -= 1);
                break;
            case EMoveDirection.Left:
                headKnot.SetLocation(headKnot.Location.x -= 1, headKnot.Location.y);
                break;
            case EMoveDirection.Right:
                headKnot.SetLocation(headKnot.Location.x += 1, headKnot.Location.y);
                break;
            case EMoveDirection.Unknown:
            default:
                break;
        }
    }
}