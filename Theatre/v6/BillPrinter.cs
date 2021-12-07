using System;
using System.Collections.Generic;
using System.Globalization;

namespace MartinFowler.Refactoring.Theatre.v6
{
    public class BillPrinter
    {
        public string Statement(Invoice invoice, IDictionary<string, Play> plays)
        {
            var result = $"Statement for {invoice.customer}\n";
            
            foreach(var perf in invoice.performances)
                result += $" {PlayFor(perf).name}: {AmountFor(perf) / 100} ({perf.audience})\n";

            result += $"Amount owed is {USD(TotalAmount())}\n";
            result += $"You earned {TotalVolumeCredits()} credits\n";
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

            int TotalAmount()
            {
                var result = 0;

                foreach(var perf in invoice.performances)
                    result += AmountFor(perf);
                
                return result;
            }

            int TotalVolumeCredits()
            {
                var result = 0;
                
                foreach(var perf in invoice.performances)
                    result = VolumeCreditsFor(perf);
                
                return result;
            }
        }
    }
}