using System;
using System.Collections.Generic;

namespace MartinFowler.Refactoring.Theatre.v3
{
    public class BillPrinter
    {
        public string Statement(Invoice invoice, IDictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = $"Statement for {invoice.customer}\n";

            foreach(var perf in invoice.performances)
            {
                // add volume credits
                volumeCredits += Math.Max(perf.audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if(PlayFor(perf).type == PlayType.Comedy)
                    volumeCredits += (int)Math.Floor(perf.audience / 5f);
                
                //print line for this order
                result += $" {PlayFor(perf).name}: {AmountFor(perf) / 100} ({perf.audience})\n";
                totalAmount += AmountFor(perf);
            }
            result += $"Amount owed is {totalAmount / 100}\n";
            result += $"You earned {volumeCredits} credits\n";
            return result;
            
            int AmountFor(Performance aPerformance)
            {
                int result;
                switch(PlayFor(aPerformance).type)
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
            
            Play PlayFor(Performance aPerformance) => plays[aPerformance.playId];
        }
    }
}