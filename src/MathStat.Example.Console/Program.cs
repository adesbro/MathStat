using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using MathStat.Distribution;

namespace MathStat.Example.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var sampleFrequencies = new FrequencyTable<string>();
            sampleFrequencies.AddRange(new[]
            {
                new FrequencyRow<string> { Item = "Adrian", Occurrences = 20 },
                new FrequencyRow<string> { Item = "John", Occurrences = 85 },
                new FrequencyRow<string> { Item = "David", Occurrences = 52 },
                new FrequencyRow<string> { Item = "June", Occurrences = 5 },
                new FrequencyRow<string> { Item = "Freya", Occurrences = 32 },
                new FrequencyRow<string> { Item = "Lars", Occurrences = 150 },
                new FrequencyRow<string> { Item = "Jenny", Occurrences = 66 },
                new FrequencyRow<string> { Item = "Hector", Occurrences = 29 },
            });

            OutputFrequencyTable("Hard-coded", sampleFrequencies);

            const int numberOfSamples = 1000000;
            System.Console.WriteLine("Generating {0:N0} samples using hard-coded frequency distribution", numberOfSamples);
            System.Console.WriteLine();

            FrequencyTable<string> generatedFrequencies;
            using (new ConsoleTimeMeasure("Generate {0:N0} samples", numberOfSamples))
            {
                using (var sampler = new RandomItemSampler<string>(sampleFrequencies))
                {
                    generatedFrequencies = sampler.Next(numberOfSamples);
                }
            }

            OutputFrequencyTable("Generated", generatedFrequencies);
            OutputCumlativeDistribution("Generated", generatedFrequencies);

            FrequencyTable<string> equalizerFrequences;
            using (new ConsoleTimeMeasure("Create normalizing frequencies with {0:N0} total occurrences", sampleFrequencies.TotalOccurrences))
            {
                equalizerFrequences = sampleFrequencies.CreateNormalizingFrequencies(sampleFrequencies.TotalOccurrences, 1);
            }

            OutputFrequencyTable("Equalizer", equalizerFrequences);
            OutputCumlativeDistribution("Equalizer", equalizerFrequences);

            System.Console.ReadKey();
        }

        private static void OutputCumlativeDistribution(string name, FrequencyTable<string> frequencies)
        {
            System.Console.WriteLine("{0} cumulative distribution", name);
            var distribution = new CumulativeDistribution<string>(frequencies);

            foreach (var distributionItem in distribution)
            {
                System.Console.WriteLine(" - {0} = {1:F4}", distributionItem.Item, distributionItem.CumulativeProbability);
            }
            System.Console.WriteLine();
        }

        private static void OutputFrequencyTable(string name, FrequencyTable<string> frequencies)
        {
            System.Console.WriteLine("{0} frequencies - total occurrences = {1}", name, frequencies.TotalOccurrences);
            foreach (var row in frequencies.OrderBy(f => f.Occurrences))
            {
                System.Console.WriteLine(" - {0} = {1:F2}% - {2}", row.Item,
                    100.0 * row.Occurrences / frequencies.TotalOccurrences, row.Occurrences);
            }
            System.Console.WriteLine();
        }
    }
}
