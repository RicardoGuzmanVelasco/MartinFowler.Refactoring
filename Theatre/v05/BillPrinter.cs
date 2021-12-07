using System;
using System.Collections.Generic;
using System.Globalization;

namespace MartinFowler.Refactoring.Theatre.v5
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
                volumeCredits = VolumeCreditsFor(perf);

                //print line for this order
                result += $" {PlayFor(perf).name}: {AmountFor(perf) / 100} ({perf.audience})\n";
                totalAmount += AmountFor(perf);
            }
            result += $"Amount owed is {USD(totalAmount)}\n";
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
            
            int VolumeCreditsFor(Performance aPerformance)
            {
                var result = Math.Max(aPerformance.audience - 30, 0);
                
                if(PlayType.Comedy == PlayFor(aPerformance).type)
                    result += (int)Math.Floor(aPerformance.audience / 5f);
                
                return result;
            }

            string USD(float i)
            {
                return (i / 100).ToString(new CultureInfo("")
                {
                    NumberFormat = new NumberFormatInfo
                    {
                        CurrencySymbol = "$",
                        NumberDecimalDigits = 2
                    }
                });
            }
        }
    }
}