using System;
using System.Collections.Generic;

namespace src.Domain;

public class Contract
{
    public Contract(
        string description, 
        double amount, 
        int periods, 
        DateTime date)
    {
        Description = description;
        Amount = amount;
        Periods = periods;
        Date = date;
    }

    public string Description { get; private set; }

    public double Amount { get; private set; }

    public int Periods { get; private set; }

    public DateTime Date { get; private set; }

    public List<Payment> Payments { get; private set; } = new();

    public void AddPayment(Payment payment) => Payments.Add(payment);

    public double GetBalance()
    {
        var balance = Amount;    
        Payments.ForEach(payment => balance -= payment.Amount);
        
        return balance;
    }

    public List<Invoice> GenerateInvoices(string type, int month, int year)
    {
        var invoiceGeneration = InvoiceGeneration.Create(type);
        return invoiceGeneration.Generate(this, month, year);
    }
}
