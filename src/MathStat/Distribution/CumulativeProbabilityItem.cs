namespace MathStat.Distribution
{
    public class CumulativeProbabilityItem<TItem, TValue> : IRange<TValue> 
        where TItem : class
    {
        public TItem Item { get; set; }
        public TValue MinValue { get; set; }
        public TValue MaxValue { get; set; }
        public double CumulativeProbability { get; set; }
    }
}