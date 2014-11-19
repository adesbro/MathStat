using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace MathStat.Distribution
{
    /// <summary>
    /// Provides the functionality to generate pseudo-random values distributed according 
    /// to a given frequency distribution.
    /// </summary>
    /// <remarks>http://en.wikipedia.org/wiki/Pseudo-random_number_sampling</remarks>
    public class RandomItemSampler<TItem>
    {
        private readonly Random _random;
        private readonly List<RangeItem<TItem, int>> _rangeItems;

        public RandomItemSampler(Random random, FrequencyTable<TItem> frequencyDistribution)
        {
            _random = random;
            var runningTotal = 1;
            _rangeItems = frequencyDistribution
                .Select(row => new RangeItem<TItem, int>
                {
                    Item = row.Item,
                    MinValue = runningTotal,
                    MaxValue = runningTotal + row.Occurrences
                })
                .Pipe(item => runningTotal = item.MaxValue + 1)
                .ToList();
        }

        /// <summary>
        /// Returns a random item according to the frequency distribution.
        /// </summary>
        public TItem Next()
        {
            var minValue = _rangeItems.First().MinValue;
            var maxValue = _rangeItems.Last().MaxValue;
            var num = _random.Next(minValue, maxValue + 1);
            return _rangeItems.BinarySearch(num, new NumberRangeComparer<TItem>());
        }

        /// <summary>
        /// Returns a number of items according to the frequency distribution, compiled into a <c>FrequencyTable</c>.
        /// </summary>
        public FrequencyTable<TItem> Next(int numberOfItems)
        {
            var frequencyTable = new FrequencyTable<TItem>();
            for (int count = 1; count <= numberOfItems; count++)
            {
                var item = Next();
                frequencyTable.AddOrUpdate(item, 1);
            }
            return frequencyTable;
        }
    }
}