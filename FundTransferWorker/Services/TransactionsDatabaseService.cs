using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FundTransferWorker.Services;

public class TransactionsDatabaseService
{
    private readonly IMongoCollection<Transaction> _transactionsCollection;

    public TransactionsDatabaseService(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);

        _transactionsCollection = mongoDatabase.GetCollection<Transaction>(
            databaseSettings.Value.CollectionName);
    }

    public async Task<List<Transaction>> GetAsync() =>
        await _transactionsCollection.Find(_ => true).ToListAsync();

    public async Task<Transaction?> GetAsync(string transactionId) =>
        await _transactionsCollection.Find(x => x.TransactionId == transactionId).FirstOrDefaultAsync();

    public async Task CreateAsync(Transaction newTransaction) =>
        await _transactionsCollection.InsertOneAsync(newTransaction);

    public async Task UpdateAsync(string id, Transaction updatedTransaction) =>
        await _transactionsCollection.ReplaceOneAsync(x => x.Id == id, updatedTransaction);

    public async Task RemoveAsync(string id) =>
        await _transactionsCollection.DeleteOneAsync(x => x.Id == id);
}
