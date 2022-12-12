namespace day11;

public class Monkey
{
    public int Id { get; set; }
    public List<ulong> Items { get; set; }
    public int TestDivisionNumber { get; set; }
    public int ThrowValueIfTrue { get; set; }
    public int ThrowValueIfFalse { get; set; }
    public string OperationType { get; set; }
    public string OperationValue { get; set; }
    public MonkeyCircle Circle { get; set; }
    public int InspectCount { get; set; }
    
    public void InspectItems(bool stressRelief = false)
    {
        var lcm = Circle.GetDivisionLcm();
        foreach (var item in Items)
        {
            InspectCount += 1;
            
            // monkey inspect item
            var valueAferInspect = Inspect(item);
            
            // stress relieve
            if (stressRelief)
                valueAferInspect /= 3;

            valueAferInspect = valueAferInspect % (ulong)lcm;

                // monkey test worry level
            var testPassed = valueAferInspect % (ulong)TestDivisionNumber == 0;
            
            // throw item
            Circle.HandleThrow(valueAferInspect, testPassed ? ThrowValueIfTrue : ThrowValueIfFalse);
        }
        Items.Clear();
    }

    private ulong Inspect(ulong item)
    {
        var value = OperationValue == "old" ? item : ulong.Parse(OperationValue);
        
        switch (OperationType)
        {
            case "+":
                return item += value;
            case "*":
                return item *= value;
                break;
        }

        return 0;
    }

    // private bool CheckDivisibility(Item item)
    // {
    //     foreach (var multiplier in item.Multipliers)
    //     {
    //         if(TestDivisionNumber)
    //     }
    //     return true;
    // }
}