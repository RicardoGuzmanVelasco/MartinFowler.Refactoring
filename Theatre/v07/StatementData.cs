using System.Collections.Generic;

namespace MartinFowler.Refactoring.Theatre.v7
{
    internal class StatementData
    {
        public string invoiceCustomer;
        public IEnumerable<Performance> performances;

        public int totalAmount;
        public int totalVolumeCredits;
    }
}