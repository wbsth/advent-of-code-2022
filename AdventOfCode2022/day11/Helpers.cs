namespace day11;

public class Helpers
{
    public static int LCM(int[] numbers)
    {
        return numbers.Aggregate(lcm);
    }
    private static int lcm(int a, int b)
    {
        return Math.Abs(a * b) / GCD(a, b);
    }
    
    private static int GCD(int a, int b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }
}