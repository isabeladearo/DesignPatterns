using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using src.Application.Repository;
using src.Domain;
using src.Infra.Collection;
using src.Infra.Database;

namespace src.Infra.Repository;

public class ContractRepository : MongoDBContext<ContractCollection>, IContractRepository
{
    public ContractRepository(IOptions<MongoDBOptions> options) : base(options) {}

    public async Task<List<Contract>> FindAllAsync(CancellationToken cancellationToken) 
    {
        var filter = Builders<ContractCollection>.Filter.Empty;
        var outputData = await FindAsync(filter, cancellationToken);

        var contracts = new List<Contract>();

        outputData.ForEach(output => {
        var contract = new Contract(output.Description, output.Amount, output.Periods, output.Date);
        output.Payments.ForEach(payment => contract.AddPayment(new Payment(payment.Amount, payment.Date, payment.Period)));

        contracts.Add(contract);
        });

        return contracts;
    }
}
