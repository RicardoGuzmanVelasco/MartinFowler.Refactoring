using System;
using System.Collections.Generic;

namespace MartinFowler.Refactoring.Theatre.v1
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
                var play = plays[perf.playId];
                var thisAmount = 0;

                switch(play.type)
                {
                    case PlayType.Tragedy:
                        thisAmount = 40000;
                        if(perf.audience > 30)
                            thisAmount += 1000 * (perf.audience - 30);
                        break;
                    case PlayType.Comedy:
                        thisAmount = 30000;
                        if(perf.audience > 20)
                            thisAmount += 10000 + 500 * (perf.audience - 20);
                        thisAmount += 300 * perf.audience;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                // add volume credits
                volumeCredits += Math.Max(perf.audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if(play.type == PlayType.Comedy)
                    volumeCredits += (int)Math.Floor(perf.audience / 5f);
                
                //print line for this order
                result += $" {play.name}: {thisAmount / 100} ({perf.audience})\n";
                totalAmount += thisAmount;
            }
            result += $"Amount owed is {totalAmount / 100}\n";
            result += $"You earned {volumeCredits} credits\n";
            return result;
        }
    }
}