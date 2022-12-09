namespace day9;

public class Knot
{
    public Knot(int startPositionX, int startPositionY)
    {
        Location = (startPositionX, startPositionY);
        LocationHistory.Add((startPositionX, startPositionY));
    }
    public int Number { get; set; }
    public (int x, int y) Location;
    public List<(int x, int y)> LocationHistory = new();
    public string DisplayName => Number == 0 ? "H" : Number.ToString();
    public Knot? ChildKnot { get; set; }

    public void SetLocation(int x, int y)
    {
        Location.x = x;
        Location.y = y;
        
        if (!LocationHistory.Contains((Location.x, Location.y)))
        {
            LocationHistory.Add((Location.x, Location.y));
        }

        if (ChildKnot != null)
        {
            // parent and child in the same place, no need to move tail
            if (Location == ChildKnot.Location)
                return;
            
            var xDiff = Math.Abs(Location.x - ChildKnot.Location.x);
            var xDiffDir = Location.x - ChildKnot.Location.x > 0 ? 1 : -1; // head on the right
            var yDiff = Math.Abs(Location.y - ChildKnot.Location.y);
            var yDiffDir = Location.y - ChildKnot.Location.y > 0 ? 1 : -1; // head above
            
            // head and tail touch each other
            if(xDiff <= 1 && yDiff <= 1)
                return;
            
            var tempX = ChildKnot.Location.x;
            var tempY = ChildKnot.Location.y;
            
            // move tail in X direction
            if (xDiff >= 2 && yDiff == 0)
            {
                tempX += xDiffDir;
            }
            // move tail in Y direction
            else if (yDiff >= 2 && xDiff == 0)
            {
                tempY += yDiffDir;
            }
            // move tail in XY direction
            else
            {
                tempX += xDiffDir;
                tempY += yDiffDir;
            }
            ChildKnot.SetLocation(tempX, tempY);
        }
    }
}