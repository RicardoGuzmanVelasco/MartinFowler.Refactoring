using System;
using System.Collections.Generic;
using System.Linq;

namespace MartinFowler.Refactoring.Theatre.v8
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
                var calculator = new PerformanceCalculator(aPerformance, PlayFor(aPerformance));
                var result = aPerformance.Clone();
                result.play = calculator.play;
                result.amount = AmountFor(result);
                result.volumeCredits = VolumeCreditsFor(result);
                return result;
            }

            Play PlayFor(Performance aPerformance) => plays[aPerformance.playId];

            int AmountFor(Performance aPerformance)
            {
                int result;
                switch(aPerformance.play.type)
                {
                    case PlayType.Tragedy:
                        result = 40000;
                        if(aPerformance.audience > 30)
                            result += 1000 * (aPerformance.audience - 30);
                        break;
                    case PlayType.Comedy:
                        result = 30000;
                        if(aPerformance.audience > 20)
                            result += 10000 + 500 * (aPerformance.audience - 20);
                        result += 300 * aPerformance.audience;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return result;
            }

            int VolumeCreditsFor(Performance aPerformance)
            {
                var result = Math.Max(aPerformance.audience - 30, 0);

                if(PlayType.Comedy == aPerformance.play.type)
                    result += (int)Math.Floor(aPerformance.audience / 5f);

                return result;
            }
        }
            static int TotalAmount(StatementData data) => data.performances.Sum(perf => perf.amount);
            static int TotalVolumeCredits(StatementData data) => data.performances.Sum(perf => perf.volumeCredits);
    }
}