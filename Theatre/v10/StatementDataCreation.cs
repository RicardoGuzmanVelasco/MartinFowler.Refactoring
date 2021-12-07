using System;
using System.Collections.Generic;
using System.Linq;

namespace MartinFowler.Refactoring.Theatre.v10
{
    internal static class StatementDataCreation
    {
        public static StatementData Create(Invoice invoice, IDictionary<string, Play> plays)
        {
            var data = new StatementData
            {
                invoiceCustomer = invoice.customer,
                performances = invoice.performances.Select(EnrichPerformance)
            };
            data.totalAmount = TotalAmount(data);
            data.totalVolumeCredits = TotalVolumeCredits(data);
            return data;

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

        static int TotalAmount(StatementData data) => data.performances.Sum(perf => perf.amount);
            static int TotalVolumeCredits(StatementData data) => data.performances.Sum(perf => perf.volumeCredits);
    }
}