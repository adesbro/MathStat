namespace MathStat.Distribution
{
    public class NumberRangeComparer<TItem> : IRangeComparer<TItem, int>
    {
        public int Compare(RangeItem<TItem, int> rangeItem, int value)
        {
            if (rangeItem.MinValue.CompareTo(value) > 0)
                return 1;

            if (rangeItem.MaxValue.CompareTo(value) < 0)
                return -1;

            return 0;
        }
    }
}