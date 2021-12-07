using System.Collections.Generic;
using System.Linq;

namespace MartinFowler.Refactoring.Theatre.v11
{
    internal class StatementData
    {
        public string invoiceCustomer;
        public IEnumerable<Performance> performances;

        public int TotalAmount => performances.Sum(perf => perf.amount);
        public int TotalVolumeCredits => performances.Sum(perf => perf.volumeCredits);
    }
}