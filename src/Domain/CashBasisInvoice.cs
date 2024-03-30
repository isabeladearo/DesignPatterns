using System.Collections.Generic;

namespace src.Domain;

public class CashBasisInvoice : IInvoiceGeneration
{
    public List<Invoice> Generate(Contract contract, int month, int year)
    {
        var invoices = new List<Invoice>();

        contract.Payments.ForEach(payment => 
        {
            if (payment.Date.Month == month && payment.Date.Year == year)
                invoices.Add(new Invoice(payment.Date, payment.Amount));
        });

        return invoices;
    }
}