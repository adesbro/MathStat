using System.Collections.Generic;

namespace MathStat.Distribution
{
    public static class ListExtensions
    {
        public static TItem BinarySearch<TItem, TValue>(this IList<RangeItem<TItem, TValue>> ranges, TValue value, IRangeComparer<TItem, TValue> comparer)
        {
            int min = 0;
            int max = ranges.Count - 1;

            while (min <= max)
            {
                int mid = (min + max) / 2;
                int comparison = comparer.Compare(ranges[mid], value);
                if (comparison == 0)
                {
                    return ranges[mid].Item;
                }
                if (comparison < 0)
                {
                    min = mid + 1;
                }
                else if (comparison > 0)
                {
                    max = mid - 1;
                }
            }
            return ranges[min].Item;
        }
    }
}