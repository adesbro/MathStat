namespace MathStat.Distribution
{
    public class Int64RangeComparer : IRangeComparer<long>
    {
        public int Compare(IRange<long> range, long value)
        {
            if (range.MinValue.CompareTo(value) > 0)
                return 1;

            if (range.MaxValue.CompareTo(value) < 0)
                return -1;

            return 0;
        }
    }
}