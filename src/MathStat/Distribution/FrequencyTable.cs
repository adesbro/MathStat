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

        public FrequencyTable()
        {
            _frequencies = new List<FrequencyRow<TItem>>();
        }

        public int TotalOccurrences { get; private set; }

        public FrequencyRow<TItem> Add(TItem value, int count)
        {
            var newRow = new FrequencyRow<TItem>
            {
                Item = value,
                Occurrences = count
            };
            return Add(newRow);
        }

        public FrequencyRow<TItem> Add(FrequencyRow<TItem> row)
        {
            _frequencies.Add(row);
            TotalOccurrences += row.Occurrences;
            return row;
        }

        public void AddRange(IEnumerable<FrequencyRow<TItem>> rows)
        {
            rows.ForEach(r => Add(r));
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
