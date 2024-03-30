using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace src.Infra.Database;

public class MongoDBContext<T> where T : notnull
{
    private readonly IMongoCollection<T> _collection;

    public MongoDBContext(IOptions<MongoDBOptions> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        _collection = database.GetCollection<T>(options.Value.CollectionName);
    }

    public async Task<List<T>> FindAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default) 
    {
        var cursor = await _collection.FindAsync(filter, null, cancellationToken);
        return cursor.ToList(cancellationToken: cancellationToken);
    }
}
