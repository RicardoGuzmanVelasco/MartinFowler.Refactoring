using System.Collections.Generic;
using System.Globalization;

namespace MartinFowler.Refactoring.Theatre.v7
{
    public class BillPrinter
    {
        public string Statement(Invoice invoice, IDictionary<string, Play> plays)
        {
            return RenderPlainText(StatementDataCreation.Create(invoice, plays));
        }
        static string RenderPlainText(StatementData data)
        {
            var result = $"Statement for {data.invoiceCustomer}\n";

            foreach(var perf in data.performances)
                result += $" {perf.play.name}: {perf.amount / 100} ({perf.audience})\n";

            result += $"Amount owed is {USD(data.totalAmount)}\n";
            result += $"You earned {data.totalVolumeCredits} credits\n";
            return result;
        }

        public string StatementHtml(Invoice invoice, IDictionary<string, Play> plays)
        {
            return RenderHtml(StatementDataCreation.Create(invoice, plays));
        }
        static string RenderHtml(StatementData data)
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