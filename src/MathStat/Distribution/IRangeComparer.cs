namespace MathStat.Distribution
{
    public interface IRangeComparer<TValue>
    {
        /// <summary>
        /// Returns 0 if value is in the specified range, less than zero if above, 
        /// greater than zero if below.
        /// </summary>
        int Compare(IRange<TValue> range, TValue value);
    }
}