using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MoreLinq;

namespace MathStat.Distribution
{
    /// <summary>
    /// Represents a univariate (single variable) frequency table.
    /// </summary>
    /// <remarks>http://en.wikipedia.org/wiki/Frequency_distribution</remarks>
    public class FrequencyTable<TItem> : IEnumerable<FrequencyRow<TItem>>
    {
        private readonly ConcurrentBag<FrequencyRow<TItem>> _frequencies;
        private readonly ConcurrentDictionary<TItem, FrequencyRow<TItem>> _itemLookup;
        private long _totalOccurrences;

        public FrequencyTable()
        {
            _frequencies = new ConcurrentBag<FrequencyRow<TItem>>();
            _itemLookup = new ConcurrentDictionary<TItem, FrequencyRow<TItem>>();
        }

        /// <summary>
        /// The sum of <c>Occurrences</c> for each row.
        /// </summary>
        public long TotalOccurrences
        {
            get { return _totalOccurrences; }
        }

        /// <summary>
        /// Returns <c>true</c> if the item value exists within the frequency table.
        /// </summary>
        public bool ContainsItem(TItem value)
        {
            return _itemLookup.ContainsKey(value);
        }
        
        /// <summary>
        /// Adds a new row to the table.
        /// </summary>
        public FrequencyRow<TItem> Add(FrequencyRow<TItem> row)
        {
            _frequencies.Add(row);
            _itemLookup[row.Item] = row;
            Interlocked.Add(ref _totalOccurrences, row.Occurrences);
            return row;
        }

        /// <summary>
        /// Adds a collection of rows to the table.
        /// </summary>
        public void AddRange(IEnumerable<FrequencyRow<TItem>> rows)
        {
            rows.ForEach(r => Add(r));
        }

        /// <summary>
        /// If the item exists (as a row), the <c>count</c> will be added to the <c>Occurrences</c> value.
        /// If the item does not exist, a new row will be added and <c>Occurrences</c> set the <c>count</c>.
        /// </summary>
        public FrequencyRow<TItem> AddOrUpdate(TItem value, long count)
        {
            if (_itemLookup.ContainsKey(value))
            {
                var row = _itemLookup[value];
                row.AddOccurrences(count);
                Interlocked.Add(ref _totalOccurrences, count);
                return row;
            }

            var newRow = new FrequencyRow<TItem>
            {
                Item = value,
                Occurrences = count
            };
            return Add(newRow);
        }

        /// <summary>
        /// Creates a frequency table that can be used to attempt to equalize the frequencies of this
        /// table across a number of data segments.
        /// </summary>
        /// <param name="numberOfItems">The number of items to add to attempt to equalize the frequencies</param>
        /// <param name="divisor">How many parts to split the data up into for equalizing. 
        /// A value of 4 will split the data into quartiles.</param>
        public FrequencyTable<TItem> CreateNormalizingFrequencies(int numberOfItems, int divisor)
        {
            var result = new FrequencyTable<TItem>();

            var dataSize = _frequencies.Count / divisor;
            var orderedDistribution = this.OrderBy(r => r.Occurrences).ToList();
            for (int count = 1; count <= divisor; count++)
            {
                var startPos = (count - 1)*dataSize;
                var endPos = startPos + dataSize - 1;
                var endPosOccurrences = orderedDistribution[endPos].Occurrences;

                for (int index = startPos; index < endPos; index++)
                {
                    result.AddOrUpdate(orderedDistribution[index].Item,
                        endPosOccurrences - orderedDistribution[index].Occurrences);
                }

                // TODO: need to 'top up' the frequencies until we've used up the numberOfItems passed in
            }

            return result;
        }

        public void Merge(IEnumerable<FrequencyRow<TItem>> rows)
        {
            foreach (var row in rows)
            {
                AddOrUpdate(row.Item, row.Occurrences);
            }
        }
        
        public IEnumerator<FrequencyRow<TItem>> GetEnumerator()
        {
            return _frequencies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public FrequencyTable<long> ToFrequencyCounts()
        {
            var rows = this
                .GroupBy(fd => fd.Occurrences)
                .Select(row => new FrequencyRow<long> { Item = row.Key, Occurrences = row.Count() });

            var table = new FrequencyTable<long>();
            table.AddRange(rows);

            return table;
        }   
    }
}
