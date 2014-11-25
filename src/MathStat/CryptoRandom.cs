using System;
using System.Security.Cryptography;

namespace MathStat
{
    /// <summary>
    /// Provides better random number generation than <c>System.Random</c> but uses a very
    /// similar interface for familiarity.
    /// </summary>
    /// <remarks>
    /// Useful help from MSDN article: http://msdn.microsoft.com/en-us/magazine/cc163367.aspx
    /// </remarks>
    public class CryptoRandom
    {
        private readonly RandomNumberGenerator _randomNumberGenerator;

        public CryptoRandom(RandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator;
        }

        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned.</param>
        public int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue) throw new ArgumentOutOfRangeException("minValue");
            if (minValue == maxValue) return minValue;

            const long max = (1 + (Int64)UInt32.MaxValue);
            var range = maxValue - minValue;
            
            while (true)
            {
                var rand = GetRandomUInt32();
                var remainder = max % range;
                if (rand < max - remainder)
                {
                    return (Int32)(minValue + (rand % range));
                }
            }
        }

        /// <summary>
        /// Returns a non-negative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number returned.</param>
        public int Next(int maxValue)
        {
            return Next(0, maxValue);
        }

        /// <summary>
        /// Returns a non-negative random number.
        /// </summary>
        public int Next()
        {
            return GetRandomInt32();
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        public double NextDouble()
        {
            return ((double) GetRandomUInt32())/UInt32.MaxValue;
        }

        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        public void NextBytes(byte[] buffer)
        {
            FillWithRandomBytes(buffer);
        }

        private int GetRandomInt32()
        {
            var buffer = new byte[4];
            FillWithRandomBytes(buffer);
            // Strip high-order bit (sign) to keep non-negative
            return BitConverter.ToInt32(buffer, 0) & Int32.MaxValue; 
        }

        private uint GetRandomUInt32()
        {
            var buffer = new byte[4];
            FillWithRandomBytes(buffer);
            return BitConverter.ToUInt32(buffer, 0);
        }

        private void FillWithRandomBytes(byte[] buffer)
        {
            _randomNumberGenerator.GetBytes(buffer);
        }
    }
}