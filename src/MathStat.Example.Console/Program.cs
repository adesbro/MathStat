using System;
using System.Linq;
using MathStat.Distribution;

namespace MathStat.Example.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var sampleFrequencies = new FrequencyTable<string>();
            sampleFrequencies.AddRange(new []
            {
                new FrequencyRow<string> { Item = "Adrian", Occurrences = 20 },
                new FrequencyRow<string> { Item = "John", Occurrences = 85 },
                new FrequencyRow<string> { Item = "David", Occurrences = 52 },
                new FrequencyRow<string> { Item = "June", Occurrences = 5 },
                new FrequencyRow<string> { Item = "Freya", Occurrences = 32 },
                new FrequencyRow<string> { Item = "Lars", Occurrences = 150 },
                new FrequencyRow<string> { Item = "Jenny", Occurrences = 66 },
            });

            OutputFrequencyTable("Hard-coded", sampleFrequencies);

            const int numberOfSamples = 10000;
            System.Console.WriteLine("Generating {0:N0} samples using hard-coded frequency distribution", numberOfSamples);
            System.Console.WriteLine();

            var random = new Random(Environment.TickCount);
            var sampler = new RandomItemSampler<string>(random, sampleFrequencies);
            var generatedFrequencies = sampler.Next(numberOfSamples);

            OutputFrequencyTable("Generated", generatedFrequencies);

            System.Console.ReadKey();
        }

        private static void OutputFrequencyTable(string name, FrequencyTable<string> sampleFrequencies)
        {
            System.Console.WriteLine("{0} frequencies as a %", name);
            foreach (var row in sampleFrequencies.OrderBy(f => f.Item))
            {
                System.Console.WriteLine(" - {0} = {1:F2}%", row.Item,
                    100.0*row.Occurrences/sampleFrequencies.TotalOccurrences);
            }
            System.Console.WriteLine();
        }
    }
}
