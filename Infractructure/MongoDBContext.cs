using Constructor_API.Core.Shared;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Constructor_API.Infractructure
{
    public class MongoDBContext
    {
        private IMongoDatabase _database;
        private readonly IConfiguration _configuration;
        public IClientSessionHandle _session;
        public MongoClient MongoClient { get; set; }
        private readonly List<Func<IClientSessionHandle, Task>> _commands = [];

        public MongoDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
            MongoClient = new MongoClient(_configuration["Server"]);
            if (MongoClient != null ) 
                _database = MongoClient.GetDatabase(_configuration["DatabaseName"]);
        }

        public async Task AddCommand(Func<IClientSessionHandle, Task> func)
        {
            _commands.Add(func);
            await Task.CompletedTask;
        }

        public async Task SaveChanges()
        {
            using (_session = await MongoClient.StartSessionAsync())
            {
                //_session.StartTransaction();
                //var commandTasks = _commands.Select(c => c());
                //await Task.WhenAll(commandTasks);
                //await _session.CommitTransactionAsync();
                //_commands.Clear();
                //return _commands.Count;

                _session.StartTransaction();
                try
                {
                    foreach (var command in _commands)
                    {
                        await command(_session);
                    }
                    await _session.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    await _session.AbortTransactionAsync();
                    throw ex;
                }
                finally
                {
                    _commands.Clear();
                }
            }
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        public void Dispose()
        {
            _session?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
