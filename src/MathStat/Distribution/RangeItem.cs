namespace MathStat.Distribution
{
    public class RangeItem<TItem, TValue>
    {
        public TItem Item { get; set; }
        public TValue MinValue { get; set; }
        public TValue MaxValue { get; set; }
    }
}