using System;

namespace src.Domain;

public class InvoiceGeneration
{
    public static IInvoiceGeneration Create(string type)
    {
        if (type == "cash")
            return new CashBasisInvoice();

        if (type == "accrual")
            return new AccrualBasisInvoice();

        throw new ArgumentException($"Invalid type: {type}");
    }
}
