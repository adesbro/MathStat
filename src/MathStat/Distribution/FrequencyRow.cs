using System.Threading;

namespace MathStat.Distribution
{
    public class FrequencyRow<TItem>
    {
        private long _occurrences;

        public TItem Item { get; set; }

        public long Occurrences
        {
            get { return _occurrences; }
            set { _occurrences = value; }
        }

        public void AddOccurrences(long count)
        {
            Interlocked.Add(ref _occurrences, count);
        }
    }
}