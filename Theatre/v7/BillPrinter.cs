using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MartinFowler.Refactoring.Theatre.v7
{
    public class BillPrinter
    {
        public string Statement(Invoice invoice, IDictionary<string, Play> plays)
        {
            return RenderPlainText(CreateStatementData(invoice, plays));
        }

        static StatementData CreateStatementData(Invoice invoice, IDictionary<string, Play> plays)
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
                var result = aPerformance.Clone();
                result.play = PlayFor(result);
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

        string RenderPlainText(StatementData data)
        {
            var result = $"Statement for {data.invoiceCustomer}\n";
            
            foreach(var perf in data.performances)
                result += $" {perf.play.name}: {perf.amount / 100} ({perf.audience})\n";

            result += $"Amount owed is {USD(data.totalAmount)}\n";
            result += $"You earned {data.totalVolumeCredits} credits\n";
            return result;
        }
        
        string RenderHtml(StatementData data)
        {
            var result = $"<h1>Statement for {data.invoiceCustomer}</h1>\n";

            result += "<table>\n";
            result += "<tr><th>play</th><th>seats</th><th>cost</th></tr>";
            foreach(var perf in data.performances)
            {
                result += $"<tr><td> {perf.play.name}</td><td>({perf.audience})</td>\n";
                result += $"<td>{USD(perf.amount)}</td></tr>\n";
            }
            result += "</table>\n";

            result += $"<p>Amount owed is {USD(data.totalAmount)}</p>\n";
            result += $"<p>You earned {data.totalVolumeCredits} credits</p>\n";
            return result;

            string USD(float aNumber)
            {
                return (aNumber / 100).ToString(new CultureInfo("")
                {
                    NumberFormat = new NumberFormatInfo
                    {
                        CurrencySymbol = "$",
                        NumberDecimalDigits = 2
                    }
                });
            }
        }
        
        static string USD(float aNumber)
        {
            return (aNumber / 100).ToString(new CultureInfo("")
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