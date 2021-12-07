using System;

namespace MartinFowler.Refactoring.Theatre.v10
{
    internal abstract class PerformanceCalculator
    {
        public readonly Performance performance;
        public readonly Play play;

        public PerformanceCalculator(Performance performance, Play play)
        {
            this.performance = performance;
            this.play = play;
        }

        public abstract int Amount { get; }

        public virtual int VolumeCredits => Math.Max(performance.audience - 30, 0);
    }

    internal class TragedyCalculator : PerformanceCalculator
    {
        public TragedyCalculator(Performance performance, Play play) : base(performance, play) { }

        public override int Amount
        {
            get
            {
                var result = 40000;
                
                if(performance.audience > 30)
                    result += 1000 * (performance.audience - 30);
                
                return result;
            }
        }
    }
    
    internal class ComedyCalculator : PerformanceCalculator
    {
        public ComedyCalculator(Performance performance, Play play) : base(performance, play) { }

        public override int Amount
        {
            get
            {
                var result = 30000;
                if(performance.audience > 20)
                    result += 10000 + 500 * (performance.audience - 20);
                result += 300 * performance.audience;
                return result;
            }
        }

        public override int VolumeCredits => base.VolumeCredits + (int)Math.Floor(performance.audience / 5f);
    }
}