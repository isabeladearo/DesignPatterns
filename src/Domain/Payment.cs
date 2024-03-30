using System;

namespace src.Domain;

public class Payment
{
    public Payment(int amount, DateTime date, int? period = null)
    {
        Amount = amount;
        Date = date;
        Period = period;
    }

    public int Amount { get; private set; }

    public DateTime Date { get; private set; }

    public int? Period { get; private set; }
}
