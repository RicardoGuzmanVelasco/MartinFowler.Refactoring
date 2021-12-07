using System;
using System.Collections.Generic;
using System.Linq;

namespace MartinFowler.Refactoring.Theatre.v11
{
    internal static class StatementDataCreation
    {
        public static StatementData Create(Invoice invoice, IDictionary<string, Play> plays)
        {
            return new StatementData
            {
                invoiceCustomer = invoice.customer,
                performances = invoice.performances.Select(EnrichPerformance)
            };

            Performance EnrichPerformance(Performance aPerformance)
            {
                var calculator = CreatePerformanceCalculator(aPerformance, PlayFor(aPerformance));
                var result = aPerformance.Clone();
                result.play = calculator.play;
                result.amount = calculator.Amount;
                result.volumeCredits = calculator.VolumeCredits;
                return result;
            }

            Play PlayFor(Performance aPerformance) => plays[aPerformance.playId];
        }

        static PerformanceCalculator CreatePerformanceCalculator(Performance perf, Play play)
        {
            return play.type switch
            {
                PlayType.Tragedy => new TragedyCalculator(perf, play),
                PlayType.Comedy => new ComedyCalculator(perf, play),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}