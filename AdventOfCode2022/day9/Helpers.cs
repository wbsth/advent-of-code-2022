namespace day9;

public static class Helpers
{
    public static EMoveDirection GetMoveDirection(string dir)
    {
        return dir switch
        {
            "U" => EMoveDirection.Up,
            "L" => EMoveDirection.Left,
            "R" => EMoveDirection.Right,
            "D" => EMoveDirection.Down,
            _ => EMoveDirection.Unknown
        };
    }
}