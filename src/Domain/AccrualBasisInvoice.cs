using System.Collections.Generic;

namespace src.Domain;

public class AccrualBasisInvoice : IInvoiceGeneration
{
    public List<Invoice> Generate(Contract contract, int month, int year)
    {
        var invoices = new List<Invoice>();

        var period = 0;

        while (period <= contract.Periods)
        {
            var date = contract.Date.AddMonths(period++);
            var amount = contract.Amount/contract.Periods;

            invoices.Add(new Invoice(date, amount));
        }

        return invoices;
    }
}