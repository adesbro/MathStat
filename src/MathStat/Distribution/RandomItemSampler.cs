using System;
using System.Security.Cryptography;

namespace MathStat.Distribution
{
    /// <summary>
    /// Provides the functionality to generate pseudo-random values distributed according 
    /// to a given frequency distribution.
    /// </summary>
    /// <remarks>http://en.wikipedia.org/wiki/Pseudo-random_number_sampling</remarks>
    public class RandomItemSampler<TItem> : IDisposable
        where TItem : class
    {
        private readonly CryptoRandom _random;
        private readonly CumulativeDistribution<TItem> _distribution;

        public RandomItemSampler(FrequencyTable<TItem> frequencyDistribution)
        {
            _random = new CryptoRandom();
            _distribution = new CumulativeDistribution<TItem>(frequencyDistribution);
        }

        /// <summary>
        /// Returns a random item according to the frequency distribution.
        /// </summary>
        public TItem Next()
        {
            var minValue = _distribution.MinValue;
            var maxValue = _distribution.MaxValue;
            var randomValue = _random.Next(minValue, maxValue + 1);
            return _distribution[randomValue].Item;
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

        #region IDisposable Implementation

        private bool IsDisposed { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!IsDisposed)
            {
                if (isDisposing)
                {
                    _random.Dispose();
                }
                IsDisposed = true;
            }
        }

        ~RandomItemSampler()
        {
            Dispose(false);
        }

        #endregion
    }
}