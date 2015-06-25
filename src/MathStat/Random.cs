using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Policy;

namespace MathStat
{
    /// <summary>
    /// Provides better random number generation than <c>System.Random</c> but uses a very
    /// similar interface for familiarity.
    /// </summary>
    /// <remarks>
    /// Useful help from MSDN article: http://msdn.microsoft.com/en-us/magazine/cc163367.aspx
    /// Types such as Int32 and Int64 are explicitly defined as the .NET types (not C# 
    /// predefined types int and long) because the size is critical and it makes it 
    /// absolutely clear.
    /// </remarks>
    public abstract class Random<TRng> : IDisposable
        where TRng : RandomNumberGenerator, new()
    {
        private TRng _randomNumberGenerator;

        protected TRng RandomNumberGenerator
        {
            get { return _randomNumberGenerator ?? (_randomNumberGenerator = CreateRandomNumberGenerator()); }
        }

        protected virtual TRng CreateRandomNumberGenerator()
        {
            return new TRng();
        }

        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned.</param>
        public Int32 Next(Int32 minValue, Int32 maxValue)
        {
            if (minValue > maxValue) throw new ArgumentOutOfRangeException("minValue");
            if (minValue == maxValue) return minValue;

            const Int64 max = (1 + (Int64)UInt32.MaxValue);
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
        public Int32 Next(Int32 maxValue)
        {
            return Next(0, maxValue);
        }

        /// <summary>
        /// Returns a non-negative random number.
        /// </summary>
        public Int32 Next()
        {
            return GetRandomInt32();
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        public Double NextDouble()
        {
            return ((Double)GetRandomUInt32()) / UInt32.MaxValue;
        }

        /// <summary>
        /// Returns a random BigInteger of the size specified which must be a multiple of 8.
        /// </summary>
        public BigInteger NextBigInteger(Int32 bits)
        {
            if (bits % 8 != 0) throw new ArgumentException("bits must be a multiple of 8");

            var randBytes = NextBytes(bits / 8);
            return new BigInteger(randBytes);
        }

        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        public void NextBytes(byte[] buffer)
        {
            FillWithRandomBytes(buffer);
        }

        /// <summary>
        /// Creates an array of bytes and fills each with a random number.
        /// </summary>
        public byte[] NextBytes(Int32 bufferSize)
        {
            var buffer = new byte[bufferSize];
            FillWithRandomBytes(buffer);
            return buffer;
        }

        private Int32 GetRandomInt32()
        {
            var buffer = new byte[sizeof(Int32)];
            FillWithRandomBytes(buffer);
            // Strip high-order bit (sign) to keep non-negative
            return BitConverter.ToInt32(buffer, 0) & Int32.MaxValue; 
        }

        private UInt32 GetRandomUInt32()
        {
            var buffer = new byte[sizeof(UInt32)];
            FillWithRandomBytes(buffer);
            return BitConverter.ToUInt32(buffer, 0);
        }

        private void FillWithRandomBytes(byte[] buffer)
        {
            RandomNumberGenerator.GetBytes(buffer);
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
                    RandomNumberGenerator.Dispose();
                }
                IsDisposed = true;
            }
        }

        ~Random()
        {
            Dispose(false);
        }

        #endregion
    }
}