using System.Collections;
using System.Collections.Generic;
using MoreLinq;

namespace MathStat.Distribution
{
    /// <summary>
    /// Represents a univariate (single variable) frequency table.
    /// </summary>
    /// <remarks>http://en.wikipedia.org/wiki/Frequency_distribution</remarks>
    public class FrequencyTable<TItem> : IEnumerable<FrequencyRow<TItem>>
    {
        private readonly List<FrequencyRow<TItem>> _frequencies;
        private readonly Dictionary<TItem, FrequencyRow<TItem>> _itemLookup;

        public FrequencyTable()
        {
            _frequencies = new List<FrequencyRow<TItem>>();
            _itemLookup = new Dictionary<TItem, FrequencyRow<TItem>>();
        }

        /// <summary>
        /// The sum of <c>Occurrences</c> for each row.
        /// </summary>
        public int TotalOccurrences { get; private set; }
        
        /// <summary>
        /// Adds a new row to the table.
        /// </summary>
        public FrequencyRow<TItem> Add(FrequencyRow<TItem> row)
        {
            _frequencies.Add(row);
            _itemLookup[row.Item] = row;
            TotalOccurrences += row.Occurrences;
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
        public FrequencyRow<TItem> AddOrUpdate(TItem value, int count)
        {
            if (_itemLookup.ContainsKey(value))
            {
                var row = _itemLookup[value];
                row.Occurrences += count;
                TotalOccurrences += count;
                return row;
            }

            var newRow = new FrequencyRow<TItem>
            {
                Item = value,
                Occurrences = count
            };
            return Add(newRow);
        }
        
        public IEnumerator<FrequencyRow<TItem>> GetEnumerator()
        {
            return ((IEnumerable<FrequencyRow<TItem>>)_frequencies).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
