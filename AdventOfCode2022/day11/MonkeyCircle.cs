namespace day11;

public class MonkeyCircle
{
    public List<Monkey> MonkeyList = new List<Monkey>();

    public int GetDivisionLcm()
    {
        var dividers = MonkeyList.Select(x => x.TestDivisionNumber).ToArray();
        return Helpers.LCM(dividers);
    }
    
    public void AddMonkey(Monkey monkey)
    {
        monkey.Circle = this;
        MonkeyList.Add(monkey);
    }

    public void HandleThrow(ulong item, int recipient)
    {
        var monkey = MonkeyList.Find(x => x.Id == recipient);
        if (monkey != null)
        {
            monkey.Items.Add(item);
        }
    }

    public void PlayRound(bool stressRelief = false)
    {
        foreach (var monkey in MonkeyList)
        {
            monkey.InspectItems(stressRelief);
        }
    }
}