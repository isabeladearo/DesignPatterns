using System.Collections.Generic;

namespace src.Domain;

public interface IInvoiceGeneration
{
    List<Invoice> Generate(Contract contract, int month, int year);
}
