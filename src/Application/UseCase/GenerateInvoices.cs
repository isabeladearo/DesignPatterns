using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using src.Application.Presenter;
using src.Application.Repository;
using src.Domain;

namespace src.Application.UseCase;

public class GenerateInvoices : IRequestHandler<Input, object>
{
    private readonly IContractRepository _contractRepository;

    public GenerateInvoices(IContractRepository contractRepository)
    {
        _contractRepository = contractRepository;
    }

    public async Task<object> Handle(Input input, CancellationToken cancellationToken)
    {
        var contracts = await _contractRepository.FindAllAsync(cancellationToken);

        var output = new List<Invoice>();

        if (contracts.Any())
        {
            contracts.ForEach(contract => 
            {
                var invoices = contract.GenerateInvoices(input.InvoiceType, input.InvoiceMonth, input.InvoiceYear);
                invoices.ForEach(invoice => output.Add(invoice));
            });
        }

        IPresenter presenter = ResponsePresenter.Create(input.ResponseType);
        return presenter.Present(output);
    }
}

public class Input : IRequest<object>
{
    public int InvoiceMonth { get; set; }
    public int InvoiceYear { get; set; }
    public string InvoiceType { get; set; }
    public string ResponseType { get; set; }
    public string UserAgent { get; set; }
    public string Host { get; set; }

    public void DefineRequestResponsible(string userAgent, string host)
    {
        UserAgent = userAgent ?? string.Empty;
        Host = host ?? string.Empty;
    }

    public override string ToString()
    {
        return $"Month: {InvoiceMonth}, Year: {InvoiceYear}, Type: {InvoiceType}, ResponseType: {ResponseType}, UserAgent: {UserAgent}, Host: {Host}";
    }
}