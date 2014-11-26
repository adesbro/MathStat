using System;
using System.Diagnostics;

namespace MathStat.Example.Console
{
    public class ConsoleTimeMeasure : IDisposable
    {
        private readonly string _description;
        private readonly Stopwatch _stopwatch;

        public ConsoleTimeMeasure(string format, params object[] args)
        {
            _description = string.Format(format, args);
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            System.Console.WriteLine("Time taken for '{0}': {1}", _description, _stopwatch.Elapsed);
            System.Console.WriteLine();
        }
    }
}