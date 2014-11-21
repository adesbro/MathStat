namespace MathStat.Distribution
{
    public class Int32RangeComparer : IRangeComparer<int>
    {
        public int Compare(IRange<int> range, int value)
        {
            if (range.MinValue.CompareTo(value) > 0)
                return 1;

            if (range.MaxValue.CompareTo(value) < 0)
                return -1;

            return 0;
        }
    }
}