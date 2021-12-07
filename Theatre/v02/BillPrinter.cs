using System;
using System.Collections.Generic;
using System.Globalization;

namespace MartinFowler.Refactoring.Theatre.v2
{
    public class BillPrinter
    {
        public string Statement(Invoice invoice, IDictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = $"Statement for {invoice.customer}\n";
            var format = new CultureInfo("format")
            {
                NumberFormat = new NumberFormatInfo
                {
                    CurrencySymbol = "$",
                    NumberDecimalDigits = 2
                }
            };
            foreach(var perf in invoice.performances)
            {
                var play = plays[perf.playId];
                var thisAmount = AmountFor(perf, play);
                
                // add volume credits
                volumeCredits += Math.Max(perf.audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if(play.type == PlayType.Comedy)
                    volumeCredits += (int)Math.Floor(perf.audience / 5f);
                
                //print line for this order
                result += $" {play.name}: {thisAmount / 100} ({perf.audience})\n";
                totalAmount += thisAmount;
            }
            result += $"Amount owed is {(totalAmount / 100).ToString(format)}\n";
            result += $"You earned {volumeCredits} credits\n";
            return result;
        }

        static int AmountFor(Performance aPerformance, Play play)
        {
            int result;
            switch(play.type)
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
    }
}