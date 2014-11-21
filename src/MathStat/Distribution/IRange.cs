namespace MathStat.Distribution
{
    public interface IRange<TValue>
    {
        TValue MinValue { get; set; }
        TValue MaxValue { get; set; }
    }
}