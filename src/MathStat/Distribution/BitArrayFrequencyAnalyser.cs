using System.Collections;
using System.Collections.Generic;
using MoreLinq;

namespace MathStat.Distribution
{
    /// <summary>
    /// Calculates the frequencies of each Bit position for a collection of <c>BitArray</c>s.
    /// </summary>
    public class BitArrayFrequencyAnalyser
    {
        public FrequencyTable<int> BitPositionFrequencies { get; private set; }
 
        public BitArrayFrequencyAnalyser()
        {
            BitPositionFrequencies = new FrequencyTable<int>();
        }

        public void AddRange(IEnumerable<BitArray> bitArrays)
        {
            bitArrays.ForEach(Add);    
        }

        public void Add(BitArray bitArray)
        {
            for(int index = 0; index < bitArray.Length; index++)
            {
                if (bitArray[index])
                {
                    BitPositionFrequencies.AddOrUpdate(index + 1, 1);
                }
            }
        }
    }
}