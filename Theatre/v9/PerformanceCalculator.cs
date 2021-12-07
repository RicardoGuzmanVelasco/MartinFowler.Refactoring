using System;

namespace MartinFowler.Refactoring.Theatre.v9
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

        public int Amount
        {
            get
            {
                int result;
                switch(play.type)
                {
                    case PlayType.Tragedy:
                        result = 40000;
                        if(performance.audience > 30)
                            result += 1000 * (performance.audience - 30);
                        break;
                    case PlayType.Comedy:
                        result = 30000;
                        if(performance.audience > 20)
                            result += 10000 + 500 * (performance.audience - 20);
                        result += 300 * performance.audience;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return result;
            }
        }

        public int VolumeCredits
        {
            get
            {
                var result = Math.Max(performance.audience - 30, 0);

                if(play.type == PlayType.Comedy)
                    result += (int)Math.Floor(performance.audience / 5f);

                return result;
            }
        }
    }
}