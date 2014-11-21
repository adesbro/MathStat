using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace MathStat.Distribution
{
    /// <summary>
    /// Converts a frequency distribution into a cumulative distribution.
    /// Each item has a cumulative probability between 0 and 1.
    /// </summary>
    /// <remarks>http://en.wikipedia.org/wiki/Cumulative_distribution_function</remarks>
    public class CumulativeDistribution<TItem> : IEnumerable<CumulativeProbabilityItem<TItem, int>>
        where TItem : class
    {
        private readonly List<CumulativeProbabilityItem<TItem, int>> _itemList;

        public CumulativeDistribution(FrequencyTable<TItem> frequencyDistribution)
        {
            int runningTotal = 0;

            _itemList = frequencyDistribution
                .OrderBy(fd => fd.Occurrences)
                .Select(row => new CumulativeProbabilityItem<TItem, int>
                {
                    Item = row.Item,
                    MinValue = runningTotal + 1,
                    MaxValue = runningTotal + row.Occurrences,
                    CumulativeProbability = 1.0*(runningTotal + row.Occurrences)/frequencyDistribution.TotalOccurrences
                })
                .Pipe(item => runningTotal = item.MaxValue)
                .ToList();
        }

        public int MinValue
        {
            get { return _itemList.First().MinValue; }
        }

        public int MaxValue
        {
            get { return _itemList.Last().MaxValue; }
        }

        public CumulativeProbabilityItem<TItem, int> this[TItem item]
        {
            get { return _itemList.Find(i => i.Item == item); }
        }

        public CumulativeProbabilityItem<TItem, int> this[int value]
        {
            get { return _itemList.BinarySearch(value, new Int32RangeComparer()); }
        }

        public IEnumerator<CumulativeProbabilityItem<TItem, int>> GetEnumerator()
        {
            return ((IEnumerable<CumulativeProbabilityItem<TItem, int>>)_itemList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}