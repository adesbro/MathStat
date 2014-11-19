namespace MathStat.Distribution
{
    public interface IRangeComparer<TItem, TValue>
    {
        /// <summary>
        /// Returns 0 if value is in the specified range, less than zero if above, 
        /// greater than zero if below.
        /// </summary>
        int Compare(RangeItem<TItem, TValue> rangeItem, TValue value);
    }
}