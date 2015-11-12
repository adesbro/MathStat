using System.Threading;

namespace MathStat.Distribution
{
    public class FrequencyRow<TItem>
    {
        private int _occurrences;

        public TItem Item { get; set; }

        public int Occurrences
        {
            get { return _occurrences; }
            set { _occurrences = value; }
        }

        public void AddOccurrences(int count)
        {
            Interlocked.Add(ref _occurrences, count);
        }
    }
}