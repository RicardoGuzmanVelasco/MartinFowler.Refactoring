using System;

namespace MartinFowler.Refactoring.Theatre.v8
{
    internal class PerformanceCalculator
    {
        public readonly Performance performance;
        public readonly Play play;
        
        public PerformanceCalculator(Performance performance, Play play)
        {
            this.performance = performance;
            this.play = play;
        }
    }
}