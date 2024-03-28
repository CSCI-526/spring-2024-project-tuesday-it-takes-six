using Game;

public class TimeTenseDataManager : IDataManager
{
    private readonly Publisher<TimeTense> timeTense = new(TimeTense.PRESENT);


    public bool IsPresent()
    {
        return timeTense.currentValue == TimeTense.PRESENT;
    }

    public bool IsPast()
    {
        return timeTense.currentValue == TimeTense.PAST;
    }

    public void SwitchTimeTense()
    {
        timeTense.Update(IsPresent() ? TimeTense.PAST : TimeTense.PRESENT);
    }

    public TimeTense GetTimeTense()
    {
        return timeTense.currentValue;
    }

    /// <summary>
    /// Set time tense to present
    /// </summary>
    public void Init()
    {
        timeTense.Update(TimeTense.PRESENT);
    }
    public Subscriber<TimeTense> CreateTimeTenseSubscriber()
    {
        return timeTense.CreateSubscriber();
    }
}
