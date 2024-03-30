using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using src.Domain;

namespace src.Application.Repository;

public interface IContractRepository
{
    Task<List<Contract>> FindAllAsync(CancellationToken cancellationToken);
}
