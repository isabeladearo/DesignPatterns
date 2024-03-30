using System;

namespace src.Domain;

public class Invoice
{
    public Invoice(DateTime date, double amount)
    {
        Date = date;
        Amount = amount;
    }

    public DateTime Date { get; set; }

    public double Amount { get; set; }
}
